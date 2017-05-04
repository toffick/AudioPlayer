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
    class Playlist: IEnumerable<Track>
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

        private List<Track> allTracks;

        private Track currentTrack;


        //TODO   миксование треков
        //TODO  нумерация треков во время восппрозведения
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

        public void mixAllTracks()
        {
            //созранить норм
            //перемешать все треки
        }


        public void setcurrentTrack(Track _tr)
        {
            currentTrack = _tr;
        }
        public void getAllTracksFromPlaylists()
        {
            allTracks = DBOperate.getAllTracksFromPlaylist(Playlistname);
        }

        public void addTrackToPlaylist(string path)
        {
            if (!isRepeateTrackInPlaylist(path))
            {
                DBOperate.addSongToPlaylist(Playlistname, path);
                allTracks.Add(new Track(path, getNewPLnumber()));
                PlaylistsSoundCountResizeEvent?.Invoke(allTracks);
            }
        }

        public void removeTrack(Track track)
        {
            DBOperate.removeSongFromPlaylist(Playlistname, track.filepath);
            allTracks.Remove(track);
            PlaylistsSoundCountResizeEvent?.Invoke(allTracks);

        }

        public void removeRangeTracks(IList<DataGridCellInfo> _tr)
        {
            foreach (var t in _tr)
            {
             //   MessageBox.Show(((Track)t).ToString)
            }
        }
        ///получить первый трек из плейлиста
        public Track getFirstTrack()
        {
            return allTracks.Count >= 0 ? allTracks[0] : null;
        }


        ///получить последний трек из плейлиста
        public Track getEndTrack()
        {
            return allTracks.Count > 0 ? allTracks[allTracks.Count - 1] : null;
        }

        public Track getTrackWithId(int i)
        {
            return (i > -1 && i < allTracks.Count) ? allTracks[i] : null;
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
            int playnumber = currentTrack.Number;
            if (playnumber < Count)
                return allTracks[playnumber];
            else
                return allTracks[0];
        }
        
        public Track getPrevTrack()
        {
            int playnumber = currentTrack.Number - 2;
            if (playnumber > -1)
                return allTracks[playnumber];
            else
                return allTracks[0];
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
