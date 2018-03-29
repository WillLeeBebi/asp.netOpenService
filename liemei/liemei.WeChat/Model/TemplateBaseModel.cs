using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.WeChat.Model
{
    public class TemplateBaseModel<T>
    {
        /// <summary>
        /// 用户的OPenID
        /// </summary>
        public string touser { get; set; }
        /// <summary>
        /// 模板ID
        /// </summary>
        public string template_id { get; set; }
        /// <summary>
        /// 点击详情链接
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 数据源
        /// </summary>
        public T data { get; set; }
    }
    public class TemplateBaseMsg
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string value { get; set; }

        private string _color = "#173177";
        /// <summary>
        /// 显示颜色
        /// </summary>
        public string color
        {
            get { return _color; }
            set { _color = value; }
        }
    }

    /// <summary>
    /// 测评完成通知模板
    /// </summary>
    public class EvaluationResultTemplate
    {
        public TemplateBaseMsg first { get; set; }

        public TemplateBaseMsg keyword1 { get; set; }

        public TemplateBaseMsg keyword2 { get; set; }

        public TemplateBaseMsg keyword3 { get; set; }

        public TemplateBaseMsg keyword4 { get; set; }
        public TemplateBaseMsg remark { get; set; }
    }
}
