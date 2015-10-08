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
    public class Chats
    {
        public List<Chat> ChatCollection { get; set; }

        public Chats(int userId, int page = 1)
        {
            GetUserChats(userId, page);
        }

        private void GetUserChats(int userId, int page)
        {
            string response = GetChatsResponse(userId, page);
            ParseChatsResponse(response);
        }

        private string GetChatsResponse(int userId, int page)
        {
            var relationshipUrl = Session.BASE_URL + "/chats";
            var client = new HttpClient();
            client.BaseAddress = new Uri(relationshipUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string json = BuildChatsRequestJson(userId, page);
            var response = client.PostAsJsonAsync(relationshipUrl, json).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        private string BuildChatsRequestJson(int userId, int page)
        {
            var chatRequest = new ChatsRequest(userId, page);
            return JsonConvert.SerializeObject(chatRequest);
        }

        private void ParseChatsResponse(string response)
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
