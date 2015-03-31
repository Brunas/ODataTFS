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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;

    public class IterationPathRepository
    {
        private readonly ITFSIterationPathProxy proxy;

        public IterationPathRepository(ITFSIterationPathProxy proxy)
        {
            this.proxy = proxy;
        }

        public IterationPath GetOne(string path)
        {
            return this.proxy.GetAllIterationPaths().SingleOrDefault(a => a.Path.Equals(path));
        }

        public IEnumerable<IterationPath> GetAll()
        {
            return this.proxy.GetAllIterationPaths();
        }

        public IEnumerable<IterationPath> GetSubIterationsByIterationPath(string path)
        {
            return this.proxy.GetSubIterations(path);
        }

        public IEnumerable<IterationPath> GetIterationPathsByProject(string projectName)
        {
            return this.proxy.GetIterationsByProject(projectName);
        }
    }
}
