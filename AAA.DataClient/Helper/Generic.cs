﻿using Ajuna.NetApi;
using Ajuna.NetApi.Model.Types;
using Ajuna.NetApi.Model.Types.Base;
using Ajuna.NetApi.Model.Types.Primitive;
using Bajun.Network.NET.NetApiExt.Generated.Model.primitive_types;
using Bajun.Network.NET.NetApiExt.Generated.Model.sp_core.crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Ajuna.Integration.Helper
{
    public static class Generic
    {
        public static BigInteger BAJU2Pico(double amount) => new BigInteger(Convert.ToUInt64(amount * Math.Pow(10, 12)));

        public static string ToHexString(this H256 h256)
        {
            return Utils.Bytes2HexString(h256.Value.Value.Select(p => p.Value).ToArray());
        }

        public static byte[] ToPublicKey(this string address)
        {
            return Utils.GetPublicKeyFrom(address);
        }

        public static string ToAddress(this AccountId32 account32, short ss58 = 42)
        {
            var pubKey = account32.Value.Value.Select(p => p.Value).ToArray();
            return pubKey.ToAddress(ss58);
        }

        public static string ToAddress(this byte[] publicKey, short ss58 = 42)
        {
            return Utils.GetAddressFrom(publicKey, ss58);
        }

        public static AccountId32 ToAccountId32(this byte[] publicKey)
        {
            var account32 = new AccountId32();
            account32.Create(publicKey);

            return account32;
        }

        public static AccountId32 ToAccountId32(this Account account)
        {
            var account32 = new AccountId32();
            account32.Create(account.Bytes);

            return account32;
        }

        public static AccountId32 ToAccountId32(this string address)
        {
            var account32 = new AccountId32();
            account32.Create(address.ToPublicKey());

            return account32;
        }

        public static H256 ToH256(this string hash)
        {
            var h256 = new H256();
            h256.Create(hash);
            return h256;
        }

        public static U128 ToU128(this BigInteger number)
        {
            var u128 = new U128();
            u128.Create(number);
            return u128;
        }

        public static U16 ToU16(this ushort number)
        {
            var u16 = new U16();
            u16.Create(number);
            return u16;
        }

        public static U16[] ToU16Array(this ushort[] bytes)
        {
            return bytes.Select(p => p.ToU16()).ToArray();
        }

        public static U8[] ToU8Array(this byte[] bytes)
        {
            return bytes.Select(p => p.ToU8()).ToArray();
        }

        public static U8[] ToU8Array(this string str)
        {
            return str.Select(p => p.ToU8()).ToArray();
        }

        public static U8 ToU8(this byte number)
        {
            var u8 = new U8();
            u8.Create(number);
            return u8;
        }

        public static U8 ToU8(this char character)
        {
            var u8 = new U8();
            u8.Create(BitConverter.GetBytes(character)[0]);
            return u8;
        }

        public static BaseOpt<U8> ToBaseOpt(this U8 u8)
        {
            var baseOpt = new BaseOpt<U8>();
            baseOpt.Create(u8);
            return baseOpt;
        }

        public static IEnumerable<IEnumerable<T>> BuildChunksWithLinqAndYield<T>(List<T> fullList, int batchSize)
        {
            int total = 0;
            while (total < fullList.Count)
            {
                yield return fullList.Skip(total).Take(batchSize);
                total += batchSize;
            }
        }
    }
}