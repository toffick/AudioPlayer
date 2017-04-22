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
            listBox_playlists.DisplayMemberPath = "Playlistname";
            listBox_playlists.ItemsSource = playlistControl.getallplaylists();

            timerPlay = new DispatcherTimer();
            timerPlay.Interval = TimeSpan.FromMilliseconds(1000);
            timerPlay.Tick += setMusicCurentInfo;
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

        private void unreplay_button_Click(object sender, RoutedEventArgs e)
        {
            musiccontrol.raplay(sender, e);
        }

        

        private void mix_button_Click(object sender, RoutedEventArgs e)
        {
            musiccontrol.mixtracks(sender, e);
        }
        private void mix_returstartpos_button_Click(object sender, RoutedEventArgs e)
        {
            musiccontrol.mixreturnstartpos(sender, e);
        }


        private void addplaylist_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void stop_button_Click(object sender, RoutedEventArgs e)
        {
            musiccontrol.stop(sender, e);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenTrack(sender,e);
        }

        private void PlaySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            musiccontrol.setTrackPosition(sender,e);
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

        private void OpenTrack(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            musiccontrol.setaudiofile(PlaySlider, e, new Uri(ofd.FileName));
        }

        /// Изменение времени и положения ползунка
        private void setMusicCurentInfo(object sender, EventArgs e)
        {

            if (musiccontrol.IsPlaying)
            {
                PlaySlider.ValueChanged -= PlaySlider_ValueChanged;
                PlaySlider.Value = musiccontrol.getTrackPosition();
                PlaySlider.ValueChanged += PlaySlider_ValueChanged;
                curenttime_textblock.Text = musiccontrol.getTrackTime().ToString(@"mm\:ss");
            }


        }

        private void mix_button_Unchecked(object sender, RoutedEventArgs e)
        {

        }


        /// Выбор плейлиста
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentplaylist_datagrid.ItemsSource = (IEnumerable<Track>)listBox_playlists.SelectedItem;
        }


        /// Отрытие трека из списка треков плейлиста
        private void currentplaylist_datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TODO вылет при смене плейлиста
            Track temptrack = (Track)currentplaylist_datagrid.SelectedItem;
            musiccontrol.setaudiofile(PlaySlider, e, new Uri(temptrack.filepath));
        }
    }
}
