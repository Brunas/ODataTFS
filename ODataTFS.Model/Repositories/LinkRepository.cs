// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

namespace Microsoft.Samples.DPE.ODataTFS.Model.Repositories
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Services;
    using System.Globalization;
    using System.Linq;
    using Microsoft.Data.Services.Toolkit.QueryModel;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.ExpressionVisitors;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;

    public class LinkRepository
    {
        private readonly ITFSLinkProxy proxy;

        public LinkRepository(ITFSLinkProxy proxy)
        {
            this.proxy = proxy;
        }

        //[RepositoryBehavior(HandlesFilter = true)]
        //TODO: add filtering support that makes its way to wiql
        public IEnumerable<Link> GetLinksByWorkItem(string id)
        {
            var workItemId = 0;
            if (!int.TryParse(id, NumberStyles.Integer, CultureInfo.InvariantCulture, out workItemId))
            {
                throw new ArgumentException("The id parameter must be numeric", "workItemId");
            }

            return this.proxy.GetLinksByWorkItem(workItemId);
        }

        public Link GetOne(string id)
        {
            throw new DataServiceException(501, "Not Implemented", "", "en-US", null);
        }

        public IEnumerable<Link> GetAll()
        {
            throw new DataServiceException(501, "Not Implemented", "The 'Link' collection cannot be enumerated as a root collection. It should depend on a WorkItem. (e.g. /WorkItems(12345)/Links)", "en-US", null);
        }
    }
}
