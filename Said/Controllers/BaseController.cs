﻿using Said.Application;
using Said.Controllers.Filters;
using Said.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Said.Controllers
{
    public class BaseController : Controller
    {
        //protected override void OnException(ExceptionContext filterContext)
        //{

        //}
        protected AdminApplication adminApplication = new AdminApplication();
        protected AdminRecordApplication adminRecordApplication = new AdminRecordApplication();
        protected ArticleApplication articleApplication = new ArticleApplication();
        protected BlogApplication blogApplication = new BlogApplication();
        protected BlogTagsApplication blogTagsApplication = new BlogTagsApplication();
        protected ClassifyApplication classifyApplication = new ClassifyApplication();
        protected CommentApplication commentApplication = new CommentApplication();
        protected ImageApplication imageApplication = new ImageApplication();
        protected ReplyApplicaiton replyApplicaiton = new ReplyApplicaiton();
        protected SongApplication songApplication = new SongApplication();
        protected TagApplication tagApplication = new TagApplication();
        protected UserApplication userApplication = new UserApplication();
        protected UserLikeApplication userLikeApplication = new UserLikeApplication();
        protected UserRecordApplication userRecordApplication = new UserRecordApplication();

        //这个控制器可以定义顶层控制器的行为

        public BaseController()
        {
            //System.Diagnostics.Debug.Write(ConfigEnum.SourceDataIP);
        }


        /// <summary>
        /// 用户完整信息
        /// </summary>
        public User UserInfo
        {
            get
            {
                return userApplication.FindById(UserId);
            }
        }

        /// <summary>
        /// 用户ID，从Session获取
        /// </summary>
        public string UserId
        {
            get
            {
                return Session["userId"].ToString();
            }
        }

        ///// <summary>
        ///// 是否是Admin
        ///// </summary>
        //public bool IsAdmin
        //{
        //    get { return Session["adminId"] != null; }
        //    //set { IsAdmin = value; }
        //}

        /// <summary>
        /// 管理员ID
        /// </summary>
        public string AdminId
        {
            get { return Session["adminId"] as string; }
            //set { AdminId = value; }
        }



        #region Other

        /// <summary>
        /// 通用返回结果到客户端方法，表示成功
        /// </summary>
        /// <returns></returns>
        protected JsonResult ResponseResult()
        {
            return Json(new { code = 0 });
        }

        /// <summary>
        /// 通用返回结果到客户端方法，表示成功，并发送指定的数据
        /// </summary>
        /// <param name="data">发送的数据</param>
        /// <returns></returns>
        protected JsonResult ResponseResult(object data)
        {
            return Json(new { code = 0, data = data });
        }

        /// <summary>
        /// 通用返回结果到客户端方法，指定错误代码
        /// </summary>
        /// <param name="code">错误代码（0表示没有错误，其他数字表示错误）</param>
        /// <param name="data">(如果有的话)发送的数据</param>
        /// <returns></returns>
        protected JsonResult ResponseResult(int code, object data = null)
        {
            return Json(new { code = code, data = data });
        }

        /// <summary>
        /// 通用返回结果到客户端方法，指定错误代码、错误信息
        /// </summary>
        /// <param name="code">错误代码（0表示没有错误，其他数字表示错误）</param>
        /// <param name="msg">错误信息</param>
        /// <returns></returns>
        protected JsonResult ResponseResult(int code, string msg)
        {
            return Json(new { code = code, msg = msg });
        }
        /// <summary>
        /// 通用返回结果到客户端方法，指定错误代码、错误信息、错误数据
        /// </summary>
        /// <param name="code">错误代码（0表示没有错误，其他数字表示错误）</param>
        /// <param name="msg">错误信息</param>
        /// <param name="data">错误数据</param>
        /// <returns></returns>
        protected JsonResult ResponseResult(int code, string msg, object data = null)
        {
            return Json(new { code = code, msg = msg, data = data });
        }

        #endregion
    }
}
