﻿using Music;
using PlayL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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


        static public List<Playlist> getAllPlaylists()
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

        static public List<Track> getAllTracksFromPlaylist(string plname)
        {
            List<Track> list = new List<Track>();
            string cmdText = $"Select MUSICFILE_PATH  From MUSIC " +
                "INNER JOIN PLAYLIST ON PLAYLIST.PL_NAME = MUSIC.MUSIC_PLAYLIST WHERE PL_NAME = @plname ";
            SqlCommand command = new SqlCommand(cmdText, sqlconnection);
            try
            {
                command.Parameters.AddWithValue("@plname", plname);

                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new Track(dr[0].ToString(), list.Count + 1));
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

        static public void addPlatlist(int plnumber, string plname)
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

        static public void deletePlaylist(string plname)
        {

            //TODO
            //string cmdText = "INSERT PLAYLIST(PL_NUMBER, PL_NAME)    VALUES(@plnumber, @plname)";
            //SqlCommand command = new SqlCommand(cmdText, sqlconnection);
            //try
            //{
            //    command.Parameters.AddWithValue("@plnumber", plnumber);
            //    command.ExecuteNonQuery();

            //}
            //catch (SqlException ee)
            //{
            //    MessageBox.Show("Ошибка чтения с базы данных:" + Environment.NewLine + ee.Message);
            //}
            //catch (Exception ee)
            //{
            //    MessageBox.Show(ee.Message);
            //}
        }

        static public void addSongToPlaylist(string plname, string path)
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

    }
}
