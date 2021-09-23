using Newtonsoft.Json;
using System.Diagnostics;

namespace Runas
{
    class Program
    {
        public static dynamic jprograms = JsonConvert.DeserializeObject(RunNormally.Properties.Resources.progs);

        public static bool DoRun(string program)
        {
            Process[] process = Process.GetProcessesByName(program);
            return (process.Length > 0);
        }
        public static void RunProcess(string programName)
        {

            if (!(DoRun(programName)))
            {
                if (programName.Equals("AutoHotKey"))
                {
                    Process.Start(jprograms[programName][0].Value, $"\u0022{jprograms[programName][1].Value.ToString()}\u0022");
                }

                else if (jprograms[programName].Count > 1)
                {
                    foreach (var prog in jprograms[programName])
                    {

                        Process.Start(prog.Value);
                    }
                }
                else if ((jprograms[programName].Count == 1) && (!(programName.Equals("RunAsMe"))))
                {
                    Process.Start(jprograms[programName][0].Value);
                }
                else if (programName.Equals("RunAsMe"))
                {
                    System.Diagnostics.Process file = new();
                    file.StartInfo.FileName = jprograms[programName][0].Value;
                    file.StartInfo.UseShellExecute = true;
                    file.StartInfo.Verb = "runas";
                    file.Start();
                }
            }
        }
        static void Main(string[] args)
        {
            Newtonsoft.Json.Linq.JArray r = new();
            string[] drivefs = System.IO.Directory.GetFiles("C:\\Program Files\\Google", "GoogleDriveFS.exe", System.IO.SearchOption.AllDirectories);
            r.Add(drivefs[^1]);
            jprograms.Add("GoogleDriveFS", r);
            foreach (var prog in jprograms)
            {
                string ProgProg = ((Newtonsoft.Json.Linq.JProperty)prog).Name;
                System.Threading.Thread thread = new(new System.Threading.ThreadStart(() => RunProcess(ProgProg)));
                thread.Start();
                //RunProcess(ProgProg);
            }
        }
    }
}
