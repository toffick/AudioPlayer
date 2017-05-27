using AudioPlayer_v1._0.WebSearch;
using AudioPlayer_v1._0.Windows;
using PlayL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSearch;

namespace WSearch
{
    class MainSearcher
    {

        WebResponse webresponse;
        HtmlParse htmlparser;

        public MainSearcher()
        {
             webresponse = new WebResponse();
             htmlparser = new HtmlParse();
        }
        public async Task<List<object>> GetFindedTrackListAsync(string query)
        {
            string html =await webresponse.GetHtmltextFromPageByLinkAsync(
                WebResponse.queryString + query);

            if (html != null)
            {
                Task<List<object>> lo = htmlparser.SearchTracksAsync(html);
                return await lo;
            }
            else
                throw new Exception("Проблемы с интернет соединением");
        }

        public void DownloadButton_Click(object obj_track)
        {
            PlaylistControl _pl = PlaylistControl.GetPlaylistControl();
            TrackInfo trinf = obj_track as TrackInfo;
            DownloadWindow dw = new DownloadWindow(_pl, trinf);
            dw.ShowDialog();
        }

        public async void CheckInternetConnection()
        {
            if(!await webresponse.checkInternetConnection())
                
             DownloadNotificationPushWIndow.ShowPushNotification("Возможно, проблемы с интернет соединением");
        }
    }
}
