using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using liemei.Common.common;
using liemei.Service.JobService;

[assembly: OwinStartup(typeof(liemei.Service.Startup))]

namespace liemei.Service
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //启动日志清理
            clearlogs clog = new clearlogs();
            clog.Start();

            //定时任务
            List<BaseJob> jobs = new List<BaseJob>();
            jobs.Add(new ClearQCPicJob());

            foreach (var job in jobs)
            {
                job.Start();
            }
        }
    }
}
