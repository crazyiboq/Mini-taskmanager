using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

class Program
{
    static List<string> blackList = new List<string>();
    static bool running = true;

    static void Main()
    {
        Thread blackListThread = new Thread(CheckBlackList);
        blackListThread.Start();

        while (running)
        {
            Console.WriteLine("\n=== Process Task Manager ===");
            Console.WriteLine("1. List all processes");
            Console.WriteLine("2. Start process by name");
            Console.WriteLine("3. Kill process by ID");
            Console.WriteLine("4. Kill processes by name");
            Console.WriteLine("5. Add to blacklist");
            Console.WriteLine("6. Remove from blacklist");
            Console.WriteLine("7. Exit");
            Console.Write("Choose an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ListProcesses();
                    break;
                case "2":
                    StartProcess();
                    break;
                case "3":
                    KillById();
                    break;
                case "4":
                    KillByName();
                    break;
                case "5":
                    AddToBlacklist();
                    break;
                case "6":
                    RemoveFromBlacklist();
                    break;
                case "7":
                    running = false;
                    Console.WriteLine("goodbye, bone boss!");
                    break;
                default:
                    Console.WriteLine("invalid choice, try again.");
                    break;
            }
        }
    }

    static void ListProcesses()
    {
        var processes = Process.GetProcesses();
        foreach (var p in processes)
        {
            Console.WriteLine($"ID: {p.Id}, Name: {p.ProcessName}");
        }
    }

    static void StartProcess()
    {
        Console.Write("Enter process name (e.g., notepad): ");
        string name = Console.ReadLine();
        try
        {
            Process.Start(name);
            Console.WriteLine("Process started.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"couldn’t start it. error: {e.Message}");
        }
    }

    static void KillById()
    {
        Console.Write("Enter process ID: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                Process.GetProcessById(id).Kill();
                Console.WriteLine("Process killed.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"couldn’t kill it. error: {e.Message}");
            }
        }
        else
        {
            Console.WriteLine("invalid ID, pal.");
        }
    }

    static void KillByName()
    {
        Console.Write("Enter process name: ");
        string name = Console.ReadLine();
        var processes = Process.GetProcessesByName(name);
        foreach (var p in processes)
        {
            try
            {
                p.Kill();
                Console.WriteLine($"Killed {p.ProcessName} (ID: {p.Id})");
            }
            catch (Exception e)
            {
                Console.WriteLine($"couldn’t kill {p.Id}. error: {e.Message}");
            }
        }
    }

    static void AddToBlacklist()
    {
        Console.Write("Enter process name to blacklist: ");
        string name = Console.ReadLine();
        if (!blackList.Contains(name))
        {
            blackList.Add(name);
            Console.WriteLine("Added to blacklist.");
        }
        else
        {
            Console.WriteLine("already blacklisted.");
        }
    }

    static void RemoveFromBlacklist()
    {
        Console.Write("Enter process name to remove: ");
        string name = Console.ReadLine();
        if (blackList.Remove(name))
        {
            Console.WriteLine("Removed from blacklist.");
        }
        else
        {
            Console.WriteLine("not found in blacklist.");
        }
    }

    static void CheckBlackList()
    {
        while (running)
        {
            foreach (var name in blackList)
            {
                var processes = Process.GetProcessesByName(name);
                foreach (var p in processes)
                {
                    try
                    {
                        p.Kill();
                        Console.WriteLine($"[blacklist] killed {p.ProcessName} (ID: {p.Id})");
                    }
                    catch { }
                }
            }
            Thread.Sleep(3000); // every 3 seconds
        }
    }
}

