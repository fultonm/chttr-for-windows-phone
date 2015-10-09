using System;
using XLabs.Cryptography;

namespace wpchttr.Core
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                GravatarUrl = Session.GRAVATAR_BASE_URL + MD5.GetMd5String(Email);
            }
        }
        public string GravatarUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        private Chats chats;
        public Chats Chats
        {
            get
            {
                if (chats == null)
                {
                    chats = new Chats(Id);
                }
                return chats;
            }
            private set { chats = value; }
        }

        public User(int id, string name, string email, DateTime createdAt)
        {
            Id = id;
            Name = name;
            Email = email;
            CreatedAt = createdAt;
        }
    }
}
