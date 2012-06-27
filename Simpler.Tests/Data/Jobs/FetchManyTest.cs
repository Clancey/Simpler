﻿using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using System.Data;
using Simpler.Data.Jobs;
using Simpler.Tests.Mocks;

namespace Simpler.Tests.Data.Jobs
{
    [TestFixture]
    public class FetchManyTest
    {
        [Test]
        public void should_return_an_object_for_each_record_returned_by_the_select_command()
        {
            // Arrange
            var job = Job.New<FetchMany<MockObject>>();

            var table = new DataTable();
            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Columns.Add("Age", Type.GetType("System.Int32"));
            table.Rows.Add(new object[] { "John Doe", "21" });
            table.Rows.Add(new object[] { "Jane Doe", "19" });

            var mockSelectCommand = new Mock<IDbCommand>();
            mockSelectCommand.Setup(command => command.ExecuteReader()).Returns(table.CreateDataReader());
            job.In.SelectCommand = mockSelectCommand.Object;

            // Act
            job.Run();

            // Assert
            Assert.That(job.Out.ObjectsFetched.Count(), Is.EqualTo(2));
            Assert.That(job.Out.ObjectsFetched[0].Name, Is.EqualTo("John Doe"));
            Assert.That(job.Out.ObjectsFetched[1].Name, Is.EqualTo("Jane Doe"));
        }
    }
}
