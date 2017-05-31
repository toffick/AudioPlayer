using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;
using Music;
using DB;
using System.Collections;
using System.Windows.Controls;

namespace PlayL
{
   public class Playlist: IEnumerable<Track>
    {
        public delegate void MyDel(object obj);
        public event MyDel PlaylistsSoundCountResizeEvent;

        public string Playlistname { get; private set; }

        public int Count
        {
            get{ return allTracks.Count; }
            private set { }
        }

        public int Playlistnumber { get; private set; }

        public List<Track> allTracks { get; private set; }

        private Track currentTrack { get; set; }

        public Playlist()
        {
            allTracks = new List<Track>();
            Playlistname = "empty_pl_name";
            Playlistnumber = -1;
        }
        public Playlist(string _playlistname, int _playlistnumber)
        {
            currentTrack = null;
            allTracks = new List<Track>();
            Playlistname = _playlistname;
            Playlistnumber = _playlistnumber;         
        }

        public void setcurrentTrack(Track _tr)
        {
            currentTrack = _tr;
        }
        public void getAllTracksFromPlaylists()
        {
            allTracks = DBOperate.GetAllTracksFromPlaylist(Playlistname);
        }

        public void addTrackToPlaylist(string path)
        {
            if (!isRepeateTrackInPlaylist(path))
            {
                DBOperate.AddSongToPlaylist(Playlistname, path);
                allTracks.Add(new Track(path, getNewPLnumber()));
                PlaylistsSoundCountResizeEvent?.Invoke(allTracks);
            }
        }

        public void removeTrack(Track track)
        {
            DBOperate.RemoveSongFromPlaylist(Playlistname, track.filepath);
            allTracks.Remove(track);
            PlaylistsSoundCountResizeEvent?.Invoke(allTracks);

        }

        public void clearPlaylist()
        {
            int c = Count;
            for (int i = c-1; i >=0; i--)
                removeTrack(allTracks[i]);
        }


        ///получить первый трек из плейлиста
        public Track getFirstTrack()
        {
            try
            {
                return allTracks.Count >= 0 ? allTracks[0] : null;
            }
            catch
            {
                return null;
            }
        }


        ///получить последний трек из плейлиста
        public Track getEndTrack()
        {
            try
            {
                return allTracks.Count > 0 ? allTracks[allTracks.Count - 1] : null;
            }
            catch
            {
                return null;
            }
        }

        public override string ToString()
        {
            string temp = null;
            foreach (Track t in allTracks)
            {
                temp += t.ToString()+Environment.NewLine;
            }
            return  temp + Environment.NewLine + "Playlist: " + Playlistname + " number " + Playlistnumber;
        }

        public Track getNextTrack()
        {
            try
            {
                int playnumber = currentTrack.Number;
                if (++playnumber < Count)
                    return allTracks[playnumber];
                else
                    return allTracks[0];
            }
            catch {
                return null;
            }
        }
        
        public Track getPrevTrack()
        {
            try
            {
                int playnumber = currentTrack.Number;
                if (--playnumber > -1)
                    return allTracks[playnumber];
                else
                    return allTracks[0];
            }
            catch
            {
                return null;
            }
        }

        public Track getCurrentTrack()
        {
            return currentTrack;
        }

        public IEnumerator<Track> GetEnumerator()
        {
            return ((IEnumerable<Track>)allTracks).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Track>)allTracks).GetEnumerator();
        }

        private int getNewPLnumber()
        {
            int i = -1;
            List<int> plnumbers = allTracks.Select(s => s.Number).ToList<int>();
            while (i < plnumbers.Count)
                if (!plnumbers.Contains(++i))
                    break;
            return i;

        }

        private bool isRepeateTrackInPlaylist(string path)
        {
            return allTracks.Select(s => s.filepath).Contains(path);
        }

    }
}
