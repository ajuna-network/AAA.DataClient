using System.Text.Json.Serialization;

namespace AAA.DataClient.Model.Subscan
{
    public class ApiTransfers : BaseApiResponse
    {
        [JsonPropertyName("data")]
        public TransferData Data { get; set; }
    }

    public class TransferData
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("transfers")]
        public Transfer[] Transfers { get; set; }
    }

    public class Transfer
    {
        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("extrinsic_index")]
        public string ExtrinsicIndex { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("block_num")]
        public int BlockNum { get; set; }

        [JsonPropertyName("block_timestamp")]
        public long BlockTimestampUnixTime { get; set; }

        [JsonIgnore]
        public DateTime BlockTimestamp
        {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(BlockTimestampUnixTime).DateTime;
            }
        }

        [JsonPropertyName("module")]
        public string Module { get; set; }

        [JsonPropertyName("amount")]
        public string AmountString { get; set; }

        [JsonIgnore]
        public double Amount
        {
            get
            {
                return double.Parse(AmountString);
            }
        }

        [JsonPropertyName("amount_v2")]
        public string AmountV2 { get; set; }

        [JsonPropertyName("usd_amount")]
        public string UsdAmount { get; set; }

        [JsonPropertyName("fee")]
        public string Fee { get; set; }

        [JsonPropertyName("nonce")]
        public int Nonce { get; set; }

        [JsonPropertyName("asset_symbol")]
        public string AssetSymbol { get; set; }

        [JsonPropertyName("asset_unique_id")]
        public string AssetUniqueId { get; set; }

        [JsonPropertyName("asset_type")]
        public string AssetType { get; set; }

        [JsonPropertyName("from_account_display")]
        public AccountDisplay FromAccountDisplay { get; set; }

        [JsonPropertyName("to_account_display")]
        public AccountDisplay ToAccountDisplay { get; set; }

        [JsonPropertyName("event_idx")]
        public int EventIdx { get; set; }
    }
}