using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Log the execution time
        LogExecutionTime();
        

        Console.WriteLine("Waiting 5 seconds...");
        await Task.Delay(5000);
        

        Console.WriteLine("Disabling devices...");
        RunShellCommand("xinput disable 15"); // Disable Mouse
        RunShellCommand("xinput disable 18"); //Disable Keyboard
        

        Console.WriteLine("Waiting 30 seconds...");
        await CountdownAsync(); // Initialize countdown file
        

        Console.WriteLine("Enabling devices...");
        RunShellCommand("xinput enable 15"); // Enable Mouse
        RunShellCommand("xinput enable 18"); //Enable Keyboard

        Console.WriteLine("Done.");
        
    }

    static void LogExecutionTime()
    {
        string logFilePath = Path.Combine(AppContext.BaseDirectory, "ExecuteLogs.txt");
        string log = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Program Executed";
        File.AppendAllText(logFilePath, log + Environment.NewLine);
    }

    static async Task CountdownAsync()
    {
        string countdownFilePath = Path.Combine(AppContext.BaseDirectory, "countdown.txt");
        int countdownValue = 31;

        // Write initial countdown value to file
        File.WriteAllText(countdownFilePath, countdownValue.ToString());

        // Countdown logic
        for (int i = countdownValue - 1; i >= 0; i--)
        {
            await Task.Delay(1000); // Wait 1 second
            File.WriteAllText(countdownFilePath, i.ToString());
        }
    }

    static void RunShellCommand(string command)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{command}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        process.Start();
        process.WaitForExit();
    }
}