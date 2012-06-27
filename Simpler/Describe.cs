using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Simpler.Proxy.Jobs;

namespace Simpler
{
    public class Describe
    {
        public static void Assembly(string assemblyName)
        {
            var noTests = new List<string>();
            var failures = new List<string>();

            var assembly = AppDomain.CurrentDomain.Load(new AssemblyName(assemblyName));
            DescribeAssembly(assembly, noTests, failures);

            if (failures.Any())
            {
                NUnit.Framework.Assert.Fail(String.Format("{0} tests failed.", failures.Count()));
            }

            if (noTests.Any())
            {
                NUnit.Framework.Assert.Inconclusive(String.Format("{0} jobs are missing tests.", noTests.Count()));
            }
        }

        public static void Job<T>() where T : Job
        {
            DescribeJob(typeof(T));
        }

        static void DescribeAssembly(Assembly assembly, List<string> noTests, List<string> failures)
        {
            var jobTypes = assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof (Job))
                    && type.IsPublic
                    && !type.IsAbstract
                    && !type.Name.Contains("Proxy"))
                .OrderBy(type => type.FullName);

            var count = jobTypes.Count();
            if (count > 0)
            {
                const string message = "Testing assembly {0}, that contains {1} jobs.";
                Console.WriteLine(String.Format(message, assembly.FullName, count));
            }

            foreach (var jobType in jobTypes)
            {
                var typeToCreate = jobType;

                var genericArguments = jobType.GetGenericArguments();
                if (genericArguments.Length > 0)
                {
                    // We only need to call the job's Test() method, so it doesn't matter
                    // what type of generic arguments are passed.
                    var objectTypes = genericArguments
                        .Select(genericArgument => typeof (object)).ToArray();
                    typeToCreate = jobType.MakeGenericType(objectTypes);
                }

                try
                {
                    Console.WriteLine("");
                    DescribeJob(typeToCreate);
                }
                catch (NoSpecsException)
                {
                    noTests.Add(typeToCreate.Name);
                }
                catch
                {
                    failures.Add(typeToCreate.Name);
                }
            }
        }

        static void DescribeJob(Type jobType)
        {
            var createJob = new CreateJob {JobType = jobType};
            createJob.Run();
            var job = (Job) createJob.JobInstance;

            Console.WriteLine("  " + job.Name);
            try
            {
                job.Specs();
            }
            catch (NoSpecsException)
            {
                Console.WriteLine("    CAN'T DO ANYTHING? (This job is missing specs.)");
                throw;
            }
        }
    }
}