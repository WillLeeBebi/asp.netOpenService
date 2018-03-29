using liemei.Common.cache;
using liemei.Common.common;
using liemei.Service.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace liemei.Service.JobService
{
    /// <summary>
    /// 定期删除昨天的微信扫码二维码图片文件
    /// </summary>
    public class ClearQCPicJob : BaseJob
    {
        public override void Start()
        {
            IsWork = true;

            ThreadPool.QueueUserWorkItem(new WaitCallback(p=> {
                while (IsWork)
                {
                    int hour = DateTime.Now.Hour;
                    if (hour>1 && hour<5)
                    {
                        ClearQC();
                    }
                    Thread.Sleep(OneHour);
                }
            }),null);
        }
        public override void Stop()
        {
            IsWork = false;
        }

        void ClearQC()
        {
            ClassLoger.Info("ClearQCPicJob.ClearQC","定时清理图片文件任务启动");

            string key = CacheKey.GetQrCodeKey(DateTime.Now.AddDays(-1));
            if (RedisBase.ContainsKey(key))
            {
                int i = 0;
                while (IsWork)
                {
                    List<string> qcimageList = RedisBase.List_GetList<string>(key, i, 1000);
                    foreach (string pathStr in qcimageList)
                    {
                        try
                        {
                            if (File.Exists(pathStr))
                            {
                                File.Delete(pathStr);
                            }
                        }
                        catch (Exception ex) { }
                    }
                    i++;
                    if (qcimageList == null || qcimageList.Count == 0)
                        break;
                }

                RedisBase.List_RemoveAll<string>(key);
            }
            
            ClassLoger.Info("ClearQCPicJob.ClearQC", "图片文件清理完毕");
        }
    }
}