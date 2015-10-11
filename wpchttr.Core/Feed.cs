using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace wpchttr.Core
{
    public class Feed : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propName));
        }

        private List<Chat> chatCollection;
        public List<Chat> ChatCollection
        {
            get { return chatCollection; }
            set
            {
                OnPropertyChanged("ChatCollection");
                chatCollection = value;
            }
        }

        private Relationships Relationships { get; set; }

        public Feed(Relationships relationships)
        {
            Relationships = relationships;
            RefreshFeed();
        }

        public async Task RefreshFeed()
        {
            await GetFeed();
        }

        private async Task GetFeed()
        {
            string response = await GetFeedResponse();
            ParseFeedResponse(response);
        }

        private async Task<string> GetFeedResponse()
        {
            var feedUrl = Session.BASE_URL + "/feed";
            var client = new HttpClient();
            client.BaseAddress = new Uri(feedUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await client.GetStringAsync(feedUrl);
        }

        private void ParseFeedResponse(string response)
        {
            ChatCollection = new List<Chat>();
            JArray ja = JArray.Parse(response);
            for (int i = 0; i < ja.Count; i++)
            {
                ParseChat(ja[i] as JObject);
            }
            int j = 0;
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

            if (chat.UserId != CurrentUser.Id)
            {
                chat.UserName = Relationships.Followers.Concat(Relationships.Following).Where(u => u.Id == chat.UserId).First().Name;
                chat.UserGravatarUrl = Relationships.Followers.Concat(Relationships.Following).Where(u => u.Id == chat.UserId).First().GravatarUrl;
            }
            else
            {
                chat.UserName = CurrentUser.Name;
                chat.UserGravatarUrl = CurrentUser.GravatarUrl;
            }
            ChatCollection.Add(chat);
        }
    }
}
