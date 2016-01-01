﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Said
{
    /// <summary>
    /// Said全局帮助类
    /// </summary>
    public class SaidCommon
    {
        /// <summary>
        /// 创建一个对象ID（新增对象的时候）
        /// </summary>
        /// <returns></returns>
        public static string CreateId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>
        /// 获取一个符合Said要求的GUID（剔除"-"号），作为新增对象使用，每次获取都会生成
        /// </summary>
        public static string GUID
        {
            get { return CreateId(); }
        }


        /// <summary>
        /// 将时间转换为本地文本（2016-01-01 22:31:54 => 昨天 22:00）
        /// </summary>
        /// <returns></returns>
        public static string DateToLocal(DateTime date)
        {
            TimeSpan timespan = DateTime.Now - date;
            if (timespan.TotalSeconds < 60)
            {
                return "刚才";
            }
            if (timespan.TotalMinutes < 60)
            {
                return "1小时";
            }
            if (timespan.TotalDays < 1 && timespan.Days < 1)
            {
                return date.ToString("今天 HH:mm");
            }
            if (timespan.TotalDays < 2 && timespan.Days < 2)
            {
                return date.ToString("昨天 HH:mm");
            }
            if (timespan.TotalDays < 3 && timespan.Days < 3)
            {
                return date.ToString("前天 HH:mm");
            }
            return date.ToString("yyyy-MM-dd HH:mm");
        }
    }
}