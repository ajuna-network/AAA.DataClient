using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajuna.Integration.Helper;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.account;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.avatar;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.config;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.season;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.primitive_types;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.sp_core.crypto;
using Substrate.Bajun.NET.NetApiExt.Generated.Storage;
using Newtonsoft.Json.Linq;
using Serilog;
using Substrate.NetApi;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.bajun_runtime;

namespace Ajuna.Integration
{
    public partial class BajunNetwork
    {
        #region Storage

        /// <summary>
        ///
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AccountId32> GetOrganizerAsync(CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.AwesomeAvatarsStorage.Organizer(token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AccountId32> GetTreasurerAsync(U16 seasonId, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.AwesomeAvatarsStorage.Treasurer(seasonId, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<SeasonStatus> GetCurrentSeasonStatusAsync(CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.AwesomeAvatarsStorage.CurrentSeasonStatus(token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="seasonId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Season> GetSeasonsAsync(U16 seasonId, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.AwesomeAvatarsStorage.Seasons(seasonId, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<U128> GetTreasuryAsync(U16 key, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.AwesomeAvatarsStorage.Treasury(key, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GlobalConfig> GetGlobalConfigsAsync(CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.AwesomeAvatarsStorage.GlobalConfigs(token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="avatarId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<BaseTuple<AccountId32, Avatar>> GetAvatarsAsync(H256 avatarId, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.AwesomeAvatarsStorage.Avatars(avatarId, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<BoundedVecT35> GetOwnersAsync(BaseTuple<AccountId32, U16> key, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.AwesomeAvatarsStorage.Owners(key, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<BaseTuple> GetLockedAvatarsAsync(H256 key, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.AwesomeAvatarsStorage.LockedAvatars(key, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<U32> GetCollectionIdAsync(CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.AwesomeAvatarsStorage.CollectionId(token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ownerAccount"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<PlayerConfig> GetPlayerConfigsAsync(AccountId32 key, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.AwesomeAvatarsStorage.PlayerConfigs(key, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ownerAccount"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<PlayerSeasonConfig> GetPlayerSeasonConfigsAsync(BaseTuple<AccountId32, U16> key, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.AwesomeAvatarsStorage.PlayerSeasonConfigs(key, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="seasonAccount"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<SeasonInfo> GetSeasonStatsAsync(BaseTuple<U16, AccountId32> seasonAccount, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.AwesomeAvatarsStorage.SeasonStats(seasonAccount, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<U128> GetTradeAsync(BaseTuple<U16, H256> key, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.AwesomeAvatarsStorage.Trade(key, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AccountId32> GetServiceAccountAsync(CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.AwesomeAvatarsStorage.ServiceAccount(token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<BoundedVecT9> GetPreparationAsync(H256 key, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.AwesomeAvatarsStorage.Preparation(key, token);
        }

        #endregion Storage

        #region Call

        /// <summary>
        ///
        /// </summary>
        /// <param name="mintOption"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> MintAsync(MintOption mintOption, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = "Mint";

            if (!IsConnected || Account == null)
            {
                return null;
            }

            var extrinsic = AwesomeAvatarsCalls.Mint(mintOption);

            return await GenericExtrinsicAsync(Account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="leader"></param>
        /// <param name="sacrificies"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> ForgeAsync(H256 leader, BaseVec<H256> sacrificies, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = "Forge";

            if (!IsConnected || Account == null)
            {
                return null;
            }

            var extrinsic = AwesomeAvatarsCalls.Forge(leader, sacrificies);

            return await GenericExtrinsicAsync(Account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="avatarId"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> TransferAvatarAsync(AccountId32 to, H256 avatarId, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = "TransferAvatar";

            if (!IsConnected || Account == null)
            {
                return null;
            }

            var extrinsic = AwesomeAvatarsCalls.TransferAvatar(to, avatarId);

            return await GenericExtrinsicAsync(Account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="howMany"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> TransferFreeMintsAsync(Account from, AccountId32 dest, U16 howMany, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = "TransferFreeMints";

            if (!IsConnected || Account == null)
            {
                return null;
            }

            var extrinsic = AwesomeAvatarsCalls.TransferFreeMints(dest, howMany);

            return await GenericExtrinsicAsync(from, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="avatarId"></param>
        /// <param name="price"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> SetPriceAsync(H256 avatarId, BaseCom<U128> price, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = "SetPrice";

            if (!IsConnected || Account == null)
            {
                return null;
            }

            var extrinsic = AwesomeAvatarsCalls.SetPrice(avatarId, price);

            return await GenericExtrinsicAsync(Account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="avatarId"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> RemovePriceAsync(H256 avatarId, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = "RemovePrice";

            if (!IsConnected || Account == null)
            {
                return null;
            }

            var extrinsic = AwesomeAvatarsCalls.RemovePrice(avatarId);

            return await GenericExtrinsicAsync(Account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="avatarId"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> BuyAsync(H256 avatarId, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = "Buy";

            if (!IsConnected || Account == null)
            {
                return null;
            }

            var extrinsic = AwesomeAvatarsCalls.Buy(avatarId);

            return await GenericExtrinsicAsync(Account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        private string UpgradeStorageKey => "UpgradeStorage";
        public bool CanUpgradeStorage(int concurrentTasks)
            => !HasToManyConcurentTaskRunning(UpgradeStorageKey, concurrentTasks);
        /// <summary>
        ///
        /// </summary>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> UpgradeStorageAsync(int concurrentTasks, CancellationToken token)
        {
            if (!IsConnected || Account == null)
            {
                return null;
            }

            var extrinsic = AwesomeAvatarsCalls.UpgradeStorage();

            return await GenericExtrinsicAsync(Account, UpgradeStorageKey, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> SetOrganizerAsync(AccountId32 organizer, CancellationToken token)
        {
            var extrinsicType = "SetOrganizer";

            if (!IsConnected || ExtrinsicManger.Running.Any())
            {
                return null;
            }

            var accountAlice = new AccountId32();
            accountAlice.Create(Utils.GetPublicKeyFrom(Alice.Value));

            var enumCall = new EnumRuntimeCall();
            var palletCall = new Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.pallet.EnumCall();
            palletCall.Create(Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.pallet.Call.set_organizer, organizer);
            enumCall.Create(RuntimeCall.AwesomeAvatars, palletCall);
            var extrinsic = SudoCalls.Sudo(enumCall);

            return await GenericExtrinsicAsync(Alice, extrinsicType, extrinsic, 1, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seasonId"></param>
        /// <param name="treasurer"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> SetTreasurerAsync(U16 seasonId, AccountId32 treasurer, CancellationToken token)
        {
            var extrinsicType = "SetTreasurer";

            if (!IsConnected || ExtrinsicManger.Running.Any())
            {
                return null;
            }

            var extrinsic = AwesomeAvatarsCalls.SetTreasurer(seasonId, treasurer);

            return await GenericExtrinsicAsync(Alice, extrinsicType, extrinsic, 1, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seasonId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> ClaimTreasuryAsync(U16 seasonId, CancellationToken token)
        {
            var extrinsicType = "ClaimTreasury";

            if (!IsConnected || ExtrinsicManger.Running.Any())
            {
                return null;
            }

            var extrinsic = AwesomeAvatarsCalls.ClaimTreasury(seasonId);

            // TODO: Check Alice is the treasurer or if it needs to be a sudo call
            return await GenericExtrinsicAsync(Alice, extrinsicType, extrinsic, 1, token);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="seasonId"></param>
        /// <param name="season"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> SetSeasonAsync(Account organizer, U16 seasonId, Season season, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = "SetSeason";

            if (!IsConnected)
            {
                return null;
            }

            var extrinsic = AwesomeAvatarsCalls.SetSeason(seasonId, season);

            return await GenericExtrinsicAsync(organizer, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="newGlobalConfig"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> UpdateGlobalConfigAsync(Account organizer, GlobalConfig newGlobalConfig, CancellationToken token)
        {
            var extrinsicType = "UpdateGlobalConfig";

            if (!IsConnected)
            {
                return null;
            }

            var extrinsic = AwesomeAvatarsCalls.UpdateGlobalConfig(newGlobalConfig);

            return await GenericExtrinsicAsync(organizer, extrinsicType, extrinsic, 1, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="howMany"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> SetFreeMintsAsync(AccountId32 dest, U16 howMany, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = "SetFreeMints";

            if (!IsConnected || Account == null)
            {
                return null;
            }

            var extrinsic = AwesomeAvatarsCalls.SetFreeMints(dest, howMany);

            return await GenericExtrinsicAsync(Account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        #endregion Call
    }
}