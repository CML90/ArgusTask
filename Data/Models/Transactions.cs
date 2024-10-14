using Cardano.Sync.Data.Models;
using Chrysalis.Cardano.Models.Plutus;
using PallasDotnet.Models;


namespace Argus_BalanceByAddressAPI.Data.Models;

public record Transactions() : IReducerModel
{

    public string TxHash { get; set; } = default!;

    public ulong TxIndex { get; set; } 

    public string Address { get; set; } = default!;

    public ulong Amount { get; set; } = default!;

    public bool isOutput { get; set; } //should change to isOutput

    public ulong Slot { get; set; } //for the rollback function - double check later
}