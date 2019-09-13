using Quartz;
using Quartz.Impl;

namespace BHDBusiness.Scheduler.Auth
{
    public class SessionSchedulerController
    {
        public static ISchedulerFactory SchedulerFactory = new StdSchedulerFactory();
        protected IScheduler SessionScheduler = null;

        public void StartCheckSession()
        {
            if (SessionScheduler == null)
            {
                SessionScheduler = SchedulerFactory.GetScheduler();
            }

            //if (sessionScheduler.InStandbyMode || !sessionScheduler.IsShutdown)
            //{
            //    sessionScheduler.Shutdown(true);
            //}
            IJobDetail sessionJobDetail = JobBuilder.Create<SessionCheckerJob>()
                .WithIdentity("sessionJobDetail").Build();
            ITrigger sessionJobTrigger = TriggerBuilder.Create().ForJob(sessionJobDetail)
                .WithCronSchedule("0 0/2 * * * ?")
                .WithIdentity("sessionJobTrigger")
                .StartNow()
                .Build();
            SessionScheduler.ScheduleJob(sessionJobDetail, sessionJobTrigger);
            SessionScheduler.Start();
        }

        public void StopCheckSession()
        {
            if (SessionScheduler == null)
            {
                SessionScheduler = SchedulerFactory.GetScheduler("sessionScheduler");
            }

            SessionScheduler.Shutdown(true);
        }
    }
}