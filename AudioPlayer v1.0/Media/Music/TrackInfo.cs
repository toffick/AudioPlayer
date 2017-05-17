using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TagLib;

namespace Music
{
    public class TrackInfo
    {
        private const string UNKNSONGNAME = "unknown_song_name";
        private const string UNNKAUTHOR = "unknown_author_name";
        private const string UNKNALBUM = "unknown_album_name";
        private const string UNKNYEAR = "unknown_album_year";

        public string SongName { get; private set; }
        public string Author { get; private set; }
        public string Album { get; private set; }
        public TimeSpan Time { get; private set; }

        public BitmapImage Picture { get; private set; }

        public uint BPM { get; private set; }
        public int AudioBitrate { get; private set; }

        public string Year { get; private set; }


        private TagLib.File file;
        public TrackInfo()
        {
            SongName = UNKNSONGNAME;
            Author = UNNKAUTHOR;
            Album = UNKNALBUM;
            Time = new TimeSpan(0, 0, 0);

        }

        public TrackInfo(string _path)
        {
            try
            {
                file = TagLib.File.Create(_path);
                SongName = getSongName();
                Album = getAlbum();
                Author = getAuthor();
                Time = getTime();
                Picture = getPicture();
                BPM = getBPM();
                AudioBitrate = getBittrate();
                Year = getYear();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

        }

        private string getYear()
        {
            return file.Tag.Year == 0 ? UNKNYEAR : file.Tag.Year.ToString();
        }

        private BitmapImage getPicture()
        {

            try
            {
                byte[] data = file.Tag.Pictures[0].Data.Data;

                BitmapImage image = new BitmapImage();
                using (var mem = new MemoryStream(data))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                return image;
            }
            catch
            {
                return null;
            }
        }

        private string getSongName()
        {
            return file.Tag.Title != null ? file.Tag.Title : file.Name.Split('\\').Last();
        }

        private string getAlbum()
        {
            return file.Tag.Album;
        }

        private string getAuthor()
        {
            return file.Tag.FirstPerformer;
        }

        private TimeSpan getTime()
        {
            try
            {
                return file.Properties.Duration;
            }
            catch
            {
                return new TimeSpan(0, 0, 0);
            }
        }

        public override string ToString()
        {
            return $"{SongName} {Author} {Album} {Time.ToString()}";
        }

        private uint getBPM()
        {
            return file.Tag.BeatsPerMinute;
        }

        private int getBittrate()
        {
            return file.Properties.AudioBitrate;
        }
    }
}
