using System;
using System.Numerics;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.avatar;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.avatar.rarity_tier;

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
            public uint[] TradeFilters { get; set; }

            public BigInteger MintFeesOne { get; set; }
            public BigInteger MintFeesThree { get; set; }
            public BigInteger MintFeesSix { get; set; }

            public BigInteger TransferAvatarFee { get; internal set; }
            public BigInteger BuyMinimum { get; internal set; }
            public byte BuyPercent { get; internal set; }
            public BigInteger UpgradeStorageFee { get; internal set; }
            public BigInteger PrepareAvatarFee { get; internal set; }

            public LogicGeneration MintLogic { get; internal set; }
            public LogicGeneration ForgeLogic { get; internal set; }

            public SeasonSetting()
            {
            }

            public static SeasonSetting Create(NetworkType networkType)
            {
                switch (networkType)
                {
                    case NetworkType.Local:
                    case NetworkType.Test:
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
                            Periods = 12,

                            TradeFilters = new uint[] { },

                            MintFeesOne = 990000000000, // 0.99 BAJU
                            MintFeesThree = 2690000000000, // 2.69 BAJU
                            MintFeesSix = 4990000000000, // 4.99 BAJU

                            TransferAvatarFee = 5000000000000, // 5.00 BAJU
                            BuyMinimum = 1000000000000, // 1.00 BAJU
                            BuyPercent = 1, // 1 %
                            UpgradeStorageFee = 54900000000000, // 54.9 BAJU
                            PrepareAvatarFee = 1000000000000, // 1.00 BAJU

                            MintLogic = LogicGeneration.Second,
                            ForgeLogic = LogicGeneration.Second
                        };

                    case NetworkType.Bajun:
                        throw new NotImplementedException($"Not implemented {networkType}!");

                    default:
                        return new SeasonSetting();
                }
            }
        }
    }
}