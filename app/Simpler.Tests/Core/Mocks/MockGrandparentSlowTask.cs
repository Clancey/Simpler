namespace Simpler.Tests.Core.Mocks
{
    public class MockGrandparentSlowTask : Task
    {
        public MockParentSlowTask MockParentSlowTask { get; set; }
        public MockSlowTask MockSlowTask { get; set; }

        public override void Execute()
        {
            Parallel.Execute(MockParentSlowTask, MockSlowTask);
        }
    }
}
