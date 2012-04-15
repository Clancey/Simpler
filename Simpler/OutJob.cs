using Simpler.Proxy;

namespace Simpler
{
    [_InjectJobs]
    public abstract class OutJob<TOut> : Job
    {
        public TOut _Out { get; set; }

        public TOut Get()
        {
            Run();
            return _Out;
        }
    }
}