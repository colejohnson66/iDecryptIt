namespace iDecryptIt_WPF
{
    public class GlobalClass
    {
        private static string m_globalVar = "";

        public static string GlobalVar
        {
            get
            {
                return m_globalVar;
            }
            set
            {
                m_globalVar = value;
            }
        }
    }
}
