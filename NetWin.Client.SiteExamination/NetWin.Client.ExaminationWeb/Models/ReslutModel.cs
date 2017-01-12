using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetWin.Client.ExaminationWeb.Models
{
    /// <summary>
    /// 通用结果返回实体
    /// </summary>
    public class ReslutModel
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        public bool Result { set; get; }

        /// <summary>
        /// 返回提示信息
        /// </summary>
        public string Message { set; get; }
    }


    /// <summary>
    /// 通用结果返回实体
    /// </summary>
    public class ReslutModel<T>
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        public bool Result { set; get; }

        /// <summary>
        /// 返回提示信息
        /// </summary>
        public string Message { set; get; }

        /// <summary>
        /// 返回参数
        /// </summary>
        public T Data { set; get; }
    }
}