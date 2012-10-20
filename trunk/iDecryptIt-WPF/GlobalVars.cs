using System.Collections.Generic;

namespace Hexware.Programs.iDecryptIt
{
    internal static class GlobalVars
    {
        internal static string[] VersionArr = new string[] {
            "5",
            "10",
            "0",
            "2B39"};
        internal const string Version = "5.10.0.2B39";
        internal static Dictionary<string, object> ExecutionArgs = new Dictionary<string, object>()
        {
            //{ "console", "" },
            { "dmg", "" }, // Passed dmg or ipsw
        };
    }
}
