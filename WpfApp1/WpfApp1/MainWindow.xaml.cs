using System;
using System.Net;
using System.Net.NetworkInformation;

namespace WebApp
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworkInterface[] nets = NetworkInterface.GetAllNetworkInterfaces();
            Console.WriteLine(nets);
        }
    }
}
