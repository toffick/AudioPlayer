using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music
{
    class Track
    {
        public string filepath { get; }
      
        public TrackInfo trackinfo { get; }

        public int Number { get; private set;  }
        public Track()
        { }
        public Track(string _path,int _n)
        {
            filepath = _path;
            Number = _n;
            trackinfo=new TrackInfo(_path);
        }

        public override string ToString()
        {
            return trackinfo.SongName;
        }
    }
}
