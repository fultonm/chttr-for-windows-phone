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
    public class Feed
    {
        public List<Chat> ChatCollection { get; set; }

        public Feed()
        {
            GetFeed();
        }

        private void GetFeed()
        {
            string response = GetFeedResponse();
            ParseFeedResponse(response);
        }

        private string GetFeedResponse()
        {
            var feedUrl = Session.BASE_URL + "/feed";
            var client = new HttpClient();
            client.BaseAddress = new Uri(feedUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client.GetStringAsync(feedUrl).Result;
        }

        private void ParseFeedResponse(string response)
        {
            ChatCollection = new List<Chat>();
            JArray ja = JArray.Parse(response);
            for (int i = 0; i < ja.Count; i++)
            {
                ParseChat(ja[i] as JObject);
            }
        }

        private void ParseChat(JObject joChat)
        {
            Chat chat = new Chat();
            chat.ChatId = joChat.ParseJsonInt("id");
            chat.UserId = joChat.ParseJsonInt("user_id");
            chat.Content = joChat.ParseJsonString("content");
            chat.CreatedAt = joChat.ParseJsonDateTime("created_at");
            chat.UpdatedAt = joChat.ParseJsonDateTime("updated_at");
            chat.PictureFileName = (joChat.SelectToken("picture") as JObject).ParseJsonString("url");
            ChatCollection.Add(chat);
        }
    }
}
