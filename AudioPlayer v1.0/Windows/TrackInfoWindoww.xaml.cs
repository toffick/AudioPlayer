using Music;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace AudioPlayer_v1._0.Windows
{
    /// <summary>
    /// Логика взаимодействия для TrackInfoWindoww.xaml
    /// </summary>
    public partial class TrackInfoWindoww : Window
    {
        private Track _track;
        public TrackInfoWindoww(Track s)
        {
            InitializeComponent();
            _track = s;
            setTrackInfo();
        }


        //public string SongName { get; private set; }
        //public string Author { get; private set; }
        //public string Album { get; private set; }
        //public TimeSpan Time { get; private set; }

        //public BitmapImage Picture { get; private set; }

        //public uint BPM { get; private set; }
        //public int AudioBitrate { get; private set; }


        public string Year { get; private set; }
        //разграничить текст о информации песни и ее ттх
        private void setTrackInfo()
        {
            image.Source = _track.trackinfo.Picture;
            songname.Content = _track.trackinfo.SongName;
            author.Content = _track.trackinfo.Author;
            year.Content = _track.trackinfo.Year;
            album.Content = _track.trackinfo.Album;

            time.Content = _track.trackinfo.Time.ToString(@"mm\:ss");
            bitrate.Content = _track.trackinfo.AudioBitrate;
            bpm.Content = _track.trackinfo.BPM;
        }


    }
}
