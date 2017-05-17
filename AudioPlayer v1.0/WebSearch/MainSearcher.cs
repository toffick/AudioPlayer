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
        public List<TrackInfo> getFindedTrack(string query)
        {
            WebResponse webresponse = new WebResponse();
            HtmlParse htmlparser = new HtmlParse();
            List<TrackInfo> tracksinfo = new List<TrackInfo>();

            string html = webresponse.getHtmltextFromPageByLink(
                WebResponse.queryString + query);

            tracksinfo = htmlparser.search(html);
            if (tracksinfo != null)
            {
                return tracksinfo;
            }
            else
                return null;

        }
    }
}
