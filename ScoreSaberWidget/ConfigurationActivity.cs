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
    [Activity(Label = "ConfigurationActivity")]
    public class ConfigurationActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Console.WriteLine("Starting config activity!");
            SetContentView(Resource.Layout.widget_config);
            base.OnCreate(savedInstanceState);

            var userIdText = FindViewById<TextView>(Resource.Id.config_userId);
            var oldUserId = FindViewById<TextView>(Resource.Id.config_oldUserId);
            oldUserId.Text = "Old UserID: " + ScoreSaberWidget.GetUserId().ToString();
            var button = FindViewById<Button>(Resource.Id.config_button);
            button.Click += delegate
            {
                Console.WriteLine("Parsed User ID: " + userIdText.Text);
                ScoreSaberWidget.SetUserId(this, long.Parse(userIdText.Text));
                Intent resultValue = new Intent();
                SetResult(Result.Ok, resultValue);
                Finish();
            };
            var cancel = FindViewById<Button>(Resource.Id.config_cancel);
            cancel.Click += delegate
            {
                Intent resultValue = new Intent();
                SetResult(Result.Ok, resultValue);
                Finish();
            };
        }
    }
}