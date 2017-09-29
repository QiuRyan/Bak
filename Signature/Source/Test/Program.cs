using System;
using JYM_SignatureContract;

namespace Jinyinmao.Signature.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SignContractService service = new SignContractService();

            service.Start();

            Console.Read();
        }
    }
}