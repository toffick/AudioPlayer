using PlayL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Control
{
    class MusicControl
    {
        private MediaPlayer mediaplayer;
        private bool isplaying;
        private Playlist currentPlaylist; 

        public MusicControl()
        {
            mediaplayer = new MediaPlayer();
            isplaying = false;
            mediaplayer.MediaEnded += playNexttrack;
        }

        public void PlayPause(object sender, EventArgs e)
        {
            if (isplaying)
            {
                mediaplayer.Pause();
                isplaying = false;
            }
            else
            {
                mediaplayer.Play();
                isplaying = true;
            }
        }

        public void opentrack(Uri url)
        {
            mediaplayer.Open(url);
            mediaplayer.Pause();
        }
        //играть следующий трэк из плейлиста
        public void playNexttrack(object sender, EventArgs e)
        {

        }

        //играть следующий трэк из плейлиста
        public void playPrevtrack(object sender, EventArgs e)
        {

        }

        //играть первый трэк из плейлиста
        public void playFirsttrack(object sender, EventArgs e)
        {

        }

        //играть последний трэк из плейлиста
        public void playEndtrack(object sender, EventArgs e)
        {

        }

        //повторять трек
        public void raplay(object sender, EventArgs e)
        {
            
        }

        //перемешать трек
        public void mixtracks(object sender, EventArgs e)
        {

        }

        public void stop(object sender, EventArgs e)
        {
            mediaplayer.Stop();
        }
    }
}
