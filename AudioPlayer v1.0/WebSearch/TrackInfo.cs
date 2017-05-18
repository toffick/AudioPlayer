using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSearch
{
    public class TrackInfo
    {
        public  static  string badTime = "null";

        public static string badAuthorName= "noauthor";

        public static string badTitle = "notitle";

        public static string badLinkValue = "nolink";



        public string Author { get; set; }
        public string Title { get; set; }
        public string Time { get; set; }
        public string Downloadlink { get; set; }

        public TrackInfo()
        {
            Author = badAuthorName;
            Title = badTitle;
            Time = badTime;
            Downloadlink = badLinkValue;
        }
        public override string ToString()
        {
            return string.Format("Автор: {0}\nНазвание: {1}\nВремя: {2}\nСсылка: +\n----------------------------\n", Author, Title, Time);
        }
    }
}
