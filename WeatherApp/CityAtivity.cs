using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Locations;
using WeatherApp.Model;
using System;
using Newtonsoft.Json;
using Com.Squareup.Picasso;
using Android.Content;

namespace WeatherApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class CityActivity : AppCompatActivity
    {
        TextView txtCity, txtLastUpdate, txtDescription, txtHumidity, txtTime, txtCelsius;
        ImageView imgView;
        EditText cityname;
        Button btngo,btnCityHome;
        string provider,city;
        OpenWeatherMap openWeatherMap = new OpenWeatherMap();


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_city);
            cityname = FindViewById<EditText>(Resource.Id.editCity);

            //NavigationPage.SetHasBackButton(this, false);

            btnCityHome = FindViewById<Button>(Resource.Id.buttonHome);
            btnCityHome.Click += (object sender, EventArgs e) => {
                btnCityHome_Click(sender, e);
            };

            btngo = FindViewById<Button>(Resource.Id.buttonGo);
            btngo.Click += (object sender, EventArgs e) => {
                btngo_Click(sender, e);
            };


            if (provider == null)
            {
                System.Diagnostics.Debug.WriteLine("No location provider found!");
                return;
            }

        }

        private void btnCityHome_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(HomeActivity));
            Finish();
        }

        private void btngo_Click(object sender, EventArgs e)
        {
            city = cityname.Text;
            if (city.Length > 0)
            {
                new GetWeather(this, openWeatherMap).Execute(Common.Common.APIRequest(city));
            }
            else
            {
                Toast.MakeText(this, " Kindly insert City Name", ToastLength.Short).Show();
            }
            
        }

        private class GetWeather : AsyncTask<string, Java.Lang.Void, string>
        {
            private CityActivity activity;
            OpenWeatherMap openWeatherMap;

            public GetWeather(CityActivity activity, OpenWeatherMap openWeatherMap)
            {
                this.activity = activity;
                this.openWeatherMap = openWeatherMap;
            }

            protected override void OnPreExecute()
            {
                base.OnPreExecute();
            }
            protected override string RunInBackground(params string[] @params)
            {
                string stream = null;
                string urlString = @params[0];
                Helper.Helper http = new Helper.Helper();
                stream = http.GetHTTPData(urlString);
                return stream;
            }
            protected override void OnPostExecute(string result)
            {

                //Controls   
                activity.txtCity = activity.FindViewById<TextView>(Resource.Id.txtCity);
                activity.txtLastUpdate = activity.FindViewById<TextView>(Resource.Id.txtLastUpdate);
                activity.txtDescription = activity.FindViewById<TextView>(Resource.Id.txtDescription);
                activity.txtHumidity = activity.FindViewById<TextView>(Resource.Id.txtHumidity);
                activity.txtTime = activity.FindViewById<TextView>(Resource.Id.txtTime);
                activity.txtCelsius = activity.FindViewById<TextView>(Resource.Id.txtCelsius);
                activity.imgView = activity.FindViewById<ImageView>(Resource.Id.imageView);
                try
                {
                    base.OnPostExecute(result);
                    if (result.Contains("Error: Not City Found"))
                    {
                        activity.txtCity.Text = "Error: Not City Found";
                        return;
                    }
                    openWeatherMap = JsonConvert.DeserializeObject<OpenWeatherMap>(result);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception : " + ex.Message);
                    activity.txtCity.Text = "Error: Not City Found ";
                    return;
                }
                //Add Data   
                activity.txtCity.Text = $"{openWeatherMap.name},{openWeatherMap.sys.country}";
                activity.txtLastUpdate.Text = $"Last Updated:{DateTime.Now.ToString("dd MMMM yyyy HH: mm ")}";
                activity.txtDescription.Text = $"{openWeatherMap.weather[0].description}";
                activity.txtHumidity.Text = $"Humidity:{ openWeatherMap.main.humidity} % ";
                activity.txtTime.Text = $"{Common.Common.UnixTimeStampToDateTime(openWeatherMap.sys.sunrise).ToString("HH: mm")}/{Common.Common.UnixTimeStampToDateTime(openWeatherMap.sys.sunset).ToString("HH: mm ")}";
                activity.txtCelsius.Text = $"{openWeatherMap.main.temp} °C";
                if (!string.IsNullOrEmpty(openWeatherMap.weather[0].icon))
                {
                    Picasso.With(activity.ApplicationContext).Load(Common.Common.GetImage(openWeatherMap.weather[0].icon)).Into(activity.imgView);
                }
            }
        }
    }
}