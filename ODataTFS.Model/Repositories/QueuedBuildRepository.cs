namespace Microsoft.Samples.DPE.ODataTFS.Model.Repositories
{
    using System.Collections.Generic;

    using Microsoft.Data.Services.Toolkit.QueryModel;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;

    public class QueuedBuildRepository
    {
        private readonly ITFSBuildProxy proxy;

        public QueuedBuildRepository(ITFSBuildProxy proxy)
        {
            this.proxy = proxy;
        }

        [RepositoryBehavior(HandlesEverything = false)]
        public IEnumerable<QueuedBuild> GetAll(ODataQueryOperation operation)
        {
            return this.proxy.GetQueuedBuilds();
        }
    }
}