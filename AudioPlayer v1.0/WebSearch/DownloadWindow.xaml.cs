using AudioPlayer_v1._0.Windows;
using PlayL;
using System;
using System.Collections.Generic;
using System.IO;
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
using VM;
using WSearch;

namespace AudioPlayer_v1._0.WebSearch
{
    /// <summary>
    /// Логика взаимодействия для DownloadWindow.xaml
    /// </summary>
    public partial class DownloadWindow : Window
    {

        PlaylistControl playlistControl;
        TrackInfo trackinfo;


        public DownloadWindow()
        {
            InitializeComponent();
        }

        public DownloadWindow(PlaylistControl _playlistControl, TrackInfo _trackinfo)
        {
            trackinfo = _trackinfo;
            playlistControl = _playlistControl;
            InitializeComponent();
            comboboxplaylists.ItemsSource = playlistControl.getallplaylists();
            comboboxplaylists.SelectedIndex = 0;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog ofd = new System.Windows.Forms.FolderBrowserDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                path.Text = ofd.SelectedPath;
        }


        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WebResponse webresponse = new WebResponse();
            try
            {
                if (Directory.Exists(path.Text))
                {
                    this.Close();

                    string downloadedTrackPath = await webresponse.DownloadTrackByLinkAsync(path.Text, trackinfo);

                    DownloadNotificationPushWIndow.ShowPushNotification($"Загрузка {downloadedTrackPath.Split('\\').Last()} заершена");

                    Playlist pl = comboboxplaylists.SelectedItem as Playlist;
                    if ((bool)cbaddtopl.IsChecked && File.Exists(downloadedTrackPath) && pl!=null)
                    {
                        pl.addTrackToPlaylist(downloadedTrackPath);
                    }
                }
                else
                    MessageBox.Show("Неверный путь");
            }
            catch (Exception ee)
            {
                this.Close();
                MessageBox.Show(ee.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
