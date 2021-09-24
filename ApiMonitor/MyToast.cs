using Microsoft.Toolkit.Uwp.Notifications;
using System;
using Windows.Foundation;
using Windows.UI.Notifications;

namespace ApiMonitor
{
    public static class MyToast
    {
        public static void Notify(
            string title, 
            DateTime expiration, 
            bool muted = false, 
            string description = null,
            TypedEventHandler<ToastNotification, object> onActivated = null)
        {
            var toast = new ToastContentBuilder().AddText(title);

            if (!string.IsNullOrWhiteSpace(description)) toast.AddText(description);

            if (muted)
                toast.AddAudio(new Uri("ms-winsoundevent:Notification.Looping.Alarm2"), false, true);

            toast.Show(toast =>
            {
                if (onActivated != null) toast.Activated += onActivated;
                toast.ExpirationTime = expiration;
            });
        }

        public static TypedEventHandler<ToastNotification, object> GetOnActivatedEvent(string title, string str) =>
            (_, __) =>
            {
                var jsonForm = new JsonForm
                {
                    Text = title
                };

                jsonForm.richTextBox1.Text = str;

                jsonForm.Show();
            };
    }
}
