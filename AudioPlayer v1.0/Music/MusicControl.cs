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
        private bool isplaying;
        private Playlist currentPlaylist;

        private double storeVolumeValue;                                        //уровень звука для восстановления
        private bool isreplay;                                                  //повторять ли текущий трек
        public MusicControl()
        {
            mediaplayer = new MediaPlayer();
            isplaying = false;
            isreplay = false;

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

            if (isplaying)
            {

                temp_image.Background = brushpl;
                mediaplayer.Pause();
                isplaying = false;
            }
            else
            {
                temp_image.Background = brushpa;
                mediaplayer.Play();
                isplaying = true;

            }
        }

        public void opentrack(object sender, EventArgs e, Uri url)
        {
            mediaplayer.Open(url);
            mediaplayer.Pause();
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


        ///перемешать трек
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

        #region работа со звуком
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


        public void setTrackPosition(double _pos)
        {
            mediaplayer.Position = TimeSpan.FromMilliseconds(
                _pos * mediaplayer.NaturalDuration.TimeSpan.TotalSeconds);
        }
        public double getTrackPosition()
        {//TODO секунды обнуляются - обнуляется слайдер воспроизведения 
            return mediaplayer.Position.Seconds / mediaplayer.NaturalDuration.TimeSpan.TotalSeconds;
        }

        public TimeSpan getTrackTime()
        {
            return mediaplayer.Position;
        }

    }
}
