﻿using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using Quartz.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace liemei.Service.JobService
{
    public static class Job
    {
        static Job()
        {
            
        }
        public static void Start()
        {
            XMLSchedulingDataProcessor xMLSchedulingDataProcessor = new XMLSchedulingDataProcessor(new SimpleTypeLoadHelper());
            IScheduler scheduler = (new StdSchedulerFactory()).GetScheduler();
            xMLSchedulingDataProcessor.ProcessFileAndScheduleJobs(AppDomain.CurrentDomain.BaseDirectory + "/quartz_jobs.xml", scheduler);
            scheduler.Start();
        }
    }
}