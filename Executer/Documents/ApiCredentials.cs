namespace Executer.Documents
{
    public class ApiCredentials
    {
        public ApiCredentials()//fcoin3
        { }
        private static string publicApiKey = "";
        public static string Get_publicApiKey() { return publicApiKey; }

        private static string privateApiKey = "";
        public static string Get_privateApiKey() { return privateApiKey; }
    }
}
