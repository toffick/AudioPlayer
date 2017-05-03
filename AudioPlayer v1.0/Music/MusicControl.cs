using PlayL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Threading;

namespace Music
{
    class MusicControl
    {

        public delegate void MyDel(Track _tr);
        public event MyDel trackChange;


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
            //TODO смена картинок
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
                temp_image.Background = brushpl;
                mediaplayer.Pause();
            }
            else
            {
                timmer.Start();
                temp_image.Background = brushpa;
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
                trackChange?.Invoke(currentPlaylist.getCurrentTrack());
                mediaplayer.Play();             
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);

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
            catch (Exception ee)
            {
                MessageBox.Show("Не удалось воспроизвести трек" + ee.Message);
            }
        }

        ///играть следующий трэк из плейлиста
        public void playPrevtrack(object sender, EventArgs e)
        {
            try
            {
                setaudiofile(currentPlaylist.getPrevTrack());
            }
            catch (Exception ee)
            {
                MessageBox.Show("Не удалось воспроизвести трек" + ee.Message);
            }
        }

        ///играть первый трэк из плейлиста
        public void playFirsttrack(object sender, EventArgs e)
        {
            try
            {
                setaudiofile(currentPlaylist.getFirstTrack());
            }
            catch (Exception ee)
            {
                MessageBox.Show("Не удалось воспроизвести трек" + ee.Message);
            }
        }

        ///играть последний трэк из плейлиста
        public void playEndtrack(object sender, EventArgs e)
        {
            try
            {
                setaudiofile(currentPlaylist.getEndTrack());
            }
            catch (Exception ee)
            {
                MessageBox.Show("Не удалось воспроизвести трек" + ee.Message);
            }
        }


        ///повторять трек
        public void raplay(object sender, EventArgs e)
        {
            isreplay = true;
        }
        public void unraplay(object sender, EventArgs e)
        {
            //TODO сделать нормальное впоспроизведение
            isreplay = false;
        }


        ///остановить воспроизведение
        public void stop(object sender, EventArgs e)
        {
            mediaplayer.Stop();
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

        public void setVolume(double _newVolume)
        {
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
            return mediaplayer.Position.Minutes*60+ mediaplayer.Position.Seconds ;
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
