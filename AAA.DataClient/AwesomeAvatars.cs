using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajuna.Integration.Helper;
using Ajuna.NetApi;
using Ajuna.NetApi.Model.Types;
using Ajuna.NetApi.Model.Types.Base;
using Ajuna.NetApi.Model.Types.Primitive;
using Bajun.Network.NET.NetApiExt.Generated.Model.bajun_runtime;
using Bajun.Network.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.account;
using Bajun.Network.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.avatar;
using Bajun.Network.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.config;
using Bajun.Network.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.season;
using Bajun.Network.NET.NetApiExt.Generated.Model.primitive_types;
using Bajun.Network.NET.NetApiExt.Generated.Model.sp_core.bounded.bounded_vec;
using Bajun.Network.NET.NetApiExt.Generated.Model.sp_core.crypto;
using Bajun.Network.NET.NetApiExt.Generated.Storage;
using Newtonsoft.Json.Linq;
using Serilog;

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
        public async Task<AccountId32> GetTreasurerAsync(CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.AwesomeAvatarsStorage.Treasurer(token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<U16> GetCurrentSeasonIdAsync(CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.AwesomeAvatarsStorage.CurrentSeasonId(token);
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
        public async Task<BoundedVecT28> GetOwnersAsync(AccountId32 key, CancellationToken token)
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
        /// <param name="ownerAccount"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AccountInfo> GetAccountsAsync(AccountId32 ownerAccount, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.AwesomeAvatarsStorage.Accounts(ownerAccount, token);
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
        public async Task<U128> GetTradeAsync(H256 key, CancellationToken token)
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
        /// <param name="myMogwais"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <summary>
        ///
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<(H256, U128)>> GetTradesAsync(CancellationToken token)
        {
            return await GetTradesAsync(null, 100, token);
        }

        public async Task<List<(H256, U128)>> GetTradesAsync(H256 startKey, uint page, CancellationToken token)
        {
            if (!IsConnected)
            {
                return null;
            }

            if (page < 2 || page > 100)
            {
                throw new NotSupportedException("Page size must be in the range of 2 - 100");
            }

            var startKeyBytes = new byte[] { };
            if (startKey != null && startKey.Bytes != null)
            {
                startKeyBytes = startKey.Bytes;
            }

            var avatarTrades = new List<(H256, U128)>();

            var account32 = new AccountId32();
            account32.Create(Utils.GetPublicKeyFrom(Account.Value));

            var keyBytes = RequestGenerator.GetStorageKeyBytesHash("AwesomeAvatars", "Trade");
            var keyBytesHexString = Utils.Bytes2HexString(keyBytes, Utils.HexStringFormat.Pure).ToLower();
            var jArray = await SubstrateClient.State.GetKeysPagedAsync(keyBytes, page, startKeyBytes, token);
            if (jArray == null)
            {
                return avatarTrades;
            }

            // Except
            var avatarIds = jArray.Select(p => p.Value<string>().Replace(keyBytesHexString, "")).ToList();
            foreach (var avatarId in avatarIds)
            {
                var key = avatarId.ToH256();
                var price = await SubstrateClient.AwesomeAvatarsStorage.Trade(key, token);
                if (price == null)
                {
                    continue;
                }

                avatarTrades.Add((key, price));
            }

            return avatarTrades;
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> UpgradeStorageAsync(int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = "UpgradeStorage";

            if (!IsConnected || Account == null)
            {
                return null;
            }

            var extrinsic = AwesomeAvatarsCalls.UpgradeStorage();

            return await GenericExtrinsicAsync(Account, extrinsicType, extrinsic, concurrentTasks, token);
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
            var palletCall = new Bajun.Network.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.pallet.EnumCall();
            palletCall.Create(Bajun.Network.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.pallet.Call.set_organizer, organizer);
            enumCall.Create(RuntimeCall.AwesomeAvatars, palletCall);
            var extrinsic = SudoCalls.Sudo(enumCall);

            return await GenericExtrinsicAsync(Alice, extrinsicType, extrinsic, 1, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="treasurer"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> SetTreasurerAsync(AccountId32 treasurer, CancellationToken token)
        {
            var extrinsicType = "SetTreasurer";

            if (!IsConnected || ExtrinsicManger.Running.Any())
            {
                return null;
            }

            var extrinsic = AwesomeAvatarsCalls.SetTreasurer(treasurer);

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
        public async Task<string> IssueFreeMintsAsync(AccountId32 dest, U16 howMany, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = "IssueFreeMints";

            if (!IsConnected || Account == null)
            {
                return null;
            }

            var extrinsic = AwesomeAvatarsCalls.IssueFreeMints(dest, howMany);

            return await GenericExtrinsicAsync(Account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        #endregion Call
    }
}