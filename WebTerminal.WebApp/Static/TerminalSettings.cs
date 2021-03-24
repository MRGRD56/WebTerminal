using System.Collections.Generic;
using WebTerminal.WebApp.Static.Helpers;

namespace WebTerminal.WebApp.Static
{
    public class TerminalSettings
    {
        public TerminalProcessInfo ProcessInfo { get; set; }

        public List<string> StartCommands { get; set; }
    }
}