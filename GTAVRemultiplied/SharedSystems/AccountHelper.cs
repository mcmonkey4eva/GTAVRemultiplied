using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Security;
using System.Collections.Specialized;
using FreneticScript;
using System.Net.Sockets;

namespace GTAVRemultiplied.SharedSystems
{
    public class AccountHelper
    {
        public static string Username;

        public static string Key;

        static AccountHelper()
        {
            if (System.IO.File.Exists("logindata.dat"))
            {
                string dat = System.IO.File.ReadAllText("logindata.dat");
                string[] d = dat.SplitFast('=');
                Username = d[0];
                Key = d[1];
            }
        }

        public static void GlobalLoginAttempt(string user, string pass, Scheduler schedule)
        {
            schedule.StartASyncTask(() =>
            {
                using (ShortWebClient wb = new ShortWebClient())
                {
                    try
                    {
                        NameValueCollection data = new NameValueCollection();
                        data["formtype"] = "login";
                        data["username"] = user;
                        data["password"] = pass;
                        data["session_id"] = "0";
                        byte[] response = wb.UploadValues("http://frenetic.xyz/account/micrologin", "POST", data);
                        string resp = GTAVUtilities.Enc.GetString(response).Trim(' ', '\n', '\r', '\t');
                        if (resp.StartsWith("ACCEPT=") && resp.EndsWith(";"))
                        {
                            string key = resp.Substring("ACCEPT=".Length, resp.Length - 1 - "ACCEPT=".Length);
                            schedule.ScheduleSyncTask(() =>
                            {
                                Log.Message("Accounts", "Login accepted!");
                                Username = user;
                                Key = key;
                                System.IO.File.WriteAllText("logindata.dat", Username + "=" + key);
                            });
                        }
                        else
                        {
                            schedule.ScheduleSyncTask(() =>
                            {
                                Log.Error("Login refused: " + resp);
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        schedule.ScheduleSyncTask(() =>
                        {
                            Log.Error("Exception while logging in...");
                            Log.Exception(ex);
                        });
                    }
                }
            });
        }

        public static string GetWebSession()
        {
            if (Username == null || Key == null)
            {
                throw new Exception("Can't get session, not logged in!");
            }
            using (ShortWebClient wb = new ShortWebClient())
            {
                NameValueCollection data = new NameValueCollection();
                data["formtype"] = "getsess";
                data["username"] = Username;
                data["session"] = Key;
                byte[] response = wb.UploadValues("http://frenetic.xyz/account/microgetsess", "POST", data);
                string resp = GTAVUtilities.Enc.GetString(response).Trim(' ', '\n', '\r', '\t');
                if (resp.StartsWith("ACCEPT=") && resp.EndsWith(";"))
                {
                    return resp.Substring("ACCEPT=".Length, resp.Length - 1 - "ACCEPT=".Length);
                }
                throw new Exception("Failed to get session: " + resp);
            }
        }

        public static void CheckWebSession(Socket sock, string username, string key)
        {
            if (username == null || key == null)
            {
                throw new ArgumentNullException();
            }
            using (ShortWebClient wb = new ShortWebClient())
            {
                NameValueCollection data = new NameValueCollection();
                data["formtype"] = "confirm";
                data["username"] = username;
                data["session"] = key;
                byte[] response = wb.UploadValues("http://frenetic.xyz/account/microconfirm", "POST", data);
                string resp = GTAVUtilities.Enc.GetString(response).Trim(' ', '\n', '\r', '\t');
                string rip = sock.RemoteEndPoint.ToString();
                if (resp.StartsWith("ACCEPT=") && resp.EndsWith(";"))
                {
                    string ip = resp.Substring("ACCEPT=".Length, resp.Length - 1 - "ACCEPT=".Length);
                    if (
                        rip.Contains("127.0.0.1")
                        || rip.Contains("[::1]")
                        || rip.Contains("192.168.0.")
                        || rip.Contains("192.168.1.")
                        || rip.Contains("10.0.0.")
                        || rip.Contains(ip))
                    {
                        Log.Message("Server", "Connection from '" + rip + "' accepted with username: " + username);
                        return;
                    }
                    throw new Exception("Connection from '" + rip + "' rejected because its IP is not " + ip + " or localhost!");
                }
                throw new Exception("Connection from '" + rip + "' rejected because: Failed to verify session!");
            }
        }

    }

    class ShortWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = 30 * 1000;
            return w;
        }
    }
}
