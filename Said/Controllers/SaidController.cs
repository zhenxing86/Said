using PagedList;
using Said.Application;
using Said.Controllers.Filters;
using Said.Models;
using Said.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Said.Controllers
{
    [UserFilterAttribute]
    public class SaidController : BaseController
    {
        /// <summary>
        /// 一页数据个数
        /// </summary>
        int PageLimit = 10;
        //
        // GET: /Said/

        public ActionResult Index(string pageIndex = null)
        {
            ViewData["NavigatorIndex"] = 2;
            int index = 1;
            if (!string.IsNullOrEmpty(pageIndex))
            {
                int.TryParse(pageIndex, out index);
            }
            var page = new Page
            {
                PageNumber = index / PageLimit + 1,
                PageSize = PageLimit
            };
            IPagedList<Article> list = ArticleApplication.Find(page);
            ViewData["total"] = list.TotalItemCount;
            ViewData["articles"] = list.ToList<Article>();
            ViewData["pageIndex"] = index;
            ViewData["maxPage"] = list.TotalItemCount % PageLimit == 0 ? list.TotalItemCount / PageLimit : list.TotalItemCount / PageLimit + 1;
            return View();
        }

        #region Pages
        /// <summary>
        /// Said文章页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Article(string id)
        {
            return View();
        }
        #endregion


        #region Service

        #endregion
    }
}
