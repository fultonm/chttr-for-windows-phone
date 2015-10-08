using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace wpchttr.Core
{
    public class Chat
    {
        public Chat(string content)
        {
            Content = content;
        }

        public Chat()
        {

        }

        public int ChatId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string PictureFileName { get; set; }

        public bool Submit()
        {
            if (String.IsNullOrEmpty(Content))
            {
                return false;
            }
            string url = Session.BASE_URL + "submit_chat";
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string request = BuildJsonRequest();
            string response = client.PostAsJsonAsync(url, request).Result.Content.ReadAsStringAsync().Result;
            return ParseJsonResponse(response);
        }

        private string BuildJsonRequest()
        {
            return JsonConvert.SerializeObject(this);
        }

        private bool ParseJsonResponse(string response)
        {
            return bool.Parse(JObject.Parse(response).SelectToken("success").ToString());
        }
    }
}
