using System;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using AAA.DataClient.Query;
using Ajuna.Integration;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Ajuna.TheOracle.DataClient.Model;
using Ajuna.TheOracle.DataClient.Model.Avatar;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.avatar;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.season;
using Substrate.Bajun.NET.NetApiExt.Generated.Storage;
using Newtonsoft.Json.Linq;
using Serilog;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.avatar.rarity_tier;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.bajun_runtime;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.sp_core.crypto;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.sp_runtime.multiaddress;
using System.Numerics;
using Ajuna.Integration.Helper;
using Ajuna.Integration.Model.Avatar;
using Ajuna.AAA.Season2.Model;
using Ajuna.AAA.Season2;
using System.Runtime.ExceptionServices;
using static System.Collections.Specialized.BitVector32;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.sp_weights.weight_v2;
using System.Collections.Generic;
using System.Linq;
using AAA.DataClient.Game;
using System.Net;
using AAA.DataClient.Model.Subscan;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_nfts.types;

namespace Ajuna.TheOracle.DataClient
{
    internal class Program
    {


        // Program entry point
        public static Task Main(string[] args) => new Program().MainAsync();

        public async Task MainAsync()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Data Parser for Bajun Network!");


            await ApiQuery.LogInAndOutFlowsAsync("MEXC", "bUMDt7QKAJTFNHRnSEGKSbti36SZe9R4d153YmUJ5vVUsK4yb", new int[] { 1, 4, 12, 24, 48 });
            await ApiQuery.LogInAndOutFlowsAsync("GATE", "bUJ8V6tiaHA8fQ8XjRgVQUpfztWeoAadwaRvADoVKm3SjmGVS", new int[] { 1, 4, 12, 24, 48 });
            await ApiQuery.LogInAndOutFlowsAsync(" GSR", "bUKcFKEBCUgvXGTpfQZwxuUF99ibD1gwGWjkDnH9JZJE5JbRP", new int[] { 1, 4, 12, 24, 48 }, true);

            //await ApiQuery.LogOutFlowsAsync("SEED", new string[] {
            //    "bULxiufQrS4cKLyesU6ojX2zKjzavemtfJFmLb7nAbLYFPu7Q",
            //    "bULWgaXWyNrn33f2h4p3gbnqVyhsL7Vxytd3brmjUXxzNjWXb",
            //    "bUJP1bQrEo9xxkz5ssnf6SvRdgdD4K8SnygKpxqzdt4KwrpR6",
            //    "bUMu5t34CZQKKrgeM3djg7RUa2X2AYuVTu75QT2MVWTAkbEQ8",
            //    "bUJssHCEEvidEBbDeAToLxBVTVbpz8An4gQGrEf1LPJHGsjkP",
            //    "bUNYiH7WSCHUAU4usSMAVyTbWtFBhEzroXfS6rm8LXPuNVBcV",
            //    "bUNMZ8qodVaCT13f7g7Zory3ysTN29bLChH9SVxaJkxouNmRe",
            //    "bUHytMQZKGAwPm3oMnZqSCWMUJQE8JEe7W2Nin4W8pNReYJ7T",
            //    "bUNXxkxBB8tj5BkwN9y2hCnxJG5kmpvfoEx2FoVqmMaW4BGRy",
            //    "bUMs4gqqE4brJCAczqTzZFx77zAqWEUkEwsZtM5HfANR3wUnF",
            //    "bULj4oF9YCiBQcs8jRJwQvE89MZxfYmYBXDLNCt2gpWmJSUou",
            //    "bULQpFDcKT5nkPvJ6R5JqsuNTu3dfTNnzk5LBrvLExJ8QQnGr",
            //    "bUNeDxY2BAGgkPGHv65Q5pq9T4Pn3tQXBLWefEvq1pSFAMqtg",
            //    "bUJZGW4WnP7pALNPfpw7on5JjtBybzCL9UHeqhgGe2FeQD7Xk",
            //    "bUMrttmieyUVkbxtVakzUET8B7rNBqpgbYs6TYRaXL2tGBqds",
            //}, new int[] { 1, 4, 12, 24, 48 });


            ////await NodeQuery.GetTotalIssuanceDataAsync(NetworkType.Bajun, "wss://bajun.api.onfinality.io/public-ws", CancellationToken.None);

            await NodeQuery.GetBlockHashsAsync(2613588, NetworkType.Bajun, true, CancellationToken.None);

            await NodeQuery.WriteAvatarGameHistoryAsync(NetworkType.Bajun, CancellationToken.None);

            //await NodeQuery.WriteAvatarGameAsync(NetworkType.Bajun, null, CancellationToken.None);

            //await FreeMintsTrackerAllAsync();

            //await FreeMintsTrackerBacthAsync();




            //DataQueries();

            ReadSeasonHistoryJsons();
        }

        private async Task FreeMintsTrackerBacthAsync()
        {
            List<FreeMintsSeason2> discordList = System.Text.Json.JsonSerializer.Deserialize<List<FreeMintsSeason2>>(File.ReadAllText("season2.json"));
            var avatarGame = JsonSerializer.Deserialize<AvatarGame>(File.ReadAllText("avatar_game.json"));
            var keys = new List<string>() { 
                "bUM6axtvdWHyizEqqsRcLigznvtTFpgR13aTRmDQ7ThaqB8N8", 
                "bUNiTEDe92dz5WRa9k25ac2Y67YJCzRbPsjb4MffxccCuAYHR",
                "bUPCrew5Q5kDy7WDidBogrV1Jnp84qrDrEpjmCZYYm6Wi5Y5h",
                "bUJhuZEZggaEaPpGYZ5A5aF11E6n14CX4M8fn55FPZRZ1LsJE"
            };

            var allAddresses = new List<string>();

            List<string> list = new();
            while (keys.Count > 0) // While there are new addresses to process
            {
                var count = 0;
                var total = keys.Count();
                allAddresses.AddRange(keys);

                var newKeys = new List<string>();
                foreach (var newaddress in keys)
                {
                    count++;
                    var tt = await ApiQuery.GetExtrinsicsAccountAsync("utility", "batch", newaddress);
                    if (tt != null && tt.Data != null && tt.Data.Extrinsics != null)
                    {
                        foreach (var extrinsic in tt.Data.Extrinsics)
                        {
                            List<ParamsExt> dataList = System.Text.Json.JsonSerializer.Deserialize<List<ParamsExt>>(extrinsic.Params);
                            List<(string?, int)> tuples = new List<(string?, int)>();
                            if (dataList.Any())
                            {
                                foreach(var dataEntry in dataList)
                                {
                                    if (dataEntry.value != null)
                                    {

                                        foreach (var dataEntryPart in dataEntry.value)
                                        {
                                            if (dataEntryPart.call_module == "AwesomeAvatars" && dataEntryPart.call_name == "transfer_free_mints")
                                            {
                                                var tuple = (dataEntryPart.@params[0].value.ToString(), int.Parse(dataEntryPart.@params[1].value.ToString()));
                                                tuples.Add(tuple);
                                            }
                                        }
                                    }
                                }

                                //var freemintTransfers = dataList.SelectMany(p => p.value?.Where(p => p.call_module == "AwesomeAvatars" && p.call_name == "transfer_free_mints").Select(p => (p.@params[0].value.ToString(), int.Parse(p.@params[1].value.ToString())))).ToList();
                                var discordMember = discordList.FirstOrDefault(p => p.Address == extrinsic.account_display.Address);
                                var discordId = discordMember != null ? discordMember.Id.ToString() : "unknown";
                                foreach (var entry in tuples)
                                {

                                    Log.Information("{blockNumber};{blockTimestamp};{discord};{address};{mints};{toAddress}", extrinsic.block_num, extrinsic.block_timestamp, discordId, extrinsic.account_display.Address, entry.Item2, Substrate.NetApi.Utils.GetAddressFrom(Substrate.NetApi.Utils.HexToByteArray(entry.Item1), 1337));
                                    list.Add($"{extrinsic.block_num};{extrinsic.block_timestamp};{discordId};{extrinsic.account_display.Address};{entry.Item2};{Substrate.NetApi.Utils.GetAddressFrom(Substrate.NetApi.Utils.HexToByteArray(entry.Item1), 1337)}");
                                }
                            }
                        }
                    }

                    Log.Information("{count} / {total} --> {new}", count, total, newKeys.Count);

                }

                keys.Clear();
                keys.AddRange(newKeys); // Replace newAddresses with new2Addresses for the next iteration
                newKeys.Clear();

            }
            File.WriteAllLines("batchFreeMintTransfers.csv", list);
        }

        private async Task FreeMintsTrackerAllAsync()
        {
            List<FreeMintsSeason2> discordList = System.Text.Json.JsonSerializer.Deserialize<List<FreeMintsSeason2>>(File.ReadAllText("season2.json"));
            var avatarGame = JsonSerializer.Deserialize<AvatarGame>(File.ReadAllText("avatar_game.json"));
            var keys = avatarGame.Players.Select(p => p.Key).ToList();

            var allAddresses = new List<string>();

            List<string> list = new List<string>();
            while (keys.Count > 0) // While there are new addresses to process
            {
                var count = 0;
                var total = keys.Count();
                allAddresses.AddRange(keys);

                var newKeys = new List<string>();
                foreach (var newaddress in keys)
                {
                    count++;
                    var tt = await ApiQuery.GetExtrinsicsAccountAsync("awesomeavatars", "transfer_free_mints", newaddress);
                    if (tt != null && tt.Data != null && tt.Data.Extrinsics != null)
                    {
                        foreach (var extrinsic in tt.Data.Extrinsics)
                        {
                            List<Params> dataList = System.Text.Json.JsonSerializer.Deserialize<List<Params>>(extrinsic.Params);
                            var publicKey = dataList.Find(p => p.type_name == "AccountId").value.ToString();
                            var address = Substrate.NetApi.Utils.GetAddressFrom(Substrate.NetApi.Utils.HexToByteArray(publicKey), 1337);

                            if (!allAddresses.Contains(address))
                            {
                                newKeys.Add(address);
                            }

                            var mints = int.Parse(dataList.Find(p => p.type_name == "MintCount").value.ToString());

                            var discordMember = discordList.FirstOrDefault(p => p.Address == extrinsic.account_display.Address);
                            var discordId = discordMember != null ? discordMember.Id.ToString() : "unknown";

                            Log.Information("{blockNumber};{blockTimestamp};{discord};{address};{mints};{toAddress}", extrinsic.block_num, extrinsic.block_timestamp, discordId, extrinsic.account_display.Address, mints, address);
                            list.Add($"{extrinsic.block_num};{extrinsic.block_timestamp};{discordId};{extrinsic.account_display.Address};{mints};{address}");
                        }
                    }

                    Log.Information("{count} / {total} --> {new}", count, total, newKeys.Count);

                }

                keys.Clear();
                keys.AddRange(newKeys); // Replace newAddresses with new2Addresses for the next iteration
                newKeys.Clear();

            }
            File.WriteAllLines("allFreeMintTransfers.csv", list);
        }

        private async Task FreeMintsTrackerAsync()
        {
            List<FreeMintsSeason2> allFreeMintsLaunch = System.Text.Json.JsonSerializer.Deserialize<List<FreeMintsSeason2>>(File.ReadAllText("season2.json"));
            List<string> list = new List<string>();
            var total = allFreeMintsLaunch.Count();
            var count = 0;

            var addresses = allFreeMintsLaunch.Select(P => P.Address).ToList();
            var newAddresses = new List<string>();
            foreach (var freemintAddress in allFreeMintsLaunch)
            {
                count++;
                var tt = await ApiQuery.GetExtrinsicsAccountAsync("awesomeavatars", "transfer_free_mints", freemintAddress.Address);
                if (tt != null && tt.Data != null && tt.Data.Extrinsics != null)
                {
                    foreach (var extrinsic in tt.Data.Extrinsics)
                    {
                        List<Params> dataList = System.Text.Json.JsonSerializer.Deserialize<List<Params>>(extrinsic.Params);
                        var publicKey = dataList.Find(p => p.type_name == "AccountId").value.ToString();
                        var address = Substrate.NetApi.Utils.GetAddressFrom(Substrate.NetApi.Utils.HexToByteArray(publicKey), 1337);

                        if (!addresses.Contains(address))
                        {
                            newAddresses.Add(address);
                        }

                        var mints = int.Parse(dataList.Find(p => p.type_name == "MintCount").value.ToString());
                        Log.Information("{blockNumber};{blockTimestamp};{discord};{address};{mints};{toAddress}", extrinsic.block_num, extrinsic.block_timestamp, freemintAddress.Id, extrinsic.account_display.Address, mints, address);
                        list.Add($"{extrinsic.block_num};{extrinsic.block_timestamp};{freemintAddress.Id};{extrinsic.account_display.Address};{mints};{address}");
                    }

                }

                Log.Information("{count} / {total} --> {new}", count, total, newAddresses.Count);

                Thread.Sleep(100);
            }

            while (newAddresses.Count > 0) // While there are new addresses to process
            {
                addresses.AddRange(newAddresses);

                var new2Addresses = new List<string>();
                foreach (var newaddress in newAddresses)
                {
                    count++;
                    var tt = await ApiQuery.GetExtrinsicsAccountAsync("awesomeavatars", "transfer_free_mints", newaddress);
                    if (tt != null && tt.Data != null && tt.Data.Extrinsics != null)
                    {
                        foreach (var extrinsic in tt.Data.Extrinsics)
                        {
                            List<Params> dataList = System.Text.Json.JsonSerializer.Deserialize<List<Params>>(extrinsic.Params);
                            var publicKey = dataList.Find(p => p.type_name == "AccountId").value.ToString();
                            var address = Substrate.NetApi.Utils.GetAddressFrom(Substrate.NetApi.Utils.HexToByteArray(publicKey), 1337);

                            if (!addresses.Contains(address))
                            {
                                new2Addresses.Add(address);
                            }

                            var mints = int.Parse(dataList.Find(p => p.type_name == "MintCount").value.ToString());
                            Log.Information("{blockNumber};{blockTimestamp};{discord};{address};{mints};{toAddress}", extrinsic.block_num, extrinsic.block_timestamp, "unknown", extrinsic.account_display.Address, mints, address);
                            list.Add($"{extrinsic.block_num};{extrinsic.block_timestamp};unknown;{extrinsic.account_display.Address};{mints};{address}");
                        }
                    }

                    Log.Information("{count} / {total} --> {new}", count, total, new2Addresses.Count);

                    Thread.Sleep(100);
                }

                newAddresses.Clear();
                newAddresses.AddRange(new2Addresses); // Replace newAddresses with new2Addresses for the next iteration
                new2Addresses.Clear();

            }
            File.WriteAllLines("freemintTransfers.csv", list);
        }

        public static EnumRuntimeCall CreateTransferCall(AccountId32 from, AccountId32 to, int token)
        {
            var fromAddress = new EnumMultiAddress();
            fromAddress.Create(MultiAddress.Id, from);

            var toAddress = new EnumMultiAddress();
            toAddress.Create(MultiAddress.Id, to);

            var balance = new BaseCom<U128>();
            balance.Create(new BigInteger(token * Math.Pow(10, 12)));

            var baseTuple = new BaseTuple<EnumMultiAddress, EnumMultiAddress, BaseCom<U128>>();
            baseTuple.Create(fromAddress, toAddress, balance);

            var call = new Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_balances.pallet.EnumCall();
            call.Create(Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_balances.pallet.Call.transfer_keep_alive, baseTuple);

            var enumCall = new EnumRuntimeCall();
            enumCall.Create(RuntimeCall.Balances, call);

            return enumCall;
        }


        public static void ReadAvatarsJson()
        {
            var avatarGame = JsonSerializer.Deserialize<AvatarGame>(File.ReadAllText("avatar_game.json"));

            var allavatars = avatarGame.Players.Where(p => p.Value != null && p.Value.AvatarInfos != null).SelectMany(p => p.Value.AvatarInfos);
            var legendaries = allavatars.Where(p => p.Rarity == RarityTier.Legendary);
            var rares = allavatars.Where(p => p.Rarity == RarityTier.Rare);
            var commons = allavatars.Where(p => p.Rarity == RarityTier.Common);
            Log.Information("Currently there are a total of {count} Avatars remaining from Season 1!", allavatars.Count());
            Log.Information("With a total of {count1} Legendary, {count2} Rare and {count3} Common Avatars!", legendaries.Count(), rares.Count(), commons.Count());

            int index = 0;
            int[,] occurances = new int[11,6];
            foreach(var legendary in legendaries)
            {
                for (int i = 0; i < legendary.Components.Count; i++)
                {
                    AvatarComponent? component = legendary.Components[i];
                    occurances[i, component.Variation]++;
                }
            }

            Log.Information("Occurances: {@Occurances}", ArrayToDictionary(occurances));
            List<(double, AvatarInfo)> list = new List<(double, AvatarInfo)>();
            foreach(var avatar in allavatars)
            {
                var scarcity = CalculateScarcityScore(occurances, avatar.Components);
                list.Add((scarcity, avatar));
            }

            foreach(var scareAvatars in list.OrderBy(p => p.Item1).Take(100))
            {
                Log.Information("{score} = {id} Price: {price}", scareAvatars.Item1, scareAvatars.Item2.Id, scareAvatars.Item2.MarketPrice);
            }
        }

        public static void ReadAvatarsJson2()
        {
            var avatarGame = JsonSerializer.Deserialize<AvatarGame>(File.ReadAllText("avatar_game.json"));
            var avatarOwner1 = avatarGame.Players.Where(p => p.Value != null && p.Value.AvatarInfos != null && p.Value.AvatarInfos.Any(p => p.SeasonId == 1));
            var avatarOwner2 = avatarGame.Players.Where(p => p.Value != null && p.Value.AvatarInfos != null && p.Value.AvatarInfos.Any(p => p.SeasonId == 2));
            var avatarOwnerAll = avatarGame.Players.Where(p => p.Value != null && p.Value.AvatarInfos != null && p.Value.AvatarInfos.Any());
            Log.Information("Avatar Owners Season 1 = {count}", avatarOwner1.Count());
            Log.Information("Avatar Owners Season 2 = {count}", avatarOwner2.Count());
            Log.Information("Avatar Owners Season All = {count}", avatarOwnerAll.Count());
            var totalPlayers1 = avatarGame.Players.Where(p => p.Value != null && p.Value.SeasonInfos.TryGetValue(1, out PlayerSeasonInfo seasonInfo) && seasonInfo != null && (seasonInfo.Bought > 0 || seasonInfo.Sold > 0 || seasonInfo.Mint > 0 || seasonInfo.Forge > 0));
            var totalPlayers2 = avatarGame.Players.Where(p => p.Value != null && p.Value.SeasonInfos.TryGetValue(2, out PlayerSeasonInfo seasonInfo) && seasonInfo != null && (seasonInfo.Bought > 0 || seasonInfo.Sold > 0 || seasonInfo.Mint > 0 || seasonInfo.Forge > 0));
            var totalPlayersAll = avatarGame.Players.Where(p => p.Value != null && p.Value.SeasonInfos.Values.Any(p => p.Bought > 0 || p.Sold > 0 || p.Mint > 0 || p.Forge > 0));

            Log.Information("Players Season 1 = {count}", totalPlayers1.Count());
            Log.Information("Players Season 2 = {count}", totalPlayers2.Count());
            Log.Information("Players Season All = {count}", totalPlayersAll.Count());
        }

        public static void ReadAvatarsJson3()
        {
            var accounts = File.ReadAllLines("latam_accounts.txt").Select(p => p.Trim()).ToArray();
            var avatarGame = JsonSerializer.Deserialize<AvatarGame>(File.ReadAllText("avatar_game.json"));
            Log.Information("Total Accounts paid for {count}!", accounts.Count());
            var avatarOwner1 = avatarGame.Players.Where(p => p.Value != null && accounts.Contains(p.Key) && p.Value.AvatarInfos != null && p.Value.AvatarInfos.Any(p => p.SeasonId == 1));
            var avatarOwner2 = avatarGame.Players.Where(p => p.Value != null && accounts.Contains(p.Key) && p.Value.AvatarInfos != null && p.Value.AvatarInfos.Any(p => p.SeasonId == 2));
            var avatarOwnerAll = avatarGame.Players.Where(p => p.Value != null && accounts.Contains(p.Key) && p.Value.AvatarInfos != null && p.Value.AvatarInfos.Any());
            Log.Information("Avatar Owners Season 1 = {count}", avatarOwner1.Count());
            Log.Information("Avatar Owners Season 2 = {count}", avatarOwner2.Count());
            Log.Information("Avatar Owners Season All = {count}", avatarOwnerAll.Count());
            var totalPlayers1 = avatarGame.Players.Where(p => p.Value != null && accounts.Contains(p.Key) && p.Value.SeasonInfos.TryGetValue(1, out PlayerSeasonInfo seasonInfo) && seasonInfo != null && (seasonInfo.Bought > 0 || seasonInfo.Sold > 0 || seasonInfo.Mint > 0 || seasonInfo.Forge > 0));
            var totalPlayers2 = avatarGame.Players.Where(p => p.Value != null && accounts.Contains(p.Key) && p.Value.SeasonInfos.TryGetValue(2, out PlayerSeasonInfo seasonInfo) && seasonInfo != null && (seasonInfo.Bought > 0 || seasonInfo.Sold > 0 || seasonInfo.Mint > 0 || seasonInfo.Forge > 0));
            var totalPlayersAll = avatarGame.Players.Where(p => p.Value != null && accounts.Contains(p.Key) && p.Value.SeasonInfos.Values.Any(p => p.Bought > 0 || p.Sold > 0 || p.Mint > 0 || p.Forge > 0));

            Log.Information("Players Season 1 = {count}", totalPlayers1.Count());
            Log.Information("Players Season 2 = {count}", totalPlayers2.Count());
            Log.Information("Players Season All = {count}", totalPlayersAll.Count());
        }

        public static void DataQueries()
        {
            var avatarGame = JsonSerializer.Deserialize<AvatarGame>(File.ReadAllText("avatar_game.json"));
            Log.Information("Total Freemints {count}!", avatarGame.Players.Sum(p => p.Value.FreeMints));

            /** free mints            */
            var inactivePlayers = avatarGame.Players
                .Where(p => p.Value.SeasonInfos == null || p.Value.SeasonInfos.Count == 0 || !p.Value.SeasonInfos.Any(p => p.Value.Sold > 0 || p.Value.Bought > 0 || p.Value.Mint > 0 || p.Value.Forge > 0)) 
                .Select(p => new { Address = p.Key, Player = p.Value })
                .ToList();

            var totalFreemintsInactive = inactivePlayers.Sum(p => p.Player.FreeMints);
            Log.Information("Total Inactive Players Freemints {count}!", inactivePlayers.Sum(p => p.Player.FreeMints));
            foreach (var inactivePlayer in inactivePlayers.OrderByDescending(p => p.Player.FreeMints).Take(100))
            {
                Log.Information("{account} Freemints {count} [{perc}]!", inactivePlayer.Address, inactivePlayer.Player.FreeMints, ((double) inactivePlayer.Player.FreeMints / totalFreemintsInactive).ToString("P2"));
            }

            /** bajuberges            */
            var allWrappedAvatars = avatarGame.Players.Where(p => p.Value.AvatarInfos != null).Select(p => new { Address = p.Key, Avatars = p.Value.AvatarInfos.Where(q => q.SeasonId == 2).Select(t => new WrappedAvatar(new AAA.Season2.Avatar(Substrate.NetApi.Utils.HexToByteArray(t.Id), Convert.ToInt32(t.SeasonId), Convert.ToInt32(t.Souls), t.Dna)))});

            var bajubaergesHolders = allWrappedAvatars.Where(p => p.Avatars.Any(q => q.ItemType == ItemType.Pet && q.ItemSubType == (HexType)PetItemType.Egg && q.CustomType2 == 0));

            foreach (var baj in bajubaergesHolders)
            {
                var bajuBerges = baj.Avatars.Where(q => q.ItemType == ItemType.Pet && q.ItemSubType == (HexType)PetItemType.Egg && q.CustomType2 == 0);
                Log.Information("{address} BajuBerge Holder", baj.Address);
                foreach(var baju in bajuBerges)
                {
                    Log.Information("- {address} Rarity:{rarity} SP:{souls}", Substrate.NetApi.Utils.Bytes2HexString(baju.Avatar.Id), baju.RarityType, baju.Avatar.SoulPoints);
                }
            }


            //var specialAvatarId = "0x5136BF322667BAAA710EC8DAB6AE6A39D0749004754DE609EC6F51FACF3296D2";
            //var specialAvatar = avatarInfoList.Where(p => Utils.HexToByteArray(p.Item1.Id).SequenceEqual(Utils.HexToByteArray(specialAvatarId)));

            /** Query Soulpoints
            var account = "bUMBURSpD9CV6rf13o54ZZ2mWR6KgvDte6b9DKBXNyrSC58wm";
            var soulPoints = avatarGame.Players.Where(p => p.Key == account && p.Value.AvatarInfos != null).Select(p => p.Value.AvatarInfos.Where(q => q.SeasonId == 2).Sum(t => t.Souls)).Sum();
            Log.Information("Account: {soul}", soulPoints);
            */

            Log.Information("My Avatars!");
            string[] accounts = new string[] { 
                "bUPQUVaaWAKZFqb56vjt6iyN9UinsBB17xVTUcgSbQDvbJCyZ",
                "bUNBy2dWPCyUKg5PHHV4H87o2xBF7CqtWb1SzF1XHRMEejKfJ",
                "bUHvbd83cAZpEb1mUwRzS5mghBEAnbYk1urvaxCyhHiUB3SKc",
                "bUMRN3fDNrEiwpmGkwiY6wBemRZSojz71LEnQeGC42vFgBvqx",
                "bUJvj6yKvqstG43xFcZW34VduDZtFA1cH4CTdRTJDL6PpY7NN",
                "bUNVidJTVVXukC6Lb7j8SvxgzpHgHFkDkUXvzMCSddMPXf8pb",
                "bUNmwkCGmSExpnTtkqXZfkzNE1uBUeVvAkXpLG1mngPcRX6jY",
            };

            var wrappedAvatars = avatarGame.Players.Where(p => p.Value.AvatarInfos != null).SelectMany(p => p.Value.AvatarInfos.Where(q => q.SeasonId == 2)
                .Select(t => new
                {
                    Owner = p.Key,
                    Price = t.MarketPrice,
                    WrappedAvatar = new WrappedAvatar(new AAA.Season2.Avatar(Substrate.NetApi.Utils.HexToByteArray(t.Id), Convert.ToInt32(t.SeasonId), Convert.ToInt32(t.Souls), t.Dna))
                }));

            for (int i = 0; i < accounts.Length; i++)
            {
                var account = accounts[i];
                var topTierEggs = wrappedAvatars.Where(p => p.Owner == account && p.WrappedAvatar.ItemType == ItemType.Pet).OrderByDescending(p => p.WrappedAvatar.RarityType).ThenBy(p => p.WrappedAvatar.CustomType2);
                foreach (var topTier in topTierEggs.Where(p => p.WrappedAvatar.ItemSubType is ((HexType)PetItemType.Egg) || p.WrappedAvatar.ItemSubType is ((HexType)PetItemType.Pet)))
                {
                    var first = topTier.WrappedAvatar.Avatar.Dna.Read(21, ByteType.Low) + 1;
                    var secon = topTier.WrappedAvatar.Avatar.Dna.Read(22, ByteType.Low) + 1;
                    var eight = topTier.WrappedAvatar.Avatar.Dna.Read(28, ByteType.Low) + 1;
                    int background = ((first * secon) + eight) % 36;
                    Log.Information("{nr}: {id} {dna} {value} {type} {rarity} SP:{souls} BG:{background}", i, Substrate.NetApi.Utils.Bytes2HexString(topTier.WrappedAvatar.Avatar.Id), Convert.ToString(topTier.WrappedAvatar.CustomType2, 2).PadLeft(8, '0'), topTier.WrappedAvatar.Avatar.Dna.Read(4, 1)[0], (Ajuna.AAA.Season2.PetItemType)topTier.WrappedAvatar.ItemSubType, topTier.WrappedAvatar.RarityType, topTier.WrappedAvatar.Avatar.SoulPoints, background);
                }
            }

            Log.Information("Market Eggs Avatars!");
            foreach (var topTier in wrappedAvatars.Where(p => p.Price != null && p.Price > 0 && p.WrappedAvatar.ItemSubType is ((HexType)PetItemType.Egg)).OrderBy(p => p.Price.Value / p.WrappedAvatar.Avatar.SoulPoints)) //(p => p.WrappedAvatar.CustomType2))
            {
                var first = topTier.WrappedAvatar.Avatar.Dna.Read(21, ByteType.Low) + 1;
                var secon = topTier.WrappedAvatar.Avatar.Dna.Read(22, ByteType.Low) + 1;
                var eight = topTier.WrappedAvatar.Avatar.Dna.Read(28, ByteType.Low) + 1;
                int background = ((first * secon) + eight) % 36;
                var listPetType = AvatarTools.BitsToEnums<PetType>(topTier.WrappedAvatar.CustomType2);
                //if (listPetType.Contains(PetType.BigHybrid)) {
                if (background == 34) { 
                    Log.Information("{id} {dna} {value} {type} {rarity} SP:{souls} BG:{background} Price:{price} SP-RATIO:{ratio}", Substrate.NetApi.Utils.Bytes2HexString(topTier.WrappedAvatar.Avatar.Id), Convert.ToString(topTier.WrappedAvatar.CustomType2, 2).PadLeft(8, '0'), topTier.WrappedAvatar.Avatar.Dna.Read(4, 1)[0], (PetItemType)topTier.WrappedAvatar.ItemSubType, topTier.WrappedAvatar.RarityType, topTier.WrappedAvatar.Avatar.SoulPoints, background, topTier.Price, Math.Round(topTier.Price.Value / topTier.WrappedAvatar.Avatar.SoulPoints, 2));
                }
            }

            /** Query market prices
            var avatarInfoList = new List<(AvatarInfo, string)>();
            foreach (var avatarInfos in avatarGame.Players.Where(p => p.Value.AvatarInfos != null
            //&& p.Key == "bUMBURSpD9CV6rf13o54ZZ2mWR6KgvDte6b9DKBXNyrSC58wm"
            ))
            {
                avatarInfos.Value.AvatarInfos.ForEach(p => avatarInfoList.Add((p, avatarInfos.Key)));
            }
            foreach (var avatar in avatarInfoList
                .Where(p => 1 == 1 
                && p.Item1.MarketPrice  > 0
                && p.Item1.Rarity == RarityTier.Legendary
                //&& p.Item1.Souls < 20
                //&& p.Item1.Components[1].Variation == 2
                //&& p.Item1.Components[2].Variation == 1
                //&& p.Item1.Components[9].Variation == 1
                //&& specialAvatar.First().Item1.Rarity == p.Item1.Rarity && AvatarInfo.Match(specialAvatar.First().Item1, p.Item1, out _)
                )
                //.OrderBy(p => p.Souls)
                .OrderBy(p => p.Item1.MarketPrice)
                .Take(50))
            {
                Log.Information("Avatar SP {0} {1} = {2} ? Market {3} => Owner {4}", avatar.Item1.Souls, avatar.Item1.Rarity, avatar.Item1.Id, avatar.Item1.MarketPrice, avatar.Item2);
            }
            */

            /** Find Matching Avatar
            var id = "0x1E7B4B3D2D0D2BE6A570A79CFC27BE0129317E19E1ECCBB34515B4FFC0B3FE47";
            var result = avatarInfoList.First(p => p.Item1.Id == id);

            Log.Information("Matching for {id}", id); 
            var listOFMatches = avatarInfoList.Where(p => p.Item1.Id != result.Item1.Id && p.Item1.SeasonId == result.Item1.SeasonId && p.Item1.MarketPrice > 0 && AvatarInfo.Match(result.Item1, p.Item1, out _) && result.Item1.Dna.Substring(1,10) == p.Item1.Dna.Substring(1, 10));

            foreach (var avatar in listOFMatches.OrderBy(p => p.Item1.MarketPrice).Select(p => p.Item1))
            {
                Log.Information("Avatar SP {0} = {1} ? Market {2}", avatar.Souls, avatar.Id, avatar.MarketPrice);
            }
            */
        }

        public static void ReadSeasonHistoryJsons()
        {

            var fileName = "blockHashDict.json";
            var avatarGameName = "avatar_game_<hash>.json";
            if (!File.Exists(fileName))
            {
                Log.Warning("No blockhash file available,");
                return;
            }
            var records = new List<string>();
            records.Add("key;SumFreeMints;TreasuryS2;totalPlayers;activePlayer;totalMints;totalForges;totalBought;totalSold;rahiFM");
            var blockHashDict = JsonSerializer.Deserialize<Dictionary<int, string>>(File.ReadAllText(fileName));
            foreach (var key in blockHashDict.Keys.OrderBy(p => p))
            {
                var blockHash = blockHashDict[key];
                var avatarGameFile = avatarGameName.Replace("<hash>", blockHash.Substring(2).ToLower());
                AvatarGame? avatarGame;
                Log.Information($"{key} Reading! {avatarGameFile}");
                if (!File.Exists(avatarGameFile))
                {
                    Log.Warning($"Can't progress past this block, as there are no data!");
                    break;
                }
                avatarGame = JsonSerializer.Deserialize<AvatarGame>(File.ReadAllText(avatarGameFile));
                if (avatarGame == null)
                {
                    Log.Warning($"No avatar game found or available for {key} pulling!");
                    break;
                }

                var totalPlayers = avatarGame.Players.Count(p => p.Value.AvatarInfos.Any(p => p.SeasonId == 2) || p.Value.SeasonInfos.Any(p => p.Key == 2 && (p.Value.Mint > 0 || p.Value.Forge > 0 || p.Value.Bought > 0 || p.Value.Sold > 0)));
                var activePlayer = avatarGame.Players.Count(p => p.Value.SeasonInfos.Any(p => p.Key == 2 && p.Value.Mint > 200 && p.Value.Forge > 100));

                var totalMints = avatarGame.Players.Sum(p => p.Value.SeasonInfos.Where(p => p.Key == 2).Sum(p => p.Value.Mint));
                var totalForges = avatarGame.Players.Sum(p => p.Value.SeasonInfos.Where(p => p.Key == 2).Sum(p => p.Value.Forge));
                var totalBought = avatarGame.Players.Sum(p => p.Value.SeasonInfos.Where(p => p.Key == 2).Sum(p => p.Value.Bought));
                var totalSold = avatarGame.Players.Sum(p => p.Value.SeasonInfos.Where(p => p.Key == 2).Sum(p => p.Value.Sold));
                //var account = "bUKnuFEbe2XgMrxNbfRq77m8WdVfDRJjNwvnmYVgCdW89jXSD"; // rahi
                var rahiFM = avatarGame.Players.Where(p => p.Key == "bULYxsXwt3v1at4CLLpKmdY9M8rKLurNiXsth9bj59HiAaUuC").Sum(p => p.Value.AvatarInfos.Where(p => p.SeasonId == 2).Select(p => new WrappedAvatar(new AAA.Season2.Avatar(Substrate.NetApi.Utils.HexToByteArray(p.Id), Convert.ToInt32(p.SeasonId), Convert.ToInt32(p.Souls), p.Dna))).Count(p => p.ItemType == ItemType.Pet && p.ItemSubType == (HexType) PetItemType.Pet && p.RarityType == RarityType.Legendary));

                var record = $"{key};{avatarGame.Players.Sum(p => p.Value.FreeMints)};{avatarGame.SeasonTreasuries[2]};{totalPlayers};{activePlayer};{totalMints};{totalForges};{totalBought};{totalSold};{rahiFM}";
                records.Add(record);


                Log.Information(record);
            }

            File.WriteAllLines("statistics-s2.csv", records);
        }

        private static double CalculateScarcityScore(int[,] occurances, List<AvatarComponent> components)
        {
            double score = 0;
            for (int i = 0; i < components.Count; i++)
            {
                int rowSum = Enumerable.Range(0, occurances.GetLength(1)).Sum(j => occurances[i, j]);
                score += (double) occurances[i, components[i].Variation] / rowSum;
            }
            return score;
        }

        public static Dictionary<string, double> ArrayToDictionary(int[,] array)
        {
            Log.Information("{count}", array.Length);
            Log.Information("{count}", array.LongLength);
            Log.Information("{count}", array.GetUpperBound(0));
            Dictionary<string, double> result = new Dictionary<string, double>();
            for (int i = 0; i < array.GetLength(0); i++)
            {
                var key1 = "";
                switch(i)
                {
                    case 0: key1 = "Background"; break;
                    case 1: key1 = "BackHand"; break;
                    case 2: key1 = "Color"; break;
                    case 3: key1 = "Tool"; break;
                    case 4: key1 = "Breast"; break;
                    case 5: key1 = "Ear"; break;
                    case 6: key1 = "Hair"; break;
                    case 7: key1 = "Face"; break;
                    case 8: key1 = "Horn"; break;
                    case 9: key1 = "FrontHand"; break;
                    case 10: key1 = "Force"; break;
                }

                int rowSum = Enumerable.Range(0, array.GetLength(1)).Sum(j => array[i, j]);

                for (int j = 0; j < array.GetLength(1); j++)
                {
                    var key2 = "";
                    switch (j)
                    {
                        case 0: key2 = "U"; break;
                        case 1: key2 = "T"; break;
                        case 2: key2 = "K"; break;
                        case 3: key2 = "O"; break;
                        case 4: key2 = "7"; break;
                        case 5: key2 = "C"; break;
                    }
                    var key = $"({key1}, {key2})";
                    var value = Math.Round((double)array[i, j] * 100 / rowSum, 2);
                    result[key] = value;
                    Log.Information("{key} -> {value} %", key.PadLeft(20), value.ToString().PadRight(5, '0').PadLeft(10));
                }
            }
            return result;
        }
    }
}