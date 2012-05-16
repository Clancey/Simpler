using System;
using Simpler.Proxy;

namespace Simpler
{
    [_InjectJobs]
    public abstract class InJob<TIn> : Job 
        where TIn : class
    {
        TIn _in;
        public TIn In
        {
            get { return _in ?? (_in = (TIn) Activator.CreateInstance(typeof (TIn))); }
            set { _in = value; }
        }
    }
}