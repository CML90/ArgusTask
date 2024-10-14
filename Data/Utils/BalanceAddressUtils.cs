using System.Text;
using System.Text.Json;
using CardanoSharp.Wallet.Enums;
using Microsoft.Extensions.Configuration;
using Chrysalis.Cardano.Models.Plutus;
using ChrysalisAddress = Chrysalis.Cardano.Models.Plutus.Address;
using System.Text.RegularExpressions;
using CardanoSharp.Wallet.Utilities;
using NSec.Cryptography;

namespace Argus_BalanceByAddressAPI.Data.Utils;
public static class BalanceAddressUtils
{
    public static List<string> MapMetadataToCborHexList(JsonElement metadataValue)
    {
        List<string> datumCborHexList = [];
        StringBuilder datumCborHex = new();
        
        Regex specialCharRegex = new(@"[:,](?!$)", RegexOptions.Compiled);

        foreach (JsonElement element in metadataValue.EnumerateArray())
        {
            if (element[0].GetInt64() == 30) continue;

            string? text = element[1].GetProperty("Text").GetString();

            if (text is null) continue;

            if (specialCharRegex.IsMatch(text)) continue;

            if (text.EndsWith(','))
            {
                datumCborHex.Append(text.TrimEnd(','));
                datumCborHexList.Add(datumCborHex.ToString());
                datumCborHex.Clear();
            }
            else
            {
                datumCborHex.Append(text);
            }
        }

        return datumCborHexList;
    }

    public static byte[] MapDatumToDatumHash(byte[] datum)
    {
        Blake2b algorithm = HashAlgorithm.Blake2b_256;

        return algorithm.Hash(datum);
    }
}