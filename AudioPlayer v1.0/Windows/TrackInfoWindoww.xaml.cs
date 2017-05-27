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

        private void setTrackInfo()
        {
            if(_track.trackinfo.Picture!=null)
                image.Source = _track.trackinfo.Picture;
            songname.Text = _track.trackinfo.SongName;
            author.Text = _track.trackinfo.Author;
            year.Text = _track.trackinfo.Year;
            album.Text = _track.trackinfo.Album;

            time.Text = _track.trackinfo.Time.ToString(@"mm\:ss");
            bitrate.Text = _track.trackinfo.AudioBitrate.ToString();
            bpm.Text = _track.trackinfo.BPM.ToString();
            path.Text = _track.filepath;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            this.Close();
        }



        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
