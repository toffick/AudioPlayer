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
        public async void downloadTrackByLink(TrackInfo track)
        {
            Console.WriteLine("Идет загрузка");
            using (var client = new WebClient())
            {
                await client.DownloadFileTaskAsync(
                    new Uri(track.Downloadlink),
                     $@"D:\БГТУ\КУРСОВОЙ ПРОЕКТ\my_api_test\folderwithtracks\{track.Author}-{track.Title}.mp3");
            }
            Console.WriteLine($"Загрузка {track.Author}-{track.Title}.mp3 завершена");
        }


        /// <summary>
        /// получить текст хтмл страницы по ссылке
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public string getHtmltextFromPageByLink(string link)
        {

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();


                if (response != null)
                {
                    var strreader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    var responseToString = strreader.ReadToEnd();
                    return responseToString;
                }

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
        public string getLinkToDownload(string secondPartOfLink)
        {
            string downloadpagelink = mainpageString + secondPartOfLink;

            try
            {
                return getHtmltextFromPageByLink(downloadpagelink).Split('\"')[3];
            }
            catch
            {
                return TrackInfo.badLinkValue;
            }


        }


    }
}
