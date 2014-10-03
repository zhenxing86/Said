﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Said.Dal;
using Said.DataHelper.DBHelper;
using Said.IDal;

namespace Said.DB
{
    /// <summary>
    /// 数据库实例访问工厂
    /// </summary>
    public static class DataFactory
    {
        #region 根据指定的枚举返回数据库访问实例对象
        /// <summary>
        /// 根据指定的枚举返回数据库访问实例对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IDBHelper Get(DBType type)
        {
            switch (type)
            {
                case DBType.SqlServer:
                    return new SqlDBHelper();
                case DBType.Oracle:
                    return new OracleDBHelper();
                case DBType.MySql:
                    return new MySqlDBHelper();
            }
            return new SqlDBHelper();
        }
        #endregion

        #region 默认返回SqlServer的数据库访问实例对象
        /// <summary>
        /// 默认返回SqlServer的数据库访问实例对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IDBHelper Get() { return new SqlDBHelper(); }
        #endregion
    }
}
