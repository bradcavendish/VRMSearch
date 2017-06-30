using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace JDMReview
{
    [Activity(Label = "CarDetailPage")]
    public class CarDetailPage : Activity
    {

        private Car car;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //hide title
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.car_detail_page);

            car = JsonConvert.DeserializeObject<Car>(Intent.GetStringExtra("car"));

            displayInfo(car);
            
        }

        private void displayInfo(Car car)
        {
            //connect text views
            TextView reg = FindViewById<TextView>(Resource.Id.regTextView);
            TextView make = FindViewById<TextView>(Resource.Id.makeTextView);
            TextView model = FindViewById<TextView>(Resource.Id.modelTextView);
            TextView body = FindViewById<TextView>(Resource.Id.bodyTextView);
            TextView colour = FindViewById<TextView>(Resource.Id.colourTextView);
            TextView date = FindViewById<TextView>(Resource.Id.dateTextView);
            TextView fuel = FindViewById<TextView>(Resource.Id.fuelTextView);
            TextView transmission = FindViewById<TextView>(Resource.Id.transmissionTextView);
            TextView aspiration = FindViewById<TextView>(Resource.Id.aspirationTextView);
            TextView engineSize = FindViewById<TextView>(Resource.Id.sizeTextView);
            TextView cylinders = FindViewById<TextView>(Resource.Id.cylindersTextView);
            TextView arrangement = FindViewById<TextView>(Resource.Id.arrangementTextView);
            TextView valvesPerCylinder = FindViewById<TextView>(Resource.Id.valvesTextView);
            TextView camType = FindViewById<TextView>(Resource.Id.camTextView);
            TextView power = FindViewById<TextView>(Resource.Id.hpTextView);
            TextView torque = FindViewById<TextView>(Resource.Id.torqueTextView);
            TextView topSpeed = FindViewById<TextView>(Resource.Id.topspeedTextView);
            TextView consumption = FindViewById<TextView>(Resource.Id.consumptionTextView);

            //set text views to car's properties
            reg.Text = car.Reg;
            make.Text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(car.Make.ToLower());
            model.Text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(car.Model.ToLower());
            body.Text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(car.Body.ToLower());
            colour.Text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(car.Colour.ToLower());
            date.Text = car.Date;
            fuel.Text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(car.Fuel.ToLower());
            transmission.Text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(car.Transmission.ToLower());            
            aspiration.Text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(car.Aspiration.ToLower());
            engineSize.Text = car.EngineSize.ToString() + "cc";
            cylinders.Text = car.NumOfCylinders.ToString();
            arrangement.Text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(car.CylinderArrangement.ToLower());
            valvesPerCylinder.Text = car.ValvesPerCylinder.ToString();
            camType.Text = car.CamType;
            power.Text = car.Power.ToString();
            torque.Text = car.Torque.ToString();
            topSpeed.Text = car.TopSpeed.ToString();
            consumption.Text = car.Consumption.ToString();
        }
    }
}