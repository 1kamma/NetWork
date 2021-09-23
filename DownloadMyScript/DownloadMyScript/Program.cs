using System;
using Newtonsoft.Json;
namespace DownloadMyScript
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.WebRequest data_element = System.Net.WebRequest.Create("https://api.github.com/gists/cd81e5005e7c7be3f71af94ab635d647");
            data_element.Method = "GET";
            data_element.ContentType = "application/json";
            data_element.Credentials = "foo";
            System.Net.WebResponse res = data_element.GetResponse();
            System.IO.Stream stream = res.GetResponseStream();
            string data_content = stream.ToString();
            dynamic data = JsonConvert.DeserializeObject(data_content);
            data_element = System.Net.WebRequest.Create(data["files"]["myscript 2.0.exe"]["raw_url"].Value);
            //byte[] binaryWriter = data_element.Method;
            //System.IO.File.WriteAllBytes("D:\\Neutral Folder\\File.exe",binaryWriter);
        }
    }
}
