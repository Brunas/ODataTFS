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

    public class UserRepository
    {
        private readonly ITFSUserProxy proxy;

        public UserRepository(ITFSUserProxy proxy)
        {
            this.proxy = proxy;
        }

        public User GetOne(string userName)
        {
            return this.proxy.GetUserByUserName(userName);
        }

        public IEnumerable<User> GetAll()
        {
            throw new DataServiceException(501, "Not Implemented", "Service does not currently list all users. Try getting a specific user (e.g. Users('user@live.com') or Users('domain:user') -- replace backslash with ':' character)", "en-US", null);
        }
    }
}
