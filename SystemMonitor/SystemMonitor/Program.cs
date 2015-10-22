using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Management;
using Microsoft.VisualBasic.Devices;

namespace SystemMonitor
{
    class Program
    {

        static void Main(string[] args)
        {            
            Console.Title = ("System Monitor");
            Console.BufferWidth = Console.WindowWidth = 43;
            Console.BufferHeight = Console.WindowHeight;
            Console.CursorVisible = false;
            ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
            string ram = getAvailableRAM();
            for (int i = 2; i > 0; i--)
            {
                foreach (var color in colors)
                {
                    Console.ForegroundColor = color;
                    Console.WriteLine("Loading or some shit...");
                    System.Threading.Thread.Sleep(60);
                    Console.Clear();
                }
            }
            string topLine = (" ╔═══════════════════════════════════════╗\n");
            string midClear = (" ║                                       ║\n");
            string midLine = (" ╠═══════════════════════════════════════╣\n");
            string bottomLine = (" ╚═══════════════════════════════════════╝\n");
            string currentDir = Directory.GetCurrentDirectory();
            // string layout = File.ReadAllText((currentDir + "\\Resources\\Layout.txt"));

            Console.Clear();
            Console.Write("{0}{2}{2}{1}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{2}{3}",topLine, midLine, midClear, bottomLine);
            while (true)
            {
                Console.SetCursorPosition(3, 2);
                Console.Write("Time: {0}    Date: {1}", timeString(), dateString());
                Console.SetCursorPosition(3, 5);
                Console.Write("OS: {0}", OSProperties());
                System.Threading.Thread.Sleep(500);
                Console.SetCursorPosition(3, 7);
                Console.Write("User Account Name: " + Environment.UserName);

            }
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
    }
}
