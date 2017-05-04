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
    class TrackInfo
    {
        private const string UNKNSONGNAME = "unknown_song_name";
        private const string UNNKAUTHOR = "unknown_author_name";
        private const string UNKNALBUM = "unknown_album_name";

        public string SongName { get; private set; }
        public string Author { get; private set; }
        public string Album { get; private set; }
        public TimeSpan Time { get; private set; }

        public BitmapImage Picture { get; private set; }

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
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

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
            try
            {

                return file.Tag.Title != null ? file.Tag.Title : file.Name.Split('\\').Last();
            }
            catch
            {
                return UNKNSONGNAME;
            }
        }

        private string getAlbum()
        {
            try
            {
                return file.Tag.Album;
            }
            catch
            {
                return UNKNALBUM;
            }
        }

        private string getAuthor()
        {
            try
            {
                return file.Tag.FirstPerformer;
            }
            catch
            {
                return UNNKAUTHOR;
            }
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

    }
}
