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

namespace Microsoft.Samples.DPE.ODataTFS.Model.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.TeamFoundation.Server;
    using System.Globalization;

    public class TFSLinkProxy : TFSBaseProxy, ITFSLinkProxy
    {
        public TFSLinkProxy(Uri uri, ICredentials credentials)
            : base(uri, credentials)
        {

        }

        public IEnumerable<Link> GetLinksByWorkItem(int workItemId)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "TFSWorkItemProxy.GetLinksByWorkItem_{0}", workItemId);

            if (HttpContext.Current.Items[key] == null)
            {
                HttpContext.Current.Items[key] = this.RequestLinksByWorkItem(workItemId);
            }

            return (IEnumerable<Link>)HttpContext.Current.Items[key];
        }

        private IEnumerable<Link> RequestLinksByWorkItem(int workItemId)
        {
            var wiql = string.Format(CultureInfo.InvariantCulture, "SELECT [System.Id] FROM WorkItems WHERE [System.Id] = {0}", workItemId);

            var workItemServer = this.TfsConnection.GetService<TeamFoundation.WorkItemTracking.Client.WorkItemStore>();

            var wiColl = this.QueryWorkItems(wiql);
            if (wiColl.Count == 0)
            {
                return new List<Link>();
            }

            List<Link> linkColl = wiColl[0].Links
                .Cast<TeamFoundation.WorkItemTracking.Client.Link>()
                .Select(l => l.ToModel(workItemServer, workItemId)).ToList();

            foreach (var l in linkColl)
            {
                if (l.BaseLinkType == TeamFoundation.WorkItemTracking.Client.BaseLinkType.ExternalLink.ToString())
                {
                    l.LinkedArtifactUri =
                        this.GetTfsWebAccessArtifactUrl(new Uri(l.LinkedArtifactUri)).ToString();
                }
            }

            return linkColl;
        }
    }
}