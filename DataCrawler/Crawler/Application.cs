using System;
using System.Net;
using System.Threading.Tasks;

namespace Crawler
{
    public class Application
    {
        private RethinkDbStore _rethinkDbStore;

        public Application()
        {
            sqlDataStore = GetSQLDataStore();
            _rethinkDbStore = new RethinkDbStore();
        }

        public SQLDataStore sqlDataStore { get; set; }

        private SQLDataStore GetSQLDataStore()
        {
            return new SQLDataStore();
        }

        public async Task Start()
        {
            var jobConfig = await _rethinkDbStore.GetJobConfigAsync();
            if (!jobConfig.CanWeRun())
            {
                return;
            }

            try
            {
                var customerTask = sqlDataStore.GetOrdersAsync();
                Console.WriteLine($"customerTask har id {customerTask.Id} og status {customerTask.Status}");

                await customerTask.ConfigureAwait(false);

                Console.WriteLine($"customerTask har id {customerTask.Id} og status {customerTask.Status}");
            }
            catch (Exception ex)
            {
                _rethinkDbStore.UpdateStatus(jobConfig); 
                throw;
            }
            


        }
    }
}