﻿using System;
using Simpler.Core.Tasks;

namespace Simpler
{
    public static class Run<TJob>
    {
        public class Output
        {
            public Output(dynamic job)
            {
                _job = job;
            }

            dynamic _job;
            public dynamic Get()
            {
                _job.Run();
                return _job.Out;
            }
        }

        public static Output Set(Action<dynamic> set)
        {
            var createJob = new CreateTask {JobType = typeof (TJob)};
            createJob.Run();
            dynamic job = createJob.JobInstance;

            set(job.In);

            return new Output(job);
        }

        public static dynamic Get()
        {
            var createJob = new CreateTask { JobType = typeof(TJob) };
            createJob.Run();
            dynamic job = createJob.JobInstance;

            job.Run();
            return job.Out;
        }
    }
}
