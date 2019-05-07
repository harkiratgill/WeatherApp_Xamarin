using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WeatherApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class HomeActivity : Activity
    {
        Button BtnCity, BtnZip;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.layout_home);


            BtnCity = FindViewById<Button>(Resource.Id.button_City);
            BtnCity.Click += (object sender, EventArgs e) => {
                BtnCity_Click(sender, e);
            };
            BtnZip = FindViewById<Button>(Resource.Id.button_Zip);
            BtnZip.Click += (object sender, EventArgs e) => {
                BtnZip_Click(sender, e);
            };
 
            //TimePicker Tpicker = FindViewById<TimePicker>(Resource.Id.TimePicker1);

            //void setCurrentTime()
            //{
            //    string time = string.Format("{0}",
            //        DateTime.Now.ToString("HH:mm").PadLeft(2, '0'));
            //}

        }

        private void BtnZip_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ActivityZip));
            Finish();
        }

        private void BtnCity_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(CityActivity));
            Finish();
        }
    }
}