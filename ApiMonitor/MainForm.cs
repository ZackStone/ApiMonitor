using System;
using System.Drawing;
using System.Windows.Forms;

namespace ApiMonitor
{
    public class MainForm : Form
    {
        private const string CONFIG_FILES_PATH = @"C:\manu\Programs\api-monitor\config-files\";

        private readonly NotifyIcon trayIcon;
        private readonly ContextMenuStrip trayMenu;

        private readonly Timer timer;

        private bool flag = true;

        public MainForm()
        {
            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenuStrip();

            trayMenu.Items.Add("On/Off", null, OnToggleClick);
            trayMenu.Items.Add("Exit", null, OnExit);

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon = new NotifyIcon
            {
                Text = "API Monitor",
                Icon = new Icon("icon.ico", 40, 40),

                // Add menu to tray icon and show it.
                ContextMenuStrip = trayMenu,
                Visible = true
            };

            timer = new Timer();
            timer.Tick += new EventHandler(TimerEventHandler);
            timer.Interval = 60 * 1000; // in miliseconds
            timer.Start();
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void OnToggleClick(object sender, EventArgs e)
        {
            flag = !flag;
            MyToast.Notify($"Monitoramento: " + (flag ? "ON" : "OFF"), DateTime.Now.AddSeconds(5), true);
        }

        private void TimerEventHandler(object sender, EventArgs e)
        {
            try
            {
                if (flag)
                    Worker.Work(CONFIG_FILES_PATH).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                MyToast.Notify(
                    $"Erro na aplicação!",
                    DateTime.Now.AddMinutes(1),
                    description: ex.Message,
                    onActivated: MyToast.GetOnActivatedEvent($"Erro na aplicação!", ex.Message));
            }
        }

        private void OnExit(object sender, EventArgs e) => Application.Exit();

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }
    }
}
