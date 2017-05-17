﻿using CsQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSearch
{
    class HtmlParse
    {

        /// <summary>
        /// найти все треки по заданному запросу
        /// </summary>
        /// <param name="html"></param>
        /// 

        private WebResponse webresponse;


        public HtmlParse()
        {
            webresponse = new WebResponse();
        }



        public List<TrackInfo> search(string html)
        {
            try
            {
                List<TrackInfo> tracksinfo = new List<TrackInfo>();
                CQ cq = CQ.Create(html);

                if (cq.Find("div.search-page__no-results").Length != 0)
                {
                    Console.WriteLine("Треков нет");
                    return null;
                }
                else
                {
                    CQ t = cq.Find("div.search-page__tracks").Find("div.musicset-track");
                    foreach (IDomObject obj in t)
                    {
                        tracksinfo.Add(getTrackInfo(obj));
                    }
                    return tracksinfo;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine("Невозможно получить значения\n" + ee.Message);
                return null;
            }
        }

        /// <summary>
        /// заполнить элемент трека из  парсинга dom элемента
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private TrackInfo getTrackInfo(IDomObject obj)
        {
            TrackInfo trackinfo = new TrackInfo();
            try
            {
                trackinfo.Time = obj.ChildNodes[1].FirstChild.NodeValue;
            }
            catch
            {
                trackinfo.Time = TrackInfo.badTime;
            }

            try
            {
                trackinfo.Author = obj.ChildNodes[0].ChildNodes[1].FirstChild.FirstChild.NodeValue;
            }
            catch
            {
                trackinfo.Author = TrackInfo.badAuthorName;
            }
            try
            {
                trackinfo.Title = obj.ChildNodes[0].ChildNodes[3].FirstChild.FirstChild.NodeValue;
            }
            catch
            {
                trackinfo.Title = TrackInfo.badTitle;
            }

            trackinfo.Downloadlink = webresponse.getLinkToDownload(obj.GetAttribute("data-url"));

            return trackinfo;
        }


    }
}
