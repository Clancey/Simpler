using System;
using Simpler.Core.Jobs;

namespace Simpler
{
    public class It<TJob> where TJob : Task
    {
        public static void Should(string expectation, Action<TJob> action)
        {
            var createJob = new CreateTask { JobType = typeof(TJob) };
            createJob.Run();
            var job = (TJob)createJob.JobInstance;

            try
            {
                action(job);
                Console.WriteLine("    can " + expectation);
            }
            catch
            {
                Console.WriteLine("    FAILED to " + expectation);
                throw;
            }
        }
    }
}