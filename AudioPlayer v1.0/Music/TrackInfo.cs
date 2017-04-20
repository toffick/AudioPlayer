using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music
{
    class TrackInfo
    {
        public string SongName { get; set; }
        public string Author { get; set; }
        public string Album { get; set; }
        public TimeSpan Time { get; set; }
        //картинка 

        public TrackInfo getTrackInfo(Uri _path)
        {
            TrackInfo ti = new TrackInfo();
            //тут юзаем tag_lib
            return ti;
        }
        public override string ToString()
        {
            return $"{SongName} {Author} {Album} {Time.ToString()}";
        }

    }
}
