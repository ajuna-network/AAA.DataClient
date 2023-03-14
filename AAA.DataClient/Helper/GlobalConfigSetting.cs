using System;
using System.Numerics;

namespace Ajuna.Integration.Helper
{
    public partial class AvatarHelper
    {
        public class GlobalConfigSetting
        {
            public bool MintOpen { get; set; }
            public BigInteger MintFeesOne { get; set; }
            public BigInteger MintFeesThree { get; set; }
            public BigInteger MintFeesSix { get; set; }
            public uint MintCooldown { get; set; } = 5;
            public ushort MintFreeMintFeeMultiplier { get; set; }
            public byte MintFreeMintTransferFee { get; set; }
            public byte MintMinFreeMintTransfer { get; set; }

            public bool ForgeOpen { get; set; } = true;

            public bool TradeOpen { get; set; }

            public BigInteger TradeMinFee { get; set; }

            public byte TradePercentFee { get; set; }

            public BigInteger AccountStorageUpgradeFee { get; set; }

            public GlobalConfigSetting()
            {
            }

            public static GlobalConfigSetting Create(NetworkType networkType)
            {
                switch (networkType)
                {
                    case NetworkType.Rococo:
                        return new GlobalConfigSetting()
                        {
                            // - MINTING ---------------------------------------------
                            MintOpen = true,
                            MintFeesOne = 990000000000,    // 0.99 BAJU
                            MintFeesThree = 2690000000000, // 2.69 BAJU
                            MintFeesSix = 4990000000000,   // 4.99 BAJU
                            MintCooldown = 5,              // 5 Blocks
                            MintFreeMintFeeMultiplier = 1, //
                            MintFreeMintTransferFee = 1,   // Free Mint Transfer Fee
                            MintMinFreeMintTransfer = 2,   // Minimum Free Mint Transfer
                            // - FORGING ---------------------------------------------
                            ForgeOpen = true,
                            // - TRADING ---------------------------------------------
                            TradeOpen = true,
                            TradeMinFee = 1000000000000, // 1 BAJU
                            TradePercentFee = 1, // 1%
                            // - ACCOUNT ---------------------------------------------
                            AccountStorageUpgradeFee = 59990000000000 // 39.99 = 1.99 // 59.99 = 2.99
                        };
                    // OG's get 0.005 BAJU // 30 - 60 FREE MINTS

                    case NetworkType.Bajun:
                    case NetworkType.Ajuna:
                        throw new NotImplementedException($"Not implemented {networkType}!");

                    // default settings
                    case NetworkType.Test:
                    default:
                        return new GlobalConfigSetting();
                }
            }
        }
    }
}