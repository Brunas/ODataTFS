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

namespace Microsoft.Samples.DPE.ODataTFS.Model.Entities
{
    using System.Collections.Generic;
    using System.Data.Services.Common;
    using Microsoft.Data.Services.Toolkit.QueryModel;
    using System;

    [DataServiceKey("SourceWorkItemId")]
    [EntityPropertyMapping("ArtifactLinkType", SyndicationItemProperty.Title, SyndicationTextContentKind.Plaintext, true)]
    public class Link
    {
        public int SourceWorkItemId { get; set; }

        public string ArtifactLinkType { get; set; }
        public string BaseLinkType { get; set; }
        public string Comment { get; set; }

        //external link
        public string LinkedArtifactUri { get; set; }

        //hyperlink
        public string Location { get; set; }

        //related link
        public string LinkTypeEnd { get; set; }
        public int RelatedWorkItemId { get; set; }

        //work item link
        //public string AddedBy { get; set; }
        //public DateTime AddedDate { get; set; }
        //public DateTime ChangedDate { get; set; }
        //public string RemovedBy { get; set; }
        //public DateTime RemovedDate { get; set; }
        //public int SourceId { get; set; }
        //public int TargetWorkItemId { get; set; }
    }
}
