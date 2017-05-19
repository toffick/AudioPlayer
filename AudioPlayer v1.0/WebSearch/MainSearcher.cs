using AudioPlayer_v1._0.WebSearch;
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
        public async Task<List<object>> GetFindedTrackListAsync(string query)
        {
            WebResponse webresponse = new WebResponse();
            HtmlParse htmlparser = new HtmlParse();

            string html =await webresponse.GetHtmltextFromPageByLinkAsync(
                WebResponse.queryString + query);

            if (html != null)
            {
                Task<List<object>> lo = htmlparser.SearchTracksAsync(html);
                return await lo;
            }
            else
                throw new Exception("Невозможно подключиться к серверу");
        }

        public void DownloadButton_Click(object obj_track, PlaylistControl _pl)
        {
            TrackInfo trinf = obj_track as TrackInfo;
            DownloadWindow dw = new DownloadWindow(_pl, trinf);
            dw.ShowDialog();
        }
    }
}
