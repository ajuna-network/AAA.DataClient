using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Bajun.Network.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.season;
using Bajun.Network.NET.NetApiExt.Generated.Model.primitive_types;

namespace Ajuna.TheOracle.DataClient.Model.Avatar
{

    public class AvatarInfo
    {
        public string Id { get; }

        public uint SeasonId { get; }

        public uint Souls { get; }

        public string Dna { get; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RarityTier Rarity { get; set; }

        public Force Force { get; set; }

        public int Score { get; set; }

        public double? MarketPrice { get; set; }

        public AvatarInfo(string id, uint seasonId, uint souls, string dna)
        {
            Id = id;
            SeasonId = seasonId;
            Souls = souls;
            Dna = dna;

            var components = new List<AvatarComponent>();
            for (int i = 0; i < (dna.Length / 2); i++)
            {
                var pos = i * 2;
                var rarity = (RarityTier)int.Parse(dna[pos].ToString());
                var variation = int.Parse(dna[pos + 1].ToString());
                components.Add(new AvatarComponent(i, rarity, variation));
            }

            Rarity = components.OrderBy(p => p.RarityTier).First().RarityTier;
            Force = (Force)components.Last().Variation;
            Score = components.Sum(p => (int)p.RarityTier);
            MarketPrice = null;
        }
    }
}