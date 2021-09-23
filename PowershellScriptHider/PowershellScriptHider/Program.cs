using System;
using Microsoft.Management;
using Microsoft;

namespace PowershellScriptHider
{
    class Program
    {
        static void Main(string[] args)
        {
            //args = new string[](){ @"D:\Drive\טוויקים למחשב\סקריפטוש\Powershell\abbyy shortcuts.ps1", ""};
            System.Management.Automation.PowerShell powerShell = System.Management.Automation.PowerShell.Create();
            //foreach (var arg in args)
            //{
            //    powerShell.AddCommand(System.IO.File.ReadAllText(arg));
            //}
            string s = System.IO.File.ReadAllText(@"D:\Drive\טוויקים למחשב\סקריפטוש\Powershell\abbyy shortcuts.ps1",System.Text.Encoding.UTF8);
            powerShell.AddScript(s);
        }
    }
}
