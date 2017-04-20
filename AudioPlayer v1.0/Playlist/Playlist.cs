using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;
using Music;

namespace PlayL
{
    class Playlist
    {
        private string Playlistname { get; set; }
        private int Playlistnumber { get; set; }

        private List<Track> allSong;



        public Playlist()
        {
            allSong = new List<Track>();
            Playlistname = "empty_pl_name";
            Playlistnumber = -1;
        }
        public Playlist(string _playlistname, int _playlistnumber)
        {
            allSong = new List<Track>();
            Playlistname = _playlistname;
            Playlistnumber = _playlistnumber;
            //initallSongFromPL();
        }

        public void mixAllTracks()
        {
            //созранить норм
            //перемешать все треки
        }



        public void addTrackToPlaylist(Uri trackuri)
        {

        }

        ///получить первый трек из плейлиста
        public Track getFirstTrack()
        {
            return allSong.Count >= 0 ? allSong[0] : null;
        }


        ///получить последний трек из плейлиста
        public Track getEndTrack()
        {
            return allSong.Count > 0 ? allSong[allSong.Count - 1] : null;
        }

        public Track getTrackWithId(int i)
        {
            return (i > -1 && i < allSong.Count) ? allSong[i] : null;
        }

      

        //public void initallSongFromPL()
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
            return "Playlist: " + Playlistname + " number " + Playlistnumber;
        }


        public Track getNextTrack()
        {
            throw new NotImplementedException();
        }
        
        public Track getPrevTrack()
        {
            throw new NotImplementedException();
        }
    }
}
