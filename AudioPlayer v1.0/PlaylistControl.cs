using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayL;
using System.Data.SqlClient;
using System.Windows;
using DB;

namespace PlaylControl
{
    class PlaylistControl
    {
        List<Playlist> allplaylists;

        public PlaylistControl()
        {
            DBOperate.InitDB();
            allplaylists = new List<Playlist>();
            InitListPlaylist();
        }



        private void InitListPlaylist()
        {
            string cmdText = "Select PL_NAME, PL_NUMBER  From PLAYLIST";
            try
            {
                SqlDataReader dr = DBOperate.selectQuery(cmdText);
                while (dr.Read())
                {
                    allplaylists.Add(new Playlist(dr[0].ToString(), int.Parse(dr[1].ToString())));
                }
            }
            catch (Exception err)
            {
                //вывод ошибки
            }

        }

        public List<Playlist> getallplaylists()
        {
            return allplaylists;
        }

        public int getNextPlNumber()
        {
            return 1;
        }
    }
}
