using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Security;
using Yunpian.conf;
using Yunpian.lib;

namespace liemei.Common.common
{
    public class Utils
    {
        /// <summary>
        /// 获取文件编码格式
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Encoding GetFileEncodingType(string filename)
        {
            Encoding encoding = Encoding.Default;
            using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (System.IO.BinaryReader br = new System.IO.BinaryReader(fs))
                {

                    Byte[] buffer = br.ReadBytes(2);
                    if (buffer[0] >= 0xEF)
                    {
                        if (buffer[0] == 0xEF && buffer[1] == 0xBB)
                            encoding = System.Text.Encoding.UTF8;
                        else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                            encoding = System.Text.Encoding.BigEndianUnicode;
                        else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                            encoding = System.Text.Encoding.Unicode;
                        else
                            encoding = System.Text.Encoding.Default;
                    }
                    else
                        encoding = System.Text.Encoding.Default;
                }
            }
            return encoding;
        }

        /// <summary>
        /// 获取文件的MD5
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileMD5(string fileName)
        {
            string md5str = "";
            if (File.Exists(fileName))
            {
                try
                {
                    FileInfo fi = new FileInfo(fileName);
                    FileStream fs = fi.OpenRead();//new FileStream(RightImage, FileMode.Open);                       
                    System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    byte[] bytes = md5.ComputeHash(fs);
                    fs.Close();
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                        sb.Append(bytes[i].ToString("x2"));
                    md5str = sb.ToString();
                }
                catch (Exception)
                {

                }
            }
            return md5str;
        }

        /// <summary>
        /// MD5函数(utf-8编码)
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str)
        {
            if (str.IsNull())
                return string.Empty;
            byte[] b = Encoding.UTF8.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');

            return ret.ToLower();
        }

        /// <summary>
        /// 生成时间戳       
        /// </summary>
        /// <returns></returns>
        public static long GetUnixTime()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /**
       * 生成时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
        * @return 时间戳
       */
        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /**
        * 生成随机串，随机串包含字母或数字
        * @return 随机串
        */
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>
        /// 时间戳转换为时间
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime UnixToDateTime(long timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Convert.ToDouble(timestamp)).ToLocalTime();
        }


        /// <summary>  
        /// 把汉字字符转码为Unicode字符集  
        /// </summary>  
        /// <param name="strGB">要转码的字符</param>  
        /// <returns>转码后的字符</returns>  
        public static string ConvertToUnicode(string strGB)
        {
            char[] chs = strGB.ToCharArray();
            StringBuilder result = new StringBuilder();
            foreach (char c in chs)
            {
                result.Append(@"\u" + char.ConvertToUtf32(c.ToString(), 0).ToString("x"));
            }
            return result.ToString();
        }

        /// <summary>  
        /// 把Unicode解码为普通文字  
        /// </summary>  
        /// <param name="unicodeString">要解码的Unicode字符集</param>  
        /// <returns>解码后的字符串</returns>  
        public static string ConvertToGB(string unicodeString)
        {
            string[] strArray = unicodeString.Split(new string[] { @"\u" }, StringSplitOptions.None);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i].Trim() == "" || strArray[i].Length < 2 || strArray.Length <= 1)
                {
                    result.Append(i == 0 ? strArray[i] : @"\u" + strArray[i]);
                    continue;
                }
                for (int j = strArray[i].Length > 4 ? 4 : strArray[i].Length; j >= 2; j--)
                {
                    try
                    {
                        result.Append(char.ConvertFromUtf32(Convert.ToInt32(strArray[i].Substring(0, j), 16)) + strArray[i].Substring(j));
                        break;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// 生成指定范围的随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandom(int min, int max)
        {
            Random ro = new Random();
            return ro.Next(min, max);
        }
        public static string Signa(Dictionary<string, object> signpara)
        {
            string signa = string.Empty;
            if (signpara == null || signpara.Count == 0)
            {
                return signa;
            }
            StringBuilder sb = new StringBuilder();
            List<string> keylist = signpara.Keys.OrderBy(x => x).ToList();
            foreach (string key in keylist)
            {
                sb.Append(string.Format("{0}_{1}", key, signpara[key].TryToString()));
            }
            sb.Append("liemei");
            signa = MD5(sb.ToString());
            return signa;
        }

        /// <summary>
        /// 用于获取全局唯一用户ID
        /// </summary>
        /// <returns></returns>
        public static string GetWeChatUUID()
        {
            return string.Format("WX{0}{1}", Utils.GetUnixTime(), Utils.GetRandom(1000, 9999));
        }

        /// <summary> 
        /// SHA1加密字符串 
        /// </summary> 
        /// <param name="source">源字符串</param> 
        /// <returns>加密后的字符串</returns> 
        public static string SHA1(string source)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(source, "SHA1");
        }
        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static int GetVersionCode(string version)
        {
            return version.Replace(".","").TryToInt(0);
        }

        /// <summary>
        /// 将字符串形式的数学公式转换为计算公式并进行计算
        /// </summary>
        /// <param name="MathematicalFormate"></param>
        /// <returns></returns>
        public static double ConvertMathematicalCalculation(string MathematicalFormate)
        {
            //MSScriptControl.ScriptControl sc = new MSScriptControl.ScriptControlClass();
            //sc.Language = "JavaScript";
            //return sc.Eval(MathematicalFormate).TryToDecimal();

            Microsoft.JScript.Vsa.VsaEngine Engine = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
            return Microsoft.JScript.Eval.JScriptEvaluate(MathematicalFormate, Engine).TryToDouble();
        }

        /// <summary>
        /// 生成复杂密码,最短{0}位，最长{0}位
        /// 1，必须包含数字
        /// 2，必须包含字母
        /// </summary>
        /// <param name="minlen"></param>
        /// <param name="maxlen"></param>
        /// <returns></returns>
        public static string GenPsw(int minlen = 6, int maxlen = 16)
        {
            string seed = "abcdefghigkLmnopqrstuvwsxy1234567890";
            StringBuilder sb = new StringBuilder();
            Random random = new Random();
            random = new Random();
            int len = random.Next(minlen, maxlen + 1);
            while (!CheckFullPwd(sb.ToString(), len, len))
            {
                sb.Append(seed[random.Next(0, seed.Length)].TryToString());
                if (sb.Length > len)
                    sb = new StringBuilder();
            }
            return sb.ToString();
        }

        /// <summary>
        /// 验证密码复杂度
        /// 1，必须包含数字
        /// 2，必须包含字母
        /// </summary>
        /// <param name="input"></param>
        /// <param name="minlen"></param>
        /// <param name="maxlen"></param>
        /// <returns></returns>
        public static bool CheckFullPwd(string input, int minlen, int maxlen)
        {
            string patt = string.Format(@"(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[^a-zA-Z0-9]).{{{0},{1}}}", minlen, maxlen);
            var regex = new Regex("[a-zA-Z]");
            var regex1 = new Regex("[0-9]");
            if (regex.IsMatch(input) && regex1.IsMatch(input))
            {
                if (input.Length >= minlen && input.Length <= maxlen)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="tcode">电话号码</param>
        /// <returns></returns>
        public static bool SendSMS(string content,string tcode)
        {
            bool flag = false;
            string cstr = string.Format("【xxxx】{0}",content);
            Dictionary<string, string> data = new Dictionary<string, string>();
            Config config = new Config(SystemSet.SMSKey);
            SmsOperator sms = new SmsOperator(config);
            data.Clear();
            data.Add("mobile", tcode);
            data.Add("text", cstr);
            var reslut = sms.singleSend(data);
            flag = reslut.success;
            if (!flag)
                ClassLoger.Fail("Utils.SendSMS短信发送失败",tcode, reslut.responseText); 
            return flag;
        }
        /// <summary>
        /// 用户发送短信验证码
        /// </summary>
        /// <param name="code">验证码</param>
        /// <param name="tcode">电话号</param>
        /// <returns></returns>
        public static bool SendSMSVCode(string code,string tcode)
        {
            string content = string.Format("您的验证码是{0}，如非本人用户请忽略本短信", code);
            return SendSMS(content, tcode);
        }
    }
}
