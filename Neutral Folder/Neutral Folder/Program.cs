using System;
namespace Neutral_Folder
{
    class Program
    {

        //public static string J = System.IO.File.ReadAllText("Neutral Folder.json");
        public static dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(Neutral_Folder.Properties.Resources.NeutralFolders);


        static void DeleteFolder(string name)
        {
            string path = json[name]["path"];
            System.IO.DirectoryInfo directory = new(path);
            try
            {
                System.IO.FileStream fileStream = null;
                directory.Delete(true);
            }
            catch
            {
                RemoveIni(path);
            }
            directory.Create();
            directory.Attributes = System.IO.FileAttributes.ReadOnly;
            SetFolderData(name);
        }
        static void RemoveIni(string path)
        {
            try
            {

                foreach (var file in System.IO.Directory.GetFiles(path))
                {
                    System.IO.File.SetAttributes(file, System.IO.FileAttributes.Normal);
                    System.IO.File.Delete(file);
                }
                foreach (var folder in System.IO.Directory.GetDirectories(path))
                {
                    System.Security.AccessControl.DirectorySecurity directorySecurity = new(folder, System.Security.AccessControl.AccessControlSections.All);
                    System.Security.Principal.NTAccount nTAccount = new("Administrators");
                    System.Security.AccessControl.FileSystemAccessRule fileSystemAccessRule = new(nTAccount, System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.AccessControlType.Allow);
                    directorySecurity.SetAccessRule(fileSystemAccessRule);

                    System.IO.Directory.Delete(folder, true);
                }
            }
            catch
            {

            }
        }
        static void SetFolderData(string name)
        {
            string path = json[name]["path"];
            string desktopini = $"{path}\\desktop.ini";
            string[] dataToWrite = json[name]["datafile"].Value.Split(@"`r`n");
            try
            {
                System.IO.File.WriteAllLines(desktopini, dataToWrite);
                System.IO.File.SetAttributes(desktopini, System.IO.FileAttributes.ReadOnly | System.IO.FileAttributes.Hidden);

            }
            catch
            {
                System.IO.File.SetAttributes(desktopini, System.IO.FileAttributes.Normal);
                System.IO.File.Delete(desktopini);
                System.IO.File.WriteAllLines(desktopini, dataToWrite);
                System.IO.File.SetAttributes(desktopini, System.IO.FileAttributes.ReadOnly | System.IO.FileAttributes.Hidden);
            }
        }
        static bool DeleteToday(string name)
        {
            string filedelete = $"{json[name]["path"]}\\{json[name]["dontdelete"]}";
            string deleteday = (1 + ((int)DateTime.Today.DayOfWeek)).ToString();
            System.Text.RegularExpressions.Regex isDayIn = new(deleteday);
            return (!(System.IO.File.Exists(filedelete)) && (isDayIn.IsMatch(json[name]["days"].Value)));
        }
        static bool DeleteToday(string name, int day)
        {
            foreach (var i in json[name]["days"])
            {
                if (day == i)
                {
                    return true;
                }
            }
            return false;
        }
        static void DeleteThisFolder(string name)
        {
            if (name.Equals("Force Neutral Folder"))
            {
                if (System.IO.File.Exists(json[name]["delete"].Value))
                {
                    DeleteFolder(name);
                }
                else if (!(System.IO.File.Exists(json[name].Value)))
                {
                    DeleteFolder(name);
                }
                else if ((System.IO.File.Exists(json[name].Value)))
                {
                    System.IO.File.Delete(json[name].Value);
                }

            }
            else if (DeleteToday(name))
            {
                DeleteFolder(name);
            }
        }
        public static void GetScheduledTask(string name)
        {
            System.Type.GetTypeFromCLSID(Guid.Parse("e34cb9f1-c7f7-424c-be29-027dcc09363a"));
        }
        static void Main(string[] args)
        {
            foreach (var k in json)
            {
                string key = ((Newtonsoft.Json.Linq.JProperty)k).Name;
                System.Threading.Thread t = new(new System.Threading.ThreadStart(() => DeleteThisFolder(key)));
                t.Start();

            }
            //ConsoleGetScheduledTask("");


        }
    }
}
