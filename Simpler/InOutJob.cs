using System;
using Simpler.Proxy;

namespace Simpler
{
    [_InjectJobs]
    public abstract class InOutJob<TIn, TOut> : Job 
        where TIn : class 
        where TOut : class
    {
        TIn _in;
        public TIn In
        {
            get { return _in ?? (_in = (TIn)Activator.CreateInstance(typeof(TIn))); }
            set { _in = value; }
        }

        TOut _out;
        public TOut Out
        {
            get { return _out ?? (_out = (TOut)Activator.CreateInstance(typeof(TOut))); }
            set { _out = value; }
        }
    }
}