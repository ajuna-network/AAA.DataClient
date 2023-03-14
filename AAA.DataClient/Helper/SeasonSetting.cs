using System;
using Bajun.Network.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.season;

namespace Ajuna.Integration.Helper
{
    public static partial class AvatarHelper
    {
        public class SeasonSetting
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public uint EarlyStart { get; set; }
            public uint Start { get; set; }
            public uint End { get; set; }
            public uint MaxTierForges { get; set; }
            public byte MaxVariations { get; set; }
            public byte MaxComponents { get; set; }
            public byte MinSacrifices { get; set; }
            public byte MaxSacrifices { get; set; }
            public RarityTier[] RarityTiers { get; set; }
            public byte[] SingleMintProbs { get; set; }
            public byte[] BatchMintProbs { get; set; }
            public byte BaseProb { get; set; }
            public byte PerPeriod { get; set; }
            public byte Periods { get; set; }

            public SeasonSetting()
            {
            }

            public static SeasonSetting Create(NetworkType networkType)
            {
                switch (networkType)
                {
                    case NetworkType.Rococo:
                        return new SeasonSetting()
                        {
                            Name = "Canary Season Alpha",
                            Description = "The chaos of the people by the people for the people!",
                            EarlyStart = 707000,
                            Start = 721400, // 2 days later
                            End = 743000, // 5 days later
                            MaxTierForges = 88,
                            MaxVariations = 6,
                            MaxComponents = 11,
                            MinSacrifices = 1,
                            MaxSacrifices = 4,
                            RarityTiers = new RarityTier[] { RarityTier.Legendary, RarityTier.Rare, RarityTier.Common },
                            SingleMintProbs = new byte[] { 95, 05 },
                            BatchMintProbs = new byte[] { 80, 20 },
                            BaseProb = 20,
                            PerPeriod = 20,
                            Periods = 12
                        };

                    case NetworkType.Bajun:
                    case NetworkType.Ajuna:
                        throw new NotImplementedException($"Not implemented {networkType}!");

                    // default settings
                    case NetworkType.Test:
                    default:
                        return new SeasonSetting();
                }
            }
        }
    }
}