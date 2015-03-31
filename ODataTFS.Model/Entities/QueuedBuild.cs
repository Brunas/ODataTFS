namespace Microsoft.Samples.DPE.ODataTFS.Model.Entities
{
    using System;
    using System.Data.Services.Common;

    [DataServiceKey("Id")]
    public class QueuedBuild
    {
        public int Id { get; set; }

        public string BuildController { get; set; }

        public string BuildAgent { get; set; }

        public string Project { get; set; }

        public string BuildDefinition { get; set; }

        public string BuildStatus { get; set; }

        public string Priority { get; set; }

        public DateTime Date { get; set; }

        public string ElapsedTime { get; set; }

        public string User { get; set; }
    }
}