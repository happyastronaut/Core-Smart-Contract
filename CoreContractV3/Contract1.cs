using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;
using System;
using System.Numerics;

namespace CoreContractV3
{
    public class Contract1 : SmartContract
    {
        public static Object Main(string action, params object[] args)
        {
            if (action == "name")
            {
                return "Core contract v3";
            }
            if (action == "getcorrcounter")
            {
                return GetCorrCounter();

            }
            if(action == "getcounter")
            {
                return GetCounter();
            }
            if(action == "registertask")
            {

                byte[] address = (byte[])args[0];
                byte[] ip = (byte[])args[1];
                return RegisterTask(address, ip);
            }
            if(action == "endtask")
            {
                byte[] address = (byte[])args[0];
                return EndTask(address);
            }
            if(action == "gettask")
            {
                return GetTask();
            }
            if(action == "init")
            {
                return Init();
            }
            return false;
        }

        private static object Init()
        {

            var counter = Storage.Get(Storage.CurrentContext, "counter").AsBigInteger();
            var curr = Storage.Get(Storage.CurrentContext, "current").AsBigInteger();
            if (counter != 0 || curr != 0)
            {
                Storage.Put(Storage.CurrentContext, "counter", 0);
                Storage.Put(Storage.CurrentContext, "current", 0);
                return true;
            }
            return false;
        }

        private static object EndTask(byte[] address)
        {
            IncrementCurrCounter();
            return true;
        }


        private static object GetTask()
        {
            var curr = GetCorrCounter();
            return Storage.Get(Storage.CurrentContext, (byte[])curr);
        }

        private static object RegisterTask(byte[] address, byte[] ip)
        {
            var counter = GetCounter();
            Storage.Put(Storage.CurrentContext, (byte[])counter, ip);
            Storage.Put(Storage.CurrentContext, ip, address);
            IncrementCounter();
            return true;
        }

        private static object GetCounter()
        {
            return Storage.Get(Storage.CurrentContext, "counter").AsBigInteger();
        }

        private static object GetCorrCounter()
        {
            return Storage.Get(Storage.CurrentContext, "current").AsBigInteger();
        }

        private static void IncrementCurrCounter()
        {
            var counter = Storage.Get(Storage.CurrentContext, "current").AsBigInteger();
            counter = counter + 1;
            Storage.Put(Storage.CurrentContext, "current", counter);
        }

        private static void IncrementCounter()
        {
            var counter = Storage.Get(Storage.CurrentContext, "counter").AsBigInteger();
            counter = counter + 1;
            Storage.Put(Storage.CurrentContext, "counter", counter);
        }
    }
}
