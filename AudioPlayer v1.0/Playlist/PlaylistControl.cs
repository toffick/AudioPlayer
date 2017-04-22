using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayL;
using System.Data.SqlClient;
using System.Windows;
using DB;
using Microsoft.Win32;

namespace PlayL
{
    class PlaylistControl
    {
        List<Playlist> allplaylists;
        Playlist currentPlaylist;

        public PlaylistControl()
        {
            DBOperate.InitDB();
            allplaylists = new List<Playlist>();
            InitListPlaylist();
            InitTracksInPlaylists();
        }

        public void InitTracksInPlaylists()
        {
            foreach (Playlist t in allplaylists)
            {
                t.getAllTracksFromPlaylists();
            }
        }

        //инициализировать список плейлистов
        public void InitListPlaylist()
        {
            string cmdText = "Select PL_NAME, PL_NUMBER  From PLAYLIST";
            try
            {
                SqlDataReader dr = DBOperate.selectQuery(cmdText);
                while (dr.Read())
                {
                    allplaylists.Add(new Playlist(dr[0].ToString(), int.Parse(dr[1].ToString())));
                }
                dr.Close();

                
            }
            catch (Exception err)
            {
                //вывод ошибки
            }

        }

        //вернуть список всех пелейлистов
        public List<Playlist> getallplaylists()
        {
            return allplaylists;
        }

        //получить следующий трек в плейлисте
        public int getNextPlNumber()
        {
            return 1;
        }

        public void addSongToCurrentPlaylist()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = ofd.InitialDirectory = @"D:\БГТУ\КУРСОВОЙ ПРОЕКТ\Tracks";
            ofd.Filter = "Файлы mp3 |*.mp3";

            try
            {
            }
            catch 
            {
            }

        }
    }
}
