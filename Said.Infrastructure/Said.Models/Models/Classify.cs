﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Said.Models
{
    /// <summary>
    /// 分类
    /// </summary>
    public class Classify : BaseModel
    {
        #region 属性
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public string ClassifyId { get; set; }
        /// <summary>
        /// 类型包含的文章数量
        /// </summary>
        public int CCount { get; set; }
        /// <summary>
        /// 类型icon
        /// </summary>
        public string CIcon { get; set; }

        /// <summary>
        /// 类型下最后一次更新的文章ID
        /// </summary>
        public string CLastBlogId { get; set; }
        /// <summary>
        /// 类型下最后一次更新的文章名称
        /// </summary>
        public string CLastBlogName { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string CName { get; set; }
        #endregion

        public override IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate()
        {
            throw new NotImplementedException();
        }
    }
}
