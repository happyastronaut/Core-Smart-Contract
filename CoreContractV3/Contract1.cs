using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace CoreContractV3
{
    public class Contract1 : SmartContract
    {
        //params: 0710
        //return: 05

        public static readonly byte[] team = "AK9cP6LdpjuaQguvahFA7ojMiDbt4gEdAK".ToScriptHash();
        public static Object Main(string action, params object[] args)
        {
            if (Runtime.Trigger == TriggerType.Application)
            {
                if (action == "name")
                {
                    return "Core contract v3";
                }
                else if (action == "registertask")
                {
                    byte[] address = (byte[])args[0];
                    byte[] ip = (byte[])args[1];
                    return RegisterTask(address, ip);
                }
                else if (action == "gettask")
                {
                    return GetTask();
                }
                else if (action == "finishtask")
                {
                    byte[] address = (byte[])args[0];
                    return FinishTask(address);
                }
                else if (action == "removetask")
                {
                    byte[] address = (byte[])args[0];
                    byte[] id = (byte[])args[1];
                    return RemoveTask(address, id);
                }
                else if (action == "init")
                {
                    return Init();
                }
            }
            return false;
        }

        private static object Init()
        {
            var id = Storage.Get(Storage.CurrentContext, "id").AsBigInteger();
            var count = Storage.Get(Storage.CurrentContext, "count").AsBigInteger();
            BigInteger init_value_id = 1;
            BigInteger init_value_count = 0;
            if (id == 0 || count == 0)
            {
                Storage.Put(Storage.CurrentContext, "id", init_value_id);
                Storage.Put(Storage.CurrentContext, "count", init_value_count);
                return true;
            }
            return false;
        }


        /* Tasks */
        private static object RegisterTask(byte[] address, byte[] ip)
        {
            if (!Runtime.CheckWitness(address)) return false;
            IncrementCount();
            var id = GetCount();
            Storage.Put(Storage.CurrentContext, id.ToByteArray(), ip);
            Storage.Put(Storage.CurrentContext, ip, address);
            return true;
        }

        private static object GetTask()
        {
            var id = GetID().ToByteArray();
            return Storage.Get(Storage.CurrentContext, id);
        }

        private static object FinishTask(byte[] from)
        {
            if (!Runtime.CheckWitness(from) || !Runtime.CheckWitness(team)) return false;
            BigInteger id = 0;
            do
            {
                IncrementID();
                id = GetID();
            }
            while (Storage.Get(Storage.CurrentContext, id.AsByteArray()) == "null".AsByteArray());
            return true;
        }

        private static object RemoveTask(byte[] from, byte[] id)
        {
            if (!Runtime.CheckWitness(from)) return false;
            Storage.Put(Storage.CurrentContext, id, "null".AsByteArray());
            return true;
        }

        /* counters */
        private static BigInteger GetCount()
        {
            return Storage.Get(Storage.CurrentContext, "count").AsBigInteger();
        }

        private static BigInteger GetID()
        {
            return Storage.Get(Storage.CurrentContext, "id").AsBigInteger();
        }

        private static void IncrementCount()
        {
            var count = GetCount();
            count = count + 1;
            Storage.Put(Storage.CurrentContext, "count", count);
        }

        private static void IncrementID()
        {
            var id = GetID();
            id = id + 1;
            Storage.Put(Storage.CurrentContext, "id", id);
        }
    }
}
