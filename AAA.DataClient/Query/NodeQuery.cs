using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ajuna.Integration;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.NetApi;
using Substrate.Bajun.NET.NetApiExt.Generated.Storage;
using Serilog;
using Substrate.NetApi.Model.Types;
using System.Text.Json;
using Ajuna.TheOracle.DataClient.Model.Avatar;
using Ajuna.TheOracle.DataClient.Model;
using Ajuna.TheOracle.DataClient;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.season;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.avatar.rarity_tier;
using Ajuna.Integration.Helper;
using Ajuna.Integration.Query;
using Ajuna.Integration.Model.Avatar;
using System.Runtime.CompilerServices;
using Ajuna.AAA.Season2.Model;
using Newtonsoft.Json.Linq;

namespace AAA.DataClient.Query
{
    public static class NodeQuery
    {
        public static async Task GetTotalIssuanceDataAsync(NetworkType networkType,  CancellationToken token)
        {
            Account account = Ajuna.Integration.Client.BaseClient.Alice;

            var client = new BajunNetwork(account, networkType, Network.GetUrl(networkType));

            if (!await client.ConnectAsync(true, true, token))
            {
                return;
            }

            var params1 = RequestGenerator.GetStorage("Balances", "TotalIssuance", Substrate.NetApi.Model.Meta.Storage.Type.Plain);
            var u16 = new U16();
            u16.Create(1);
            var params2 = AwesomeAvatarsStorage.TreasuryParams(u16);
            var list = new List<string>();
            for (uint i = 1841812; i < 1841816; i = i + 1)
            {
                var blockNumber = new BlockNumber();
                blockNumber.Create(i);
                var blockHash = await client.SubstrateClient.Chain.GetBlockHashAsync(blockNumber, token);
                var totalIssuance = await client.SubstrateClient.GetStorageAsync<U128>(params1, blockHash.Value, token);
                var treasury = await client.SubstrateClient.GetStorageAsync<U128>(params2, blockHash.Value, token);
                Log.Information("{blockHash};{block};{value};{treasury};{kumulative}", blockHash.Value, i, totalIssuance.Value, treasury != null ? treasury.Value : 0, totalIssuance.Value + (treasury != null ? treasury.Value : 0));
                list.Add($"{i};{totalIssuance.Value};{(treasury != null ? treasury.Value : 0)};{totalIssuance.Value + (treasury != null ? treasury.Value : 0)}");
            }

            File.WriteAllLines("totalissuance.csv", list);
            await client.DisconnectAsync();

        }

        public static async Task<Hash> GetBlockHashAsync(NetworkType networkType, CancellationToken token)
        {
            Account account = Ajuna.Integration.Client.BaseClient.Alice;

            var client = new BajunNetwork(account, networkType, Network.GetUrl(networkType));

            if (!await client.ConnectAsync(true, true, token))
            {
                return null;
            }

            Hash? result = await client.SubstrateClient.Chain.GetBlockHashAsync(token);

            await client.DisconnectAsync();

            return result;
        }

        public static async Task<Dictionary<int, string>> GetBlockHashsAsync(int startBlockPast, NetworkType networkType, bool readFileExist, CancellationToken token)
        {
            var fileName = "blockHashDict.json";

            var result = new Dictionary<int, string>();
            if (readFileExist && File.Exists(fileName))
            {
                result = JsonSerializer.Deserialize<Dictionary<int, string>>(File.ReadAllText(fileName));
            }
            Account account = Ajuna.Integration.Client.BaseClient.Alice;

            var client = new BajunNetwork(account, networkType, Network.GetUrl(networkType));

            if (!await client.ConnectAsync(true, true, token))
            {
                return null;
            }

            var currentBlockNumber = (int)(await client.SubstrateClient.SystemStorage.Number(token)).Value;

            var currentPastBlock = startBlockPast;

            while (currentPastBlock < currentBlockNumber)
            {
                if (!result.ContainsKey(currentPastBlock))
                {
                    if (!await client.ConnectAsync(true, true, token))
                    {
                        return null;
                    }

                    var blockNumber = new BlockNumber();
                    blockNumber.Create((uint)currentPastBlock);

                    var currentBlockHash = await client.SubstrateClient.Chain.GetBlockHashAsync(blockNumber, token);

                    result.Add(currentPastBlock, currentBlockHash.Value);
                    Log.Information("{0} - {1}", currentPastBlock, currentBlockHash.Value);
                }

                currentPastBlock = currentPastBlock + 300;
            }

            await client.DisconnectAsync();

            File.WriteAllText(fileName, JsonSerializer.Serialize(result));

            return result;
        }

        public static async Task WriteAvatarGameHistoryAsync(NetworkType networkType, CancellationToken token)
        {
            var fileName = "blockHashDict.json";
            var avatarGameName = "avatar_game_<hash>.json";
            if (!File.Exists(fileName))
            {
                Log.Warning("No blockhash file available,");
                return; 
            }
            var blockHashDict = JsonSerializer.Deserialize<Dictionary<int, string>>(File.ReadAllText(fileName));
            foreach (var key in blockHashDict.Keys.OrderBy(p => p)) 
            { 
                var blockHash = blockHashDict[key];
                var avatarGameFile = avatarGameName.Replace("<hash>", blockHash.Substring(2).ToLower());
                AvatarGame? avatarGame;
                Log.Information($"{key} pulling! {avatarGameFile}");
                if (File.Exists(avatarGameFile))
                {
                    //avatarGame = JsonSerializer.Deserialize<AvatarGame>(File.ReadAllText("avatar_game.json"));
                    continue;
                } 
                else
                {
                    Hash hash = new Hash();
                    hash.Create(blockHashDict[key]);
                    avatarGame = await WriteAvatarGameAsync(networkType, hash, token);
                }

                if (avatarGame != null) 
                {
                    continue;
                }

                Log.Warning($"No avatar game found or available for {key} pulling!");
            }
        }

        public static async Task<AvatarGame?> WriteAvatarGameAsync(NetworkType networkType, Hash? blockHash, CancellationToken token)
        {
            var blockHashStr = "";
            if (blockHash == null)
            {
                blockHash = await GetBlockHashAsync(networkType, token);
            } 
            else
            {
                blockHashStr = "_" + Utils.Bytes2HexString(blockHash.Bytes, Utils.HexStringFormat.Pure).ToLower();
            }

            AvatarGame? avatarGame = await DataTasks.PollGameDataAsync(networkType, blockHash.Value, token);

            if (avatarGame == null)
            {
                Log.Warning("Could write file as we have no avatar game!");
                return null;
            }

            File.WriteAllText($"avatar_game{blockHashStr}.json", JsonSerializer.Serialize(avatarGame, new JsonSerializerOptions()
            {
                WriteIndented = true,
            }));

            return avatarGame;
        }

    }
}
