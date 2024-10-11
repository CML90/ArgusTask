using Argus_BalanceByAddressAPI.Data;
using Argus_BalanceByAddressAPI.Data.Models;
using Cardano.Sync.Reducers;
using CardanoSharp.Wallet.Models.Transactions;
using Microsoft.EntityFrameworkCore;
using PallasDotnet.Models;

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
        Console.WriteLine("Hello World"); 
        /*Console.WriteLine(response); //response is empty
        _logger.LogInformation("Processing new block at slot {slot}", response.Tip.Slot); //response.Block
        // Implement your logic here

        using BalanceAddressDbContext _dbContext = dbContextFactory.CreateDbContext();

        //check the transactio bodies thats in the new block

        //IEnumerable<TransactionBody> transactions = response.Block.TransactionBodies; 

        //what is transaction bodies here? is it really in cardano.Sync or in Pallas.NET?
        // - lets just try printing it (BlockCbor) out to see first
        Console.WriteLine(response.BlockCbor);

        //after removing and reinstalling Palla.NET to match the version in JPGStore and Levvy, 
        //Block is giving errors and wants to be chantged to BlockCbor
        //by looking at the data structures, following NextResponse, which has the Point Tip and then Point has Slot and hash

        /*foreach (TransactionBody tx in transactions)
        {
            await ProcessOutputsAsync(response.Tip.Slot, tx, _dbContext); // receiving assets
            await ProcessInputsAsync(response.Tip.Slot, tx, _dbContext); // sending assets
        }

        await _dbContext.SaveChangesAsync();
        await _dbContext.DisposeAsync();*/
    }

    public async Task RollBackwardAsync(NextResponse response)
    {
       /* Console.WriteLine(response); //response is empty
        _logger.LogInformation("Rollback at slot {slot}", response.Tip.Slot); //Block.Slot
        // Implement rollback logic here

        //use _dbContext to interact with the created DB
        //What's dbContextFactory ?
        using BalanceAddressDbContext _dbContext = dbContextFactory.CreateDbContext();
        //this checks where the rollback is happening
        //Block is from Pallas, go and check it later
        ulong rollbackSlot = response.Tip.Slot; //Block.Slot

        IQueryable<BalanceAddress> rollbackEntries = _dbContext.BalanceAddress    //check this
        .AsNoTracking()
        .Where(ba => ba.Slot > rollbackSlot); //checks table for stored slot numbers, we dont have the Slot yet - added in Models


        _dbContext.BalanceAddress.RemoveRange(rollbackEntries); //remove thentries with those slot numbers

        // Save changes
        await _dbContext.SaveChangesAsync();
        await _dbContext.DisposeAsync();*/

    }

    // private async Task ProcessInputsAsync(ulong Slot, TransactionBody tx, BalanceAddressDbContext _dbContext)
    // {
    //     //add here
    //     //checking Docker for now, then coming back to this
    // }

    // private async Task ProcessOutputsAsync(ulong Slot, TransactionBody tx, BalanceAddressDbContext _dbContext)
    // {
    //     //add here
    // }
}