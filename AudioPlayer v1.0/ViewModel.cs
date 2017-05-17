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
    class ViewModel
    {

        /// перетаскивание музыки/папок в плейлист
        static public void datagrid_Drop(PlaylistControl playlistControl, DragEventArgs e)
        {
            var data = e.Data as DataObject;
            if (data.ContainsFileDropList())
            {
                var files = data.GetFileDropList();
                foreach (string s in files)
                {
                    if (Directory.Exists(s))
                    {
                        addnewfilefromfolder(playlistControl, s);
                    }
                    else
                       if ("mp3".Contains(s.Split('.').Last()))
                        playlistControl.currentPlaylist.addTrackToPlaylist(s);
                }
            }
        }

        private static void addnewfilefromfolder(PlaylistControl playlistControl, string path)
        {
            try
            {               
                List<string> filepath = Directory.GetFiles(path).ToList();
                List<string> directoriespath = Directory.GetDirectories(path).ToList();

                filepath.ForEach(file =>
                {
                    if ("mp3".Contains(file.Split('.').Last()))
                        playlistControl.currentPlaylist.addTrackToPlaylist(file);               

                });

                directoriespath.ForEach(s => addnewfilefromfolder(playlistControl, s));
            }
            catch
            {
                MessageBox.Show("Создайте плейлист");
            }
        }

        static public void addnewfiles(PlaylistControl playlistControl)
        {
            System.Windows.Forms.FolderBrowserDialog ofd = new System.Windows.Forms.FolderBrowserDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                addnewfilefromfolder(playlistControl, ofd.SelectedPath);





        }
    }
}
