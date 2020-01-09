using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V4.App;
using Android.Widget;
using Jux.Droid.Data;
using Jux.Interface;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(MessageCenter))]
namespace Jux.Droid.Data
{
    public class MessageCenter : IMessageCenter
    {
        private Context mContext;
        private NotificationManager mNotificationManager;
        private NotificationCompat.Builder mBuilder;
        public static String NOTIFICATION_CHANNEL_ID = "10023";

        public MessageCenter()
        {
            mContext = global::Android.App.Application.Context;
        }
        public void LongMessage(string Message)
        {
            Toast.MakeText(Application.Context, Message, ToastLength.Long).Show();
        }

        public void Notification(string Title, string Message)
        {
            try
            {
                var intent = new Intent(mContext, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                intent.PutExtra(Title, Message);
                var pendingIntent = PendingIntent.GetActivity(mContext, 0, intent, PendingIntentFlags.OneShot);


                mBuilder = new NotificationCompat.Builder(mContext);
                mBuilder.SetSmallIcon(Resource.Drawable.Logo);
                mBuilder.SetContentTitle(Title)
                        .SetAutoCancel(true)
                        .SetContentTitle(Title)
                        .SetContentText(Message)
                        .SetChannelId(NOTIFICATION_CHANNEL_ID)
                        .SetPriority((int)NotificationPriority.High)
                        .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
                        .SetVisibility((int)NotificationVisibility.Public)
                        .SetSmallIcon(Resource.Drawable.Logo)
                        .SetContentIntent(pendingIntent);



                NotificationManager notificationManager = mContext.GetSystemService(Context.NotificationService) as NotificationManager;

                if (global::Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.O)
                {
                    NotificationImportance importance = global::Android.App.NotificationImportance.High;

                    NotificationChannel notificationChannel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, Title, importance);
                    notificationChannel.EnableLights(true);
                    notificationChannel.SetShowBadge(true);
                    notificationChannel.Importance = NotificationImportance.High;
                    if (notificationManager != null)
                    {
                        mBuilder.SetChannelId(NOTIFICATION_CHANNEL_ID);
                        notificationManager.CreateNotificationChannel(notificationChannel);
                    }
                }

                notificationManager.Notify(0, mBuilder.Build());
            }
            catch (Exception ex)
            {
                //
            }
        }

        public void ShortMessage(string Message)
        {
            Toast.MakeText(Application.Context, Message, ToastLength.Short).Show();
        }
    }
}