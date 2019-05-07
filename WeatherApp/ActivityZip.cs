using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.Squareup.Picasso;
using Newtonsoft.Json;
using WeatherApp.Model;

namespace WeatherApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ActivityZip : AppCompatActivity
    {
        TextView txtZip, txtLastUpdateZip, txtDescriptionZip, txtHumidityZip, txtTimeZip, txtCelsiusZip;
        ImageView imgViewZip;
        EditText zipcode;
        Button btngo,btnHomeZip;
        string zip;
        OpenWeatherMap openWeatherMap = new OpenWeatherMap();

        public object NavigationPage { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.layout_zip);
            zipcode = FindViewById<EditText>(Resource.Id.editZip);


            btnHomeZip = FindViewById<Button>(Resource.Id.buttonHomeZip);
            btnHomeZip.Click += (object sender, EventArgs e) => {
                btnHomeZip_Click(sender, e);
            };

        btngo = FindViewById<Button>(Resource.Id.buttonZipGo);
            btngo.Click += (object sender, EventArgs e) => {
                btngo_Click(sender, e);
            };
        }
        
        private void btnHomeZip_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(HomeActivity));
            Finish();
        }

        private void btngo_Click(object sender, EventArgs e)
        {
            zip = zipcode.Text;
            if (zip.Length > 0)
            {
                new GetWeather(this, openWeatherMap).Execute(Common.Common.APIRequest(zip));
            }
            else
            {
                Toast.MakeText(this, " Kindly insert City Name", ToastLength.Short).Show();
            }

        }

        private class GetWeather : AsyncTask<string, Java.Lang.Void, string>
        {
            //private ProgressDialog pd = new ProgressDialog(Application.Context);
            OpenWeatherMap openWeatherMap;
            private ActivityZip activity;

            public GetWeather(ActivityZip activity, OpenWeatherMap openWeatherMap)
            {
                this.activity = activity;
                this.openWeatherMap = openWeatherMap;
            }


            protected override void OnPreExecute()
            {
                base.OnPreExecute();

                //pd.Window.SetType(Android.Views.WindowManagerTypes.SystemAlert);
                //pd.SetTitle("Please wait....");
                //pd.Show();
            }
            protected override string RunInBackground(params string[] @params)
            {
                string stream = null;
                string urlString = @params[0];
                Helper.Helper http = new Helper.Helper();
                //urlString = Common.Common.APIRequest(lat.ToString(), lng.ToString());
                stream = http.GetHTTPData(urlString);
                return stream;
            }
            protected override void OnPostExecute(string result)
            {

                //Controls   
                activity.txtZip = activity.FindViewById<TextView>(Resource.Id.txtZip);
                activity.txtLastUpdateZip = activity.FindViewById<TextView>(Resource.Id.txtLastUpdateZip);
                activity.txtDescriptionZip = activity.FindViewById<TextView>(Resource.Id.txtDescriptionZip);
                activity.txtHumidityZip = activity.FindViewById<TextView>(Resource.Id.txtHumidityZip);
                activity.txtTimeZip = activity.FindViewById<TextView>(Resource.Id.txtTimeZip);
                activity.txtCelsiusZip = activity.FindViewById<TextView>(Resource.Id.txtCelsiusZip);
                activity.imgViewZip = activity.FindViewById<ImageView>(Resource.Id.imageViewZip);
                try
                {
                    base.OnPostExecute(result);
                    if (result.Contains("Error: Not City Found"))
                    {
                        //  pd.Dismiss();
                        activity.txtZip.Text = "Error: Not City Found";
                        return;
                    }
                    openWeatherMap = JsonConvert.DeserializeObject<OpenWeatherMap>(result);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception : " + ex.Message);
                    activity.txtZip.Text = "Error: Not City Found ";
                    return;
                }


                //pd.Dismiss();

                //Add Data   
                activity.txtZip.Text = $"{openWeatherMap.name},{openWeatherMap.sys.country}";
                activity.txtLastUpdateZip.Text = $"Last Updated:{DateTime.Now.ToString("dd MMMM yyyy HH: mm ")}";
                activity.txtDescriptionZip.Text = $"{openWeatherMap.weather[0].description}";
                activity.txtHumidityZip.Text = $"Humidity:{ openWeatherMap.main.humidity} % ";
                activity.txtTimeZip.Text = $"{Common.Common.UnixTimeStampToDateTime(openWeatherMap.sys.sunrise).ToString("HH: mm")}/{Common.Common.UnixTimeStampToDateTime(openWeatherMap.sys.sunset).ToString("HH: mm ")}";
                activity.txtCelsiusZip.Text = $"{openWeatherMap.main.temp} °C";
                if (!string.IsNullOrEmpty(openWeatherMap.weather[0].icon))
                {
                    Picasso.With(activity.ApplicationContext).Load(Common.Common.GetImage(openWeatherMap.weather[0].icon)).Into(activity.imgViewZip);
                }
            }
        }
    }
}