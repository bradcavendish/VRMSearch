using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

using SQLite;


namespace JDMReview
{
    [Activity(Label = "JDMReview", MainLauncher = true, Icon = "@drawable/icon")]

    

    public class MainActivity : Activity
    {

        //create db path
        static string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "dbCar.db3");
        SQLiteConnection db = new SQLiteConnection(dbPath);

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //hide title
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            Button goButton = FindViewById<Button>(Resource.Id.button1);
            Button historyButton = FindViewById<Button>(Resource.Id.historyButton);

            
            //set up db table incase it is first time and db does not exist
            db.CreateTable<Car>();

            //handle button click
            goButton.Click += GoButton_Click;
            historyButton.Click += HistoryButton_Click;
                      
        }

        private void HistoryButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(History));
            intent.PutExtra("dbPath", dbPath);
            StartActivity(intent);
        }

        private async void GoButton_Click (object sender, EventArgs e)
        {
            //link for edit text with reg no.
            EditText regNo = FindViewById<EditText>(Resource.Id.editText1);

            //create progress dialog
            ProgressDialog m = new ProgressDialog(this);
            m.SetCancelable(false);
            m.SetMessage("Fetching Car Info...");

            //use progressdialog to show searching
            m.Show();

            //find car with api
            //get text in text box
            string regWithSpace = regNo.Text;
            //remove spaces with regex
            string reg = Regex.Replace(regWithSpace, @"\s+", "");
            //make url
            string url = "https://uk1.ukvehicledata.co.uk/api/datapackage/VehicleData?api_rv=2&api_nullitems=1&auth_apikey=0a5a9dd8-e7ea-4494-84a3-13c5c597ddc9&key_VRM=" + reg;

            JsonValue json = await Fetch(url);
            m.Hide();
            //get status code
            string statusCode = getStatusCode(json);

            if (statusCode == "KeyInvalid")
            {
                Toast.MakeText(this, "Invalid VRM", ToastLength.Long).Show();
            }
            else if (statusCode == "ServiceUnavailable")
            {
                Toast.MakeText(this, "VRM Not Found", ToastLength.Long).Show();
            }
            else if (statusCode == "Success")
            {
                //create car object from json
                Car car = getCarFromJSON(json);
                car.Reg = reg;

                //save the car in local db
                saveToDB(car, db);

                //convert car object to jsoon serialized so can be passed with intent
                var serializedCar = JsonConvert.SerializeObject(car);
                //create intent
                var intent = new Intent(this, typeof(CarDetailPage));
                //add the serialized car to the intent with a key of car
                intent.PutExtra("car", serializedCar);
                //start the intent
                StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this, "Error", ToastLength.Long).Show();
            }

        }

        private void saveToDB(Car car, SQLiteConnection db)
        {          
            //store car
            db.Insert(car);
        }
       

        private Car getCarFromJSON(JsonValue json)
        {
            //create empty car object
            Car c = new Car();

            //open up json data
            JsonValue response = json["Response"];
            JsonValue dataItems = response["DataItems"];
            JsonValue vehicleRegistration = dataItems["VehicleRegistration"];
            JsonValue technicalDetails = dataItems["TechnicalDetails"];
            JsonValue general = technicalDetails["General"];
            JsonValue engine = general["Engine"];
            JsonValue performance = technicalDetails["Performance"];
            JsonValue torque = performance["Torque"];
            JsonValue power = performance["Power"];
            JsonValue maxSpeed = performance["MaxSpeed"];
            JsonValue consumption = technicalDetails["Consumption"];
            JsonValue combined = consumption["Combined"];

            //fill in car object
            c.Colour = vehicleRegistration["Colour"] ?? "N/A";
            c.EngineSize = vehicleRegistration["EngineCapacity"] ?? "N/A";
            c.Model = vehicleRegistration["Model"] ?? "N/A";
            c.Body = vehicleRegistration["DoorPlanLiteral"] ?? "N/A";

            //get part of date i want
            string date = vehicleRegistration["DateFirstRegisteredUk"] ?? "N/A";
            if (date != "N/A")
            {
                c.Date = date.Substring(0, 10);
            }
            else
            {
                c.Date = date;
            }

            c.Make = vehicleRegistration["Make"] ?? "N/A";
            c.Transmission = vehicleRegistration["TransmissionType"] ?? "N/A";
            c.Fuel = vehicleRegistration["FuelType"] ?? "N/A";
            c.ValvesPerCylinder = engine["ValvesPerCylinder"] ?? 0;
            c.Aspiration = engine["Aspiration"] ?? "N/A";
            c.NumOfCylinders = engine["NumberOfCylinders"] ?? 0;
            c.CylinderArrangement = engine["CylinderArrangement"] ?? "N/A";
            c.CamType = engine["ValveGear"] ?? "N/A";
            c.Torque = torque["FtLb"] ?? 0.0;
            c.Power = power["Bhp"] ?? 0.0;
            c.TopSpeed = maxSpeed["Mph"] ?? 0;
            c.Consumption = combined["Mpg"] ?? 0.0;
            
            return c;
        }

        private async Task<JsonValue> Fetch(string url)
        {

            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            // Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {
                // Get a stream representation of the HTTP web response:
                using (Stream stream = response.GetResponseStream())
                {
                    // Use this stream to build a JSON document object:
                    JsonValue jsonDoc = await Task.Run(() => JsonValue.Load(stream));
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                    // Return the JSON document:
                    return jsonDoc;
                }
            }
        }

        private Weather ParseJSON(JsonValue json)
        {
            JsonValue results = json["weatherObservation"];

            double temp = results["temperature"];
            double humidity = results["humidity"];

            Weather w = new Weather(temp, humidity);
            return w;

        }

        private string getStatusCode(JsonValue json)
        {
            JsonValue response = json["Response"];
            string statusCode = response["StatusCode"];
            return statusCode;
        }

      
    }
}

