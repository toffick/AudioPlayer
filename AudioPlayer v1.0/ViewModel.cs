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
                List<string> filepath = new List<string>();
                var files = data.GetFileDropList();
                foreach (string s in files)
                {
                    if (Directory.Exists(s))
                    {
                        filepath = Directory.GetFiles(s).ToList();
                        filepath.ForEach(file =>
                        {
                            if ("mp3".Contains(file.Split('.').Last()))
                                playlistControl.currentPlaylist.addTrackToPlaylist(file);
                        });
                    }
                    else
                       if ("mp3".Contains(s.Split('.').Last()))
                        playlistControl.currentPlaylist.addTrackToPlaylist(s);
                }
            }
        }

        private void addnewfilew()
        {

        }

        static public void addnewfiles(PlaylistControl playlistControl)
        {
            System.Windows.Forms.FolderBrowserDialog ofd = new System.Windows.Forms.FolderBrowserDialog();
            //ofd.InitialDirectory = @"D:\БГТУ\КУРСОВОЙ ПРОЕКТ\Tracks";
            ofd.ShowDialog();


        }
    }
}
