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
    using Microsoft.TeamFoundation.Framework.Client;
    using Microsoft.TeamFoundation.Framework.Common;

    public class TFSUserProxy : TFSBaseProxy, ITFSUserProxy
    {
        public TFSUserProxy(Uri uri, ICredentials credentials)
            : base(uri, credentials)
        {

        }

        public User GetUserByUserName(string userName)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "TFSUserProxy.GetUserByEmail_{0}", userName);

            if (HttpContext.Current.Items[key] == null)
            {
                HttpContext.Current.Items[key] = this.RequestUserByUserName(userName);
            }

            return (User)HttpContext.Current.Items[key];
        }

        private User RequestUserByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            { throw new ArgumentNullException("userName"); }
            if (userName.Contains(":"))
            {
                userName = userName.Replace(":", "\\");
            }

            IIdentityManagementService gss = (IIdentityManagementService)this.TfsConnection.GetService(typeof(IIdentityManagementService));
            TeamFoundationIdentity identity = gss.ReadIdentity(IdentitySearchFactor.General, userName, MembershipQuery.Expanded, ReadIdentityOptions.ExtendedProperties);

            if (identity == null)
            {
                throw new System.Data.Services.DataServiceException(404, "Not Found", "User lookup failed", "en-US", null);
            }

            return identity.ToModel(userName);
        }
    }
}