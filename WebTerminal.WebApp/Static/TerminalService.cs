using System;
using System.Diagnostics;

namespace WebTerminal.WebApp.Static
{
    public static class TerminalService
    {
        public static ProcessStartInfo ConsoleProcessInfo { get; set; }
        
        public static Process ConsoleProcess { get; set; }

        private static void OnOutput(string data)
        {
            Output += $"{data}\n";
            OnAfterOutput?.Invoke();
        }
        
        public static Action OnAfterOutput { get; set; }

        public static void InputLine(string content)
        {
            if (content.Trim() is "[System.Console]::Clear()" or "clear" or "cls")
            {
                Output = "";
            }

            if (content.Trim().ToLower() == "@reset")
            {
                Output = "";
                ConsoleProcess.Kill();
                StartProcess();
            }
            else
            {
                ConsoleProcess.StandardInput.WriteLine(content);
            }
        }

        public static string Output { get; private set; }
        
        // public static string GetOutput()
        // {
        //     var stdout = ConsoleProcess.StandardOutput;
        //     var stdoutContent = "";
        //     string lastLine;
        //     do
        //     {
        //         lastLine = stdout.ReadLine();
        //         stdoutContent += lastLine + "\n";
        //         Debug.WriteLine(lastLine);
        //     } while (lastLine != null);
        //     
        //     return stdoutContent;
        // }

        public static void Initialize()
        {
            ConsoleProcessInfo = new ProcessStartInfo("powershell.exe")
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            StartProcess();
        }

        private static void StartProcess()
        {
            ConsoleProcess = Process.Start(ConsoleProcessInfo);
            ConsoleProcess.BeginOutputReadLine();
            ConsoleProcess.OutputDataReceived += (sender, args) =>
            {
                OnOutput(args.Data);
            };
        }
    }
}