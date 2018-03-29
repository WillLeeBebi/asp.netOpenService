using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace liemei.Service.JobService
{
    public abstract class BaseJob
    {
        public abstract void Start();

        public abstract void Stop();

        protected bool IsWork { get; set; }

        private int _OneHour = 1000 * 60 * 60;
        /// <summary>
        /// 1小时
        /// </summary>
        public int OneHour
        {
            get
            {
                return _OneHour;
            }
        }

        private int _OneMinute = 1000 * 60;
        /// <summary>
        /// 1分钟
        /// </summary>
        public int OneMinute
        {
            get
            {
                return _OneMinute;
            }
        }
    }
}