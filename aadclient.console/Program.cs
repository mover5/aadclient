using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aadclient.authentication.Cache;

namespace aadclient.console
{
    class Program
    {
        static void Main(string[] args)
        {
            TokenCache.InitializeTokenCache().Wait();
            Console.ReadKey();
        }
    }
}
