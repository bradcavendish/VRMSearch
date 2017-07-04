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
    class CarListViewAdapter : BaseAdapter<Car>
    {
        public List<Car> cars;
        private Context context;

        public CarListViewAdapter(Context context, List<Car> cars)
        {
            this.cars = cars;
            this.context = context;           
        }

        public override Car this[int position] { get { return cars[position]; } }

        public override int Count
        {
            get { return cars.Count(); }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(context).Inflate(Resource.Layout.listViewRow, null, false);
            }

            TextView regTextView = row.FindViewById<TextView>(Resource.Id.listViewReg);
            TextView makeTextView = row.FindViewById<TextView>(Resource.Id.listViewMake);
            TextView modelTextView = row.FindViewById<TextView>(Resource.Id.listViewModel);

            regTextView.Text = cars[position].Reg;
            makeTextView.Text = cars[position].Make;
            modelTextView.Text = cars[position].Model;

            return row;
        }
    }
}