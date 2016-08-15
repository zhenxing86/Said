﻿using Said.Application;
using Said.Common;
using Said.Models;
using Said.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Said.Areas.Back.Controllers
{
    public class BlogController : BaseController
    {
        #region Pages
        //
        // GET: /Back/Blog/

        public ActionResult Index()
        {
            ViewData["models"] = BlogApplication.FindAllToListSection().ToList();//仅包含关键数据：BTitle,BSummary,CName,BDate,BPV,BComment
            return View();
        }

        [HttpGet]
        public ActionResult AddBlog()
        {
            ViewBag.Title = "添加Blog - Said后台管理系统 ";
            //初始化页面需要的数据
            ViewData["ClassifysList"] = ClassifyApplication.Find();
            ViewData["Tags"] = TagApplication.Find();
            ViewData["TagList"] = TagApplication.Find();
            //TOD 这个方法跟踪进去，有TODO
            var test = BlogApplication.GetAllBlogFileName().ToList<Blog>();
            //TODO 添加成功了之后要检查文件名是否读出来了
            foreach (Blog item in test)
            {
                Console.Write(item.BName);
            }
            ViewData["BlogFiles"] = test;

            return View();
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return View("Error");
            var model = BlogApplication.Find(id);
            if (model == null)
            {
                return RedirectToAction("Index", new
                {
                    formUrl = Request.Url.AbsoluteUri
                });
            }
            //初始化页面需要的数据
            ViewData["ClassifysList"] = ClassifyApplication.Find();
            ViewData["TagList"] = TagApplication.Find().ToList();
            ViewData["BlogFiles"] = BlogApplication.GetAllBlogFileName().ToList();
            ViewData["BlogTags"] = BlogTagsApplication.FindByBlogId(model.BlogId).ToList();
            return View(model);
        }


        /// <summary>
        /// 预览文章页
        /// </summary>
        /// <param name="BImg"></param>
        /// <param name="BTitle"></param>
        /// <param name="BHTML"></param>
        /// <param name="ClassifyId"></param>
        /// <param name="BTag"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Preview(string BImg, string BTitle, string BHTML, string ClassifyId, IList<string> BTag)
        {
            ViewData["BTag"] = BTag;
            ViewData["BImg"] = BImg;
            ViewData["BTitle"] = BTitle;
            ViewData["BHTML"] = HttpUtility.UrlDecode(BHTML);
            ViewData["ClassifyId"] = ClassifyId;
            ViewData["NavigatorIndex"] = 1;
            return View();
        }
        #endregion





        #region Service
        [HttpPost]
        public JsonResult AddBlog(Blog model)
        {


            //if (string.IsNullOrWhiteSpace(model.ClassifyId))
            //    return ResponseResult(1, "没有填写分类信息");

            //修正编码数据
            model = UrlCommon.DecodeModel(model);
            IList<Tag> tags = null;
            if (!String.IsNullOrWhiteSpace(Request["Tags"]))
            {
                //反序列化tag
                tags = JavaScriptCommon.DeSerialize<IList<Tag>>(UrlCommon.Decode(Request["Tags"]));
            }
            else {
                return ResponseResult(1, new { msg = "标签不允许为空" });
            }

            string validateResult = BlogApplication.ValidateAndCorrectSubmit(model);
            if (validateResult == null)
            {
                BlogApplication.AddBlog(model, tags);
                return ResponseResult(new { id = model.BlogId });
            }
            else
            {
                return ResponseResult(1, new { msg = validateResult });
            }

        }


        [HttpPost]
        public JsonResult Edit(Blog newModel)
        {
            newModel = UrlCommon.DecodeModel(newModel);
            if (string.IsNullOrWhiteSpace(newModel.BlogId))
                return ResponseResult(-1, "要编辑的文章ID不正确（无法获取）");
            var model = BlogApplication.Find(newModel.BlogId);
            IList<Tag> tags = null;
            if (!string.IsNullOrWhiteSpace(Request["Tags"]))
            {
                //反序列化tag
                tags = JavaScriptCommon.DeSerialize<IList<Tag>>(UrlCommon.Decode(Request["Tags"]));
            }
            else {
                return ResponseResult(1, new { msg = "标签不允许为空" });
            }
            //TODO 应该先对两个blog进行修改，如果发现是一样的就不修改blog了
            string validateResult = BlogApplication.ValidateAndCorrectSubmit(newModel);
            if (validateResult == null)
            {
                if (BlogApplication.EditBlog(newModel, model, tags) > 0)
                    return ResponseResult(new { id = newModel.BlogId });
                else
                    return ResponseResult(2, "修改Blog失败");
            }
            else
            {
                return ResponseResult(1, new { msg = validateResult });
            }
        }

        /// <summary>
        /// 分页获取Blog列表
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="search"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public JsonResult GetBlogList(int limit, int offset, string search = null, string sort = null, string order = null)
        {
            var page = new Page
            {
                PageNumber = offset / limit + 1,
                PageSize = limit
            };
            var res = BlogApplication.FindToListSectionByKeywords(page, "这是一篇测试文章");
            return Json(new
            {
                //hasNextPage = res.HasNextPage,
                //hasPreviousPage = res.HasPreviousPage,
                total = res.Count,
                rows = res.ToList<Blog>()
            }, JsonRequestBehavior.AllowGet);
        }



        #region 逻辑删除一篇Said
        /// <summary>
        /// 逻辑删除一篇文章
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        public JsonResult Delete(string id)
        {
            Blog model = BlogApplication.Find(id);
            if (model == null)
                return ResponseResult(1, "要删除的文章不存在（数据库未检索到该文章ID）");
            return BlogApplication.LogicDelete(model) > 0 ?
                ResponseResult()
                : ResponseResult(2, "从数据库中删除文章失败");
        }

        /// <summary>
        /// 物理删除一篇文章
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        public JsonResult RealDelete(string id)
        {
            Blog model = BlogApplication.Find(id);
            if (model == null)
                return ResponseResult(1, "要删除的文章不存在（数据库未检索到该文章ID）");
            return SaidCommon.Transaction(() =>
            {
                var blogTags = BlogTagsApplication.FindByBlogId(model.BlogId);
                if (blogTags != null && blogTags.Count() > 0)
                {
                    if (BlogTagsApplication.DeleteByBlogId(model.BlogId) <= 0)
                    {
                        return ResponseResult(3, "删除文章失败（删除Blog和标签对应的关系失败）");
                    };
                }
                return BlogApplication.DeleteBlog(model) > 0 ?
                ResponseResult()
                : ResponseResult(2, "从数据库中删除文章失败");
            });

        }
        #endregion
        #endregion


    }
}
