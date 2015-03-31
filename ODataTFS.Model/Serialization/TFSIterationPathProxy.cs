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
    using System.Text;
    using System.Data.Services;
    using System.Globalization;
    using System.Net;
    using System.Web;
    using System.Xml;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.TeamFoundation.Proxy;
    using Microsoft.TeamFoundation.Server;

    public class TFSIterationPathProxy : TFSBaseProxy, ITFSIterationPathProxy
    {
        public TFSIterationPathProxy(Uri uri, ICredentials credentials) : base(uri, credentials)
        { }

        public IEnumerable<IterationPath> GetAllIterationPaths()
        {
            if (HttpContext.Current.Items[this.GetAllIterationPathsKey()] == null)
            {
                HttpContext.Current.Items[this.GetAllIterationPathsKey()] = this.RequestAllIterationPaths();
            }

            return (IEnumerable<IterationPath>)HttpContext.Current.Items[this.GetAllIterationPathsKey()];
        }

        public IEnumerable<IterationPath> GetSubIterations(string rootIterationName)
        {
            if (HttpContext.Current.Items[rootIterationName] == null)
            {
                HttpContext.Current.Items[rootIterationName] = this.RequestSubIterations(rootIterationName);
            }

            return (IEnumerable<IterationPath>)HttpContext.Current.Items[rootIterationName];
        }

        public IEnumerable<IterationPath> GetIterationsByProject(string projectName)
        {
            var css = this.TfsConnection.GetService<ICommonStructureService3>();
            var allStructures = css.ListStructures(css.GetProjectFromName(projectName).Uri);
            var iterationPathsXml = css.GetNodesXml(allStructures.Where(s => s.StructureType.Equals(Microsoft.TeamFoundation.Common.StructureType.ProjectLifecycle)).Select(a => a.Uri).ToArray(), true);
            var rootIterationPaths = iterationPathsXml.ChildNodes.Cast<XmlNode>().Where(a => a.FirstChild != null)
                .SelectMany(a => a.FirstChild.ChildNodes.Cast<XmlNode>().
                    SelectMany(c => this.ParseIterationPathFromNodes(c)));

            return ExtractAllIterationPaths(rootIterationPaths).ToArray();
        }

        private IEnumerable<IterationPath> RequestAllIterationPaths()
        {
            var css = this.TfsConnection.GetService<ICommonStructureService3>();
            var allStructures = css.ListAllProjects().SelectMany(p => css.ListStructures(p.Uri));
            var iterationPathsXml = css.GetNodesXml(allStructures.Where(s => s.StructureType.Equals(Microsoft.TeamFoundation.Common.StructureType.ProjectLifecycle)).Select(a => a.Uri).ToArray(), true);
            var rootIterationPaths = iterationPathsXml.ChildNodes.Cast<XmlNode>().Where(a => a.FirstChild != null)
                .SelectMany(a => a.FirstChild.ChildNodes.Cast<XmlNode>()
                    .SelectMany(c => this.ParseIterationPathFromNodes(c)));

            return ExtractAllIterationPaths(rootIterationPaths).ToArray();
        }

        private IEnumerable<IterationPath> RequestSubIterations(string rootIterationName)
        {
            var css = this.TfsConnection.GetService<ICommonStructureService3>();
            var allStructures = css.ListAllProjects().SelectMany(p => css.ListStructures(p.Uri));
            var iterationPathsXml = css.GetNodesXml(allStructures.Where(s => s.StructureType.Equals(Microsoft.TeamFoundation.Common.StructureType.ProjectLifecycle)).Select(a => a.Uri).ToArray(), true);
            var iterations = ExtractAllIterationPaths(iterationPathsXml.ChildNodes.Cast<XmlNode>().Where(a => a.FirstChild != null)
                .SelectMany(a => a.FirstChild.ChildNodes.Cast<XmlNode>()
                    .SelectMany(c => this.ParseIterationPathFromNodes(c))));

            var encodedPath = EntityTranslator.EncodePath(string.Format(CultureInfo.InvariantCulture, "{0}\\", rootIterationName.TrimEnd('\\')));
            if (iterations.SingleOrDefault(a => a.Path.Equals(rootIterationName.TrimEnd('\\'))) == null)
            {
                throw new DataServiceException(404, "Not Found", string.Format(CultureInfo.InvariantCulture, "The IterationPath specified could not be found: {0}", rootIterationName), "en-US", null);
            }

            return iterations.Where(a => a.Path.StartsWith(encodedPath, StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        private IEnumerable<IterationPath> RequestAllIterationPathsByProject(string projectName)
        {
            var css = this.TfsConnection.GetService<ICommonStructureService3>();
            var allStructures = css.ListStructures(css.GetProjectFromName(projectName).Uri);
            var iterationPathsXml = css.GetNodesXml(allStructures.Where(s => s.StructureType.Equals(Microsoft.TeamFoundation.Common.StructureType.ProjectLifecycle)).Select(a => a.Uri).ToArray(), true);
            var rootIterationPaths = iterationPathsXml.ChildNodes.Cast<XmlNode>().Where(a => a.FirstChild != null)
                .SelectMany(a => a.FirstChild.ChildNodes.Cast<XmlNode>()
                    .SelectMany(c => this.ParseIterationPathFromNodes(c)));

            return ExtractAllIterationPaths(rootIterationPaths).ToArray();
        }

        private static IEnumerable<IterationPath> ExtractAllIterationPaths(IEnumerable<IterationPath> iterations)
        {
            var allIterations = new List<IterationPath>();
            if (iterations != null)
            {
                allIterations.AddRange(iterations.SelectMany(a => ExtractAllIterationPaths(a.SubIterations)));
                allIterations.AddRange(iterations);
            }

            return allIterations;
        }

        

        private IEnumerable<IterationPath> ParseIterationPathFromNodes(XmlNode currentNode)
        {
            var results = new List<IterationPath>();
            var subAreas = default(IEnumerable<IterationPath>);

            if (currentNode.ChildNodes != null)
            {
                var childrenNode = currentNode.ChildNodes.Cast<XmlNode>().SingleOrDefault(n => n.Name.Equals("Children"));
                if (childrenNode != null)
                {
                    subAreas = childrenNode.ChildNodes.Cast<XmlNode>().SelectMany(n => this.ParseIterationPathFromNodes(n));
                }
            }

            if (currentNode.Attributes["Name"] != null && currentNode.Attributes["Path"] != null)
            {
                results.Add(currentNode.ToModel(subAreas));
            }

            return results;
        }

        private string GetAllIterationPathsKey()
        {
            return string.Format(CultureInfo.InvariantCulture, "TFSIterationPathProxy.GetAllIterationPaths");
        }

        private string GetIterationPathsByProjectKey(string projectName)
        {
            return string.Format(CultureInfo.InvariantCulture, "TFSIterationPathProxy.GetIterationPathsByProject_{0}", projectName);
        }
    }
}
