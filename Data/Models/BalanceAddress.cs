using Cardano.Sync.Data.Models;
using Cardano.Sync.Data.Models.Datums;

namespace Argus_BalanceByAddressAPI.Data.Models;

//double check the inheritance and types
public record BalanceAddress() : IReducerModel
{
    //Address is a type in Argus somewhere - verify this info and then maybe can use Address userAddress
    public string Address { get; set; } = default!;
    public ulong Balance { get; set; } = default!; //may have other assets like tokens
}