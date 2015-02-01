﻿using Said.Models;
using Said.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Said.Application
{
    public static class TagApplication
    {
        private static ITagService service;
        public static ITagService Context
        {
            get { return service ?? (service = new TagService(new Domain.Said.Data.DatabaseFactory())); }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="song">实体</param>
        /// <returns></returns>
        public static int Add(Tag model)
        {
            Context.Add(model);
            return Context.Submit();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public static int Update(Tag model)
        {
            Context.Update(model);
            return Context.Submit();
        }

        /// <summary>
        /// 根据对象删除
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <returns>返回受影响的行数</returns>
        public static int Delete(Tag model)
        {
            Context.Delete(model);
            return Context.Submit();
        }

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>返回受影响的行数</returns>
        public static int Delete(string id)
        {
            Context.Delete(m => m.TagId == id);
            return Context.Submit();
        }

        /// <summary>
        /// 根据长ID查找一个分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Tag Find(long id)
        {
            return Context.GetById(id);
        }
        /// <summary>
        /// 根据ID查找一个分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Tag Find(string id)
        {
            return Context.GetById(id);
        }
        /// <summary>
        /// 无条件查询全部
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Tag> Find()
        {
            return Context.GetAll();
        }
    }
}