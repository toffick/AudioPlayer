using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music
{
    class Track
    {
        public Uri filepath { get; }
      
        public TrackInfo trackinfo { get; }

        public Track(Uri _path)
        {
            filepath = _path;
            trackinfo.getTrackInfo(_path);
        }       
    }
}
