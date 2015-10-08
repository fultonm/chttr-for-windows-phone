using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpchttr.Core
{
    class ChatsRequest
    {
        public int UserId { get; set; }
        public int Page { get; set; }

        public ChatsRequest(int userId, int page)
        {
            UserId = userId;
            Page = page;
        }
    }
}
