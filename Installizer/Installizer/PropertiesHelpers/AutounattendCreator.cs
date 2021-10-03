namespace Installizer.PropertiesHelpers
{
    public class AutounattendCreator
    {

        private System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
        int drive;
        bool x64;
        bool uefi;
        string type;
        string lang;
        string computername;
        string language;
        public AutounattendCreator(int drive = 0, bool x64 = true, bool uefi = true, string type = "pro", string lang = "0409", string computername = null)
        {

        }
        public void SetLanguage(string language)
        {

        }
    }
}
