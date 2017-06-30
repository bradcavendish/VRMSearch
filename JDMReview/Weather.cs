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

namespace JDMReview
{
    class Weather
    {

        private double temp;
        private double humidity;

        public Weather(double temp, double humidity)
        {
            this.temp = temp;
            this.humidity = humidity;
        }

        public double Temp
        {
            get { return temp; }
        }
        public double Humidity
        {
            get { return humidity; }
        }

    }
}