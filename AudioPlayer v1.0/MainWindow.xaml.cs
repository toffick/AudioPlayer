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
using System.IO;
using System.Drawing;
using VM;
using DB;
using AudioPlayer_v1._0.Windows;
using WSearch;

namespace AudioPlayer_v1._0
{


    //TODO смена картинки на кнопке

    public partial class MainWindow : Window
    {

        private OpenFileDialog ofd = new OpenFileDialog();
        private MusicControl musiccontrol;
        private PlaylistControl playlistControl;
        public DispatcherTimer timerPlay;
        private MainSearcher mainsearcher;

        public TimeSpan trackTime { get; set; }
        private bool isWSopen = false;
        public MainWindow()
        {
            InitializeComponent();
            InitializePlayerComponents();

            musiccontrol.trackChangeEvent += setTrackInfo;                                   //получить информацию о треке при его 
            playlistControl.PlaylistsResizeEvent += refreshPlaylistsListBox;            //обновлять кол-во плейлистов при смене их количества
        }

        private void InitializePlayerComponents()
        {
            try
            {
                mainsearcher = new MainSearcher();
                timerPlay = new DispatcherTimer();
                playlistControl = new PlaylistControl();
                musiccontrol = new MusicControl(PlaySlider, timerPlay);
                timerPlay.Interval = TimeSpan.FromMilliseconds(500);
                timerPlay.Tick += setMusicCurentInfo;


                refreshPlaylistsListBox();

                if (playlistControl.CountPL > 0)
                {
                    playlistControl.setCurrentPlaylist(playlistControl.getallplaylists()[0]);
                    musiccontrol.setCurrentPlaylist(playlistControl.getallplaylists()[0]);
                    refreshPlaylistsDataGrid(playlistControl.currentPlaylist.allTracks);
                    playlistControl.currentPlaylist.PlaylistsSoundCountResizeEvent += refreshPlaylistsDataGrid;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("Произошел сбой запуска приложения" + Environment.NewLine + ee.Message);
                musiccontrol.stop(playpause_button, null);
                DBOperate.Disconnect();
                this.Close();
            }
        }


        #region Control buttons/sliders
        private void stop_button_Click(object sender, RoutedEventArgs e)
        {
            musiccontrol.stop(playpause_button, e);
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

        private void replay_button_Click(object sender, RoutedEventArgs e)
        {
            musiccontrol.raplay(sender, e);
        }

        private void unreplay_button_Click(object sender, RoutedEventArgs e)
        {
            musiccontrol.unraplay(sender, e);
        }

        //показать информацию о треке 
        private void addplaylist_button_Click(object sender, RoutedEventArgs e)
        {
            PlaylistName pl_name = new PlaylistName();
            pl_name.ShowDialog();
            if (pl_name.Plname != null)
            {
                playlistControl.addPlaylist(pl_name.Plname);
                listBox_playlists.SelectedItem = playlistControl.currentPlaylist;
            }

        }

        private void PlaySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            musiccontrol.setTrackPosition(e.NewValue);
        }
        #endregion


        #region Volume
        //выключить звук
        private void mute_Checked(object sender, RoutedEventArgs e)
        {
            musiccontrol.mute(sender, e);
        }
        ///включить звук
        private void unmute(object sender, RoutedEventArgs e)
        {
            musiccontrol.unmute(sender, e);
        }

        ///регулировка звука
        private void volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            musiccontrol.setVolume(button_mute, e.NewValue);
        }
        #endregion


        #region Set static/curent track info
        /// Изменение времени и положения ползунка
        private void setMusicCurentInfo(object sender, EventArgs e)
        {

            PlaySlider.ValueChanged -= PlaySlider_ValueChanged;
            PlaySlider.Value = musiccontrol.getTrackPosition();
            PlaySlider.ValueChanged += PlaySlider_ValueChanged;
            alltime_textbox.Content = musiccontrol.getTrackTimePosition().ToString(@"mm\:ss") + '/' + trackTime.ToString(@"mm\:ss");
        }

        ///установить информвцию о треке
        private void setTrackInfo(Track _tr)
        {
            volume.Value = 0.5;
            trackTime = musiccontrol.getAllTrackTime().Time;
            PlaySlider.Maximum = _tr.trackinfo.Time.TotalSeconds;
            PlaySlider.Value = 0;
            label_album.Content = _tr.trackinfo.Album;
            label_authorname.Content = _tr.trackinfo.Author;
            label_songname.Content = _tr.trackinfo.SongName;
            Image_backgroundtrackimage.Source = _tr.trackinfo.Picture;

            currentplaylist_datagrid.SelectedItem = _tr;

        }
        #endregion

        /// Выбор плейлиста 
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox_playlists.SelectedItem != null)
            {
                refreshPlaylistsDataGrid(listBox_playlists.SelectedItem);
                musiccontrol.setCurrentPlaylist((Playlist)listBox_playlists.SelectedItem);
                playlistControl.setCurrentPlaylist((Playlist)listBox_playlists.SelectedItem);
                playlistControl.currentPlaylist.PlaylistsSoundCountResizeEvent += refreshPlaylistsDataGrid;
                currentplaylist_datagrid.SelectedItem = playlistControl.currentPlaylist.getCurrentTrack();
            }

        }

        /// Отрытие трека из списка треков плейлиста
        private void currentplaylist_datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Track temptrack = (Track)currentplaylist_datagrid.SelectedItem;
            if (temptrack != null && temptrack != playlistControl.getCurentTrack())
            {
                openTrack(temptrack);
            }

        }



        ///установить трек
        private void openTrack(Track _tr)
        {
            musiccontrol.setaudiofile(_tr);
            timerPlay.Start();
        }


        #region Refresh controls
        ///обновить список плейлистов
        private void refreshPlaylistsListBox()
        {
            listBox_playlists.ItemsSource = playlistControl.getallplaylists();
            listBox_playlists.Items.Refresh();
        }

        ///обновит ьсписок треков
        private void refreshPlaylistsDataGrid(object tracks)
        {
            currentplaylist_datagrid.Items.Clear();
            if (tracks != null)
                foreach (Track tr in (IEnumerable<Track>)tracks)
                {
                    currentplaylist_datagrid.Items.Add(tr);
                }
        }

        #endregion




        ///удалить трек из текущего плейлиста
        private void deletetrackfromcurentplaylist_button_Click(object sender, RoutedEventArgs e)
        {
            Track _tmp = (currentplaylist_datagrid.SelectedItem) as Track;
            if (_tmp != null)
                playlistControl.removeTrackFromCurentPlaylist(_tmp);
        }

        /// добавить новый терк в текущий плейлист
        private void addtracktocurentplaylist_button_Click(object sender, RoutedEventArgs e)
        {
            playlistControl.addSongToCurrentPlaylist();
        }

        /// перетаскивание музыки/папок в плейлист
        private void currentplaylist_datagrid_Drop(object sender, DragEventArgs e)
        {
            ViewModel.datagrid_Drop(playlistControl, e);
        }

    
        #region Closed form
        private void menu_close_Click(object sender, RoutedEventArgs e)
        {
            musiccontrol.stop(playpause_button, null);
            DBOperate.Disconnect();
            App.Current.Shutdown();
            this.Close();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            musiccontrol.stop(playpause_button, null);
            DBOperate.Disconnect();
            App.Current.Shutdown();
        }
        #endregion

        #region ContexMenuButtons
        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_playlists.SelectedIndex == -1)
                return;
            playlistControl.removePlaylist((Playlist)listBox_playlists.SelectedItem);
            if (playlistControl.currentPlaylist != null)
            {
                refreshPlaylistsDataGrid(playlistControl.currentPlaylist.allTracks);
                listBox_playlists.SelectedItem = playlistControl.currentPlaylist;
                musiccontrol.setCurrentPlaylist(playlistControl.currentPlaylist);
            }
            else
            {
                musiccontrol.setCurrentPlaylist(null);
                refreshPlaylistsDataGrid(null);
            }


        }

        private void MenuItemClear_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_playlists.SelectedIndex == -1)
                return;
            playlistControl.clearPlaylist((Playlist)listBox_playlists.SelectedItem);
        }

        private void MenuItemDataGridDelete_Click(object sender, RoutedEventArgs e)
        {
            Track _tmp = (currentplaylist_datagrid.SelectedItem) as Track;
            if (_tmp != null)
                playlistControl.removeTrackFromCurentPlaylist(_tmp);
        }

        private void MenuItemDataGridInfo_Click(object sender, RoutedEventArgs e)
        {
            if ((Track)currentplaylist_datagrid.SelectedItem != null)
                new TrackInfoWindoww((Track)currentplaylist_datagrid.SelectedItem).ShowDialog();

        }


        #endregion

        private void addfoldertocurentplaylist_button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.addnewfiles(playlistControl);
        }


        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.Key)
                {
                    case Key.Space:
                        musiccontrol.PlayPause(playpause_button, e);
                        break;
                    case Key.Delete:
                        playlistControl.removeTrackFromCurentPlaylist((Track)currentplaylist_datagrid.SelectedItem);
                        break;
                    case Key.Enter:
                        if (!isWSopen)
                            find_web_search_button.Focus();
                            find_web_search_button_Click(sender, e);
                        break;

                    default: break;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }


        private void Web_search_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(isWSopen=!isWSopen))
            {
                currentplaylist_container.Visibility = Visibility.Visible;
               grid_websearch.Visibility = Visibility.Hidden;
            }
            else
            {
                grid_websearch.Visibility = Visibility.Visible;
                currentplaylist_container.Visibility = Visibility.Hidden;
            }
        }

        #region WEBSEARCH
        private async void find_web_search_button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (webSearch_textbox.Text.Length != 0)
                {
                    List<object> tracks;
                    if ((tracks = await mainsearcher.getFindedTracks(webSearch_textbox.Text)) != null)
                    {
                        datagrid_web_search.ItemsSource = tracks;
                    }
                    else
                        MessageBox.Show("Ничего не найдено");

                    webSearch_textbox.Clear();


                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
           

        }

        private void datagrid_web_search_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainsearcher.downloadbutton(e.AddedItems[0],playlistControl);
        }

        #endregion
    }
}

