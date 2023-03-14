using System.Numerics;
using Ajuna.Integration;
using Ajuna.Integration.Helper;
using Ajuna.NetApi;
using Ajuna.NetApi.Model.Types;
using Ajuna.NetApi.Model.Types.Base;
using Ajuna.NetApi.Model.Types.Primitive;
using Ajuna.TheOracle.DataClient.Model;
using Ajuna.TheOracle.DataClient.Model.Avatar;
using Bajun.Network.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.account;
using Bajun.Network.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.avatar;
using Bajun.Network.NET.NetApiExt.Generated.Model.sp_core.bounded.bounded_vec;
using Bajun.Network.NET.NetApiExt.Generated.Model.sp_core.crypto;
using Serilog;

namespace Ajuna.TheOracle.DataClient
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
        public static async Task<AvatarGame> PollGameDataAsync(string url, byte[] blockHash, CancellationToken token)
        {
            Account account = BajunNetwork.Alice;

            var client = new BajunNetwork(account, url);

            if (!await client.ConnectAsync(true, true, token))
            {
                return null;
            }

            var avatarGame = new AvatarGame();
            avatarGame.SeasonTreasuries = await GetTreasuryAsync(client, blockHash, token);

            var seasonInfoDict = await GetSeasonStatsAsync(client, blockHash, token);
            var avatarInfoDict = await GetAvatarAccountsAsync(client, blockHash, token);
            var avatarsDict = await GetAvatarsAsync(client, blockHash, token);
            var avatarTradesDict = await GetTradesAsync(client, blockHash, token);

            await client.DisconnectAsync();

            var playerDict = new Dictionary<string, PlayerInfo>();
            avatarGame.Players = playerDict;

            foreach (var key in avatarInfoDict.Keys)
            {
                var accountInfo = avatarInfoDict[key];

                seasonInfoDict.TryGetValue(key, out SeasonInfo? seasonInfo);
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

                playerDict.Add(key, new PlayerInfo()
                {
                    FreeMints = accountInfo.FreeMints.Value,
                    StorageTier = (int)accountInfo.StorageTier.Value,
                    MintStats = new MintStats()
                    {
                        FirstMint = (int)accountInfo.Stats.Mint.First.Value,
                        lastMint = (int)accountInfo.Stats.Mint.Last.Value,

                        SeasonParticipated = accountInfo.Stats.Mint.SeasonsParticipated.Value.Value.Value.Select(x => (int)x.Value).ToArray(),
                    },
                    ForgeStats = new ForgeStats()
                    {
                        FirstForge = (int)accountInfo.Stats.Forge.First.Value,
                        LastForge = (int)accountInfo.Stats.Forge.Last.Value,

                        SeasonParticipated = accountInfo.Stats.Mint.SeasonsParticipated.Value.Value.Value.Select(x => (int)x.Value).ToArray(),
                    },
                    MarketStats = new MarketStats()
                    {
                        Bought = (int)accountInfo.Stats.Trade.Bought.Value,
                        Sold = (int)accountInfo.Stats.Trade.Sold.Value
                    },
                    CurrentSeasonStats = new CurrentSeasonStats()
                    {
                        Minted = seasonInfo != null ? (int)seasonInfo.Minted.Value : 0,
                        Forged = seasonInfo != null ? (int)seasonInfo.Forged.Value : 0,
                    },
                    AvatarInfos = avatarInfos
                });
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
        public static async Task<Dictionary<string, SeasonInfo>> GetSeasonStatsAsync(BajunNetwork client, byte[] blockHash, CancellationToken token)
        {
            var module = "AwesomeAvatars";
            var item = "SeasonStats";

            string? nextStorageKey = null;
            List<string[]> storageEntries;

            var result = new Dictionary<string, SeasonInfo>();

            do
            {
                storageEntries = await client.GetGenericNextAsync(module, item, nextStorageKey, 1000, blockHash, token);

                foreach (var storageChangeSet in storageEntries)
                {
                    var storageKeyString = storageChangeSet[0];
                    var key = storageKeyString.Substring(storageKeyString.Length - 64);
                    var pubKey = Utils.HexToByteArray(key);
                    var address = Utils.GetAddressFrom(pubKey, 1337);

                    var seasonInfo = new SeasonInfo();
                    seasonInfo.Create(storageChangeSet[1]);

                    result.Add(address, seasonInfo);

                    nextStorageKey = storageKeyString;
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
        public static async Task<Dictionary<string, AccountInfo>> GetAvatarAccountsAsync(BajunNetwork client, byte[] blockHash, CancellationToken token)
        {
            var module = "AwesomeAvatars";
            var item = "Accounts";

            string? nextStorageKey = null;
            List<string[]> storageEntries;

            var result = new Dictionary<string, AccountInfo>();

            do
            {
                storageEntries = await client.GetGenericNextAsync(module, item, nextStorageKey, 1000, blockHash, token);
                foreach (var storageChangeSet in storageEntries)
                {
                    var storageKeyString = storageChangeSet[0];
                    var key = storageKeyString.Substring(storageKeyString.Length - 64);
                    var pubKey = Utils.HexToByteArray(key);
                    var address = Utils.GetAddressFrom(pubKey, 1337);

                    var accountInfo = new AccountInfo();
                    accountInfo.Create(storageChangeSet[1]);

                    result.Add(address, accountInfo);

                    nextStorageKey = storageKeyString;
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
        public static async Task<Dictionary<int, double>> GetTreasuryAsync(BajunNetwork client, byte[] blockHash, CancellationToken token)
        {
            var module = "AwesomeAvatars";
            var item = "Treasury";

            string? nextStorageKey = null;
            List<string[]> storageEntries;

            var result = new Dictionary<int, double>();

            do
            {
                storageEntries = await client.GetGenericNextAsync(module, item, nextStorageKey, 1000, blockHash, token);
                foreach (var storageChangeSet in storageEntries)
                {
                    var storageKeyString = storageChangeSet[0];
                    var key = storageKeyString.Substring(storageKeyString.Length - 4);
                    var seasonId = new U16();
                    seasonId.Create(key);

                    var treasury = new U128();
                    treasury.Create(storageChangeSet[1]);

                    result.Add(seasonId.Value, Math.Round((double)(treasury.Value / BigInteger.Pow(10, 12)),0));

                    nextStorageKey = storageKeyString;
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
        public static async Task<Dictionary<string, string[]>> GetOwnersAsync(BajunNetwork client, byte[] blockHash, CancellationToken token)
        {
            var module = "AwesomeAvatars";
            var item = "Owners";

            string? nextStorageKey = null;
            List<string[]> storageEntries;

            var result = new Dictionary<string, string[]>();

            do
            {
                storageEntries = await client.GetGenericNextAsync(module, item, nextStorageKey, 1000, blockHash, token);
                foreach (var storageChangeSet in storageEntries)
                {
                    var storageKeyString = storageChangeSet[0];
                    var key = storageKeyString.Substring(storageKeyString.Length - 64);
                    var pubKey = Utils.HexToByteArray(key);
                    var address = Utils.GetAddressFrom(pubKey, 1337);

                    var avatarsVec = new BoundedVecT28();
                    avatarsVec.Create(storageChangeSet[1]);

                    var avatarIds = avatarsVec.Value.Value.Select(p => p.ToHexString()).ToArray();

                    result.Add(address, avatarIds);

                    nextStorageKey = storageKeyString;
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
        public static async Task<Dictionary<string, List<AvatarInfo>>> GetAvatarsAsync(BajunNetwork client, byte[] blockHash, CancellationToken token)
        {
            var module = "AwesomeAvatars";
            var item = "Avatars";

            string? nextStorageKey = null;
            List<string[]> storageEntries;

            var result = new Dictionary<string, List<AvatarInfo>>();

            do
            {
                storageEntries = await client.GetGenericNextAsync(module, item, nextStorageKey, 1000, blockHash, token);
                foreach (var storageChangeSet in storageEntries)
                {
                    var storageKeyString = storageChangeSet[0];
                    var key = storageKeyString.Substring(storageKeyString.Length - 64);

                    var accountId32AvatarTuple = new BaseTuple<AccountId32, Avatar>();
                    accountId32AvatarTuple.Create(storageChangeSet[1]);

                    var accountId32 = (AccountId32)accountId32AvatarTuple.Value[0];

                    var address = accountId32.ToAddress(1337);

                    var avatar = (Avatar)accountId32AvatarTuple.Value[1];

                    var dna = Utils.Bytes2HexString(avatar.Dna.Value.Value.Select(p => p.Value).ToArray(), Utils.HexStringFormat.Pure);

                    var avatarInfo = new AvatarInfo("0x" + key, avatar.SeasonId.Value, avatar.Souls.Value, dna);

                    if (result.TryGetValue(address, out List<AvatarInfo>? avatars))
                    {
                        avatars.Add(avatarInfo);
                    }
                    else
                    {
                        result.Add(address, new List<AvatarInfo>() { avatarInfo });
                    }

                    nextStorageKey = storageKeyString;
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
        public static async Task<Dictionary<string, double>> GetTradesAsync(BajunNetwork client, byte[] blockHash, CancellationToken token)
        {
            var module = "AwesomeAvatars";
            var item = "Trade";

            string? nextStorageKey = null;
            List<string[]> storageEntries;

            var result = new Dictionary<string, double>();

            do
            {
                storageEntries = await client.GetGenericNextAsync(module, item, nextStorageKey, 1000, blockHash, token);
                foreach (var storageChangeSet in storageEntries)
                {
                    var storageKeyString = storageChangeSet[0];
                    var key = storageKeyString.Substring(storageKeyString.Length - 64);

                    var optPrice = new U128();
                    optPrice.Create(storageChangeSet[1]);

                    result.Add("0x" + key, (double)(optPrice.Value / BigInteger.Pow(10, 12)));

                    nextStorageKey = storageKeyString;
                }
            } while (storageEntries.Any());

            Log.Debug("{0}.{1} storage {2} records parsed.", module, item, result.Count);

            return result;
        }
    }
}