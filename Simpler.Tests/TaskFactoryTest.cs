﻿using NUnit.Framework;
using Simpler.Tests.Injection.Mocks;

namespace Simpler.Tests
{
    [TestFixture]
    public class TaskFactoryTest
    {
        [Test]
        public void should_inject_sub_tasks_before_execution_if_given_type_is_decorated_with_inject_sub_tasks_attribute()
        {
            // Arrange
            var task = TaskFactory<MockParentTask>.Create();

            // Act
            task.Execute();

            // Assert
            Assert.That(task.SubTaskWasInjected, Is.True);
        }
    }
}
