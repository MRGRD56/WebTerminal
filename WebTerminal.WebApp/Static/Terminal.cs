using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace WebTerminal.WebApp.Static
{
    public static class Terminal
    {
        public static ProcessStartInfo ConsoleProcessInfo { get; set; }

        public static Process ConsoleProcess { get; set; }

        private static TerminalSettings Settings { get; set; }

        private static void OnOutput(string data)
        {
            Output += $"{data}\n";
            OnAfterOutput?.Invoke();
        }

        public static Action OnAfterOutput { get; set; }

        public static void RunCommand(string content)
        {
            switch (content.ToLower())
            {
                case "#help":
                    Output += "\n" +
                              "WebTerminal by MRGRD56\n" +
                              "'#clear' or '#cls' - clear output\n" +
                              "'#reset' - restart the terminal\n" +
                              "'#reset -b' - restart the terminal without running StartCommands\n";
                    break;
                case "#clear":
                case "#cls":
                    Output = "";
                    break;
                case "#reset":
                    Output = "";
                    ConsoleProcess.Kill();
                    StartProcess();
                    break;
                case "#reset -b":
                    Output = "";
                    ConsoleProcess.Kill();
                    StartProcess(false);
                    break;
                default:
                    ConsoleProcess.StandardInput.WriteLine(content);
                    break;
            }
        }

        public static string Output { get; private set; }

        public static void Initialize()
        {
            LoadSettings();
            ConsoleProcessInfo = new ProcessStartInfo(Settings.ProcessInfo.Name, Settings.ProcessInfo.Arguments)
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            StartProcess();
        }

        private static void StartProcess(bool runStartCommands = true)
        {
            ConsoleProcess = Process.Start(ConsoleProcessInfo);
            ConsoleProcess.BeginOutputReadLine();
            ConsoleProcess.OutputDataReceived += (sender, args) => { OnOutput(args.Data); };
            ConsoleProcess.Exited += (sender, args) =>
            {
                Output += "Процесс был завершён. Для повторного запуска наберите '#reset'.";
            };

            if (runStartCommands)
            {
                Settings.StartCommands.ForEach(RunCommand);
            }
        }

        private static void LoadSettings()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resource = "WebTerminal.WebApp.Properties.terminalSettings.json";

            using var stream = assembly.GetManifestResourceStream(resource);
            using var streamReader = new StreamReader(stream);
            var json = streamReader.ReadToEnd();
            Settings = JsonConvert.DeserializeObject<TerminalSettings>(json);
        }
    }
}