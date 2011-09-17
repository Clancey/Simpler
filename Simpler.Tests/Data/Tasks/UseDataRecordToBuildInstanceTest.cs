using NUnit.Framework;
using Simpler.Data.Tasks;
using System.Data;
using Moq;

namespace Simpler.Tests.Data.Tasks
{
    [TestFixture]
    public class UseDataRecordToBuildInstanceTest
    {
        [Test]
        public void should_populate_object_using_all_columns_in_the_data_record()
        {
            // Arrange
            var task = TaskFactory<UseDataRecordToBuildInstance>.Create();

            var mockDataRecord = new Mock<IDataRecord>();
            mockDataRecord.Setup(dataRecord => dataRecord.FieldCount).Returns(2);
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(0)).Returns("Name");
            mockDataRecord.Setup(dataRecord => dataRecord["Name"]).Returns("John Doe");
            mockDataRecord.Setup(dataRecord => dataRecord.GetName(1)).Returns("Age");
            mockDataRecord.Setup(dataRecord => dataRecord["Age"]).Returns(21);
            task.DataRecord = mockDataRecord.Object;

            // Act
            task.Execute();

            // Assert
            Assert.That(task.Object.Name, Is.EqualTo("John Doe"));
            Assert.That(task.Object.Age, Is.EqualTo(21));
        }
    }
}
