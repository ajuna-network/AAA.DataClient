using System.Text.Json;
using Ajuna.Integration;
using Ajuna.NetApi;
using Ajuna.NetApi.Model.Types;
using Ajuna.NetApi.Model.Types.Base;
using Ajuna.TheOracle.DataClient.Model;
using Ajuna.TheOracle.DataClient.Model.Avatar;
using Bajun.Network.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.season;
using Serilog;

namespace Ajuna.TheOracle.DataClient
{
    internal class Program
    {
        // Program entry point
        public static Task Main(string[] args) => new Program().MainAsync();

        public async Task MainAsync()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Data Parser for Bajun Network!");

            await GetAvatarGameDataAsync("wss://rpc-parachain.bajun.network", CancellationToken.None);
        }

        public async Task GetAvatarGameDataAsync(string url, CancellationToken token)
        {
            await CheckAvatarsAsync(url, token);
        }

        private async Task CheckAvatarsAsync(string url, CancellationToken token)
        {
            var blockHash = await GetBlockHashAsync(url, token);
            AvatarGame avatarGame = await DataTasks.PollGameDataAsync(url, blockHash.Bytes, token);

            var avatarInfoList = new List<AvatarInfo>();
            foreach (var avatarInfos in avatarGame.Players.Values.Where(p => p.AvatarInfos != null))
            {
                avatarInfos.AvatarInfos.ForEach(p => avatarInfoList.Add(p));
            }

            foreach (var avatar in avatarInfoList.Where(p => p.Souls < 25 && p.MarketPrice > 0).OrderBy(p => p.MarketPrice))
            {
                Log.Information("Avatar SP {0} = {1} ? Market {2}", avatar.Souls, avatar.Id, avatar.MarketPrice);
            }
        }

        public async Task<Hash> GetBlockHashAsync(string url, CancellationToken token)
        {
            Account account = BajunNetwork.Alice;

            var client = new BajunNetwork(account, url);

            if (!await client.ConnectAsync(true, true, token))
            {
                return null;
            }

            var result = await client.SubstrateClient.Chain.GetBlockHashAsync(token);

            await client.DisconnectAsync();

            return result;
        }

        public async Task<Dictionary<int, string>> GetBlockHashsAsync(int startBlockPast, string url, CancellationToken token)
        {
            var fileName = "blockHashDict.json";

            var result = new Dictionary<int, string>();
            if (File.Exists(fileName))
            {
                result = JsonSerializer.Deserialize<Dictionary<int, string>>(File.ReadAllText(fileName));
            }
            Account account = BajunNetwork.Alice;

            var client = new BajunNetwork(account, url);

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
    }
}