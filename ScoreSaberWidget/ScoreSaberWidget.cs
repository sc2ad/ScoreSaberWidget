using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.App.Job;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ScoreSaberWidget
{
    [BroadcastReceiver(Label = "@string/widget_name")]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/widget")]
    public class ScoreSaberWidget : AppWidgetProvider
    {
        // milliseconds before rescheduling job (at least this long between automatic updates)
        public const long Interval = 3600000;
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            Console.WriteLine("Updating ScoreSaber Widget!");
            ScheduleUpdateNow(context);
        }

        public static void ScheduleUpdateNow(Context context)
        {
            var jobScheduler = context.GetSystemService(Java.Lang.Class.FromType(typeof(JobScheduler))) as JobScheduler;
            jobScheduler.CancelAll();
            var parameters = new PersistableBundle();
            parameters.PutLong("UserID", 76561198126780301L);
            var jobBuilder = new JobInfo.Builder(1, new ComponentName(context, Java.Lang.Class.FromType(typeof(UpdateService))));
            jobBuilder.SetPeriodic(Interval);
            var jobInfo = jobBuilder.SetExtras(parameters).Build();
            var result = jobScheduler?.Schedule(jobInfo);
            Console.WriteLine("Scheduled a job. Result: " + result);
        }
    }
}