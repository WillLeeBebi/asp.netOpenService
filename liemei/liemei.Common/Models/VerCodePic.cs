using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Common.Models
{
    /// <summary>
    /// 二维码图片
    /// </summary>
    public class VerCodePic
    {
        /// <summary>
        /// 图片链接
        /// </summary>
        public string PicURL { get; set; }
        /// <summary>
        /// 第一个字位置
        /// </summary>
        public FontPoint Font1 { get; set; }
        /// <summary>
        /// 第二个字位置
        /// </summary>
        public FontPoint Font2 { get; set; }
        /// <summary>
        /// 第三个字位置
        /// </summary>
        public FontPoint Font3 { get; set; }
        /// <summary>
        /// 第四个字位置
        /// </summary>
        public FontPoint Font4 { get; set; }
    }
    /// <summary>
    /// 文字位置
    /// </summary>
    public class FontPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
