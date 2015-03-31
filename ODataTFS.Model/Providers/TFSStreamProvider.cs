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

namespace Microsoft.Samples.DPE.ODataTFS.Model.Providers
{
    using System.Data.Services;
    using System.IO;
    using System.Net;
    using Microsoft.Data.Services.Toolkit.Providers;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using System;
    using System.Text;
    using System.Globalization;

    public class TFSStreamProvider : EntityUrlReadOnlyStreamProvider
    {
        private readonly ICredentials credentials;

        public TFSStreamProvider(ICredentials credentials)
        {
            if (credentials != null)
            {
                NetworkCredential incomingCreds = (NetworkCredential)credentials;
                NetworkCredential netCred = new NetworkCredential(incomingCreds.UserName, incomingCreds.Password);
                this.credentials = netCred;
            }
        }

        public override Stream GetReadStream(object entity, string etag, bool? checkETagForEquality, DataServiceOperationContext operationContext)
        {
            var streameable = entity as IStreamEntity;
            if (streameable == null)
            {
                return null;
            }

            using (var client = new WebClient { Credentials = this.credentials })
            {
                System.Uri streamingLoc = streameable.GetUrlForStreaming();
                var credentialsStr = string.Format(
                    CultureInfo.InvariantCulture,
                    @"{0}:{1}",
                    ((NetworkCredential)this.credentials).UserName,
                    ((NetworkCredential)this.credentials).Password);
                client.Headers[HttpRequestHeader.Authorization] = "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(credentialsStr));
                var bytes = client.DownloadData(streamingLoc);
                return new MemoryStream(bytes);
            }
        }

        public override Stream GetWriteStream(object entity, string etag, bool? checkETagForEquality, DataServiceOperationContext operationContext)
        {
            var attachment = entity as Attachment;
            var tempFile = Path.Combine(Path.GetTempPath(), attachment.Id);

            return new FileStream(tempFile, FileMode.Create, FileAccess.ReadWrite);
        }

        public override string ResolveType(string entitySetName, DataServiceOperationContext operationContext)
        {
            return entitySetName == "Attachments" ? "Microsoft.Samples.DPE.ODataTFS.Model.Entities.Attachment" : null;
        }
    }
}
