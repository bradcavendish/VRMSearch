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

using SQLite;
using Newtonsoft.Json;

namespace JDMReview
{
    [Activity(Label = "History")]
    public class History : Activity
    {

        private List<Car> carsInDb;
        private ListView listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);

            //set layout
            SetContentView(Resource.Layout.search_history_page);           

            //create empty list of cars
            carsInDb = new List<Car>();
            //make reference to listView
            listView = FindViewById<ListView>(Resource.Id.carListView);
            //create click event for listview item
            listView.ItemClick += ListView_ItemClick;

            //get db path from intent and create connection
            var dbPath = Intent.GetStringExtra("dbPath");
            var db = new SQLiteConnection(dbPath);

            //connect to car table
            var table = db.Table<Car>();

            //create a car object for each roq found in table
            foreach (var c in table)
            {
                Car car = new Car();


                car.Aspiration = c.Aspiration;
                car.Body = c.Body;
                car.CamType = c.CamType;
                car.Colour = c.Colour;
                car.Consumption = c.Consumption;
                car.CylinderArrangement = c.CylinderArrangement;
                car.Date = c.Date;
                car.EngineSize = c.EngineSize;
                car.Fuel = c.Fuel;
                car.Make = c.Make;
                car.Model = c.Model;
                car.NumOfCylinders = c.NumOfCylinders;
                car.Power = c.Power;
                car.Reg = c.Reg;
                car.TopSpeed = c.TopSpeed;
                car.Torque = c.Torque;
                car.Transmission = c.Transmission;
                car.ValvesPerCylinder = c.ValvesPerCylinder;

                //add car object to array
                carsInDb.Add(car);           
            }

            //reverse order of list to get most recent at top
            carsInDb.Reverse();

            CarListViewAdapter adapter = new CarListViewAdapter(this, carsInDb);
            listView.Adapter = adapter;

        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //get the car at position clicked
            Car car = carsInDb[e.Position];

            //use intent to show car detail page with car clicked on

            //serialize that car
            var serializedCar = JsonConvert.SerializeObject(car);

            //create intent
            var intent = new Intent(this, typeof(CarDetailPage));

            //attach serialized car to intent
            intent.PutExtra("car", serializedCar);

            //start activity
            StartActivity(intent);
            
        }
    }
}