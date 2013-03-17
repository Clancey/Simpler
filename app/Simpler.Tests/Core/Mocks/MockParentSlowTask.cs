namespace Simpler.Tests.Core.Mocks
{
    public class MockParentSlowTask : Task
    {
        public MockSlowTask MockSlowTask { get; set; }

        public override void Execute()
        {
            for (int i = 0; i < 10; i++)
            {
                MockSlowTask.Execute();                
            }
        }
    }
}
