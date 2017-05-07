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
using Music;
using System.Windows.Data;

namespace PlayL
{
    class PlaylistControl
    {
        public delegate void MyDel();
        public event MyDel PlaylistsResizeEvent;


        List<Playlist>  allplaylists;
        public Playlist currentPlaylist { get; private set; }

        public PlaylistControl()
        {
            try
            {
                DBOperate.InitDB();
                allplaylists = DBOperate.getAllPlaylists();
                InitTracksInPlaylists();
                currentPlaylist = allplaylists.Count == 0 ? null : allplaylists[0];
            }
            catch
            {
                MessageBox.Show("Ошибка при инициализации плейлитов");
            }

        }

        public void InitTracksInPlaylists()
        {
            foreach (Playlist t in allplaylists)
            {
                t.getAllTracksFromPlaylists();
            }
        }

        //вернуть список всех пелейлистов
        public List<Playlist> getallplaylists()
        {
            return allplaylists;
        }

        public void removeTrackFromCurentPlaylist(Track _tr)
        {
            currentPlaylist.removeTrack(_tr);
        }


        public void addSongToCurrentPlaylist()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = ofd.InitialDirectory = @"D:\БГТУ\КУРСОВОЙ ПРОЕКТ\Tracks";
            ofd.Filter = "Файлы mp3 |*.mp3";
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    currentPlaylist.addTrackToPlaylist(ofd.FileName);
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
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
                setCurrentPlaylist(allplaylists[allplaylists.Count - 1]);
                DBOperate.addPlatlist(plnumber, plname);
                PlaylistsResizeEvent?.Invoke();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Не удалось добавить плейлист "+ee.Message);
            }

        }

        public void removePlaylist(Playlist _pl)
        {
            DBOperate.removePlaylist(_pl.Playlistname) ;
            allplaylists.Remove(_pl);
            currentPlaylist = allplaylists.Count == 0 ? null : allplaylists[0];
            PlaylistsResizeEvent?.Invoke();


        }

        public void clearPlaylist(Playlist _pl)
        {
            currentPlaylist.clearPlaylist();
                
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

        public void setCurrentPlaylist(Playlist _pl)
        {
            currentPlaylist = _pl;
        }

        public Track getCurentTrack()
        {
            return currentPlaylist.getCurrentTrack();
        }

      
    }
}
