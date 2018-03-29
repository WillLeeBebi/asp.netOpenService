using liemei.Dal;
using liemei.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Bll
{
    public class WeChatLoginBll
    {
        public async Task<bool> AddWeChatLogin(WeChatLogin _weChatLogin)
        {
            return  WeChatLoginDal.Ins.AddWeChatLogin(_weChatLogin);
        }

        public async Task<bool> UpdateWeChatLogin(WeChatLogin _weChatLogin)
        {
            return WeChatLoginDal.Ins.UpdateWeChatLogin(_weChatLogin);
        }
        public WeChatLogin GetWeChatLoginByUUID(string uuid)
        {
            return WeChatLoginDal.Ins.GetWeChatLoginByUUID(uuid);
        }
    }
}
