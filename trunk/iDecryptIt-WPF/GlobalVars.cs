namespace Hexware.Programs.iDecryptIt
{
    /// <summary>
    /// nothing
    /// </summary>
    public class GlobalVars
    {
        /// <summary>
        /// nothing
        /// </summary>
        private static string[] _executionargs = null;
        /// <summary>
        /// nothing
        /// </summary>
        private static string[] _versionArr = new string[] {
            "5",
            "10",
            "0",
            "2B39"};
        /// <summary>
        /// nothing
        /// </summary>
        private static string _version = "5.10.0.2B39";


        /// <summary>
        /// nothing
        /// </summary>
        public static string[] executionargs
        {
            get
            {
                return _executionargs;
            }
            set
            {
                _executionargs = value;
            }
        }
        /// <summary>
        /// nothing
        /// </summary>
        public static string[] versionArr
        {
            get
            {
                return _versionArr;
            }
        }
        /// <summary>
        /// nothing
        /// </summary>
        public static string version
        {
            get
            {
                return _version;
            }
        }
    }
}
