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

namespace PlayL
{
    class Playlist: IEnumerable<Track>
    {
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
            string cmdText = $"Select MUSICFILE_PATH  From MUSIC INNER JOIN PLAYLIST ON PLAYLIST.PL_NAME = MUSIC.MUSIC_PLAYLIST WHERE PL_NAME =  '{Playlistname}' ";

            using(SqlDataReader dr = DBOperate.executeQuery(cmdText))
            {
                while (dr.Read())
                {
                    Track temp = new Track(dr[0].ToString(), Count+1);
                    allTracks.Add(temp);
                }
            }
        }

        public void addTrackToPlaylist(Uri trackuri)
        {

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

        

        //public void initallTracksFromPL()
        //{

        //    try
        //    {
        //        string strSQL = "Select * From PLAYLIST";
        //        SqlCommand myCommand = new SqlCommand(strSQL, sqlc);
        //        SqlDataReader dr = myCommand.ExecuteReader();
        //        while (dr.Read())
        //            MessageBox.Show(string.Format("ID: {0} Car Pet Name: {1}", dr[0].ToString(), dr[1].ToString()));
        //    }
        //    catch (Exception ee)
        //    {
        //        MessageBox.Show(ee.Message);
        //    }

        //}



        /////////////////////////////
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
    }
}
