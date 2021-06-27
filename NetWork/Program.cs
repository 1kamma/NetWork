using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Management;
using System.Text.RegularExpressions;
using NAudio.CoreAudioApi;
using SimpleWifi;
using DotRas;

namespace NetWork
{
    class Program
    {

        public static string[] StringsOfPaths = System.IO.Directory.GetFiles(@"C:\Program Files\Google\Drive File Stream\", "GoogleDriveFS.exe", System.IO.SearchOption.AllDirectories);
        public static Dictionary<string, string> progs = new() { { "GoogleDriveFS", StringsOfPaths[^1] }, { "googledrivesync", @"C:\Program Files\Google\Drive\googledrivesync.exe" }, { "qbittorrent", @"C:\Program Files\qBittorrent\qbittorrent.exe" }, { "Surfshark", @"C:\Program Files (x86)\Surfshark\Surfshark.exe" } };
        public static Dictionary<string, string> paths = new() { { StringsOfPaths[^1], "GoogleDriveFS.exe" }, { @"C:\Program Files\Google\Drive\googledrivesync.exe", "googledrivesync.exe" }, { @"C:\Program Files\qBittorrent\qbittorrent.exe", "qbittorrent.exe" }, { @"C:\Program Files (x86)\Surfshark\Surfshark.exe", "Surfshark" } };
        //public static string[,] progs = new string[2,5] { { "GoogleDriveFS.exe", "googledrivesync.exe", "qbittorrent.exe", "Surfshark.exe", "mstsc.exe" },{ StringsOfPaths[StringsOfPaths.Length-1], "C:\\Program Files\\Google\\Drive\\googledrivesync.exe", "C:\\Program Files\\qBittorrent\\qbittorrent.exe", "C:\\Program Files (x86)\\Surfshark\\Surfshark.exe" , "mstsc.exe" } };
        /// <summary>
        /// This Function Stops the RDP connection to the office computer. it takes 'stop' parameter. if it is true, it stops the rdp. otherwise, it starts it.
        /// </summary>
        /// <param name="stop">A bool parameter. it swiches between stopping (default) and starting rdp session.</param>
        public static void StopRDP(bool stop = true)
        {
            if (stop)
            {
                Process[] ids = Process.GetProcessesByName("mstsc");
                foreach (var id in ids)
                {

                    ManagementObjectSearcher search = new($"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {id.Id}");
                    Regex rdp = new("(משרדוש.rdp|161.2)");
                    foreach (var searchedObject in search.Get())
                    {
                        if (rdp.Match(searchedObject["CommandLine"].ToString()).Success)
                        {
                            id.Kill();
                        }
                    }
                    Console.WriteLine("");
                }

            }
            else
            {
                Process[] ids = Process.GetProcessesByName("mstsc");
                if (ids.Length < 1)
                {
                    StartProgram("rdp");
                }
                else
                {
                    foreach (var id in ids)
                    {
                        bool started = false;
                        ManagementObjectSearcher search = new($"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {id.Id}");
                        Regex rdp = new("(משרדוש.rdp|161.2)");
                        foreach (var searchedObject in search.Get())
                        {
                            if (rdp.Match(searchedObject["CommandLine"].ToString()).Success)
                            {
                                started = true;
                            }
                        }
                        if (!started)
                        {
                            StartProgram("rdp");
                        }

                }
                    Console.WriteLine("");

                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connect"></param>
        /// <returns></returns>
        public static bool Dial(bool connect = true)
        {
            if (connect)
            {
                RasDialer rasDialer = new();
                rasDialer.AllowUseStoredCredentials = true;
                rasDialer.PhoneBookPath = @"C:\Users\סארט\AppData\Roaming\Microsoft\Network\Connections\Pbk\ek.pbk";
                rasDialer.PhoneNumber = "ek";
                rasDialer.EntryName = "ek";
                rasDialer.DialAsync();
            }
            return false;
        }
        /// <summary>
        /// GetProgram switches between the name and the path of the file. if the name is given, it returns the path. if the path is given, it returns the name.
        /// </summary>
        /// <param name="NameOrPath">file or path of the program</param>
        /// <returns>path if name has given, name if path has given. empty string if found nothing.</returns>
        public static string GetProgram(string NameOrPath)
        {
            /*
            this function decides if the program is a name, or a location.
            if it is a location, it returns the name of the program
            if it is a name, it returns the location
            otherwise, it returns empty string
            */

            if (progs.ContainsKey(NameOrPath))
            {
                return progs[NameOrPath];
            }
            if (paths.ContainsKey(NameOrPath))
            {
                return paths[NameOrPath];
            }
            return "";

        }
        public static void StopProgram(string[] Name, bool mute = true)
        {
            foreach (string name in Name)
            {
                try
                {
                    if (progs.ContainsKey(name))
                    {
                        Process[] p = Process.GetProcessesByName(name);
                        foreach (Process proc in p)
                        {
                            proc.Kill(true);
                        }

                    }
                    else
                    {
                        if (name.Contains("mstsc") || name.Contains("rdp"))
                        {
                            StopRDP();
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
            MuteSystem(mute);
        }
        public static void StartProgram(string[] Name, bool mute = false)
        {
            foreach (var process_name in Name)
            {
                if (process_name.Contains("rdp") || process_name.Contains("mstsc"))
                {
                    StopRDP(false);
                    continue;
                }
                else if (Process.GetProcessesByName(process_name).Length < 1)
                {
                    string process;
                    if (progs.ContainsKey(process_name))
                    {
                        process = progs[process_name];
                    }
                    else if (paths.ContainsKey(process_name))
                    {
                        process = process_name;
                    }
                    else
                    {
                        process = @"C:\Windows\System32\rundll32.exe";
                    }
                    Process.Start(process);
                }
            }
            MuteSystem(mute);
        }
        public static string GetWifiNetwork()
        {
            SimpleWifi.Win32.WlanClient wlan = new();
            return wlan.Interfaces[0].CurrentConnection.profileName;
        }
        public static bool? IsItGoodWifi(string currentWifi)
        {
            Regex regex = new(currentWifi);
            bool? goodWifi;
            goodWifi = regex.IsMatch("Oliver Oliver5 Oliver x018_497622 TNCAPE5A34D MSBR Azrieli_Modiin_WIFI lu shalmata mickey Mickey Network 192.168.1. Silmarill wintunshark0 saret Saret huji-meonot") ? true : (regex.IsMatch("HUJI-netXeduroam HUJI-guest 132.64") ? false:null);

            return goodWifi;
        }
        static bool LanEthernet(System.Net.NetworkInformation.NetworkInterface networkAdapter)
        {
            if (networkAdapter.Name.Contains("Ethernet") || (networkAdapter.NetworkInterfaceType.ToString().Contains("Ethernet")))
            {
                return true;
            }
            return false;
        }
        static bool IgnoreLan(System.Net.NetworkInformation.NetworkInterface networkAdapter)
        {
            if (!(networkAdapter.Name.Contains("Bluetooth")) && (!(networkAdapter.Description.Contains("TAP"))) && (!(networkAdapter.Description.Contains("Hyper-V"))))
            {
                return true;
            }
            return false;
        }
        static string GetLan(System.Net.NetworkInformation.NetworkInterface networkAdapter)
        {
            if (LanEthernet(networkAdapter) && IgnoreLan(networkAdapter))
            {
                return  networkAdapter.GetPhysicalAddress().ToString();
            }
            return null;
        }
        static bool GetVpn(System.Net.NetworkInformation.NetworkInterface net)
        {
            if (net.Name.Contains("Surfshark") || net.Name.Contains("shark") || net.Description.Contains("TAP"))
            {
                return net.OperationalStatus.ToString().Equals("Up");
            }
            return false;
        }
        public static Dictionary<string, dynamic> GetNetwork()
        {
            Dictionary<string, dynamic> net_dict = new() { { "vpn", false }, { "lan", false }, { "good_lan", false }, { "ek", false } };
            System.Net.NetworkInformation.NetworkInterface[] nets = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            string getlan = "";
            string? lannull;
            bool getgoodland = false;
            string getwifi;
            getwifi = GetWifiNetwork();
            foreach (var net in nets)
            {
                if (net.Name == "ek")
                {
                    net_dict["ek"] |= true;
                }

                net_dict["vpn"] |= GetVpn(net);
                lannull= GetLan(net);
                if (lannull is not null)
                {
                    getlan = lannull;
                }
                if (AllowedMac(net.GetPhysicalAddress().ToString()))
                {
                    getgoodland |= true ;
                }

            }
            net_dict.Add("Wi-Fi", IsItGoodWifi(getwifi));
            net_dict["good_lan"]=getgoodland;
            if (getlan.Length > 0)
            {
                net_dict["lan"]= true;
            }
        
            //net_dict.Add("good_lan", AllowedMac());
            return net_dict;
        }
        private static string HirarcyNetworks(Dictionary<string, dynamic> network_dictionary)
        {
            if (network_dictionary["vpn"])
            {
                return "vpn";
            }
            else if (network_dictionary["ek"])
            {
                return "ek";
            }
            else if (network_dictionary["good_lan"])
            {
                return "good_lan";
            }
            else if (network_dictionary["lan"])
            {
                return "lan";
            }
            return "Wi-Fi";

        }
        public static bool AllowedMac(string mac, string[] those_are_the_AllowedMacs = null)
        {
            string[] AllowedMac_adresses;
            if (!(those_are_the_AllowedMacs is null))
            {
                AllowedMac_adresses = those_are_the_AllowedMacs;
            }
            else
            {
                AllowedMac_adresses = new string[] { "00E04C6813E4" };
            }
            foreach (var mac_address in AllowedMac_adresses)
            {
                if (mac_address.Equals(mac))
                {
                    return true;
                }
            }
            return false;

        }
        public static bool AllowedMac(string[] mac, string[] those_are_the_AllowedMacs = null)
        {
            foreach (var single_mac in mac)
            {
                if (AllowedMac(single_mac, those_are_the_AllowedMacs))
                {
                    return true;
                }
            }
            return false;
        }
        public static void StartProgram(string process_name)
        {
            if (process_name.Contains("rdp") || process_name.Contains("mstsc"))
            {
                Process.Start("mstsc.exe", @"D:\Drive\מסמכים\vms\משרדוש.rdp");
            }
            else if (Process.GetProcessesByName(process_name).Length < 1)
            {
                string process;
                if (progs.ContainsKey(process_name))
                {
                    process = progs[process_name];
                }
                else if (paths.ContainsKey(process_name))
                {
                    process = process_name;
                }
                else
                {
                    process = @"C:\Windows\System32\rundll32.exe";
                }
                Process.Start(process);
            }
        }
        public static void MuteSystem(bool mute = true)
        {
            MMDeviceEnumerator mMDeviceEnumerator = new();
            mMDeviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).AudioEndpointVolume.Mute = mute;
        }
        public static void DoTheSchtik()
        {
            var network_connection = GetNetwork();
            string[] programs_to_run;
            string[] programs_to_stop;
            bool mute;
            switch (HirarcyNetworks(network_connection))
            {
                case "vpn":
                    programs_to_run = new string[] { "googledrivesync", "GoogleDriveFS", "qbittorrent", "Surfshark" };
                    programs_to_stop = new string[] { "rdp" };
                    mute = false;
                    break;
                case "ek":
                    programs_to_run = new string[] { "rdp", "googledrivesync", "GoogleDriveFS" };
                    programs_to_stop = new string[] { "qbittorrent", "Surfshark" };
                    mute = false;
                    break;
                case "good_lan":
                    programs_to_run = new string[] { "googledrivesync", "GoogleDriveFS", "qbittorrent", "Surfshark" };
                    programs_to_stop = new string[] { "rdp" };
                    mute = false;
                    break;
                case "lan":
                    programs_to_run = new string[] { "rdp", "googledrivesync", "GoogleDriveFS" };
                    programs_to_stop = new string[] { "qbittorrent", "Surfshark" };
                    mute = false;
                    Dial(true);
                    break;
                default:
                    if (network_connection["Wi-Fi"])
                    {
                        programs_to_run = new string[] { "googledrivesync", "GoogleDriveFS", "qbittorrent", "Surfshark" };
                        programs_to_stop = new string[] { "rdp" };
                        mute = false;
                        break;
                    }
                    else
                    {
                        programs_to_run = new string[] { "googledrivesync", "GoogleDriveFS" };
                        programs_to_stop = new string[] { "qbittorrent", "Surfshark" };
                        mute = true;
                    }
                    break;
            }
            if (programs_to_run is not null)
            {
                StartProgram(programs_to_run, mute);
            }
            if (programs_to_stop is not null)
            {
                StopProgram(programs_to_stop, mute);
            }
            System.Windows.Forms.Application.Exit();
        }
        static void Main(string[] args)
        {
            DoTheSchtik();
            
        }
    }
}