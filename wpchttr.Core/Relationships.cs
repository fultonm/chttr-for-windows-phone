using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace wpchttr.Core
{
    public class Relationships : INotifyPropertyChanged
    {
        public Relationships()
        {
            CurrentUserRelationships();
            ErrorInformation = new List<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propName));
        }

        private void CurrentUserRelationships()
        {
            var response = GetRelationshipsResponse();
            ParseResponse(response);
            FollowingCount = Following.Count;
            FollowersCount = Followers.Count;
        }

        private string GetRelationshipsResponse()
        {
            var relationshipUrl = Session.BASE_URL + "/relationships";
            var client = new HttpClient();
            client.BaseAddress = new Uri(relationshipUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync(relationshipUrl).Result;
            return response.Content.ReadAsStringAsync().Result;
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
                    DateTime userCreatedAt = joUser.ParseJsonDateTime("created_at");
                    User user = new User(userId, userName, userCreatedAt);
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

        public List<User> Followers { get; set; }

        public List<User> Following { get; set; }

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