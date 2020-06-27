using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using ScoreSaber;

namespace ScoreSaberWidget
{
    [Service]
    public class UpdateService : Service
    {
        private ScoreSaberAPI api;
        public override void OnCreate()
        {
            var client = new HttpClient();
            api = new ScoreSaberAPI(client);
            base.OnCreate();
        }
        public override IBinder OnBind(Intent intent)
        {
            // Unnecessary
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            var views = GetViews(this);

            var widget = new ComponentName(this, Java.Lang.Class.FromType(typeof(ScoreSaberWidget)).Name);
            var manager = AppWidgetManager.GetInstance(this);
            manager.UpdateAppWidget(widget, views);
            return base.OnStartCommand(intent, flags, startId);
        }

        private RemoteViews GetViews(Context context)
        {
            ulong profileId = 76561198126780301;
            var task = api.GetUserData(profileId).ConfigureAwait(true);
            var userInfo = task.GetAwaiter().GetResult();

            var views = new RemoteViews(context.PackageName, Resource.Layout.widget_data);

            views.SetTextViewText(Resource.Id.username, userInfo.Username);
            views.SetTextViewText(Resource.Id.rank, userInfo.GlobalRank.ToString());

            var actionIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(api.GetProfileUrl(profileId)));
            var pending = PendingIntent.GetActivity(context, 0, actionIntent, 0);
            views.SetOnClickPendingIntent(Resource.Id.widget, pending);
            return views;
        }
    }
}