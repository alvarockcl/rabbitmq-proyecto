using System;
using System.Collections.Concurrent;
using System.Text;


namespace rpc_client
{
    public class Program{
        public static void Main(string[] args)
        {
            var rpcClient = new RpcClient();
            Console.WriteLine(" [x] Requesting fib(30)");
            var response = rpcClient.Call("30");

            Console.WriteLine(" [.] Got '{0}'", response);
            rpcClient.Close();
        }
    }
}
