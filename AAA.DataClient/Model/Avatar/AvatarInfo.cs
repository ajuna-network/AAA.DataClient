using System.Text.Json.Serialization;
using Ajuna.TheOracle.DataClient.Model.Avatar;
using Ajuna.TheOracle.DataClient.Model.Avatar.Ajuna.Integration.Model.Avatar;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.avatar.rarity_tier;

namespace Ajuna.Integration.Model.Avatar
{
    public class AvatarInfo
    {
        public string Id { get; }

        public uint SeasonId { get; }

        public uint Souls { get; }

        public string Dna { get; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RarityTier Rarity { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ForceType Force { get; set; }

        [JsonIgnore]
        public int Score { get; set; }

        public double? MarketPrice { get; set; }

        [JsonIgnore]
        public List<AvatarComponent> Components { get; set; }

        public AvatarInfo(string id, uint seasonId, uint souls, string dna)
        {
            Id = id;
            SeasonId = seasonId;
            Souls = souls;
            Dna = dna;

            Components = new List<AvatarComponent>();
            if (seasonId == 1)
            {
                for (int i = 0; i < dna.Length / 2; i++)
                {
                    var pos = i * 2;
                    var rarity = (RarityTier)(int.Parse(dna[pos].ToString()) + 1);
                    var variation = int.Parse(dna[pos + 1].ToString());
                    Components.Add(new AvatarComponent(i, rarity, variation));
                }
            }
            else if (seasonId == 2)
            {
                for (int i = 21; i < 32; i++)
                {
                    var pos = i * 2;
                    var rarity = RarityTier.None;
                    if (int.TryParse(dna[pos].ToString(), out int value1)) {
                        rarity = (RarityTier)(value1 + 1);
                    }
                    var variation = 0;
                    if (int.TryParse(dna[pos + 1].ToString(), out int value2))
                    {
                        variation = value2;
                    }
                    Components.Add(new AvatarComponent(i, rarity, variation));
                }
            }

            Rarity = Components.OrderBy(p => p.RarityTier).First().RarityTier;
            Force = (ForceType)Components.Last().Variation;
            Score = Components.Sum(p => (int)p.RarityTier);
            MarketPrice = null;
        }

        public static bool Match(AvatarInfo lead, AvatarInfo entity, out List<int> matches)
        {
            matches = new List<int>();

            if (lead.Rarity != entity.Rarity)
            {
                return false;
            }

            // amount of same variations
            int mirror = 0;

            for (int i = 0; i < lead.Components.Count; i++)
            {
                // gen at position i from avatar A
                var genA = lead.Components[i];
                // gen at position i from avatar B
                var genB = entity.Components[i];

                // check if they have same rarity
                var sameRarity = genA.RarityTier == genB.RarityTier;

                // check if current gene is lower then lowest gen rarity (Rarity) or if gene already has highest rarity (prob. obsolet)
                var maxed = genA.RarityTier > lead.Rarity;

                // if same rarity and on lowest rarity check if gene is a matching candidate, imp. matching is not equal mirror
                if (sameRarity && !maxed && MatchGen(genA.Variation, genB.Variation))
                {
                    matches.Add(i);
                }

                // check if the genes are the same
                var mirrored = genA.Variation == genB.Variation;

                // if the genes are same rarity not on lowest rarity and same components, then they count as mirrored
                if (mirrored) // && sameRarity && maxed
                {
                    mirror += 1;
                }
            }

            // 0 matches = false, 1 match + 2 mirrors || 2 match + 1 mirror || 3+ matches = true
            return matches.Count > 0 && matches.Count * 2 + mirror >= 6;
        }

        private static bool MatchGen(int v1, int v2)
        {
            // neighboor indexes are counted as matching example index 0 matches with index 1 and index max
            var match = v1 - 1 == v2
                || v1 + 1 == v2
                || (v1 == 0 || v2 == 0) && v1 + v2 == 6 - 1;
            return match;
        }
    }
}