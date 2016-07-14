using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;

namespace Crawler
{
    public class RethinkDbStore
    {
        public static RethinkDB R = RethinkDB.R;
        public static string Db = "Database1";
        public static string JobConfigTable = "jobConfig";

        public RethinkDbStore()
        {
            Connection = R.Connection()
                .Hostname("localhost")
                .Port(RethinkDBConstants.DefaultPort)
                .Timeout(60)
                .Connect();
        }

        public IConnection Connection { get; set; }

        public async Task<JobConfig> GetJobConfigAsync()
        {
            var result = await R.Db(Db)
                .Table(JobConfigTable)
                  //.Filter(R.JSON("type": "serviceName"))
                  .Filter(R.HashMap("TypeService", "HastusEntity"))
                .RunCursorAsync<JobConfig>(Connection);

            return (result as IEnumerable<JobConfig>).First();
        }

        public void UpdateStatus(JobConfig jobConfigTable)
        {
            var config = new JobConfig(1, DateTimeOffset.Now, StatusEnum.Aborted, jobConfigTable.TypeService);

            var resultat = R.Db(Db)
                .Table(jobConfigTable)
                .Insert(config).OptArg("conflict", "replace")
                .RunResult(Connection);
        }

        public async Task Test()
        {
            var feed = await R.Db(Db).Table(JobConfigTable)
                              .Changes().RunChangesAsync<JobConfig>(Connection);

            foreach (var message in feed)
            {
                Console.WriteLine($"{message.NewValue.Status}: {message.OldValue.Status}");
            }

        }
    }
}