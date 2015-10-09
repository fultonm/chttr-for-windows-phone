using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XLabs.Cryptography;

namespace wpchttr.Core
{
    public class CurrentUser : INotifyPropertyChanged
    {
        public CurrentUser(string email, string password)
        {
            Email = email;
            Password = password;
            ErrorInformation = new List<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propName));
        }

        public bool SignIn()
        {
            var jsonRequest = BuildJsonRequest(Email, Password);

            try
            {
                var response = PostSignIn(jsonRequest);
                var allFieldsPopulated = ParseResponse(response);
            }
            catch (Exception)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(Email))
            {
                GetGravatarUrl(Email);
            }
            return true;
        }

        private void GetGravatarUrl(string rawEmail)
        {
            GravatarUrl = Session.GRAVATAR_BASE_URL + MD5.GetMd5String(rawEmail);
        }

        private string PostSignIn(string jsonRequest)
        {
            var signInUrl = "http://localhost:3000/sign_in";
            var client = new HttpClient();
            client.BaseAddress = new Uri(signInUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.PostAsJsonAsync(signInUrl, jsonRequest).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        private string BuildJsonRequest(string email, string password)
        {
            var session = new Session(email, password);
            return JsonConvert.SerializeObject(session);
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

            ParseId(jo);
            ParseName(jo);
            ParseCreatedAt(jo);
            ParseUpdatedAt(jo);
            ParsePasswordDigest(jo);
            ParseRememberDigest(jo);
            ParseActivatedDigest(jo);
            ParseAdmin(jo);
            ParseActivated(jo);
            ParseActivatedAt(jo);
            ParseResetDigest(jo);
            ParseResetRequestedAt(jo);

            return true;
        }

        private void ParseId(JObject jo)
        {
            try
            {
                Id = (int) jo.SelectToken("id");
            }
            catch (Exception e)
            {
                ErrorInformation.Add(e.ToString());
            }
        }

        private void ParseName(JObject jo)
        {
            try
            {
                Name = (string) jo.SelectToken("name");
            }
            catch (Exception e)
            {
                ErrorInformation.Add(e.ToString());
            }
        }

        private void ParseCreatedAt(JObject jo)
        {
            var jt = jo.SelectToken("created_at");
            if (string.IsNullOrEmpty(jt.ToString()))
            {
                return;
            }
            try
            {
                CreatedAt = (DateTime) jt;
            }
            catch (Exception e)
            {
                ErrorInformation.Add(e.ToString());
            }
        }

        private void ParseUpdatedAt(JObject jo)
        {
            var jt = jo.SelectToken("updated_at");
            if (string.IsNullOrEmpty(jt.ToString()))
            {
                return;
            }
            try
            {
                UpdatedAt = (DateTime) jt;
            }
            catch (Exception e)
            {
                ErrorInformation.Add(e.ToString());
            }
        }

        private void ParsePasswordDigest(JObject jo)
        {
            try
            {
                PasswordDigest = (string) jo.SelectToken("password_digest");
            }
            catch (Exception e)
            {
                ErrorInformation.Add(e.ToString());
            }
        }

        private void ParseRememberDigest(JObject jo)
        {
            try
            {
                RememberDigest = (string) jo.SelectToken("remember_digest");
            }
            catch (Exception e)
            {
                ErrorInformation.Add(e.ToString());
            }
        }

        private void ParseActivatedDigest(JObject jo)
        {
            try
            {
                ActivationDigest = (string) jo.SelectToken("activation_digest");
            }
            catch (Exception e)
            {
                ErrorInformation.Add(e.ToString());
            }
        }

        private void ParseAdmin(JObject jo)
        {
            try
            {
                Admin = (bool) jo.SelectToken("admin");
            }
            catch (Exception e)
            {
                ErrorInformation.Add(e.ToString());
            }
        }

        private void ParseActivated(JObject jo)
        {
            try
            {
                Activated = (bool) jo.SelectToken("activated");
            }
            catch (Exception e)
            {
                ErrorInformation.Add(e.ToString());
            }
        }

        private void ParseActivatedAt(JObject jo)
        {
            var jt = jo.SelectToken("activated_at");
            if (string.IsNullOrEmpty(jt.ToString()))
            {
                return;
            }
            try
            {
                ActivatedAt = (DateTime) jt;
            }
            catch (Exception e)
            {
                ErrorInformation.Add(e.ToString());
            }
        }

        private void ParseResetDigest(JObject jo)
        {
            try
            {
                ResetDigest = (string) jo.SelectToken("reset_digest");
            }
            catch (Exception e)
            {
                ErrorInformation.Add(e.ToString());
            }
        }

        private void ParseResetRequestedAt(JObject jo)
        {
            var jt = jo.SelectToken("reset_requested_at");
            if (string.IsNullOrEmpty(jt.ToString()))
            {
                return;
            }
            try
            {
                ResetRequestedAt = (DateTime) jt;
            }
            catch (Exception e)
            {
                ErrorInformation.Add(e.ToString());
            }
        }

        #region fields

        public int Id { get; set; }

        public string Name { get; set; }

        private string email;

        public string Email
        {
            get { return email; }

            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        public Relationships Relationships { get; set; }

        public Feed Feed { get; set; }

        public Chats Chats { get; set; }

        public string GravatarUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string PasswordDigest { get; set; }

        public string RememberDigest { get; set; }

        public string ActivationDigest { get; set; }

        public bool Admin { get; set; }

        public bool Activated { get; set; }

        public DateTime ActivatedAt { get; set; }

        public string ResetDigest { get; set; }

        public DateTime ResetRequestedAt { get; set; }

        public List<string> ErrorInformation { get; set; }

        #endregion
    }
}