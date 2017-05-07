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
            image.Source = _track.trackinfo.Picture;
        }


    }
}
