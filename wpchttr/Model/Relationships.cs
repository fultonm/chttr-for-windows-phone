using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace wpchttr.Model
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
        }

        private string GetRelationshipsResponse()
        {
            var relationshipUrl = Session.BASE_URL + "/relationship_stats";
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

            ParseFollowing(jo);
            ParseFollowers(jo);

            return true;
        }

        private void ParseFollowers(JObject jo)
        {
            try
            {
                Followers = (int) jo.SelectToken("followers");
            }
            catch (Exception e)
            {
                ErrorInformation.Add(e.ToString());
            }
        }

        private void ParseFollowing(JObject jo)
        {
            try
            {
                Following = (int) jo.SelectToken("following");
            }
            catch (Exception e)
            {
                ErrorInformation.Add(e.ToString());
            }
        }

        #region fields;

        private int followers;

        public int Followers
        {
            get { return followers; }

            set
            {
                followers = value;
                OnPropertyChanged("Followers");
            }
        }

        private int following;

        public int Following
        {
            get { return following; }

            set
            {
                following = value;
                OnPropertyChanged("Following");
            }
        }

        public List<string> ErrorInformation { get; set; }

        #endregion
    }
}