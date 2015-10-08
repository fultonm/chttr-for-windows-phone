using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpchttr.Core
{
    public class Chat
    {
        public int ChatId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string PictureFileName { get; set; }
    }
}
