using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WSearch
{
    class WebResponse
    {
        public static string queryString = "http://zaycev.net/search.html?query_search=";

        public static string mainpageString = "http://zaycev.net";
        /// <summary>
        /// скачать трек по прямой ссылке
        /// </summary>
        /// <param name="track"></param>
        /// 
        public Task<bool> checkInternetConnection()
        {
            return Task.Run( ( ) => {
                try
                {
                    using ( var client = new WebClient( ) )
                    using ( var stream = client.OpenRead( "http://www.google.com" ) )
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            } );
        }
        public async Task<string> DownloadTrackByLinkAsync(string path, TrackInfo track)
        {
            try
            {
                using (var client = new WebClient())
                {
                    string downloadedeTrackPath = path + $"\\{track.Author}-{track.Title}.mp3";
                    await client.DownloadFileTaskAsync(
                        new Uri(track.Downloadlink), 
                        downloadedeTrackPath);

                    return downloadedeTrackPath;
                }
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message + Environment.NewLine + "Невозможно скачать трек");
            }
        }


        /// <summary>
        /// получить текст хтмл страницы по ссылке
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public async Task<string> GetHtmltextFromPageByLinkAsync(string link)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
                Task<System.Net.WebResponse> wr = request.GetResponseAsync();
                HttpWebResponse response = (HttpWebResponse)(await wr);


                if (response != null)
                {
                    var strreader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    var responseToString = strreader.ReadToEnd();
                    return responseToString;

                }
                else
                    return null;

            }
            catch (Exception ee)
            {
                Console.WriteLine("Невозможно подключиться к серверу\n" + ee.Message);

                return null;
            }

        }




        /// <summary>
        /// получить прямую ссылку на трек из атрибута трека data-url
        /// </summary>
        /// <param name="jsonDownpage"></param>
        /// <returns></returns>
        public async Task<string> GetLinkToDownloadAsync(string secondPartOfLink)
        {
            string downloadpagelink = mainpageString + secondPartOfLink;

            try
            {
                string link = await GetHtmltextFromPageByLinkAsync(downloadpagelink);
                return link.Split('\"')[3];
            }
            catch
            {
                return TrackInfo.badLinkValue;
            }
        }








        /// <summary>
        /// Получить текст хтмл страницы из html
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public string getHtmlText(string filepath)
        {
            FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs);
            return sr.ReadToEnd();
        }
    }
}
