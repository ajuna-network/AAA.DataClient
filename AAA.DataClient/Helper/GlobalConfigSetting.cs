using System;

namespace Ajuna.Integration.Helper
{
    public partial class AvatarHelper
    {
        public class GlobalConfigSetting
        {
            public bool MintOpen { get; set; }
            public uint MintCooldown { get; set; } = 5;
            public ushort MintFreeMintFeeMultiplier { get; set; }

            public bool ForgeOpen { get; set; } = true;

            public bool TransferOpen { get; set; } = true;
            public byte FreeMintTransferFee { get; set; }
            public byte MinFreeMintTransfer { get; set; }

            public bool TradeOpen { get; set; }

            public bool NftTransferOpen { get; set; } = true;

            public GlobalConfigSetting()
            {
            }

            public static GlobalConfigSetting Create(NetworkType networkType)
            {
                switch (networkType)
                {
                    case NetworkType.Local:
                    case NetworkType.Test:
                        return new GlobalConfigSetting()
                        {
                            MintOpen = true,
                            MintCooldown = 5,
                            MintFreeMintFeeMultiplier = 1,

                            ForgeOpen = true,

                            TransferOpen = true,
                            FreeMintTransferFee = 2,
                            MinFreeMintTransfer = 1,

                            TradeOpen = true,

                            NftTransferOpen = true,
                        };

                    case NetworkType.Bajun:
                        throw new NotImplementedException($"Not implemented {networkType}!");

                    default:
                        return new GlobalConfigSetting();
                }
            }
        }
    }
}