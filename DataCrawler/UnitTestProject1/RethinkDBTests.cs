using System;
using System.Threading.Tasks;
using Crawler;
using NUnit.Framework;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;

namespace UnitTestProject1
{
    [TestFixture]
    public class RethinkDBTests
    {
        public static RethinkDB R = RethinkDB.R;

        public static string Db = "Database1";
        public static string JobConfigTable = "jobConfig";

        const string type = "HastusEntity";

        [Test]
        public void Test1()
        {
            var connection = R.Connection()
           .Hostname("localhost")
           .Port(RethinkDBConstants.DefaultPort)
           .Timeout(60)
           .Connect();

            //var test = R.Db(Db).TableCreate(JobConfig).Run(connection);

            var config = new JobConfig(1, DateTimeOffset.Now, StatusEnum.Aborted, type);

            var resultat = R.Db(Db)
                .Table(JobConfigTable)
                .Insert(config).OptArg("conflict", "replace")
                .RunResult(connection);
            
        }

        [Test]
        public async Task Test2()
        {
            var connection = R.Connection()
          .Hostname("localhost")
          .Port(RethinkDBConstants.DefaultPort)
          .Timeout(60)
          .Connect();

            var feed = await R.Db(Db).Table(JobConfigTable)
                              .Changes().RunChangesAsync<JobConfig>(connection);

            foreach (var message in feed)
            {
                Console.WriteLine($"{message.NewValue.Status}: {message.OldValue.Status}");
            }

        }
    }
}