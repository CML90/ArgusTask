using Argus_BalanceByAddressAPI.Data;
using Argus_BalanceByAddressAPI.Data.Models;
using Cardano.Sync.Reducers;
using Microsoft.EntityFrameworkCore;
using PallasDotnet.Models;

namespace Argus_BalanceByAddressAPI.Reducer;


//This is a bit vague for me as of now, 
//Do i need to put in chainsync or will it do it automatically? Like, when i run the creation of the DB 
// it start the Reducer? or do i call it somewhere? Or is it called already in Program.cs?
// 
//How and what logic do i put in the RollForward or RollBackward?
//Base from Argus.Sync README with Reducer, and then mixing in JPGStore.Sync.reducers.ListingByAddressReducer
public class BalanceAddressReducer
(
    IDbContextFactory<BalanceAddressDBContext> dbContextFactory,
    IConfiguration configuration,
    ILogger<BalanceAddressReducer> logger
) : IReducer<BalanceAddress> //errors without argument in <> as IReducer in Argus has <T> - how come tan's works without it?
{
    private readonly ILogger<BalanceAddressReducer> _logger = logger;

    public async Task RollForwardAsync(NextResponse response)
    {
        _logger.LogInformation("Processing new block at slot {slot}", response.Block.Slot);
        // Implement your logic here - now this is the difficult (?) part
    }

    public async Task RollBackwardAsync(NextResponse response)
    {
        _logger.LogInformation("Rollback at slot {slot}", response.Block.Slot);
        // Implement rollback logic here
    }
}