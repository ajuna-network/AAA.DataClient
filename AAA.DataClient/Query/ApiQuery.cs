using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AAA.DataClient.Model.Subscan;
using Newtonsoft.Json.Linq;
using Serilog;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_identity.types;
using Substrate.NetApi.Model.Rpc;

namespace AAA.DataClient.Query
{
    public static class ApiQuery
    {
        private const string ApiKey = "0ce352d3d9dc51f880a036506cb15071"; // Replace with your actual API key

        public static async Task LogOutFlowsAsync(string abrev, string[] addresses, int[] periods, bool logAll = false)
        {
            var now = DateTime.UtcNow;

            List<Transfer> transfers = new();
            foreach (var address in addresses)
            {
                var response = await GetScanTransfersAsync(address);
                transfers.AddRange(response.Data.Transfers);
            }
            foreach (var period in periods)
            {
                if (now.Subtract(transfers.Min(p => p.BlockTimestamp)).TotalHours < period)
                {
                    continue;
                }
                var outFlowPeriod = Math.Floor(transfers.Where(p => now.Subtract(p.BlockTimestamp).TotalHours < period && addresses.Contains(p.From)).Sum(p => p.Amount));
                Log.Information("{abrev} ({period}h):  out |-> {out} BAJU", abrev, period.ToString().PadLeft(2), outFlowPeriod.ToString().PadLeft(10, ' '), outFlowPeriod.ToString().PadLeft(10, ' '));
            }

            var accounts = await GetScanAccountsAsync(addresses);

            if (accounts.Data == null || accounts.Data.List == null || accounts.Data.List.Length == 0)
            {
                return;
            }

            Log.Information("{abrev}: {amount} (LOCK:{lock})", abrev, 
                accounts.Data.List.Where(p => p.Balance != null && double.TryParse(p.Balance, out _)).Sum(p => double.Parse(p.Balance)),
                accounts.Data.List.Where(p => p.Balance != null && double.TryParse(p.BalanceLock, out _)).Sum(p => double.Parse(p.BalanceLock)));
        }

        public static async Task LogInAndOutFlowsAsync(string abrev, string address, int[] periods, bool logAll = false)
        {
            var now = DateTime.UtcNow;
            var transfers = await GetScanTransfersAsync(address);

            foreach (var period in periods)
            {
                if (now.Subtract(transfers.Data.Transfers.Min(p => p.BlockTimestamp)).TotalHours < period)
                {
                    continue;
                }

                var inTransfers = transfers.Data.Transfers.Where(p => now.Subtract(p.BlockTimestamp).TotalHours < period && p.To == address).OrderBy(p => p.BlockTimestamp);
                //foreach(var inTransfer in inTransfers)
                //{
                //    Log.Information("{abrev}:  in |-> {in} BAJU ... {address}", abrev, inTransfer.Amount.ToString().PadLeft(10, ' '), inTransfer.FromAccountDisplay.Address);
                //}
                var inflowMexc24h = Math.Floor(inTransfers.Sum(p => p.Amount));
                var ouflowMexc24h = Math.Floor(transfers.Data.Transfers.Where(p => now.Subtract(p.BlockTimestamp).TotalHours < period && p.From == address).Sum(p => p.Amount));
                Log.Information("{abrev} ({period}h):  in |-> {in} BAJU ... out <-| {ou} BAJU", abrev, period.ToString().PadLeft(2), inflowMexc24h.ToString().PadLeft(10, ' '), ouflowMexc24h.ToString().PadLeft(10, ' '));
            }

            if (logAll)
            {
                foreach (var transfer in transfers.Data.Transfers)
                {
                    if (transfer.To == "bUKpjRnAgSwspa7LqpuGUy5Dyk939xTixanVwaBXWWYfoFLLm")
                    {
                        Log.Information("{abrev}: TRANSFER TO {exchange} |-> {amount} BAJU [{time}]", abrev, "MEXC".ToString().PadLeft(5), transfer.Amount.ToString().PadLeft(10, ' '), transfer.BlockTimestamp);
                    }
                    else if (transfer.To == "bUKW8vne75VyTF9DLEHFPw9MwmCTCsv9MtT3WmEHufPVD1VjC")
                    {
                        Log.Information("{abrev}: TRANSFER TO {exchange} |-> {amount} BAJU [{time}]", abrev, "GATE".ToString().PadLeft(5), transfer.Amount.ToString().PadLeft(10, ' '), transfer.BlockTimestamp);
                    }
                }
            }

            var accounts = await GetScanAccountsAsync(new string[] { address });

            if (accounts.Data == null || accounts.Data.List == null || accounts.Data.List.Length == 0)
            {
                return;
            }

            foreach (var account in accounts.Data.List)
            {
                Log.Information("{abrev}: {amount} (LOCK:{lock})", abrev, accounts.Data.List[0].Balance, accounts.Data.List[0].BalanceLock);
            }
        }

        public static async Task<Model.Subscan.ApiTransfers> GetScanTransfersAsync(string address)
        {

            var apiUrl = "https://bajun.api.subscan.io/api/v2/scan/transfers";

            int row = 100;
            int page = 0;

            string direction = "all";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-API-Key", ApiKey);

            var jsonPayload =
                $"{{" +
                $"\"row\": {row}, " +
                $"\"page\": {page}, " +
                $"\"address\": \"{address}\", " +
                $"\"direction\": \"{direction}\"" +
                $"}}";

            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var responseStr = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<AAA.DataClient.Model.Subscan.ApiTransfers>(responseStr, jsonSerializerOptions);
            }
            else
            {
                throw new NotSupportedException($"Error: {response.ReasonPhrase}");
            }
        }

        private static async Task<Model.Subscan.ApiAccounts> GetScanAccountsAsync(string[] addresses)
        {

            var apiUrl = "https://bajun.api.subscan.io/api/v2/scan/accounts";

            int row = 100;
            int page = 0;

            string direction = "all";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-API-Key", ApiKey);

            var jsonPayload =
                $"{{" +
                $"\"row\": {row}, " +
                $"\"page\": {page}, " +
                $"\"address\": [\"{string.Join("\", \"", addresses)}\"] " +
                $"}}";

            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var responseStr = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<AAA.DataClient.Model.Subscan.ApiAccounts>(responseStr, jsonSerializerOptions);
            }
            else
            {
                throw new NotSupportedException($"Error: {response.ReasonPhrase}");
            }
        }

        public static async Task<Model.Subscan.ApiExtrinsics> GetExtrinsicsAccountAsync(string module, string call, string address)
        {

            var apiUrl = "https://bajun.api.subscan.io/api/scan/extrinsics";

            int row = 100;
            int page = 0;

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-API-Key", ApiKey);

            var jsonPayload =
                $"{{" +
                $"\"row\": {row}, " +
                $"\"page\": {page}, " +
                $"\"module\": \"{module}\", " +
                $"\"call\": \"{call}\", " +
                $"\"address\": \"{address}\" " +
                $"}}";


            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };

                var responseStr = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<AAA.DataClient.Model.Subscan.ApiExtrinsics>(responseStr, jsonSerializerOptions);
            }
            else
            {
                throw new NotSupportedException($"Error: {response.ReasonPhrase}");
            }
        }

    }
}
