using System;

namespace Simpler.Tests.Mocks
{
	public class MockSubJob<T> : Job, IDisposable
	{
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }

        public bool DisposeWasCalled { get; set; }

        public void Dispose()
        {
            DisposeWasCalled = true;
        }
    }
}
