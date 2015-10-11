using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace wpchttr.Core
{
    public class Relationships : INotifyPropertyChanged
    {
        public Relationships()
        {
            Followers = Following = new List<User>();
            ErrorInformation = new List<string>();
            RefreshRelationships();
        }

        public async Task RefreshRelationships()
        {
            await CurrentUserRelationships();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propName));
        }

        private async Task CurrentUserRelationships()
        {
            var response = await GetRelationshipsResponse();
            ParseResponse(response);
            FollowingCount = Following.Count;
            FollowersCount = Followers.Count;
        }

        private async Task<string> GetRelationshipsResponse()
        {
            var relationshipUrl = Session.BASE_URL + "/relationships";
            var client = new HttpClient();
            client.BaseAddress = new Uri(relationshipUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync(relationshipUrl).Result;
            return await response.Content.ReadAsStringAsync();
        }

        private bool ParseResponse(string response)
        {
            JObject jo;
            try
            {
                jo = JObject.Parse(response);
            }
            catch (Exception e)
            {
                ErrorInformation.Add(e.ToString());
                return false;
            }
            Followers = ParseUsers(jo, "followers");
            Following = ParseUsers(jo, "following");

            return true;
        }

        private List<User> ParseUsers(JObject jo, string group)
        {
            List<User> users = new List<User>();
            try
            {
                JArray jaUsers = jo.SelectToken(group) as JArray;
                foreach (JObject joUser in jaUsers)
                {
                    int userId = joUser.ParseJsonInt("id");
                    string userName = joUser.ParseJsonString("name");
                    string email = joUser.ParseJsonString("email");
                    DateTime userCreatedAt = joUser.ParseJsonDateTime("created_at");
                    User user = new User(userId, userName, email, userCreatedAt);
                    users.Add(user);
                }
            }
            catch (Exception e)
            {
                ErrorInformation.Add(e.ToString());
            }
            return users;
        }

        #region fields;

        private List<User> followers;
        public List<User> Followers
        {
            get { return followers; }
            set
            {
                OnPropertyChanged("Followers");
                followers = value;
            }
        }

        private List<User> following;
        public List<User> Following
        {
            get { return following; }
            set
            {
                OnPropertyChanged("Following");
                following = value;
            }
        }

        public List<User> AllRelationships { get; set; }

        private int followersCount;

        public int FollowersCount
        {
            get { return followersCount; }

            set
            {
                followersCount = value;
                OnPropertyChanged("FollowersCount");
            }
        }

        private int followingCount;

        public int FollowingCount
        {
            get { return followingCount; }

            set
            {
                followingCount = value;
                OnPropertyChanged("FollowingCount");
            }
        }

        internal void Refresh()
        {
            CurrentUserRelationships();
        }

        public List<string> ErrorInformation { get; set; }

        #endregion
    }
}