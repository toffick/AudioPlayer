using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music
{
    class Track
    {
        public Uri filepath { get; set; }
        public string SongName { get; set; }
        public string Author { get; set; }
        public string Album { get; set; }
        public TimeSpan Time { get; set; }

        public Track()
        { }

        public Track(string _sn, string _au, string _al, TimeSpan _ts)
        {
            SongName = _sn;
            Author = _au;
            Album = _al;
            Time = _ts;
        }

        public override string ToString()
        {
            return $"{SongName} {Author} {Album} {Time.ToString()}";
        }
        //picture of album 

    }
}
