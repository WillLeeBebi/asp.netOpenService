using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Common.Models
{
    public class JsonResult<T>
    {
        /// <summary>
        /// Json状态码：0.接口逻辑正常但值为空；1.接口逻辑正常，有返回值
        ///             -1.接口异常；-2.签名失败；-3.访问接口超时
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// Json状态说明
        /// </summary>
        public string msg { get; set; }

        public T Result { get; set; }

        public string ResultMsg { get; set; }
    }
    /// <summary>
    /// 返回列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonResultList<T>
    {
        public int code { get; set; }
        public string msg { get; set; }

        public List<T> Result { get; set; }

        public long Total { get; set; }
    }
}
