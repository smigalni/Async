using System;
using Newtonsoft.Json;

namespace Crawler
{
    public class JobConfig
    {
        public JobConfig(int id, DateTimeOffset lastUpdated, StatusEnum status, string type)
        {
            Id = id;
            LastUpdated = lastUpdated;
            Status = status;
            TypeService = type;
        }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        public DateTimeOffset LastUpdated { get; }

        public StatusEnum Status { get; }

        public string TypeService { get; }

        public bool CanWeRun()
        {
            return  Status == StatusEnum.Finished || Status == StatusEnum.Aborted;
        }
    }


    public enum TypeEnum
    {
        HastusEntity,
        SignalEntity
    }

    public enum StatusEnum
    {
        Started,
        Finished,
        Aborted
    }
}