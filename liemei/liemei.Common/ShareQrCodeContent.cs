using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Common
{
    /// <summary>
    /// 分享二维码内容操作
    /// </summary>
    public class ShareQrCodeContent
    {
        /// <summary>
        /// 分享二维码内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string EvaluationResultShareQrCodeContent(string id)
        {
            return string.Format("http://xxxx/Index?erid={0}",id);
        }
    }
}
