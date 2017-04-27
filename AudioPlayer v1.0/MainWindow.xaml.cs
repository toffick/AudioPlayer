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
        

        public TimeSpan trackTime { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            timerPlay = new DispatcherTimer();
            musiccontrol = new MusicControl(PlaySlider, timerPlay);
            playlistControl = new PlaylistControl();
            timerPlay.Interval = TimeSpan.FromMilliseconds(500);

            timerPlay.Tick += setMusicCurentInfo;
            musiccontrol.trackChange += setTrackInfo;                                   //получить информацию о треке при его 
            playlistControl.PlaylistsResizeEvent += refreshPlaylistsListBox;            //обновлять кол-во плейлистов при смене их количества

            listBox_playlists.DisplayMemberPath = "Playlistname";
            refreshPlaylistsListBox();
            volume.Value = 0.5;

           

        }

        private void prevTrack_Click(object sender, RoutedEventArgs e)
        {
            musiccontrol.playPrevtrack(sender, e);
        }

        private void playpause_Click(object sender, RoutedEventArgs e)
        {

            musiccontrol.PlayPause(sender, e);
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
            musiccontrol.unraplay(sender, e);
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
            PlaylistName pl_name = new PlaylistName();
            pl_name.ShowDialog();          
            if(pl_name.Plname!=null)
                playlistControl.addPlaylist(pl_name.Plname);
            
        }

        private void stop_button_Click(object sender, RoutedEventArgs e)
        {
            musiccontrol.stop(sender, e);
        }

        private void PlaySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            musiccontrol.setTrackPosition(e.NewValue);
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
            //TODO ставишь мут - двигаешь ползунок - убираешь мут - звук как до мута
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

        /// Изменение времени и положения ползунка
        private void setMusicCurentInfo(object sender, EventArgs e)
        {
                PlaySlider.ValueChanged -= PlaySlider_ValueChanged;
                PlaySlider.Value = musiccontrol.getTrackPosition();
                PlaySlider.ValueChanged += PlaySlider_ValueChanged;
                alltime_textbox.Text = musiccontrol.getTrackTimePosition().ToString(@"mm\:ss")+'/'+ trackTime.ToString(@"mm\:ss");
        }

        ///установить информвцию о треке
        private void setTrackInfo(Track _tr)
        {
            trackTime = musiccontrol.getAllTrackTime().Time;
            PlaySlider.Maximum = _tr.trackinfo.Time.TotalSeconds;
            PlaySlider.Value = 0;
            label_album.Content =  _tr.trackinfo.Album;
            label_authorname.Content = _tr.trackinfo.Author;
            label_songname.Content = _tr.trackinfo.SongName;
        }

        private void mix_button_Unchecked(object sender, RoutedEventArgs e)
        {

        }


        /// Выбор плейлиста
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refreshPlaylistsDataGrid(listBox_playlists.SelectedItem);
            musiccontrol.setCurrentPlaylist((Playlist)listBox_playlists.SelectedItem);
        }


        /// Отрытие трека из списка треков плейлиста
        private void currentplaylist_datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Track temptrack = (Track)currentplaylist_datagrid.SelectedItem;
            if (temptrack != null)
            {
                openTrack(temptrack);
            }
        }

        private void openTrack(Track _tr)
        {
            musiccontrol.setaudiofile(_tr);
            timerPlay.Start();
        }

        private void refreshPlaylistsListBox()
        {
            listBox_playlists.ItemsSource = playlistControl.getallplaylists();
            listBox_playlists.Items.Refresh();
          //  playlistControl.getallplaylists().ToList<Playlist>().ForEach(s=>MessageBox.Show(s.ToString()));

        }
        private void refreshPlaylistsDataGrid(object tracks)
        {
            currentplaylist_datagrid.ItemsSource = (IEnumerable<Track>)tracks;

        }
    }
}
