using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace Crawler
{
    public class SQLDataStore
    {
        public SQLDataStore()
        {
            //Connection = new SqlConnection
            //{
            //    ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;" +
            //                       "Initial Catalog=testDB;" +
            //                       "Integrated Security = True;" +
            //                       "Pooling=False;"

            //};

            ConnectionNW = new SqlConnection()
            {
                ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;" +
                                   "Initial Catalog=NORTHWND;" +
                                   "Integrated Security=True"
            };
            ConnectionNW.Open();
        }

        public SqlConnection Connection { get; set; }

        public SqlConnection ConnectionNW{ get; set; }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            Task<IEnumerable<Order>> taskSQL = default(Task<IEnumerable<Order>>);
            try
            {
                var concurrentDictionary = new ConcurrentDictionary<int, Task>();

                taskSQL =  ConnectionNW.QueryAsync<Order>("Select * From [dbo].[Orders]");
                
                Console.WriteLine($"taskSQL har id {taskSQL.Id} og status {taskSQL.Status}");
                concurrentDictionary.TryAdd(0, taskSQL);
                #region Test

                var task2 = DelayTask();
                concurrentDictionary.TryAdd(1, task2);

                var task3 = DelayTask();
                concurrentDictionary.TryAdd(2, task3);

                #endregion

                await Task.WhenAll(concurrentDictionary.Values).ConfigureAwait(false);

               

                Console.WriteLine($"taskSQL har id {taskSQL.Id} og status {taskSQL.Status}");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return taskSQL.Result;
        }

        private Task DelayTask()
        {
            var task = Task.Delay(1000);
            return task;
        }
    }
}