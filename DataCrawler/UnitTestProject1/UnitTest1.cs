using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Crawler;
using Dapper;
using NUnit.Framework;
using Shouldly;

namespace UnitTestProject1
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public async Task Test()
        {
            var sqlDataStore = new SQLDataStore();

            var customer = await sqlDataStore.GetOrdersAsync();
            customer.Count().ShouldBeGreaterThan(0);
        }

        [Test]
        public void TestMethod1()
        {
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            var connection = new SqlConnection
            {
                ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;" +
                                   "Initial Catalog=testDB;" +
                                   "Integrated Security = True;" +
                                   "Pooling=False;"
            };


            connection.Open();


            var rows = connection.Query("Select * From [dbo].[Table]").ToList();
            stopwatch.Stop();

            Console.WriteLine(stopwatch.Elapsed);

            var test = rows;
        }

        [Test]
        public void TestStart()
        {
            var application = new Application();
            var start = application.Start();

            var test = start;
        }

        [Test]
        public async Task Test1()
        {
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            var ConnectionNW = new SqlConnection()
            {
                ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;" +
                               "Initial Catalog=NORTHWND;" +
                               "Integrated Security=True"
            };
            ConnectionNW.Open();

            var taskSQL = ConnectionNW.QueryAsync<Order>("Select * From [dbo].[Orders]");
            await taskSQL;

            stopwatch.Stop();

            Console.WriteLine(stopwatch.Elapsed);
        }

      
    }
}