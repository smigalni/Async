using System;
using System.Runtime.Remoting;
using Crawler;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Shouldly;

namespace UnitTestProject1
{
    [TestFixture()]
    public class FL
    {
        const string type = "HastusEntity";
        [Test]
        public void Test1()
        {
            var jobConfig = new JobConfig(1, DateTimeOffset.Now, StatusEnum.Finished, type);

            var canWeRun = jobConfig.CanWeRun();
            canWeRun.ShouldBe(true);
        }
        [Test]
        public void Test2()
        {
            var jobConfig = new JobConfig(1, DateTimeOffset.Now, StatusEnum.Aborted, type);

            var canWeRun = jobConfig.CanWeRun();
            canWeRun.ShouldBe(true);
        }
        [Test]
        public void Test3()
        {
            var jobConfig = new JobConfig(1, DateTimeOffset.Now, StatusEnum.Started, type);

            var canWeRun = jobConfig.CanWeRun();
            canWeRun.ShouldBe(false);
        }

    }


}