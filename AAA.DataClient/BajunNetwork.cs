using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Ajuna.Integration.Client;
using Ajuna.Integration.Helper;
using Ajuna.NetApi;
using Ajuna.NetApi.Model.Types;
using Ajuna.NetApi.Model.Types.Base;
using Ajuna.NetApi.Model.Types.Primitive;
using Bajun.Network.NET.NetApiExt.Generated.Model.bajun_runtime;
using Bajun.Network.NET.NetApiExt.Generated.Model.frame_system;
using Bajun.Network.NET.NetApiExt.Generated.Model.orml_vesting;
using Bajun.Network.NET.NetApiExt.Generated.Model.sp_core.bounded.bounded_vec;
using Bajun.Network.NET.NetApiExt.Generated.Model.sp_core.crypto;

using Bajun.Network.NET.NetApiExt.Generated.Model.sp_runtime.multiaddress;
using Bajun.Network.NET.NetApiExt.Generated.Storage;
using Serilog;

namespace Ajuna.Integration
{
    public partial class BajunNetwork : BaseClient
    {
        public Account Account { get; set; }

        public NetworkType NetworkType { get; set; }

        public BajunNetwork(Account account, NetworkType networkType, string url) : base(url)
        {
            Account = account;
            NetworkType = networkType;
        }

        public BajunNetwork(Account account, string url) : base(url)
        {
            Account = account;
            NetworkType = NetworkType.Test;
        }

        public async Task<U32> GetBlocknumberAsync(CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.SystemStorage.Number(token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AccountInfo> GetAccountAsync(CancellationToken token)
        {
            if (Account == null || Account.Value == null)
            {
                Log.Warning("No account set!");
                return null;
            }

            return await SubstrateClient.SystemStorage.Account(Account.Value.ToAccountId32(), token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="account32"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AccountInfo> GetAccountAsync(Account from, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.SystemStorage.Account(from.ToAccountId32(), token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="startStorageKey"></param>
        /// <param name="page"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<List<(string, AccountId32, AccountInfo)>> GetAccountsAsync(string startStorageKey, uint page, CancellationToken token)
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
            if (startStorageKey != null)
            {
                startKeyBytes = Utils.HexToByteArray(startStorageKey);
            }

            var accountInfos = new List<(string, AccountId32, AccountInfo)>();
            var keyBytes = RequestGenerator.GetStorageKeyBytesHash("System", "Account");

            var storageKeys = await SubstrateClient.State.GetKeysPagedAsync(keyBytes, page, startKeyBytes, token);
            if (storageKeys == null)
            {
                return accountInfos;
            }

            foreach (var storageKey in storageKeys)
            {
                var storageKeyString = storageKey.ToString();
                var key = storageKeyString.Substring(storageKeyString.Length - 64);
                var pubKey = Utils.HexToByteArray(key);

                var accountId32 = new AccountId32();
                accountId32.Create(pubKey);

                var accountInfo = await SubstrateClient.SystemStorage.Account(accountId32, token);
                if (accountInfo == null)
                {
                    continue;
                }

                accountInfos.Add((storageKeyString, accountId32, accountInfo));
            }

            return accountInfos;
        }

        public async Task<List<(string, AccountId32, AccountInfo)>> GetAccountsNextAsync(string startStorageKey, uint page, CancellationToken token)
        {
            if (!IsConnected)
            {
                return null;
            }

            if (page < 2 || page > 1000)
            {
                throw new NotSupportedException("Page size must be in the range of 2 - 1000");
            }

            var startKeyBytes = new byte[] { };
            if (startStorageKey != null)
            {
                startKeyBytes = Utils.HexToByteArray(startStorageKey);
            }

            var accountInfos = new List<(string, AccountId32, AccountInfo)>();
            var keyBytes = RequestGenerator.GetStorageKeyBytesHash("System", "Account");

            var storageKeys = await SubstrateClient.State.GetKeysPagedAsync(keyBytes, page, startKeyBytes, token);
            if (storageKeys == null || !storageKeys.Any())
            {
                return accountInfos;
            }

            var storageChangeSets = await SubstrateClient.State.GetQueryStorageAtAsync(storageKeys.Select(p => Utils.HexToByteArray(p.ToString())).ToList(), null, token);
            if (storageChangeSets != null)
            {
                foreach (var storageChangeSet in storageChangeSets.Changes)
                {
                    var storageKeyString = storageChangeSet[0];
                    var key = storageKeyString.Substring(storageKeyString.Length - 64);
                    var pubKey = Utils.HexToByteArray(key);

                    var accountId32 = new AccountId32();
                    accountId32.Create(pubKey);

                    var accountInfo = new AccountInfo();
                    accountInfo.Create(storageChangeSet[1]);

                    accountInfos.Add((storageKeyString, accountId32, accountInfo));
                }
            }

            return accountInfos;
        }

        public async Task<List<string[]>> GetGenericNextAsync(string module, string item, string? startStorageKey, uint page, byte[] blockHash, CancellationToken token)
        {
            if (!IsConnected)
            {
                return null;
            }

            if (page < 2 || page > 1000)
            {
                throw new NotSupportedException("Page size must be in the range of 2 - 1000");
            }

            byte[] startKeyBytes = null;
            if (startStorageKey != null)
            {
                startKeyBytes = Utils.HexToByteArray(startStorageKey);
            }

            var genericInfos = new List<string[]>();
            var keyBytes = RequestGenerator.GetStorageKeyBytesHash(module, item);

            var storageKeys = await SubstrateClient.State.GetKeysPagedAtAsync(keyBytes, page, startKeyBytes, blockHash, token);
            if (storageKeys == null || !storageKeys.Any())
            {
                return genericInfos;
            }

            var storageChangeSets = await SubstrateClient.State.GetQueryStorageAtAsync(storageKeys.Select(p => Utils.HexToByteArray(p.ToString())).ToList(), blockHash, token);
            if (storageChangeSets != null)
            {
                foreach (var storageChangeSet in storageChangeSets.Changes)
                {
                    genericInfos.Add( new string[] { storageChangeSet[0], storageChangeSet[1] });
                }
            }

            return genericInfos;
        }

        /// <summary>
        /// Get vesting schedule.
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<BoundedVecT19> GetVestingSchedulesAsync(AccountId32 from, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            return await SubstrateClient.VestingStorage.VestingSchedules(from, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="startKey"></param>
        /// <param name="page"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<List<(string, AccountId32, BoundedVecT19)>> GetVestingSchedulesAsync(string startStorageKey, uint page, CancellationToken token)
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
            if (startStorageKey != null)
            {
                startKeyBytes = Utils.HexToByteArray(startStorageKey);
            }

            var vestingSchedules = new List<(string, AccountId32, BoundedVecT19)>();
            var keyBytes = RequestGenerator.GetStorageKeyBytesHash("Vesting", "VestingSchedules");

            //var keyBytesHexString = Utils.Bytes2HexString(keyBytes, Utils.HexStringFormat.Pure).ToLower();
            var storageKeys = await SubstrateClient.State.GetKeysPagedAsync(keyBytes, page, startKeyBytes, token);
            if (storageKeys == null)
            {
                return vestingSchedules;
            }

            foreach (var storageKey in storageKeys)
            {
                var storageKeyString = storageKey.ToString();
                var key = storageKeyString.Substring(storageKeyString.Length - 64);
                var pubKey = Utils.HexToByteArray(key);
                var address = Utils.GetAddressFrom(pubKey, 1337);

                //Log.Information("Got entry for {0}", address);

                var accountId32 = new AccountId32();
                accountId32.Create(pubKey);

                var vestingSchedule = await SubstrateClient.VestingStorage.VestingSchedules(accountId32, token);
                if (vestingSchedule == null)
                {
                    continue;
                }

                vestingSchedules.Add((storageKeyString, accountId32, vestingSchedule));
            }

            return vestingSchedules;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> FaucetAsync(CancellationToken token)
        {
            return await FaucetAsync(Alice, 1000, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> FaucetAsync(Account from, int tokens, CancellationToken token)
        {
            var extrinsicType = "Faucet";

            if (!IsConnected || Account == null || ExtrinsicManger.Running.Any())
            {
                return null;
            }

            var accountFaucet = new AccountId32();
            accountFaucet.Create(Utils.GetPublicKeyFrom(from.Value));

            var account32 = new AccountId32();
            account32.Create(Utils.GetPublicKeyFrom(Account.Value));

            var multiAddress = new EnumMultiAddress();
            multiAddress.Create(MultiAddress.Id, account32);

            var amount = new BaseCom<U128>();
            amount.Create(tokens * 1000000000000);

            var extrinsic = BalancesCalls.Transfer(multiAddress, amount);

            return await GenericExtrinsicAsync(from, extrinsicType, extrinsic, 1, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> FaucetAsync(Account from, Account to, int tokens, CancellationToken token)
        {
            var extrinsicType = "Faucet";

            if (!IsConnected || to == null || ExtrinsicManger.Running.Any())
            {
                return null;
            }

            var accountAlice = new AccountId32();
            accountAlice.Create(Utils.GetPublicKeyFrom(from.Value));

            var multiAddress = new EnumMultiAddress();
            multiAddress.Create(MultiAddress.Id, to.ToAccountId32());

            var amount = new BaseCom<U128>();
            amount.Create(tokens * 1000000000000);

            var extrinsic = BalancesCalls.Transfer(multiAddress, amount);

            return await GenericExtrinsicAsync(from, extrinsicType, extrinsic, 1, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> FaucetAsync(int tokens, Account to, CancellationToken token)
        {
            var extrinsicType = "Faucet";

            if (!IsConnected || to == null || ExtrinsicManger.Running.Any())
            {
                return null;
            }

            var accountAlice = new AccountId32();
            accountAlice.Create(Utils.GetPublicKeyFrom(Alice.Value));

            var multiAddress = new EnumMultiAddress();
            multiAddress.Create(MultiAddress.Id, to.ToAccountId32());

            var amount = new BaseCom<U128>();
            amount.Create(tokens * 1000000000000);

            var extrinsic = BalancesCalls.Transfer(multiAddress, amount);

            return await GenericExtrinsicAsync(Alice, extrinsicType, extrinsic, 1, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        /// <param name="amount"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> BatchAllAsync(List<EnumRuntimeCall> callList, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = "BatchAll";

            if (!IsConnected || Account == null || callList == null || callList.Count == 0)
            {
                return null;
            }

            var calls = new BaseVec<EnumRuntimeCall>();
            calls.Create(callList.ToArray());

            var extrinsic = UtilityCalls.BatchAll(calls);

            return await GenericExtrinsicAsync(Account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        /// <param name="amount"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> TransferKeepAliveAsync(AccountId32 to, BigInteger amount, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = "TransferKeepAlive";

            if (!IsConnected || Account == null)
            {
                return null;
            }

            var multiAddress = new EnumMultiAddress();
            multiAddress.Create(MultiAddress.Id, to);

            var balance = new BaseCom<U128>();
            balance.Create(amount);

            var extrinsic = BalancesCalls.TransferKeepAlive(multiAddress, balance);

            return await GenericExtrinsicAsync(Account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        /// <param name="start"></param>
        /// <param name="period"></param>
        /// <param name="periodCount"></param>
        /// <param name="perPeriod"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> VestingScheduledTransferAsync(AccountId32 to, uint start, uint period, uint periodCount, BigInteger perPeriod, CancellationToken token)
        {
            var extrinsicType = "VestedTransfer";

            var multiAddress = new EnumMultiAddress();
            multiAddress.Create(MultiAddress.Id, to);

            var vestingSchedule = new VestingSchedule();

            var startU32 = new U32();
            startU32.Create(start);
            vestingSchedule.Start = startU32;

            var periodU32 = new U32();
            periodU32.Create(period);
            vestingSchedule.Period = periodU32;

            var periodCountU32 = new U32();
            periodCountU32.Create(periodCount);
            vestingSchedule.PeriodCount = periodCountU32;

            var perPeriodU32 = new BaseCom<U128>();
            perPeriodU32.Create(perPeriod);
            vestingSchedule.PerPeriod = perPeriodU32;

            var extrinsic = VestingCalls.VestedTransfer(multiAddress, vestingSchedule);

            return await GenericExtrinsicAsync(Account, extrinsicType, extrinsic, 10, token);
        }
    }
}