using System;
using System.Collections.Generic;
using BHDBusiness.Cache;
using Quartz;

namespace BHDBusiness.Scheduler.Auth
{
    public class SessionCheckerJob : IJob
    {
        private const int SessionMaxDuration = 60; //in minutes

        public void Execute(IJobExecutionContext context)
        {
            CheckSession();


            //Console.WriteLine("===check session");
        }

        protected void CheckSession()
        {
            DateTime now = DateTime.Now;
            IEnumerator<KeyValuePair<string, Dictionary<string, object>>> iterator =
                SessionCache.GetSessionAttributeEnum();
            while (iterator.MoveNext())
            {
                KeyValuePair<string, Dictionary<string, object>> item = iterator.Current;
                if (!item.Value.ContainsKey(SessionCache.KEY_LAST_ACTIVE_DATE))
                {
                    var lastActiveDate = (DateTime) item.Value[SessionCache.KEY_LAST_ACTIVE_DATE];
                    TimeSpan tmp = now.Subtract(lastActiveDate);
                    if (tmp.Minutes > SessionMaxDuration)
                    {
                        SessionCache.GetSessionAttributes(item.Key).Remove(item.Key);
                    }
                }
                else
                {
                    SessionCache.GetSessionAttributes(item.Key).Remove(item.Key);
                }
            }
        }
    }
}