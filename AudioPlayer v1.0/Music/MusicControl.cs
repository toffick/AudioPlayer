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

namespace Music
{
    class MusicControl
    {
        private MediaPlayer mediaplayer;
        private Playlist currentPlaylist;

        private double storeVolumeValue;                                        //уровень звука для восстановления
        private bool isreplay;                                                  //повторять ли текущий трек
        public bool IsPlaying { get; private set; }

        Slider slider_play;
        public MusicControl()
        {
            mediaplayer = new MediaPlayer();
            IsPlaying = false;
            isreplay = false;
            mediaplayer.MediaOpened += setSliderMaximumValue;

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

                temp_image.Background = brushpl;
                mediaplayer.Pause();
                IsPlaying = false;
            }
            else
            {
                temp_image.Background = brushpa;
                mediaplayer.Play();
                IsPlaying = true;

            }
        }

        public void opentrack(object sender, EventArgs e, Uri url)
        {
            mediaplayer.Open(url);
            slider_play = sender as Slider;
        }



        ///играть следующий трэк из плейлиста
        public void playNexttrack(object sender, EventArgs e)
        {
            try
            {
                //если треков нет - плеер замолкает
                if (isreplay)
                {
                    mediaplayer.Open(new Uri(currentPlaylist.getNextTrack().filepath));
                }
                else
                {
                    //TODO воспроизводить тот же трек
                    mediaplayer.Open(new Uri(currentPlaylist.getNextTrack().filepath));
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
                mediaplayer.Open(new Uri(currentPlaylist.getPrevTrack().filepath));
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
                mediaplayer.Open(new Uri(currentPlaylist.getFirstTrack().filepath));
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
                mediaplayer.Open(new Uri(currentPlaylist.getEndTrack().filepath));
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
            isreplay = false;
        }


        ///перемешать треки
        public void mixtracks(object sender, EventArgs e)
        {
            //TODO перемешивааем треки. пока не ебу, каким образом
        }

        ///вернуть начальное положение треков после перемешания
        public void mixreturnstartpos(object sender, EventArgs e)
        {
            //TODO тут возвращаем первоначальное раположение треков
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

        public void setTrackPosition(object sender, EventArgs e)
        {
            var ee = e as RoutedPropertyChangedEventArgs<double>;
            mediaplayer.Position = TimeSpan.FromSeconds(ee.NewValue);
        }
        public double getTrackPosition()
        {
            return mediaplayer.Position.Minutes*60+ mediaplayer.Position.Seconds ;
        }
        public void setSliderMaximumValue(object sender, EventArgs e)
        {
            slider_play.Maximum = mediaplayer.NaturalDuration.TimeSpan.TotalSeconds;
        }


        public TimeSpan getTrackTime()
        {
            return mediaplayer.Position;
        }

    }
}
