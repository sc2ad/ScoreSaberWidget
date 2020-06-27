using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

using ScoreSaber;

namespace ScoreSaberWidget
{
    [Service(Name = "com.sc2ad.ScoreSaberWidget.UpdateJob", Permission = "android.permission.BIND_JOB_SERVICE")]
    public class UpdateService : JobService
    {
        private ScoreSaberAPI api;

        private void CreateWidget(Context context, ulong val)
        {
            Console.WriteLine("Creating widget!");
            var views = GetViews(context, val);

            var widget = new ComponentName(context, Java.Lang.Class.FromType(typeof(ScoreSaberWidget)).Name);
            var manager = AppWidgetManager.GetInstance(context);
            manager.UpdateAppWidget(widget, views);
            Console.WriteLine("Created widget, should have set the AppWidgetManager to use the latest views!");
        }

        private RemoteViews GetViews(Context context, ulong profileId)
        {
            Console.WriteLine("Getting profile for id: " + profileId);
            var task = api.GetUserData(profileId);
            var userInfo = task.GetAwaiter().GetResult();

            Console.WriteLine("Creating RemoteViews for username: " + userInfo.Username);

            var views = new RemoteViews(context.PackageName, Resource.Layout.widget_data);

            Console.WriteLine($"Username: {userInfo.Username} Rank: {userInfo.GlobalRank}");

            views.SetTextViewText(Resource.Id.username, userInfo.Username);
            views.SetTextViewText(Resource.Id.rank, "#" + userInfo.GlobalRank.ToString());

            //var actionIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(api.GetProfileUrl(profileId)));
            //var pending = PendingIntent.GetActivity(context, 0, actionIntent, 0);
            //views.SetOnClickPendingIntent(Resource.Id.widget, pending);
            var intent = new Intent(context, Java.Lang.Class.FromType(typeof(RefreshActivity)));
            var refreshPending = PendingIntent.GetActivity(context, 0, intent, 0);
            views.SetOnClickPendingIntent(Resource.Id.refresh, refreshPending);
            Console.WriteLine("Created RemoveViews!");
            return views;
        }

        public override bool OnStartJob(JobParameters @params)
        {
            Console.WriteLine("UpdateService starting!");
            var client = new HttpClient();
            api = new ScoreSaberAPI(client);
            ulong val = (ulong)@params.Extras.GetLong("UserID");
            Task.Run(() => CreateWidget(this, val));
            return true;
        }

        public override bool OnStopJob(JobParameters @params)
        {
            Console.WriteLine("UpdateService complete!");
            return false;
        }
    }
}