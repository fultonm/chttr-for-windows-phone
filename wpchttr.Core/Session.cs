namespace wpchttr.Core
{
    internal class Session
    {
        public static string BASE_URL = "http://chttr.wartimestudios.com/api/";
        public static string GRAVATAR_BASE_URL = "http://www.gravatar.com/avatar/";

        public Session(string email, string password)
        {
            Password = password;
            Email = email;
        }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}