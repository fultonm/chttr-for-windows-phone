using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpchttr.Core
{
    public static class Extensions
    {
        public static string ParseJsonString(this JObject jo, string token)
        {
            string parsed = string.Empty;
            try
            {
                parsed = (string)jo.SelectToken(token);
            }
            catch (Exception)
            {

            }
            return parsed;
        }
        public static int ParseJsonInt(this JObject jo, string token)
        {
            int parsed = int.MinValue;
            try
            {
                parsed = (int)jo.SelectToken(token);
            }
            catch (Exception)
            {

            }
            return parsed;
        }
        public static DateTime ParseJsonDateTime(this JObject jo, string token)
        {
            DateTime parsed = DateTime.MinValue;
            try
            {
                parsed = (DateTime)jo.SelectToken(token);
            }
            catch (Exception)
            {

            }
            return parsed;
        }
    }
}
