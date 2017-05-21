using Music;
using PlayL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DB
{
    static class DBOperate
    {
        static private string connstr = "Data Source=DESKTOP-FFV5E68\\SQLEXPRESS;Initial Catalog=AUDIOPLAYERDB;Integrated Security=true;";
        static private SqlConnection sqlconnection;

        static public bool InitDB()
        {
            try
            {
                sqlconnection = new SqlConnection(connstr);
                sqlconnection.Open();
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return false;
            }
        }
        static public void Disconnect()
        {
            try
            {
                sqlconnection.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        static public List<Playlist> GetAllPlaylistsFromTable()
        {
            List<Playlist> list = new List<Playlist>();
            string cmdText = "Select PL_NAME, PL_NUMBER  From PLAYLIST";
            SqlCommand command = new SqlCommand(cmdText, sqlconnection);
            try
            {
                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new Playlist(dr[0].ToString(), int.Parse(dr[1].ToString())));
                    }
                }
            }
            catch (SqlException ee)
            {
                MessageBox.Show("Ошибка чтения с базы данных:" + Environment.NewLine + ee.Message);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

            return list;
        }
        static public List<Track> GetAllTracksFromPlaylist(string plname)
        {
            List<Track> list = new List<Track>();
            string cmdText = $"Select MUSICFILE_PATH  From MUSIC " +
                "INNER JOIN PLAYLIST ON PLAYLIST.PL_NAME = MUSIC.MUSIC_PLAYLIST WHERE PL_NAME = @plname ";
            SqlCommand command = new SqlCommand(cmdText, sqlconnection);
            try
            {
                command.Parameters.AddWithValue("@plname", plname);

                var nonexiststracks = new List<string>();
                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string filepath = dr[0].ToString();
                        if (File.Exists(filepath))
                            list.Add(new Track(filepath, list.Count));
                        else
                            nonexiststracks.Add(filepath);
                    }
                }
                nonexiststracks.ForEach(s => RemoveSongFromPlaylist(plname,s));
            }
            catch (SqlException ee)
            {
                MessageBox.Show("Ошибка чтения с базы данных:" + Environment.NewLine + ee.Message);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

            return list;
        }

        static public void AddPlatlist(int plnumber, string plname)
        {
            string cmdText = "INSERT PLAYLIST(PL_NUMBER, PL_NAME)    VALUES(@plnumber, @plname)";
            SqlCommand command = new SqlCommand(cmdText, sqlconnection);
            try
            {
                command.Parameters.AddWithValue("@plnumber", plnumber);
                command.Parameters.AddWithValue("@plname", plname);
                command.ExecuteNonQuery();

            }
            catch (SqlException ee)
            {
                MessageBox.Show("Ошибка чтения с базы данных:" + Environment.NewLine + ee.Message);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
        static public void AddSongToPlaylist(string plname, string path)
        {
            string cmdText = "INSERT MUSIC ( MUSIC_PLAYLIST,MUSICFILE_PATH) VALUES(@plname,@musicpath)";
            SqlCommand command = new SqlCommand(cmdText, sqlconnection);
            try
            {
                command.Parameters.AddWithValue("@plname", plname);
                command.Parameters.AddWithValue("@musicpath", path);
                command.ExecuteNonQuery();

            }
            catch (SqlException ee)
            {
                MessageBox.Show("Ошибка чтения с базы данных:" + Environment.NewLine + ee.Message);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }


        static public void RemoveSongFromPlaylist(string plname, string path)
        {
            string cmdText = "DELETE FROM MUSIC WHERE MUSIC_PLAYLIST = @pl AND MUSICFILE_PATH = @mp";
            SqlCommand command = new SqlCommand(cmdText, sqlconnection);
            try
            {
                command.Parameters.AddWithValue("@pl", plname);
                command.Parameters.AddWithValue("@mp", path);
                command.ExecuteNonQuery();

            }
            catch (SqlException ee)
            {
                MessageBox.Show("Ошибка чтения с базы данных:" + Environment.NewLine + ee.Message);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
        static public void RemovePlaylist(string plname)
        {
            string cmdText = "DELETE FROM MUSIC WHERE MUSIC_PLAYLIST = @pl DELETE FROM PLAYLIST WHERE PL_NAME = @pl1";
            SqlCommand command = new SqlCommand(cmdText, sqlconnection);
            try
            {
                command.Parameters.AddWithValue("@pl", plname);
                command.Parameters.AddWithValue("@pl1", plname);
                command.ExecuteNonQuery();

            }
            catch (SqlException ee)
            {
                MessageBox.Show("Ошибка чтения с базы данных:" + Environment.NewLine + ee.Message);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
    }
}
