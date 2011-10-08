using System;
using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using Simpler.Data;
using Simpler.Data.Tasks;
using Moq;
using System.Data;
using Simpler.Tests.Mocks;

namespace Simpler.Tests.Data.Tasks
{
    [TestFixture]
    public class FetchListTest
    {
        [Test]
        public void should_return_a_dynamic_object_for_each_record_returned_by_the_select_command()
        {
            // Arrange
            var task = TaskFactory<FetchList>.Create();

            var table = new DataTable();
            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Age", Type.GetType("System.Int32"));
            table.Rows.Add(new object[] { "John Doe", "21" });
            table.Rows.Add(new object[] { "Jane Doe", "19" });

            var mockSelectCommand = new Mock<IDbCommand>();
            mockSelectCommand.Setup(command => command.ExecuteReader()).Returns(table.CreateDataReader());
            task.SelectCommand = mockSelectCommand.Object;

            // Act
            task.Execute();

            // Assert
            Assert.That(task.ObjectsFetched.Count(), Is.EqualTo(2));
            Assert.That(task.ObjectsFetched[0].Name, Is.EqualTo("John Doe"));
            Assert.That(task.ObjectsFetched[1].Name, Is.EqualTo("Jane Doe"));
        }

        [Test]
        public void can_create_the_command_using_inputs()
        {
            // Arrange
            var task = TaskFactory<FetchList>.Create();

            var mockSelectCommand = new Mock<IDbCommand>();
            mockSelectCommand.Setup(command => command.ExecuteReader()).Returns(new DataTableReader(new DataTable()));

            var mockConnection = new Mock<IDbConnection>();
            mockConnection.Setup(c => c.CreateCommand()).Returns(mockSelectCommand.Object);

            // Act
            task.Execute(
                new FetchListInputs
                {
                    Connection = mockConnection.Object,
                    Sql = "select something"
                });

            // Assert
            mockSelectCommand.VerifySet(c => c.CommandText = "select something");
        }

        // todo - [Test]
        public void can_create_the_command_and_parameters_using_inputs()
        {
            // Arrange
            var task = TaskFactory<FetchList>.Create();

            // Act
            task.Execute(
                new
                {
                    Sql = "select something"
                });

            // Assert
        }

        // todo - [Test]
        public void should_use_first_connection_in_config_and_given_sql()
        {
            // Arrange
            var task = TaskFactory<FetchList>.Create();

            // Act
            task.Execute(
                new
                {
                    Sql = "select something"
                });

            // Assert
        }
    }
}
