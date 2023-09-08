using Ajuna.TheOracle.DataClient.Model.Avatar;
using Ajuna.TheOracle.DataClient.Model.Avatar.Ajuna.Integration.Model.Avatar;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.avatar.rarity_tier;

namespace AAA.DataClient.Game
{
    public class AvatarStats
    {
        public int AttackRating { get; internal set; }

        public int ForceDamage { get; internal set; }

        public ForceType ForceType { get; internal set; }

        public int DefenseRating { get; internal set; }

        public int HitPoints { get; internal set; }

        public AvatarStats(RarityTier rarityTier, List<AvatarComponent> genetic)
        {
            CalculateStats(rarityTier, genetic);
        }

        public void CalculateStats(RarityTier rarityTier, List<AvatarComponent> genetic)
        {
            var occurrencesSorted = genetic
                .GroupBy(p => p.Variation)
                .ToDictionary(k => k.Key, v => v.Count())
                .OrderByDescending(p => p.Value)
                .ToList();

            var varHighestOccurance = genetic.GroupBy(p => p.Variation).Max(g => g.Count());
            var varHighestOccuranceCount = genetic.Count(p => p.Variation == varHighestOccurance);

            var varFollowOccurance = 0;
            var varFollowOccuranceCount = 0;

            var currentVariation = 0;
            var currentVariationCount = 0;
            for (int i = 0; i < genetic.Count; i++)
            {
                if (currentVariation == genetic[i].Variation)
                {
                    currentVariationCount++;
                }
                else
                {
                    currentVariation = genetic[i].Variation;
                    currentVariationCount = 1;
                }

                if (varFollowOccuranceCount < currentVariationCount)
                {
                    varFollowOccurance = currentVariation;
                    varFollowOccuranceCount = currentVariationCount;
                }
            }

            var variationsCount = genetic.Select(p => p.Variation).Distinct().Count();

            var force = genetic.Select(p => p.Variation).Last();
            var forceOccuranceCount = genetic.Count(p => p.Variation == force);

            AttackRating = varFollowOccuranceCount + (int)rarityTier;
            DefenseRating = variationsCount + (int)rarityTier;

            ForceType = (ForceType)force;
            ForceDamage = forceOccuranceCount + (force == varFollowOccurance ? varFollowOccuranceCount : 0) + (int)rarityTier / 2;
            HitPoints = (genetic.Count - varHighestOccuranceCount) + 2 + (int)rarityTier * 2;
        }
    }

    public class MogwaiType
    {
        public ForceCategory ForceCategory { get; set; }
        public ForceType ForceType { get; set; }
        public MogwaiAttribute MogwaiAttribute { get; set; }
        public string Description { get; set; }
        public string Ability { get; set; }
    }

    public class MogwaiGame
    {

            public Dictionary<ForceType, MogwaiType> MogwaiTypes { get; set; }

        public MogwaiGame()
        {
            MogwaiTypes = new Dictionary<ForceType, MogwaiType>
            {
                {
                    ForceType.Kinetic,
                    new MogwaiType
                    {
                        ForceCategory = ForceCategory.Physical,
                        ForceType = ForceType.Kinetic,
                        MogwaiAttribute = MogwaiAttribute.Strength,
                        Description = "Fast, aggressive, weak hits, multiple hits, cheap AP costs",
                        Ability = "Reduce the cost of all current cards in each player's hand by 2"
                    }
                },
                {
                    ForceType.Solar,
                    new MogwaiType
                    {
                        ForceCategory = ForceCategory.Physical,
                        ForceType = ForceType.Solar,
                        MogwaiAttribute = MogwaiAttribute.Agility,
                        Description = "Equipment, control, blinding, high-impact abilities",
                        Ability = "Inflict Exposed 3 on both active Mogwai"
                    }
                },
                {
                    ForceType.Thermal,
                    new MogwaiType
                    {
                        ForceCategory = ForceCategory.Physical,
                        ForceType = ForceType.Thermal,
                        MogwaiAttribute = MogwaiAttribute.Body,
                        Description = "Burn, damage-over-time, outlast, explosive",
                        Ability = "Inflict Burn 4 on both active Mogwai"
                    }
                },
                {
                    ForceType.Astral,
                    new MogwaiType
                    {
                        ForceCategory = ForceCategory.Spiritual,
                        ForceType = ForceType.Astral,
                        MogwaiAttribute = MogwaiAttribute.Logic,
                        Description = "Heavy damage, spirits, late-game, high SP costs",
                        Ability = "3 random cards in each player's deck gain Overload"
                    }
                },
                {
                    ForceType.Dream,
                    new MogwaiType
                    {
                        ForceCategory = ForceCategory.Spiritual,
                        ForceType = ForceType.Dream,
                        MogwaiAttribute = MogwaiAttribute.Willpower,
                        Description = "Spiritual ailments, control, unique effects",
                        Ability = "If you have no cards in hand, receive Frenzy 10"
                    }
                },
                {
                    ForceType.Empathy,
                    new MogwaiType
                    {
                        ForceCategory = ForceCategory.Spiritual,
                        ForceType = ForceType.Empathy,
                        MogwaiAttribute = MogwaiAttribute.Charisma,
                        Description = "Support, utilize backup Mogwai, healing",
                        Ability = "Disarm all Equipment on both active Mogwai"
                    }
                }
            };
        }
    }
}