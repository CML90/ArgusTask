using Cardano.Sync.Data.Models;
using Cardano.Sync.Data.Models.Datums;

namespace Argus_BalanceByAddressAPI.Data.Models;

//double check the inheritance and types
public record BalanceAddress : IReducerModel
{
    //because Address is a type in Argus somewhere - verify this info and then maybe can use Address userAddress
    public string Address { get; set; } = default!;
    public Lovelace Balance { get; set; } = default!;
}