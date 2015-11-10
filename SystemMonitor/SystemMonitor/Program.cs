using System;
using System.Linq;
using System.Management;
using System.Threading;
using System.Net.NetworkInformation;
using System.Net;
using System.Text; //Usings.


namespace SystemMonitor
{
    class Program
    { //Program
        //
        static void Main(string[] args)
        {
            Thread getCPUFrequency =
                 new Thread(getCPUFreq);
            Thread windowSize =
                new Thread(windowSizer);
            Thread timeWriter =
                new Thread(writeTime);
            Thread ipUpdate =
                new Thread(writePing); //Create threads.

            Console.Title = ("System Monitor");
            Console.WindowWidth = 43;
            Console.WindowHeight = 25;
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.CursorVisible = false; //Set-up Console Window.

            ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
            double counter = 0;
            for (int i = 3; i > 0; i--)
            {
                counter = 0;
                foreach (var color in colors)
                {
                    Console.ForegroundColor = color;
                    if (counter > 3)
                    {
                        Console.WriteLine("\n\n\n\n\n                 Loading...\n");
                    }
                    else if (counter > 1.5)
                    {
                        Console.WriteLine("\n\n\n\n\n                 Loading..");
                    }
                    else
                    {
                        Console.WriteLine("\n\n\n\n\n                 Loading.");
                    }
                    counter += 0.3;
                    Thread.Sleep(60);
                    Console.Clear();
                }
            } //Loading Screen

            windowSize.Start();
            Thread.Sleep(400);
            timeWriter.Start();
            Thread.Sleep(2000);
            ipUpdate.Start();
            Thread.Sleep(400);
            getCPUFrequency.Start(); //Start tasks, with intervals.

        } //Main.
        //
        static string timeString()
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            return time;
        }      //Current Time to string.
        //
        static string dateString()
        {
            string time = DateTime.Now.ToString("dd/MM/yyyy");
            return time;
        }      //Current Date to string.
        //
        static string OSProperties()
        {
            var name = (from x in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get().OfType<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();
            return name != null ? name.ToString() : "Unknown";
        }    //Get OS version.
        //
        static string getAvailableRAM()
        {
            string RAMAvailable = "";
            return RAMAvailable;
        } //Get RAM Available. [NOT FUNCTIONAL]
        //
        public static void windowSizer()
        {
            string topLine = (" ╔═══════════════════════════════════════╗\n");
            string midClear = (" ║                                       ║\n");
            string midLine = (" ╠═══════════════════════════════════════╣\n");
            string bottomLine = (" ╚═══════════════════════════════════════╝\n");
            string osProperties = OSProperties();
            string userName = Environment.UserName;
            //while (true)
            Console.WindowWidth = 43;
            Console.WindowHeight = 25;
            Console.SetCursorPosition(0, 0);
            Console.Write("{0}{2}{2}{1}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{3}", topLine, midLine, midClear, bottomLine);
            Console.SetCursorPosition(3, 5);
            Console.Write("OS: {0}", osProperties);
            Console.SetCursorPosition(3, 7);
            Console.Write("User Account Name: " + userName);
            //Thread.Sleep(2000);
        }//Write overlay & make sure window is scaled.
        //
        public static void writeTime()
        {
            while (true)
            {
                Console.SetCursorPosition(3, 2);
                if (Console.CursorLeft == 3 | Console.CursorTop == 2)
                {
                    Console.Write("Time: {0}    Date: {1}", timeString(), dateString());
                }
                Thread.Sleep(60);
            }
        }  //Write Date and Time.
        //
        public static string pingGoogle()
        {
            Ping pingSender = new Ping();
            IPAddress address = IPAddress.Parse("8.8.8.8");

            string data = "8BytesTo";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            int timeout = 2000;

            try
            {
                PingReply reply = pingSender.Send(address, timeout, buffer);
                if (reply.Status == IPStatus.Success)
                {
                    return (reply.RoundtripTime).ToString();
                }
                else
                {
                    return ("Failure");
                }
            }
            catch (PingException)
            {
                return ("Failure");
            }

        } //Ping google.
          //
        public static void writePing()
        {
            while (true)
            {
                string currentping = pingGoogle();
                Console.SetCursorPosition(3, 16);
                if (Console.CursorLeft == 3 | Console.CursorTop == 16) //Make sure to write to correct location on screen - prevents graphical errors.
                {
                    Console.Write("Ping: {0}    \n", currentping);
                    Console.CursorLeft = 3;
                    Console.Write("Previous Ping will go here lel.");
                }
                Thread.Sleep(2000);
            }
        } //Write result of pingGoogle().
          //
        public static void getCPUFreq()
        {
            while (true)
            {
                using (ManagementObject Mo = new ManagementObject("Win32_Processor.DeviceID='CPU0'"))
                {
                    double currentsp = (uint)(Mo["CurrentClockSpeed"]);
                    double Maxsp = (uint)(Mo["MaxClockSpeed"]);
                    //double utilisation = (uint)(Mo[""]);

                    string GHZcurrentsp = (currentsp / 1000).ToString("G3");
                    string GHZMaxsp = (Maxsp / 1000).ToString("G3");
                    string CPUUsage = ((currentsp / Maxsp) * 100).ToString("G3");
                    Console.SetCursorPosition(3, 10);
                    if (Console.CursorLeft == 3 | Console.CursorTop == 10)
                    {
                        Console.Write("CPU Clock Speed:\n");
                        Console.CursorLeft = 3;
                        Console.Write("Current: {0} GHz    \n", GHZcurrentsp);
                        Console.CursorLeft = 3;
                        Console.Write("Maximum: {0} GHz   \n", GHZMaxsp);
                        Console.CursorLeft = 3;
                        Console.Write("Utilised: {0}%    ", CPUUsage);
                    }
                }
                Thread.Sleep(600);
            }
        }//Get CPU info and usage, write to console.
         //
        public static int globalTimer()
        {
            while (true)
            {
                int clock = 0;
                Thread.Sleep(50);
                clock++;
                if (clock > 20)
                {
                    clock = 0;
                }
                return clock;
            }
        }//Attempt to sychronise schedule events. [NOT FUNCTIONAL]

        public static string externalIP()//Grab external IP from checkip.dyndnss.org
        {

            string url = "http://checkip.dyndns.org";
            WebRequest req = WebRequest.Create(url);
            WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            string response = sr.ReadToEnd().Trim();
            string[] a = response.Split(':');
            string a2 = a[1].Substring(1);
            string[] a3 = a2.Split('<');
            string a4 = a3[0];
            //
            string externalip = new WebClient().DownloadString("http://icanhazip.com");

            //
            //
            return a4;
        }

    }
} //Close
