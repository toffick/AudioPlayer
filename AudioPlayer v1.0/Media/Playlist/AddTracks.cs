using Microsoft.Win32;
using PlayL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VM
{
    static class AddTracks
    {

        /// перетаскивание музыки/папок в плейлист
        static public void datagrid_Drop( DragEventArgs e)
        {
            PlaylistControl playlistControl = PlaylistControl.GetPlaylistControl();
            var data = e.Data as DataObject;
            if (data.ContainsFileDropList())
            {
                var files = data.GetFileDropList();
                foreach (string s in files)
                {
                    if (Directory.Exists(s))
                    {
                        AddNewTrackByPath(playlistControl, s);
                    }
                    else
                       if (".mp3|.wav".Contains(Path.GetExtension(s)))
                        playlistControl.currentPlaylist.addTrackToPlaylist(s);
                }
            }
        }

        public static void AddNewTrackByPath(PlaylistControl playlistControl, string path)
        {
            try
            {               
                List<string> filepath = Directory.GetFiles(path).ToList();
                List<string> directoriespath = Directory.GetDirectories(path).ToList();

                directoriespath.ForEach(s => AddNewTrackByPath(playlistControl, s));

                filepath.ForEach(file =>
                {
                    if (".mp3|.wav".Contains(Path.GetExtension(file)))
                        playlistControl.currentPlaylist.addTrackToPlaylist(file);
                });
            }
            catch
            {
                MessageBox.Show("Создайте плейлист");
            }
        }


        static public void addnewfiles()
        {
            PlaylistControl playlistControl = PlaylistControl.GetPlaylistControl();
            System.Windows.Forms.FolderBrowserDialog ofd = new System.Windows.Forms.FolderBrowserDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                AddNewTrackByPath(playlistControl, ofd.SelectedPath);
        }
    }
}
