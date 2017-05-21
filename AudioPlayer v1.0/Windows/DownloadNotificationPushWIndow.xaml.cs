using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AudioPlayer_v1._0.Windows
{
    /// <summary>
    /// Логика взаимодействия для DownloadNotificationPushWIndow.xaml
    /// </summary>
    public partial class DownloadNotificationPushWIndow : Window
    {
        public DownloadNotificationPushWIndow(string text)
        {
            InitializeComponent();
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                var workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea; 
                var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

                Push_string.Text = text;

                this.Left = corner.X - this.ActualWidth;
                this.Top = corner.Y - this.ActualHeight;
            }));
        }

        public static void ShowPushNotification(string text)
        {
            new DownloadNotificationPushWIndow(text).Show();
        }
        private void CloseMethod(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
