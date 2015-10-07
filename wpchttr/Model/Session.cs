﻿namespace wpchttr.Model
{
    internal class Session
    {
        public static string BASE_URL = "http://localhost:3000";
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