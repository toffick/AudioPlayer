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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Music;
using PlayL;
using Microsoft.Win32;
using System.Threading;
using System.Windows.Threading;

namespace AudioPlayer_v1._0
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private OpenFileDialog ofd = new OpenFileDialog();
        private MusicControl musiccontrol;
        private PlaylistControl playlistControl;
        public DispatcherTimer timerPlay;
        public TimeSpan myTime { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            musiccontrol = new MusicControl();
            playlistControl = new PlaylistControl();
            volume.Value = 0.5;
            PlaylistsList.ItemsSource = playlistControl.getallplaylists();

            Track kk = new Track(@"D:\Загрузки\e7bb15496a0d1b.mp3");
            MessageBox.Show(kk.trackinfo.ToString());
            timerPlay = new DispatcherTimer();
            timerPlay.Interval = TimeSpan.FromMilliseconds(1000);
            timerPlay.Tick += new EventHandler(starttrack);
        }

        private void prevTrack_Click(object sender, RoutedEventArgs e)
        {
            musiccontrol.playPrevtrack(sender, e);
        }

        private void playpause_Click(object sender, RoutedEventArgs e)
        {

            musiccontrol.PlayPause(sender, e);
            timerPlay.Start();
        }

        private void nexttrack_button_Click(object sender, RoutedEventArgs e)
        {
            musiccontrol.playNexttrack(sender, e);
        }

        private void firsttrack_button_Click(object sender, RoutedEventArgs e)
        {
            musiccontrol.playFirsttrack(sender, e);
        }

        private void finishtrack_Click(object sender, RoutedEventArgs e)
        {
            musiccontrol.playEndtrack(sender, e);
        }

        private void menu_button_Click(object sender, RoutedEventArgs e)
        {



        }

        private void replay_button_Click(object sender, RoutedEventArgs e)
        {
            musiccontrol.raplay(sender, e);
        }

        private void mix_button_Click(object sender, RoutedEventArgs e)
        {
            musiccontrol.mixtracks(sender, e);
        }

        private void addplaylist_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void stop_button_Click(object sender, RoutedEventArgs e)
        {
            musiccontrol.stop(sender, e);
        }

        private void currentplaylist_datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //
            MessageBox.Show("s");
            //musiccontrol.setaudiofile(currentplaylist_datagrid.SelectedItem);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"D:\БГТУ\КУРСОВОЙ ПРОЕКТ";
            ofd.ShowDialog();
            musiccontrol.opentrack(sender, e, new Uri(ofd.FileName));

        }

        private void PlaySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            musiccontrol.setTrackPosition(PlaySlider.Value);
        }
        #region работа с громкостью звука
        //выключить звук
        private void mute_Checked(object sender, RoutedEventArgs e)
        {
            musiccontrol.mute(sender, e);
        }
        //включить звук
        private void unmute(object sender, RoutedEventArgs e)
        {
            musiccontrol.unmute(sender, e);
        }

        //регулировка звука
        private void volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            musiccontrol.setVolume(e.NewValue);
        }
        #endregion
        private void addtracktocurentplaylist_button_Click(object sender, RoutedEventArgs e)
        {
            playlistControl.addSongToCurrentPlaylist();
        }

        private void PlaylistsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //тут устанавливать выбранный плейлист в поле текущего плейлиста в playlistControl
        }

        private void starttrack(object sender, EventArgs e)
        {
            PlaySlider.ValueChanged -= PlaySlider_ValueChanged;
            PlaySlider.Value = 1000 * musiccontrol.getTrackPosition();
            myTime = musiccontrol.getTrackTime();
            curenttime_textblock.Text = $"{myTime.Minutes}:{myTime.Seconds}";
            PlaySlider.ValueChanged += PlaySlider_ValueChanged;

        }


    }
}
