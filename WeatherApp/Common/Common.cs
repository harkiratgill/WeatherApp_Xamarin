using System;
using System.Text;

namespace WeatherApp.Common
{
    class Common
    {
        public static string API_KEY = "4d49f6319aa23f8f5fbdd54dbfa9b468";
        public static string API_LINK = "https://api.openweathermap.org/data/2.5/weather";
        //public static string APIRequest(string lat, string lng)
        public static string APIRequest(string city)
        {
            StringBuilder sb = new StringBuilder(API_LINK);
            sb.AppendFormat("?q={0}&APPID={1}&units=metric", city, API_KEY);
            //sb.AppendFormat("?lat={0}&lon={1}&APPID={2}&units=metric", lat, lng, API_KEY);
            return sb.ToString();
        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(unixTimeStamp).ToLocalTime();
            return dt;
        }
        public static string GetImage(string icon)
        {
            return "http://openweathermap.org/img/w/"+icon+".png";
        }


    }
}