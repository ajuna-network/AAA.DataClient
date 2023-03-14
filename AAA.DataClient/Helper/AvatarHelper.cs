using System.Linq;
using Ajuna.NetApi.Model.Types.Base;
using Ajuna.NetApi.Model.Types.Primitive;
using Bajun.Network.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.config;
using Bajun.Network.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.season;
using Bajun.Network.NET.NetApiExt.Generated.Model.sp_core.bounded.bounded_vec;

namespace Ajuna.Integration.Helper
{
    public static partial class AvatarHelper
    {
        public static MintFees SetDefault(this MintFees result, GlobalConfigSetting setting)
        {
            result.One = new U128();
            result.One.Create(setting.MintFeesOne);

            result.Three = new U128();
            result.Three.Create(setting.MintFeesThree);

            result.Six = new U128();
            result.Six.Create(setting.MintFeesSix);

            return result;
        }

        private static MintConfig SetDefault(this MintConfig result, GlobalConfigSetting setting)
        {
            result.Open = new Bool();
            result.Open.Create(setting.MintOpen);

            result.Fees = new MintFees().SetDefault(setting);

            result.Cooldown = new U32();
            result.Cooldown.Create(setting.MintCooldown);

            result.FreeMintFeeMultiplier = new U16();
            result.FreeMintFeeMultiplier.Create(setting.MintFreeMintFeeMultiplier);

            result.FreeMintTransferFee = new U16();
            result.FreeMintTransferFee.Create(setting.MintFreeMintTransferFee);

            result.MinFreeMintTransfer = new U16();
            result.MinFreeMintTransfer.Create(setting.MintMinFreeMintTransfer);

            return result;
        }

        private static ForgeConfig SetDefault(this ForgeConfig result, GlobalConfigSetting setting)
        {
            result.Open = new Bool();
            result.Open.Create(setting.ForgeOpen);

            return result;
        }

        private static TradeConfig SetDefault(this TradeConfig result, GlobalConfigSetting setting)
        {
            result.Open = new Bool();
            result.Open.Create(setting.TradeOpen);

            result.MinFee = new U128();
            result.MinFee.Create(setting.TradeMinFee);

            result.PercentFee = new U8();
            result.PercentFee.Create(setting.TradePercentFee);

            return result;
        }

        private static AccountConfig SetDefault(this AccountConfig result, GlobalConfigSetting setting)
        {
            result.StorageUpgradeFee = new U128();
            result.StorageUpgradeFee.Create(setting.AccountStorageUpgradeFee);

            return result;
        }

        public static GlobalConfig SetDefault(this GlobalConfig result, GlobalConfigSetting setting)
        {
            result.Mint = new MintConfig().SetDefault(setting);
            result.Forge = new ForgeConfig().SetDefault(setting);
            result.Trade = new TradeConfig().SetDefault(setting);
            result.Account = new AccountConfig().SetDefault(setting);

            return result;
        }

        public static Season SetDefault(this Season season, SeasonSetting setting)
        {
            season = new Season();

            //season.Name = new BoundedVecT1();
            season.Name = new BoundedVecT4();
            season.Name.Value = new BaseVec<U8>();
            season.Name.Value.Create(setting.Name.ToU8Array());

            //season.Description = new BoundedVecT2();
            season.Description = new BoundedVecT5();
            season.Description.Value = new BaseVec<U8>();
            season.Description.Value.Create(setting.Description.ToU8Array());

            season.EarlyStart = new U32();
            season.EarlyStart.Create(setting.EarlyStart);

            season.Start = new U32();
            season.Start.Create(setting.Start);

            season.End = new U32();
            season.End.Create(setting.End);

            season.MaxTierForges = new U32();
            season.MaxTierForges.Create(setting.MaxTierForges);

            season.MaxVariations = new U8();
            season.MaxVariations.Create(setting.MaxVariations);

            season.MaxComponents = new U8();
            season.MaxComponents.Create(setting.MaxComponents);

            season.MinSacrifices = new U8();
            season.MinSacrifices.Create(setting.MinSacrifices);

            season.MaxSacrifices = new U8();
            season.MaxSacrifices.Create(setting.MaxSacrifices);

            //season.Tiers = new BoundedVecT3();
            season.Tiers = new BoundedVecT6();
            season.Tiers.Value = new BaseVec<EnumRarityTier>();
            season.Tiers.Value.Create(setting.RarityTiers
                .Select(p =>
                {
                    var enumRarityTier = new EnumRarityTier();
                    enumRarityTier.Create(p);
                    return enumRarityTier;
                }).ToArray());

            //season.SingleMintProbs = new BoundedVecT4();
            season.SingleMintProbs = new BoundedVecT7();
            season.SingleMintProbs.Value = new BaseVec<U8>();
            season.SingleMintProbs.Value.Create(setting.SingleMintProbs.ToU8Array());

            //season.BatchMintProbs = new BoundedVecT4();
            season.BatchMintProbs = new BoundedVecT7();
            season.BatchMintProbs.Value = new BaseVec<U8>();
            season.BatchMintProbs.Value.Create(setting.BatchMintProbs.ToU8Array());

            season.BaseProb = new U8();
            season.BaseProb.Create(setting.BaseProb);

            season.PerPeriod = new U32();
            season.PerPeriod.Create(setting.PerPeriod);

            season.Periods = new U16();
            season.Periods.Create(setting.Periods);

            return season;
        }
    }
}