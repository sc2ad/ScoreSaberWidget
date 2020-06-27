using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ScoreSaberWidget
{
    [Activity(Label = "RefreshActivity")]
    public class RefreshActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void OnStart()
        {
            base.OnStart();
            Console.WriteLine("Refreshing ScoreSaberWiget!");
            ScoreSaberWidget.ScheduleUpdateNow(this);
            Finish();
        }
    }
}