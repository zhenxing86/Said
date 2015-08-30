﻿using PagedList;
using Said.Models;
using Said.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Said.Application
{
    public class BannerApplication
    {
        private static IBannerService service;
        public static IBannerService Context
        {
            get { return service ?? (service = new BannerService(new Domain.Said.Data.DatabaseFactory())); }
        }

        /// <summary>
        /// 添加一个Banner
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Add(Banner model)
        {
            Context.Add(model);
            return service.Submit();
        }

        /// <summary>
        /// 获取指定数量的横幅（日期倒序）
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Banner> GetTop(int count)
        {
            return service.GetTop(count);
        }


        /// <summary>
        /// 获取所有的横幅对象（日期倒序）
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Banner> GetAll()
        {
            return service.GetAllDesc(m => m.Date);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="page">分页对象</param>
        /// <param name="keywords">要查询的关键字</param>
        /// <returns>返回封装后的IPagedList对象</returns>
        public static IPagedList<Banner> Find(Models.Data.Page page, string keywords)
        {
            return Context.GetPage(page, m => m.Description.Contains(keywords), m => m.Date);
        }



        #region 逻辑
        /// <summary>
        /// 验证和修正新增的Banner对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string ValidateAndCorrectSubmit(Banner model)
        {
            if (model == null) return "服务器接收的数据不正确";
            if (string.IsNullOrWhiteSpace(model.Link))
            {
                return "Banner链接不允许为空";
            }
            if (string.IsNullOrWhiteSpace(model.SourceCode) || string.IsNullOrWhiteSpace(model.HTML))
                return "Banner文本源码不允许为空";
            model.Date = DateTime.Now;
            return string.Empty;
        }
        #endregion
    }

}
