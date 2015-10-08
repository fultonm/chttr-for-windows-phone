using System;

namespace wpchttr.Core
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
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

        public User(int id, string name, DateTime createdAt)
        {
            Id = id;
            Name = name;
            CreatedAt = createdAt;
        }
    }
}
