using PlayL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Threading;

namespace Music
{
    class MusicControl
    {

        public delegate void MyDel(Track _tr);

        public event MyDel trackChangeEvent;


        private MediaPlayer mediaplayer;
        private Playlist currentPlaylist;

        private double storeVolumeValue;                                        //уровень звука для восстановления
        private bool isreplay;                                                  //повторять ли текущий трек
        public bool IsPlaying { get; private set; }


        Slider slider_play;
        DispatcherTimer timmer;
        public MusicControl(Slider _sl, DispatcherTimer _dt)
        {
            mediaplayer = new MediaPlayer();
            IsPlaying = false;
            isreplay = false;
            slider_play = _sl;
            timmer = _dt;
            mediaplayer.MediaEnded += playNexttrack;                              //по завершению трека играть следующий
        }




        public void PlayPause(object sender, EventArgs e)
        {
            Button temp_image = sender as Button;

            var brushpa = new ImageBrush();
            brushpa.ImageSource = new BitmapImage(
                new Uri(@"D:\БГТУ\КУРСОВОЙ ПРОЕКТ\AudioPlayer v1.0\AudioPlayer v1.0\resources\icons\pause.png", UriKind.Absolute));
            var brushpl = new ImageBrush();
            brushpl.ImageSource = new BitmapImage(
                new Uri(@"D:\БГТУ\КУРСОВОЙ ПРОЕКТ\AudioPlayer v1.0\AudioPlayer v1.0\resources\icons\play-button.png", UriKind.Absolute));

            if (IsPlaying)
            {
                timmer.Stop();
                mediaplayer.Pause();
            }
            else
            {
                timmer.Start();
                mediaplayer.Play();

            }
            IsPlaying = !IsPlaying;

        }

        public void setaudiofile(Track track)
        {
            try
            {

                IsPlaying = true;
                mediaplayer.Open(new Uri(track.filepath));
                currentPlaylist.setcurrentTrack(track);
                trackChangeEvent?.Invoke(currentPlaylist.getCurrentTrack());
                mediaplayer.Play();
            }
            catch
            {
                stop(null,null);
            }
        }



        ///играть следующий трэк из плейлиста
        public void playNexttrack(object sender, EventArgs e)
        {
            try
            {
                if (isreplay)
                {
                    setaudiofile(currentPlaylist.getCurrentTrack());
                }
                else
                {
                    setaudiofile(currentPlaylist.getNextTrack());
                }

            }
            catch 
            {
                stop(null, null);
            }
        }

        ///играть следующий трэк из плейлиста
        public void playPrevtrack(object sender, EventArgs e)
        {
            try
            {
                setaudiofile(currentPlaylist.getPrevTrack());
            }
            catch 
            {
                stop(null, null);
            }
        }

        ///играть первый трэк из плейлиста
        public void playFirsttrack(object sender, EventArgs e)
        {
            try
            {
                setaudiofile(currentPlaylist.getFirstTrack());
            }
            catch
            {
                stop(null, null);
            }
        }

        ///играть последний трэк из плейлиста
        public void playEndtrack(object sender, EventArgs e)
        {
            try
            {
                setaudiofile(currentPlaylist.getEndTrack());
            }
            catch
            {
                stop(null, null);
            }
        }


        ///повторять трек
        public void raplay(object sender, EventArgs e)
        {
            isreplay = true;
        }
        public void unraplay(object sender, EventArgs e)
        {
            isreplay = false;
        }


        ///остановить воспроизведение
        public void stop(object sender, EventArgs e)
        {
            mediaplayer.Stop();
            PlayPause(sender, e);
        }

        #region Работа со звуком
        public void mute(object sender, EventArgs e)
        {
            storeVolumeValue = mediaplayer.Volume;
            mediaplayer.Volume = 0d;
        }
        public void unmute(object sender, EventArgs e)
        {
            mediaplayer.Volume = storeVolumeValue;
        }

        public void setVolume(object sender, double _newVolume)
        {
            ToggleButton but = sender as ToggleButton;
            if (but != null)
            {
                if (_newVolume == 0)
                {
                    but.IsChecked = true;
                    storeVolumeValue = 0;
                }
                else
                    but.IsChecked = false;
            }
            mediaplayer.Volume = _newVolume;
        }

        #endregion


        /// установить текущий плейлист
        public void setCurrentPlaylist(Playlist _pl)
        {
            currentPlaylist = _pl;
        }

        /// установить текуую поизицтю трека 
        public void setTrackPosition(double _pos)
        {
            mediaplayer.Position = TimeSpan.FromSeconds(_pos);
        }

        /// получить текущую позицию трека в секундах 
        public double getTrackPosition()
        {
            return mediaplayer.Position.Minutes * 60 + mediaplayer.Position.Seconds;
        }

        ///получить текущее время трека 
        public TimeSpan getTrackTimePosition()
        {
            return mediaplayer.Position;
        }

        /// получить инфомацию о треке
        public TrackInfo getAllTrackTime()
        {
            return currentPlaylist.getCurrentTrack().trackinfo;
        }

    }
}
