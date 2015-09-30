using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwinkleStars.Infrastructure.Utils
{
    public class PagingHelper
    {
        public int TotalPage { get; set; }
        public int CurentPage { get; set; }
        public int StartIndex { get; set; }
        public int Count { get; set; }

        public static int DefaultItemPerPage = 20;

        public static PagingHelper GetPageInfo(int totalItem, int? pageNo, int? itemsToLoad)
        {
            var pageInfo = new PagingHelper();
            var page = pageNo.HasValue && pageNo.Value > 0 ? pageNo.Value : 1;
            var itemPerPage = itemsToLoad.HasValue && itemsToLoad.Value > 0 ? itemsToLoad.Value : DefaultItemPerPage;
            if (totalItem == 0)
            {
                pageInfo.Count = 0;
                return pageInfo;
            }
            pageInfo.TotalPage = ((totalItem % itemPerPage) > 0) ? (totalItem / itemPerPage) + 1 : (totalItem / itemPerPage);
            pageInfo.CurentPage = (page > pageInfo.TotalPage) ? 0 : page;
            pageInfo.StartIndex = (pageInfo.CurentPage == 0) ? 0 : (pageInfo.CurentPage - 1) * itemPerPage;
            pageInfo.Count = (pageInfo.CurentPage == 0) ? 0 : (pageInfo.CurentPage == pageInfo.TotalPage) ? totalItem - pageInfo.StartIndex : itemPerPage;
            return pageInfo;
        }

        public static T FindIndex<T>(IEnumerable<T> searchRange, Func<T, bool> predicate)
        {
            var result = searchRange.FirstOrDefault(predicate);
            return result;
        }
    }
}