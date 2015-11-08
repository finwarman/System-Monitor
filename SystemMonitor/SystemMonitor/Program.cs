using System;
using System.Linq;
using System.Management;
using System.Threading;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;

namespace SystemMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            //getCPUFreq();

            Thread windowSize =
                new Thread(windowSizer);
            Thread timeWriter =
                new Thread(writeTime);
            Thread ipUpdate =
                new Thread(pingGoogle);

            Console.Title = ("System Monitor");
            Console.WindowWidth = 43;
            Console.WindowHeight = 25;
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);

            Console.CursorVisible = false;
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
            }
            windowSize.Start();
            Thread.Sleep(50);
            timeWriter.Start();
            Thread.Sleep(800);
            ipUpdate.Start();
        }
        static void End()
        {
            Console.ReadKey();
            Console.Clear();
        }
        static string timeString()
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            return time;
        }
        static string dateString()
        {
            string time = DateTime.Now.ToString("dd/MM/yyyy");
            return time;
        }
        static string OSProperties()
        {
            var name = (from x in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get().OfType<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();
            return name != null ? name.ToString() : "Unknown";
        }
        static string getAvailableRAM()
        {
            string RAMAvailable = "";
            return RAMAvailable;
        }
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
            Thread.Sleep(2000);
        }
        public static void writeTime()
        {
            while (true)
            {
                Console.SetCursorPosition(3, 2);
                Console.Write("Time: {0}    Date: {1}", timeString(), dateString());
                Thread.Sleep(60);
            }
        }
        public static void pingGoogle()
        {
            Ping pingSender = new Ping();
            IPAddress address = IPAddress.Parse("8.8.8.8");

            string data = "8BytesTo";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            int timeout = 2000;

            while (true)
            {
                Console.SetCursorPosition(3, 16);
                Console.Write("              ");
                try
                {
                    PingReply reply = pingSender.Send(address, timeout, buffer);
                    if (reply.Status == IPStatus.Success)
                    {
                        Console.SetCursorPosition(3, 16);
                        Console.Write("Ping: {0}      ", reply.RoundtripTime);
                    }
                    else
                    {
                        Console.SetCursorPosition(3, 16);
                        Console.Write("Ping: Failure");
                    }
                }
                catch (PingException)
                {
                    Console.SetCursorPosition(3, 16);
                    Console.Write("Ping: Failure");
                }
                Thread.Sleep(2000);
            }
        }
        public static void writeping()
        {
            string ping1 = "1";
            string ping2 = "2";
            string ping3 = "3";
            while (true)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write(ping1);
                Console.SetCursorPosition(0, 0);
                Console.Write(ping2);
                Console.SetCursorPosition(0, 0);
                Console.Write(ping3);
            }
        }
        public static void getCPUFreq()
        {
            while (true)
            {
                using (ManagementObject Mo = new ManagementObject("Win32_Processor.DeviceID='CPU0'"))
                {
                    double currentsp = (uint)(Mo["CurrentClockSpeed"]);
                    double Maxsp = (uint)(Mo["MaxClockSpeed"]);

                    string GHZcurrentsp = (currentsp / 1000).ToString("G3");
                    string GHZMaxsp = (Maxsp / 1000).ToString("G3");
                    string CPUUsage = ((currentsp / Maxsp)*100).ToString("G3");
                    Console.Clear();
                    Console.WriteLine("Current Clock Speed: {0} GHz\nMaximum Clock Speed: {1} GHz\nPercentage utilised: {2}%", GHZcurrentsp, GHZMaxsp, CPUUsage);

                }
            }

        }
    }
}
