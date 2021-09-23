using System;
using Newtonsoft.Json;
using System.Diagnostics;
namespace Runas
{
    class Program
    {
        public static dynamic jprograms = JsonConvert.DeserializeObject(Properties.Resources.progs);

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
                        if (System.IO.File.Exists(prog.Value))
                        { 
                            Process.Start(prog.Value);
                        }
                    }
                }
                else if (jprograms[programName].Count == 1)
                {
                    Process.Start(jprograms[programName][0].Value);
                }
            }
        }
        static void Main(string[] args)
        {
            foreach(var prog in jprograms)
            {
                string ProgProg = ((Newtonsoft.Json.Linq.JProperty)prog).Name;
                System.Threading.Thread thread = new (new System.Threading.ThreadStart(() => RunProcess(ProgProg)));
                thread.Start();
            }
        }
    }
}
