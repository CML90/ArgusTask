using Argus_BalanceByAddressAPI.Data;
using Argus_BalanceByAddressAPI.Data.Models;
using Argus_BalanceByAddressAPI.Data.Extensions;
using Cardano.Sync.Reducers;
using Microsoft.EntityFrameworkCore;
using PallasDotnet.Models;
using Chrysalis.Cbor;
using Cardano.Sync.Data.Models.Datums;

namespace Argus_BalanceByAddressAPI.Reducer;


//This is a bit vague for me as of now, 
//Do i need to put in chainsync or will it do it automatically? Like, when i run the creation of the DB - CardanoNodeClient() ?
// 
//How and what logic do i put in the RollForward or RollBackward? -> the logic that checks the blocks and then according to state, need to remove from table or add
//Base from Argus.Sync README with Reducer, and then mixing in JPGStore.Sync.reducers.ListingByAddressReducer
public class BalanceAddressReducer
(
    IDbContextFactory<BalanceAddressDbContext> dbContextFactory,
    IConfiguration configuration,
    ILogger<BalanceAddressReducer> logger
) : IReducer<BalanceAddress> //errors without argument in <> as IReducer in Argus has <T> - how come tan's works without it?
{
    private readonly ILogger<BalanceAddressReducer> _logger = logger;

    public async Task RollForwardAsync(NextResponse response)
    {
        using BalanceAddressDbContext _dbContext = dbContextFactory.CreateDbContext();

        Console.WriteLine($"response: {response}");
        Console.WriteLine(response); //response is empty
        _logger.LogInformation("Processing new block at slot {slot}", response.Block); //response.Tip.Slot
        // Implement your logic here

        //check the transactio bodies thats in the new block

        IEnumerable<TransactionBody> transactions = response.Block.TransactionBodies;

        //what is transaction bodies here? is it really in cardano.Sync or in Pallas.NET?
        // - lets just try printing it (BlockCbor) out to see first
        //Console.WriteLine(response.BlockCbor);

        //after removing and reinstalling Palla.NET to match the version in JPGStore and Levvy, 
        //Block is giving errors and wants to be chantged to BlockCbor
        //by looking at the data structures, following NextResponse, which has the Point Tip and then Point has Slot and hash

        int x = 0;
        foreach (TransactionBody tx in transactions)
        {
            //await ProcessOutputsAsync(response.Tip.Slot, tx, _dbContext); // receiving assets
            //await ProcessInputsAsync(response.Tip.Slot, tx, _dbContext); // sending assets
            if (x >= 5) break;
            Console.WriteLine($"Transaction {x}:");
            Console.WriteLine(tx);

            var inputList = tx.Inputs.ToList(); //try to see if i can use IEnumerable<> here too or LINQ
            Console.WriteLine("Transaction Inputs:");
            Console.WriteLine(inputList[0]);

            var outputList = tx.Outputs.ToList(); //try to see if i can use IEnumerable<> here too or LINQ
            Console.WriteLine("Transaction Outputs:");
            Console.WriteLine(outputList[0]);

            var mintList = tx.Mint.ToList(); //try to see if i can use IEnumerable<> here too or LINQ
            Console.WriteLine("Mint Inputs:");
            if (mintList != null && mintList.Any()) Console.WriteLine(mintList[0]);

            var redsList = tx.Redeemers?.ToList(); //try to see if i can use IEnumerable<> here too or LINQ
            Console.WriteLine("Redeemers Inputs:");
            if (redsList != null && redsList.Any()) Console.WriteLine(redsList[0]);

            x++;

            await ProcessOutputsAsync(response.Block.Slot, tx, _dbContext);
            await ProcessInputsAsync(response.Block.Slot, tx, _dbContext);
        }



        //try out deserialization:
        //hmmm.... it takes a byte[]  hem how do i make it a byte array -> i dont understnad how JPGStore does it
        //var deserializedTransaction = CborSerializer.Deserialize<TransactionBody>(response);

        //try serializing
        //var serializedTransaction = CborSerializer.Serialize(transactions); //cannot convert to Chrysalic ICBor


        //await _dbContext.SaveChangesAsync();
        //await _dbContext.DisposeAsync();
    }

    public async Task RollBackwardAsync(NextResponse response)
    {
        Console.WriteLine(response); //response is empty
        _logger.LogInformation("Rollback at slot {slot}", response.Tip.Slot); //Block.Slot
        // Implement rollback logic here

        //use _dbContext to interact with the created DB
        //What's dbContextFactory ?
        using BalanceAddressDbContext _dbContext = dbContextFactory.CreateDbContext();

        //this checks where the rollback is happening
        //Block is from Pallas, go and check it later
        ulong rollbackSlot = response.Block.Slot; //Block.Slot

        IQueryable<Transactions> rollbackEntries = _dbContext.Transactions
        .AsNoTracking()
        .Where(tr => tr.Slot > rollbackSlot); //checks table for slot numbers greater than rollback slot

        foreach (var transaction in rollbackEntries)
        {
            var balanceEntry = await _dbContext.BalanceAddress
            .FirstOrDefaultAsync(ba => ba.Address == transaction.Address);

            if (balanceEntry != null)
            {
                // Determine the amount to update and whether it's an output or input
                if (transaction.isOutput) //lovelace was given to them
                {
                    // For outputs, subtract from the balance
                    balanceEntry.Balance -= transaction.Amount;
                    _logger.LogInformation("Subtracted {amount} from balance for address {address}", transaction.Amount, transaction.Address);

                }
                else //was taken
                {
                    // For inputs, add to the balance
                    balanceEntry.Balance += transaction.Amount;
                    _logger.LogInformation("Added {amount} to balance for address {address}", transaction.Amount, transaction.Address);
                }

                _dbContext.BalanceAddress.Update(balanceEntry);

            }

        }

        _dbContext.Transactions.RemoveRange(rollbackEntries); //remove the entries with those slot numbers

        await _dbContext.SaveChangesAsync();
        await _dbContext.DisposeAsync();
    }

    private async Task ProcessInputsAsync(ulong BlockSlot, TransactionBody tx, BalanceAddressDbContext _dbContext)
    {

        List<(string TxHash, ulong TxIndex)> inputOutRefs = tx.Inputs
    .Select(input => (input.Id.ToHex().ToLowerInvariant(), input.Index))
    .ToList();

        try
        {
            ulong totalOutput = 0; //total amount being sent to other addresses
            foreach (TransactionOutput output in tx.Outputs)
            {
                totalOutput += output.Amount.Coin;
            }

            //ulong totalInput = 0;
            foreach (var input in inputOutRefs)
            {
                Console.WriteLine($"INPUTOUTREFS: {input}");

                //subtract the amount sent (but what about fees?)
                //fee = total input - total output
                //total output = sum of all transaction output amounts -> totalOutput
                //total input = amount in tr.ID in another transaction

                //totalInput += //get amount from transaction matching the hash+ index

                //but what if i wasnt able to get the data from before and so an input doesnt match?
                //for now, just use transaction output nlng

                //add new transaction
                var existingTransaction = await _dbContext.Transactions
                .FirstOrDefaultAsync(tr => tr.TxHash + tr.TxIndex == input.TxHash + input.TxIndex && tr.isOutput == true);


                if (existingTransaction != null) //if theres a matching ID, but then it has the same problem of excluding linked transactions that are lacking
                {
                    Transactions newTransaction = new()
                    {
                        TxHash = input.TxHash, //may be used later on as an input?
                        TxIndex = input.TxIndex,
                        Address = existingTransaction.Address, //get the address from the address that owns the ID
                        Amount = existingTransaction.Amount, //the amount from the transaction above
                        isOutput = false,
                        Slot = BlockSlot
                    };

                    _dbContext.Transactions.Add(newTransaction);
                    await _dbContext.SaveChangesAsync();
                }

            }

            //ulong fee = totalInput - totalOutput;
            //ulong total = totalInput + fee;
            var firstTransaction = await _dbContext.Transactions
            .FirstOrDefaultAsync(tr => tr.TxHash + tr.TxIndex == inputOutRefs[0].TxHash + inputOutRefs[1].TxIndex); //all inputs come from the same sender - address

            if (firstTransaction != null)
            {
                string MatchedAddress = firstTransaction.Address;

                var balanceAddressEntry = await _dbContext.BalanceAddress
                .AsNoTracking()
                .FirstOrDefaultAsync(ba => ba.Address == MatchedAddress);

                if (balanceAddressEntry != null)
                {
                    var updatedBalanceAddress = new BalanceAddress
                    {
                        Address = balanceAddressEntry.Address,
                        Balance = balanceAddressEntry.Balance - totalOutput
                    };


                    _dbContext.BalanceAddress.Update(updatedBalanceAddress);
                }
                else
                {
                    _logger.LogWarning($"No balance entry found for address: {MatchedAddress}");
                }

            }
            else
            {
                _logger.LogWarning($"No main transaction found for input: {inputOutRefs[0]}");
            }

        }
        catch (Exception e)
        {
            _logger.LogError("Error: {message}", e.ToString());
        }


        //var deserialized = CborSerializer.Deserialize<PallasDotnet.Models.Hash>(cborData: tx.Id.Bytes); 
        //i have no idea what i did but i hope it works
        //Console.WriteLine($"DESERIALIZE: {deserialized}");
    }

    private async Task ProcessOutputsAsync(ulong BlockSlot, TransactionBody tx, BalanceAddressDbContext _dbContext)
    {
        string txHash = tx.Id.ToHex().ToLowerInvariant();

        foreach (TransactionOutput output in tx.Outputs)
        {

            //test to Bech32()
            string? Bech32Addr = output.Address.Raw.ToBech32(); //copied Tan's AddressExtension class
            Console.WriteLine($"ADDRESS:  {Bech32Addr}"); //worked! understand later

            ulong Coin = output.Amount.Coin;
            Console.WriteLine($"LOVELACE: {Coin}"); //worked!

            if (Bech32Addr is null || !Bech32Addr.StartsWith("addr")) continue;
            //not sure of this: string pkh = Convert.ToHexString(new CardanoSharpAddress(outputBech32Addr).GetPublicKeyHash()).ToLowerInvariant();
            //if (output.Datum is null) continue; //Datum holds Type and Date = System.byte[] and Raw = System.Byte[]
            //what does it mean if its null?

            try
            {
                var existingEntry = await _dbContext.BalanceAddress
                .AsNoTracking()
                .FirstOrDefaultAsync(ab => ab.Address == Bech32Addr);
                //check if address exists in table

                Transactions newTransaction = new()
                {
                    TxHash = txHash,
                    TxIndex = output.Index, //may be used later on as an input?
                    Address = Bech32Addr,
                    Amount = Coin,
                    isOutput = true,
                    Slot = BlockSlot
                };

                _dbContext.Transactions.Add(newTransaction);

                if (existingEntry == null)
                {
                    Console.WriteLine("No Entry");
                    //add address, balance, and slot
                    BalanceAddress addNew = new()
                    {
                        Address = Bech32Addr, //maybe something like a patch would be better?
                        Balance = Coin,
                    };

                    _dbContext.BalanceAddress.Add(addNew);
                }
                else
                {
                    Console.WriteLine($"EXISTS: {existingEntry}");
                    //add balance to existing balance
                    BalanceAddress updatedEntry = new()
                    {
                        Address = existingEntry.Address,
                        Balance = existingEntry.Balance + Coin
                    };

                    _dbContext.BalanceAddress.Update(updatedEntry);

                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error: {message}", e.Message);
            }
        }

    }
}