using AAA.DataClient.Game;
using Ajuna.Integration.Helper;
using Ajuna.Integration.Model.Avatar;
using Ajuna.TheOracle.DataClient.Model;
using Serilog;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.account;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.avatar;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.primitive_types;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.sp_core.crypto;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using System.Diagnostics;
using System.Numerics;

namespace Ajuna.Integration.Query
{
    public static class DataTasks
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="url"></param>
        /// <param name="blockHash"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<AvatarGame?> PollGameDataAsync(NetworkType networkType, string? blockHash, CancellationToken token)
        {
            Account account = Client.BaseClient.Alice;

            var client = new BajunNetwork(account, networkType, Network.GetUrl(networkType));

            if (!await client.ConnectAsync(true, true, token))
            {
                return null;
            }

            var avatarGame = new AvatarGame();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            avatarGame.SeasonTreasuries = await GetTreasuryAsync(client, blockHash, token);
            stopwatch.Stop();

            Log.Debug($"GetTreasuryAsync took {stopwatch.Elapsed.TotalSeconds} seconds.");
            Log.Debug($"GetTreasuryAsync returned {avatarGame.SeasonTreasuries.Count} items.");
            stopwatch.Restart();
            var seasonInfoDict = await GetSeasonStatsAsync(client, blockHash, token);
            stopwatch.Stop();

            Log.Debug($"GetSeasonStatsAsync took {stopwatch.Elapsed.TotalSeconds} seconds.");
            Log.Debug($"GetSeasonStatsAsync returned {seasonInfoDict.Count} items.");

            stopwatch.Restart();
            var playerSeasonInfoDict = await GetPlayerSeasonStatsAsync(client, blockHash, token);
            stopwatch.Stop();

            Log.Debug($"GetPlayerSeasonStatsAsync took {stopwatch.Elapsed.TotalSeconds} seconds.");
            Log.Debug($"GetPlayerSeasonStatsAsync returned {playerSeasonInfoDict.Count} items.");

            stopwatch.Restart();
            var avatarInfoDict = await GetAvatarAccountsAsync(client, blockHash, token);
            stopwatch.Stop();

            Log.Debug($"GetAvatarAccountsAsync took {stopwatch.Elapsed.TotalSeconds} seconds.");
            Log.Debug($"GetAvatarAccountsAsync returned {avatarInfoDict.Count} items.");

            stopwatch.Restart();
            var avatarsDict = await GetAvatarsAsync(client, blockHash, token);
            stopwatch.Stop();

            Log.Debug($"GetAvatarsAsync took {stopwatch.Elapsed.TotalSeconds} seconds.");
            Log.Debug($"GetAvatarsAsync returned {avatarsDict.Count} items.");

            stopwatch.Restart();
            var avatarTradesDict = await GetTradesAsync(client, blockHash, token);
            stopwatch.Stop();

            Log.Debug($"GetTradesAsync took {stopwatch.Elapsed.TotalSeconds} seconds.");
            Log.Debug($"GetTradesAsync returned {avatarTradesDict.Count} items.");

            await client.DisconnectAsync();

            var playerDict = new Dictionary<string, PlayerInfo>();
            avatarGame.Players = playerDict;

            var keys = avatarsDict.Keys.ToList();
            keys.AddRange(avatarInfoDict.Keys);
            keys = keys.Distinct().ToList();

            foreach (var key in keys)
            {
                avatarInfoDict.TryGetValue(key, out PlayerConfig? accountInfo);
                playerSeasonInfoDict.TryGetValue(key + "_1", out PlayerSeasonConfig playerSeasonConfig1);
                playerSeasonInfoDict.TryGetValue(key + "_2", out PlayerSeasonConfig playerSeasonConfig2);
                seasonInfoDict.TryGetValue(key + "_1", out SeasonInfo? seasonInfo1);
                seasonInfoDict.TryGetValue(key + "_2", out SeasonInfo? seasonInfo2);
                avatarsDict.TryGetValue(key, out List<AvatarInfo>? avatarInfos);

                // add market price if available
                if (avatarInfos != null)
                {
                    foreach (var avatarInfo in avatarInfos)
                    {
                        if (avatarTradesDict.TryGetValue(avatarInfo.Id, out double price))
                        {
                            avatarInfo.MarketPrice = price;
                        }
                    }
                }

                Dictionary<int, PlayerSeasonInfo> playerSeasonInfo = new Dictionary<int, PlayerSeasonInfo>();
                if (playerSeasonConfig1 != null && playerSeasonConfig1.Stats != null)
                {
                    var playerSeasonInfo1 = new PlayerSeasonInfo
                    {
                        FirstMint = playerSeasonConfig1.Stats.Mint.First.Value,
                        LastMint = playerSeasonConfig1.Stats.Mint.Last.Value,
                        FirstForge = playerSeasonConfig1.Stats.Forge.First.Value,
                        LastForge = playerSeasonConfig1.Stats.Forge.Last.Value,
                        Bought = playerSeasonConfig1.Stats.Trade.Bought.Value,
                        Sold = playerSeasonConfig1.Stats.Trade.Sold.Value
                    };
                    if (seasonInfo1 != null)
                    {
                        playerSeasonInfo1.Mint = seasonInfo1.Minted.Value;
                        playerSeasonInfo1.Forge = seasonInfo1.Forged.Value;
                    }
                    playerSeasonInfo.Add(1, playerSeasonInfo1);
                }
                if (playerSeasonConfig2 != null && playerSeasonConfig2.Stats != null)
                {
                    var playerSeasonInfo2 = new PlayerSeasonInfo
                    {
                        FirstMint = playerSeasonConfig2.Stats.Mint.First.Value,
                        LastMint = playerSeasonConfig2.Stats.Mint.Last.Value,
                        FirstForge = playerSeasonConfig2.Stats.Forge.First.Value,
                        LastForge = playerSeasonConfig2.Stats.Forge.Last.Value,
                        Bought = playerSeasonConfig2.Stats.Trade.Bought.Value,
                        Sold = playerSeasonConfig2.Stats.Trade.Sold.Value
                    };
                    if (seasonInfo2 != null)
                    {
                        playerSeasonInfo2.Mint = seasonInfo2.Minted != null ? seasonInfo2.Minted.Value : 0;
                        playerSeasonInfo2.Forge = seasonInfo2.Forged != null ? seasonInfo2.Forged.Value : 0;
                    }
                    playerSeasonInfo.Add(2, playerSeasonInfo2);
                }

                var playerInfo = new PlayerInfo()
                {
                    FreeMints = accountInfo != null && accountInfo.FreeMints != null ? accountInfo.FreeMints.Value : 0,
                    SeasonInfos = playerSeasonInfo,
                    AvatarInfos = avatarInfos ?? new List<AvatarInfo> { },
                };

                playerDict.Add(key, playerInfo);
            }

            return avatarGame;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="client"></param>
        /// <param name="blockHash"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<Dictionary<string, SeasonInfo>> GetSeasonStatsAsync(BajunNetwork client, string? blockHash, CancellationToken token)
        {
            var module = "AwesomeAvatars";
            var item = "SeasonStats";

            byte[]? nextStorageKey = null;
            List<(byte[], BaseTuple<U16, AccountId32>, SeasonInfo)> storageEntries;

            var result = new Dictionary<string, SeasonInfo>();

            do
            {
                storageEntries = await client.GetAllStoragePagedAsync<BaseTuple<U16, AccountId32>, SeasonInfo>(client.SubstrateClient, module, item, nextStorageKey, 1000, blockHash, token);

                foreach (var storageEntry in storageEntries)
                {
                    result.Add(((AccountId32)storageEntry.Item2.Value[1]).ToAddress(1337) + "_" + (U16)storageEntry.Item2.Value[0], storageEntry.Item3);

                    nextStorageKey = storageEntry.Item1;
                }
            } while (storageEntries.Any());

            Log.Debug("{0}.{1} storage {2} records parsed.", module, item, result.Count);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="client"></param>
        /// <param name="blockHash"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<Dictionary<string, PlayerSeasonConfig>> GetPlayerSeasonStatsAsync(BajunNetwork client, string? blockHash, CancellationToken token)
        {
            var module = "AwesomeAvatars";
            var item = "PlayerSeasonConfigs";

            byte[]? nextStorageKey = null;
            List<(byte[], BaseTuple<AccountId32, U16>, PlayerSeasonConfig)> storageEntries;

            var result = new Dictionary<string, PlayerSeasonConfig>();

            do
            {
                storageEntries = await client.GetAllStoragePagedAsync<BaseTuple<AccountId32, U16>, PlayerSeasonConfig>(client.SubstrateClient, module, item, nextStorageKey, 1000, blockHash, token);

                foreach (var storageEntry in storageEntries)
                {
                    result.Add(((AccountId32)storageEntry.Item2.Value[0]).ToAddress(1337) + "_" + (U16)storageEntry.Item2.Value[1], storageEntry.Item3);

                    nextStorageKey = storageEntry.Item1;
                }
            } while (storageEntries.Any());

            Log.Debug("{0}.{1} storage {2} records parsed.", module, item, result.Count);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="client"></param>
        /// <param name="blockHash"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<Dictionary<string, PlayerConfig>> GetAvatarAccountsAsync(BajunNetwork client, string? blockHash, CancellationToken token)
        {
            var module = "AwesomeAvatars";
            var item = "PlayerConfigs";

            byte[]? nextStorageKey = null;
            List<(byte[], AccountId32, PlayerConfig)> storageEntries;

            var result = new Dictionary<string, PlayerConfig>();

            do
            {
                storageEntries = await client.GetAllStoragePagedAsync<AccountId32, PlayerConfig>(client.SubstrateClient, module, item, nextStorageKey, 1000, blockHash, token);

                foreach (var storageEntry in storageEntries)
                {
                    result.Add(storageEntry.Item2.ToAddress(1337), storageEntry.Item3);

                    nextStorageKey = storageEntry.Item1;
                }
            } while (storageEntries.Any());

            Log.Debug("{0}.{1} storage {2} records parsed.", module, item, result.Count);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="client"></param>
        /// <param name="blockHash"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<Dictionary<int, double>> GetTreasuryAsync(BajunNetwork client, string? blockHash, CancellationToken token)
        {
            var module = "AwesomeAvatars";
            var item = "Treasury";

            byte[]? nextStorageKey = null;
            List<(byte[], U16, U128)> storageEntries;

            var result = new Dictionary<int, double>();

            do
            {
                storageEntries = await client.GetAllStoragePagedAsync<U16, U128>(client.SubstrateClient, module, item, nextStorageKey, 1000, blockHash, token);

                foreach (var storageEntry in storageEntries)
                {
                    result.Add(storageEntry.Item2.Value, Math.Round((double)(storageEntry.Item3.Value / BigInteger.Pow(10, 12)), 0));

                    nextStorageKey = storageEntry.Item1;
                }
            } while (storageEntries.Any());

            Log.Debug("{0}.{1} storage {2} records parsed.", module, item, result.Count);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="client"></param>
        /// <param name="blockHash"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<Dictionary<string, string[]>> GetOwnersAsync(BajunNetwork client, string? blockHash, CancellationToken token)
        {
            var module = "AwesomeAvatars";
            var item = "Owners";

            byte[]? nextStorageKey = null;
            List<(byte[], AccountId32, BoundedVecT35)> storageEntries;

            var result = new Dictionary<string, string[]>();

            do
            {
                storageEntries = await client.GetAllStoragePagedAsync<AccountId32, BoundedVecT35>(client.SubstrateClient, module, item, nextStorageKey, 1000, blockHash, token);

                foreach (var storageEntry in storageEntries)
                {
                    result.Add(storageEntry.Item2.ToAddress(1337), storageEntry.Item3.Value.Value.Select(p => p.ToHexString()).ToArray());

                    nextStorageKey = storageEntry.Item1;
                }
            } while (storageEntries.Any());

            Log.Debug("{0}.{1} storage {2} records parsed.", module, item, result.Count);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="client"></param>
        /// <param name="blockHash"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<Dictionary<string, List<AvatarInfo>>> GetAvatarsAsync(BajunNetwork client, string? blockHash, CancellationToken token)
        {
            var module = "AwesomeAvatars";
            var item = "Avatars";

            byte[]? nextStorageKey = null;
            List<(byte[], H256, BaseTuple<AccountId32, Avatar>)> storageEntries;

            var result = new Dictionary<string, List<AvatarInfo>>();

            do
            {
                storageEntries = await client.GetAllStoragePagedAsync<H256, BaseTuple<AccountId32, Avatar>>(client.SubstrateClient, module, item, nextStorageKey, 1000, blockHash, token);

                foreach (var storageEntry in storageEntries)
                {
                    if (storageEntry.Item3 == null || storageEntry.Item3.Value == null)
                    {
                        nextStorageKey = storageEntry.Item1;
                        continue;
                    }
                    var address = ((AccountId32)storageEntry.Item3.Value[0]).ToAddress(1337);

                    var avatar = (Avatar)storageEntry.Item3.Value[1];

                    var dna = Utils.Bytes2HexString(avatar.Dna.Value.Value.Select(p => p.Value).ToArray(), Utils.HexStringFormat.Pure);

                    var avatarInfo = new AvatarInfo(storageEntry.Item2.ToHexString(), avatar.SeasonId.Value, avatar.Souls.Value, dna);

                    if (result.TryGetValue(address, out List<AvatarInfo>? avatars))
                    {
                        avatars.Add(avatarInfo);
                    }
                    else
                    {
                        result.Add(address, new List<AvatarInfo>() { avatarInfo });
                    }

                    nextStorageKey = storageEntry.Item1;
                }
            } while (storageEntries.Any());

            Log.Debug("{0}.{1} storage {2} records parsed.", module, item, result.Count);

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="client"></param>
        /// <param name="blockHash"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<Dictionary<string, double>> GetTradesAsync(BajunNetwork client, string? blockHash, CancellationToken token)
        {
            var module = "AwesomeAvatars";
            var item = "Trade";

            byte[]? nextStorageKey = null;
            List<(byte[], BaseTuple<U16, H256>, U128)> storageEntries;

            var result = new Dictionary<string, double>();

            do
            {
                storageEntries = await client.GetAllStoragePagedAsync<BaseTuple<U16, H256>, U128>(client.SubstrateClient, module, item, nextStorageKey, 1000, blockHash, token);

                foreach (var storageEntry in storageEntries)
                {
                    result.Add(((H256)storageEntry.Item2.Value[1]).ToHexString(), (double)(storageEntry.Item3.Value / BigInteger.Pow(10, 12)));

                    nextStorageKey = storageEntry.Item1;
                }
            } while (storageEntries.Any());

            Log.Debug("{0}.{1} storage {2} records parsed.", module, item, result.Count);

            return result;
        }
    }
}