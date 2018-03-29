using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using liemei.Common.common;
using System.IO;
using System.Collections.Concurrent;
using liemei.Common.Models;
using System.Collections;

namespace liemei.Common
{
    public class SystemSet
    {
        private static string _mysqlConnectionString = "server=localhost;port=3306;user id=root;password=123456;Charset=utf8;database=test";
        public static string MysqlConnectionString
        {
            get
            {
                return _mysqlConnectionString;
            }
        }

        private static string _appid = "";
        /// <summary>
        /// 微信开放平台唯一标识
        /// </summary>
        public static string Appid
        {
            get
            {
                if (string.IsNullOrEmpty(_appid))
                    _appid = System.Configuration.ConfigurationSettings.AppSettings["Appid"].TryToString();
                return _appid;
            }
        }

        private static string _appsecret = "";
        /// <summary>
        /// 微信开放平台appsecret
        /// </summary>
        public static string Appsecret
        {
            get
            {
                if (_appsecret.IsNull())
                    _appsecret = System.Configuration.ConfigurationSettings.AppSettings["Appsecret"].TryToString();
                return _appsecret;
            }
        }

        private static string _Serviceappid = "";
        /// <summary>
        /// 微信服务号唯一标识
        /// </summary>
        public static string Serviceappid
        {
            get
            {
                if (string.IsNullOrEmpty(_Serviceappid))
                    _Serviceappid = System.Configuration.ConfigurationSettings.AppSettings["Serviceappid"].TryToString();
                return _Serviceappid;
            }
        }
        private static string _Serviceappsecret = "";
        /// <summary>
        /// 微信开放平台appsecret
        /// </summary>
        public static string Serviceappsecret
        {
            get
            {
                if (_Serviceappsecret.IsNull())
                    _Serviceappsecret = System.Configuration.ConfigurationSettings.AppSettings["Serviceappsecret"].TryToString();
                return _Serviceappsecret;
            }
        }

        private static string _Token;
        /// <summary>
        /// 微信服务号令牌
        /// </summary>
        public static string Token
        {
            get
            {
                if (_Token.IsNull())
                    _Token = System.Configuration.ConfigurationSettings.AppSettings["Token"].TryToString();
                return _Token;
            }
        }

        private static string _Mch_id;
        /// <summary>
        /// 微信支付商户号ID
        /// </summary>
        public static string Mch_id
        {
            get
            {
                if (_Mch_id.IsNull())
                    _Mch_id = System.Configuration.ConfigurationSettings.AppSettings["Mch_id"].TryToString();
                return _Mch_id;
            }
        }

        private static string _Mch_Key;
        /// <summary>
        /// 微信支付商户号秘钥
        /// </summary>
        public static string Mch_Key
        {
            get
            {
                if (_Mch_Key.IsNull())
                {
                    _Mch_Key = System.Configuration.ConfigurationSettings.AppSettings["Mch_Key"].TryToString();
                }
                return _Mch_Key;
            }
        }

        private static string _EncodingAESKey;
        public static string EncodingAESKey
        {
            get
            {
                if (string.IsNullOrEmpty(_EncodingAESKey))
                {
                    _EncodingAESKey = System.Configuration.ConfigurationSettings.AppSettings["EncodingAESKey"].TryToString();
                }
                return _EncodingAESKey;
            }
        }

        private static decimal _RoyaltyRate=0;
        /// <summary>
        /// 分享测评分成比例
        /// </summary>
        public static decimal RoyaltyRate
        {
            get
            {
                if (_RoyaltyRate==0)
                {
                    _RoyaltyRate = System.Configuration.ConfigurationSettings.AppSettings["RoyaltyRate"].TryToDecimal();
                }
                return _RoyaltyRate;
            }
        }


        private static string _SMSKey;
        /// <summary>
        /// 短信平台KEY
        /// </summary>
        public static string SMSKey
        {
            get
            {
                if (string.IsNullOrEmpty(_SMSKey))
                {
                    _SMSKey = System.Configuration.ConfigurationSettings.AppSettings["SMSKey"].TryToString();
                }
                return _SMSKey;
            }
        }

        private static string _BaseMenu;
        /// <summary>
        /// 公共号基础菜单Json
        /// </summary>
        public static string BaseMenu
        {
            get
            {
                if (string.IsNullOrEmpty(_BaseMenu))
                {
                    string basemenuFile = Path.Combine(Directory.GetCurrentDirectory(), "BasicsMenu.json");
                    _BaseMenu = FileHelper.GetFileContent(basemenuFile);
                }
                return _BaseMenu;
            }
        }

        private static string _UserMenu;
        /// <summary>
        /// 普通用户微信公共号菜单
        /// </summary>
        public static string UserMenu
        {
            get
            {
                if (string.IsNullOrEmpty(_UserMenu))
                {
                    string UserMenuFile = Path.Combine(Directory.GetCurrentDirectory(), "UserMenu.json");
                    _UserMenu = FileHelper.GetFileContent(UserMenuFile);
                }
                return _UserMenu;
            }
        }

        private static string _DemoMenu;
        /// <summary>
        /// 演示账号微信公共号
        /// </summary>
        public static string DemoMenu
        {
            get
            {
                if (string.IsNullOrEmpty(_DemoMenu))
                {
                    string UserMenuFile = Path.Combine(Directory.GetCurrentDirectory(), "DemoMenu.json");
                    _DemoMenu = FileHelper.GetFileContent(UserMenuFile);
                }
                return _DemoMenu;
            }
        }

        private static string _AdminMenu;
        /// <summary>
        /// 管理员微信公共号菜单
        /// </summary>
        public static string AdminMenu
        {
            get
            {
                if (string.IsNullOrEmpty(_AdminMenu))
                {
                    string adminmenuFile = Path.Combine(Directory.GetCurrentDirectory(), "AdminMenu.json");
                    _AdminMenu = FileHelper.GetFileContent(adminmenuFile);
                }
                return _AdminMenu;
            }
        }

        private static string _webResourcesSite = "";
        /// <summary>
        /// 文件所在站点
        /// </summary>
        public static string WebResourcesSite
        {
            get
            {
                if(_webResourcesSite.IsNull())
                    _webResourcesSite = System.Configuration.ConfigurationSettings.AppSettings["WebResourcesSite"].TryToString();
                return _webResourcesSite;
            }
        }

        private static string _filePath = "";
        /// <summary>
        /// 文件存放根目录
        /// </summary>
        public static string ResourcesPath
        {
            get
            {
                if(_filePath.IsNull())
                    _filePath= System.Configuration.ConfigurationSettings.AppSettings["ResourcesPath"].TryToString();
                return _filePath;
            }
        }
        /// <summary>
        /// 图片文件存放的相对路径
        /// </summary>
        public static string PicPath
        {
            get
            {
                return string.Format("images/{0}/{1}",DateTime.Now.Year,DateTime.Now.Month);
            }
        }
        /// <summary>
        /// 验证码图片存放相对路径
        /// </summary>
        public static string VerCodePicPath
        {
            get
            {
                return string.Format("images/VerCodePic");
            }
        }
        /// <summary>
        /// 验证码图片资源文件夹
        /// </summary>
        public static string VerCodePicSourcePath
        {
            get
            {
                return string.Format("images/VerCodePicSource");
            }
        }
        private static ArrayList _FontPoint;
        public static ArrayList FontPoint
        {
            get
            {
                if (_FontPoint==null)
                {
                    _FontPoint = new ArrayList();

                    for (int x=0;x<10;x++)
                    {
                        for (int y=0;y<5;y++)
                        {
                            _FontPoint.Add(new Models.FontPoint() { X = x * 28, Y = y * 20 });
                        }
                    }
                }
                return _FontPoint;
            }
        }
        /// <summary>
        /// 缩略图目录
        /// </summary>
        public static string Thumbnail
        {
            get
            {
                return string.Format("images/thumbnail");
            }
        }
        /// <summary>
        /// 分享二维码图片路径
        /// </summary>
        public static string QrCodePic
        {
            get
            {
                return "images/QrCode";
            }
        }
        private static string[] _RedisWriteHosts;
        /// <summary>
        /// Redis写操作
        /// </summary>
        public static string[] RedisWriteHosts
        {
            get
            {
                if(_RedisWriteHosts == null)
                    _RedisWriteHosts = System.Configuration.ConfigurationSettings.AppSettings["RedisWriteHosts"].Split(new char[] { ';' });
                return _RedisWriteHosts;
            }
        }

        private static string[] _RedisReadOnlyHosts;
        /// <summary>
        /// Redis读取操作
        /// </summary>
        public static string[] RedisReadOnlyHosts
        {
            get
            {
                if(_RedisReadOnlyHosts == null)
                    _RedisReadOnlyHosts = System.Configuration.ConfigurationSettings.AppSettings["RedisReadOnlyHosts"].Split(new char[] { ';' });
                return _RedisReadOnlyHosts;
            }
        }

        private static string _WindowsClientUserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) WindowsClient/537.36 (KHTML, like Gecko) xx.com/56.0.2924.87 Safari/537.36";

        public static string WindowsClientUserAgent
        {
            get { return _WindowsClientUserAgent; }
        }

        private static string _WebClientUserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) WebClient/537.36 (KHTML, like Gecko) xx.com/56.0.2924.87 Safari/537.36";

        public static string WebClientUserAgent
        {
            get
            {
                return _WebClientUserAgent;
            }
        }

        private static long _TimeOut=30000;

        public static long TimeOut
        {
            get
            {
                return _TimeOut;
            }
        }

        private static string _MqttServiceIP;
        public static string MqttServiceIP
        {
            get
            {
                if (string.IsNullOrEmpty(_MqttServiceIP))
                {
                    _MqttServiceIP= System.Configuration.ConfigurationSettings.AppSettings["MqttServiceIP"].TryToString();
                }
                return _MqttServiceIP;
            }
        }

        private static string _MqttUserName;
        public static string MqttUserName
        {
            get
            {
                if (string.IsNullOrEmpty(_MqttUserName))
                {
                    _MqttUserName= System.Configuration.ConfigurationSettings.AppSettings["MqttUserName"].TryToString();
                }
                return _MqttUserName;
            }
        }

        private static string _MqttPassword;

        public static string MqttPassword
        {
            get
            {
                if (string.IsNullOrEmpty(_MqttPassword))
                {
                    _MqttPassword = System.Configuration.ConfigurationSettings.AppSettings["MqttPassword"].TryToString();
                }
                return _MqttPassword;
            }
        }

        private static string _EvaluationTemplate_id_short = "xxxx";
        /// <summary>
        /// 测评完成模板消息编号
        /// </summary>
        public static string EvaluationTemplate_id_short
        {
            get
            {
                return _EvaluationTemplate_id_short;
            }
        }

        private static int _AdminUserTagID = 102;
        /// <summary>
        /// 
        /// </summary>
        public static int AdminUserTagID
        {
            get { return _AdminUserTagID; }
        }
        private static int _UserTagID = 101;
        /// <summary>
        /// 普通用户标签ID
        /// </summary>
        public static int UserTagID
        {
            get { return _UserTagID; }
        }
        private static string _WordheaderPic;
        /// <summary>
        /// 生成word文档页眉图片
        /// </summary>
        public static string WordheaderPic
        {
            get
            {
                if (_WordheaderPic.IsNull())
                    _WordheaderPic = Path.Combine(ResourcesPath, Thumbnail,"logo.png");
                return _WordheaderPic;
            }
        }

        public static string WordheaderTxt
        {
            get
            {
                return "xx";
            }
        }

        private static string _WordFilePath;
        public static string WordFilePath
        {
            get
            {
                if (_WordFilePath.IsNull())
                    _WordFilePath = "docfile/world";
                return _WordFilePath;
            }
        }

        private static string _ExcelFilePath;
        /// <summary>
        /// excel存储路径
        /// </summary>
        public static string ExcelFilePath
        {
            get
            {
                if (_ExcelFilePath.IsNull())
                    _ExcelFilePath = "docfile/excel";
                return _ExcelFilePath;
            }
        }
      

        public static string EyeKey_AppId { get; } = "xxx";
        public static string EyeKey_AppKey { get; } = "xxxx";
        public static string crowd_id { get; } = "xxx";
    }
}
