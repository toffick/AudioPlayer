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



        private Slider slider_play;
        private DispatcherTimer timmer;
        private MediaPlayer mediaplayer;
        private Playlist currentPlaylist;

        private double storeVolumeValue;                                        //уровень звука для восстановления
        public bool IsReplay {get; private set;}                                           //повторять ли текущий трек
        public bool IsPlaying { get; private set; }

        public MusicControl(Slider _sl, DispatcherTimer _dt)
        {
            mediaplayer = new MediaPlayer();
            IsPlaying = false;
            IsReplay = false;
            slider_play = _sl;
            timmer = _dt;
            mediaplayer.MediaEnded += PlayNextTrack;                              //по завершению трека играть следующий
        }



        #region Set curent PLAYLIST/TRACK
        /// установить текущий плейлист
        public void RetCurrentPlaylist(Playlist _pl)
        {
            currentPlaylist = _pl;
        }


        public void SetTrack(Track track)
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

        #endregion


        #region Play controls methods

        public void PlayPause(object sender, EventArgs e)
        {
            Button temp_image = sender as Button;

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
        ///играть следующий трэк из плейлиста
        public void PlayNextTrack(object sender, EventArgs e)
        {
            try
            {
                if (IsReplay)
                {
                    SetTrack(currentPlaylist.getCurrentTrack());
                }
                else
                {
                    SetTrack(currentPlaylist.getNextTrack());
                }
                timmer.Start();


            }
            catch 
            {
                stop(null, null);
            }
        }

        ///играть следующий трэк из плейлиста
        public void PlayPrevTrack(object sender, EventArgs e)
        {
            try
            {
                SetTrack(currentPlaylist.getPrevTrack());
                timmer.Start();

            }
            catch 
            {
                stop(null, null);
            }
        }

        ///играть первый трэк из плейлиста
        public void PlayFirstTrack(object sender, EventArgs e)
        {
            try
            {
                SetTrack(currentPlaylist.getFirstTrack());
                timmer.Start();

            }
            catch
            {
                stop(null, null);
            }
        }

        ///играть последний трэк из плейлиста
        public void PlayEndTrack(object sender, EventArgs e)
        {
            try
            {
                SetTrack(currentPlaylist.getEndTrack());
                timmer.Start();

            }
            catch
            {
                stop(null, null);
            }
        }

        ///повторять трек
        public void Replay(object sender, EventArgs e)
        {
            IsReplay = true;
        }
        public void UnReplay(object sender, EventArgs e)
        {
            IsReplay = false;
        }

        ///остановить воспроизведение
        public void stop(object sender, EventArgs e)
        {
            mediaplayer.Stop();
            PlayPause(sender, e);
        }
        #endregion


        #region Работа со звуком
        public void Mute(object sender, EventArgs e)
        {
            storeVolumeValue = mediaplayer.Volume;
            mediaplayer.Volume = 0d;
        }
        public void UnMute(object sender, EventArgs e)
        {
            mediaplayer.Volume = storeVolumeValue;
        }

        public void SetVolume(object sender, double _newVolume)
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


        #region Track position
        /// установить текуую поизицтю трека 
        public void SetTrackPosition(double _pos)
        {
            mediaplayer.Position = TimeSpan.FromSeconds(_pos);
        }

        /// получить текущую позицию трека в секундах 
        public double GetTrackPosition()
        {
            return mediaplayer.Position.Minutes * 60 + mediaplayer.Position.Seconds;
        }

        ///получить текущее время трека 
        public TimeSpan GetTrackTimePosition()
        {
            return mediaplayer.Position;
        }

        /// получить инфомацию о треке
        public TrackInfo GetAllTrackTime()
        {
            return currentPlaylist.getCurrentTrack().trackinfo;
        }
        #endregion
    }
}
