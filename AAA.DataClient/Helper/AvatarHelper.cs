using Substrate.Bajun.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.avatar;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.avatar.rarity_tier;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.config;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.fee;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.season;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using System.Linq;

namespace Ajuna.Integration.Helper
{
    public static partial class AvatarHelper
    {
        private static MintConfig SetDefault(this MintConfig result, GlobalConfigSetting setting)
        {
            result.Open = new Bool();
            result.Open.Create(setting.MintOpen);

            result.Cooldown = new U32();
            result.Cooldown.Create(setting.MintCooldown);

            result.FreeMintFeeMultiplier = new U16();
            result.FreeMintFeeMultiplier.Create(setting.MintFreeMintFeeMultiplier);

            return result;
        }

        private static ForgeConfig SetDefault(this ForgeConfig result, GlobalConfigSetting setting)
        {
            result.Open = new Bool();
            result.Open.Create(setting.ForgeOpen);

            return result;
        }

        private static TransferConfig SetDefault(this TransferConfig result, GlobalConfigSetting setting)
        {
            result.Open = new Bool();
            result.Open.Create(setting.TransferOpen);

            result.FreeMintTransferFee = new U16();
            result.FreeMintTransferFee.Create(setting.FreeMintTransferFee);

            result.MinFreeMintTransfer = new U16();
            result.MinFreeMintTransfer.Create(setting.MinFreeMintTransfer);

            return result;
        }

        private static TradeConfig SetDefault(this TradeConfig result, GlobalConfigSetting setting)
        {
            result.Open = new Bool();
            result.Open.Create(setting.TradeOpen);

            return result;
        }

        private static NftTransferConfig SetDefault(this NftTransferConfig result, GlobalConfigSetting setting)
        {
            result.Open = new Bool();
            result.Open.Create(setting.NftTransferOpen);

            return result;
        }

        public static GlobalConfig SetDefault(this GlobalConfig result, GlobalConfigSetting setting)
        {
            result.Mint = new MintConfig().SetDefault(setting);
            result.Forge = new ForgeConfig().SetDefault(setting);
            result.Transfer = new TransferConfig().SetDefault(setting);
            result.Trade = new TradeConfig().SetDefault(setting);
            result.NftTransfer = new NftTransferConfig().SetDefault(setting);

            return result;
        }

        public static MintFees SetDefault(this MintFees result, SeasonSetting setting)
        {
            result.One = new U128();
            result.One.Create(setting.MintFeesOne);

            result.Three = new U128();
            result.Three.Create(setting.MintFeesThree);

            result.Six = new U128();
            result.Six.Create(setting.MintFeesSix);

            return result;
        }

        private static Fee SetDefault(this Fee result, SeasonSetting setting)
        {
            result.Mint = new MintFees().SetDefault(setting);

            result.TransferAvatar = new U128();
            result.TransferAvatar.Create(setting.TransferAvatarFee);

            result.BuyMinimum = new U128();
            result.BuyMinimum.Create(setting.BuyMinimum);

            result.BuyPercent = new U8();
            result.BuyPercent.Create(setting.BuyPercent);

            result.UpgradeStorage = new U128();
            result.UpgradeStorage.Create(setting.UpgradeStorageFee);

            result.PrepareAvatar = new U128();
            result.PrepareAvatar.Create(setting.PrepareAvatarFee);

            return result;
        }

        public static Season SetDefault(this Season season, SeasonSetting setting)
        {
            season = new Season();

            season.Name = new BoundedVecT4();
            season.Name.Value = new BaseVec<U8>();
            season.Name.Value.Create(setting.Name.ToU8Array());

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

            season.Tiers = new BoundedVecT6();
            season.Tiers.Value = new BaseVec<EnumRarityTier>();
            season.Tiers.Value.Create(setting.RarityTiers
                .Select(p =>
                {
                    var enumRarityTier = new EnumRarityTier();
                    enumRarityTier.Create(p);
                    return enumRarityTier;
                }).ToArray());

            season.SingleMintProbs = new BoundedVecT7();
            season.SingleMintProbs.Value = new BaseVec<U8>();
            season.SingleMintProbs.Value.Create(setting.SingleMintProbs.ToU8Array());

            season.BatchMintProbs = new BoundedVecT7();

            season.BatchMintProbs.Value = new BaseVec<U8>();
            season.BatchMintProbs.Value.Create(setting.BatchMintProbs.ToU8Array());

            season.BaseProb = new U8();
            season.BaseProb.Create(setting.BaseProb);

            season.PerPeriod = new U32();
            season.PerPeriod.Create(setting.PerPeriod);

            season.Periods = new U16();
            season.Periods.Create(setting.Periods);

            season.TradeFilters = new BoundedVecT8();
            season.TradeFilters.Value = new BaseVec<U32>();
            season.TradeFilters.Value.Create(setting.TradeFilters.ToU32Array());

            season.Fee = new Fee().SetDefault(setting);

            season.MintLogic = new EnumLogicGeneration();
            season.MintLogic.Create(setting.MintLogic);

            season.ForgeLogic = new EnumLogicGeneration();
            season.ForgeLogic.Create(setting.ForgeLogic);

            return season;
        }
    }
}