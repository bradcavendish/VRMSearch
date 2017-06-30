using SQLite;

namespace JDMReview
{
    class Car
    {
        //public properties
        public string Make{get;set;}
        public string Model
        {
            get;set;
        }
        public string Body
        {
            get;set;
        }
        public string Colour
        {
            get;set;
        }
        public string Fuel
        {
            get;set;
        }
        public string Transmission
        {
            get;set;
        }
        public string Aspiration
        {
            get;set;
        }
        public string CylinderArrangement
        {
            get;set;
        }
        public string CamType
        {
            get;set;
        }
        public string Reg
        {
            get;set;
        }
        public string Date
        {
            get;set;
        }
        public int? NumOfCylinders
        {
            get;set;
        }
        public int? EngineSize
        {
            get;set;
        }
        public int? TopSpeed
        {
            get;set;
        }
        public int? ValvesPerCylinder
        {
            get;set;
        }
        public double? Power
        {
            get;set;
        }
        public double? Torque
        {
            get;set;
        }
        public double? Consumption
        {
            get;set;
        }

        public override string ToString()
        {
            return Reg + " " + Make + " " + Model;
        }
    }
}