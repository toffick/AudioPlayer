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
        public delegate void MyDel();
        public event MyDel PlaylistsResizeEvent;


        List<Playlist> allplaylists;
        Playlist currentPlaylist;

        public PlaylistControl()
        {
            DBOperate.InitDB();
            allplaylists = new List<Playlist>();
            InitListPlaylists();
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
        public void InitListPlaylists()
        {
            string cmdText = "Select PL_NAME, PL_NUMBER  From PLAYLIST";
            try
            {
                SqlDataReader dr = DBOperate.executeQuery(cmdText);
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

        public void addPlaylist(string plname)
        {
            try
            {
                if (isRepeatedName(plname))
                    throw new Exception("Плейлист с таким именем уже существует");
                int plnumber = getNewPLnumber();
                allplaylists.Add(new Playlist(plname, plnumber));
                DBOperate.executeQuery($"INSERT PLAYLIST(PL_NUMBER, PL_NAME)    VALUES({plnumber}, '{plname}')").Close();
                PlaylistsResizeEvent?.Invoke();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Не удалось добавить плейлист "+ee.Message);
            }

        }

        private int getNewPLnumber()
        {
            int i = -1;
            List<int> plnumbers = allplaylists.Select(s => s.Playlistnumber).ToList<int>();
            while (i < plnumbers.Count)
                if (!plnumbers.Contains(++i))
                    break;
            return i;

        }

        private bool isRepeatedName(string str)
        {
            return allplaylists.Select(s => s.Playlistname).Contains<string>(str);
        }
    }
}
