using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WebTerminal.WebApp
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
            ConsoleProcess.CancelOutputRead();
            ConsoleProcess.StandardInput.WriteLine(content);
            ConsoleProcess.BeginOutputReadLine();
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
            ConsoleProcessInfo = new ProcessStartInfo("pwsh.exe")
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            ConsoleProcess = Process.Start(ConsoleProcessInfo);
            ConsoleProcess.BeginOutputReadLine();
            ConsoleProcess.OutputDataReceived += (sender, args) =>
            {
                OnOutput(args.Data);
            };
        }
    }
}