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

namespace Microsoft.Samples.DPE.ODataTFS.Model
{
    using System;
    using System.Data.Services;
    using System.Globalization;
    using System.Linq;
    using Microsoft.Data.Services.Toolkit.QueryModel;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.Samples.DPE.ODataTFS.Model.Repositories;
    using Microsoft.Samples.DPE.ODataTFS.Model.Serialization;
    using Microsoft.TeamFoundation;

    public class TFSData : ODataContext
    {
        private readonly TFSProxyFactory tfsProxyFactory;

        public TFSData(TFSProxyFactory tfsProxyFactory)
        {
            this.tfsProxyFactory = tfsProxyFactory;
        }

        public IQueryable<Build> Builds
        {
            get { return CreateQuery<Build>(); }
        }

        public IQueryable<QueuedBuild> QueuedBuilds
        {
            get { return CreateQuery<QueuedBuild>(); }

        }

        public IQueryable<BuildDefinition> BuildDefinitions
        {
            get { return CreateQuery<BuildDefinition>(); }
        }

        public IQueryable<Changeset> Changesets
        {
            get { return CreateQuery<Changeset>(); }
        }

        public IQueryable<Project> Projects
        {
            get { return CreateQuery<Project>(); }
        }

        public IQueryable<WorkItem> WorkItems
        {
            get { return CreateQuery<WorkItem>(); }
        }

        public IQueryable<Attachment> Attachments
        {
            get { return CreateQuery<Attachment>(); }
        }

        public IQueryable<Change> Changes
        {
            get { return CreateQuery<Change>(); }
        }

        public IQueryable<Query> Queries
        {
            get { return CreateQuery<Query>(); }
        }

        public IQueryable<Branch> Branches
        {
            get { return CreateQuery<Branch>(); }
        }

        public IQueryable<AreaPath> AreaPaths
        {
            get { return CreateQuery<AreaPath>(); }
        }

        public IQueryable<Link> Links
        {
            get { return CreateQuery<Link>(); }
        }

        public IQueryable<IterationPath> IterationPaths
        {
            get { return CreateQuery<IterationPath>(); }
        }

        public IQueryable<User> Users
        {
            get { return CreateQuery<User>(); }
        }

        public void TriggerBuild(string project, string definition)
        {
            var proxy = this.tfsProxyFactory.TfsBuildDefinitionProxy;

            proxy.QueueBuild(project, definition);
        }

        public override object RepositoryFor(string fullTypeName)
        {
            try
            {
                if (fullTypeName == typeof(Build).FullName || fullTypeName == typeof(Build[]).FullName)
                {
                    return new BuildRepository(this.tfsProxyFactory.TfsBuildProxy);
                }

                if (fullTypeName == typeof(QueuedBuild).FullName || fullTypeName == typeof(QueuedBuild[]).FullName)
                {
                    return new QueuedBuildRepository(this.tfsProxyFactory.TfsBuildProxy);
                }

                if (fullTypeName == typeof(BuildDefinition).FullName || fullTypeName == typeof(BuildDefinition[]).FullName)
                {
                    return new BuildDefinitionRepository(this.tfsProxyFactory.TfsBuildDefinitionProxy);
                }

                if (fullTypeName == typeof(Changeset).FullName || fullTypeName == typeof(Changeset[]).FullName)
                {
                    return new ChangesetRepository(this.tfsProxyFactory.TfsChangesetProxy);
                }

                if (fullTypeName == typeof(Project).FullName || fullTypeName == typeof(Project[]).FullName)
                {
                    return new ProjectRepository(this.tfsProxyFactory.TfsProjectProxy);
                }

                if (fullTypeName == typeof(Change).FullName || fullTypeName == typeof(Change[]).FullName)
                {
                    return new ChangeRepository(this.tfsProxyFactory.TfsChangeProxy);
                }

                if (fullTypeName == typeof(WorkItem).FullName || fullTypeName == typeof(WorkItem[]).FullName)
                {
                    return new WorkItemRepository(this.tfsProxyFactory.TfsWorkItemProxy);
                }

                if (fullTypeName == typeof(Attachment).FullName || fullTypeName == typeof(Attachment[]).FullName)
                {
                    return new AttachmentRepository(this.tfsProxyFactory.TfsAttachmentProxy);
                }

                if (fullTypeName == typeof(Query).FullName || fullTypeName == typeof(Query[]).FullName)
                {
                    return new QueryRepository(this.tfsProxyFactory.TfsQueryProxy);
                }

                if (fullTypeName == typeof(Branch).FullName || fullTypeName == typeof(Branch[]).FullName)
                {
                    return new BranchRepository(this.tfsProxyFactory.TfsBranchProxy);
                }

                if (fullTypeName == typeof(AreaPath).FullName || fullTypeName == typeof(AreaPath[]).FullName)
                {
                    return new AreaPathRepository(this.tfsProxyFactory.TfsAreaPathProxy);
                }

                if (fullTypeName == typeof(Link).FullName || fullTypeName == typeof(Link[]).FullName)
                {
                    return new LinkRepository(this.tfsProxyFactory.TfsLinkProxy);
                }

                if (fullTypeName == typeof(IterationPath).FullName || fullTypeName == typeof(IterationPath[]).FullName)
                {
                    return new IterationPathRepository(this.tfsProxyFactory.TfsIterationPathProxy);
                }

                if (fullTypeName == typeof(User).FullName || fullTypeName == typeof(User[]).FullName)
                {
                    return new UserRepository(this.tfsProxyFactory.TfsUserProxy);
                }

                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "The type '{0}' does not have its corresponding repository", fullTypeName));
            }
            catch (TeamFoundationServerUnauthorizedException ex)
            {
                throw new DataServiceException(403, "Forbidden", "Could not connect to the TFS Server. Make sure you are including the appropriate credentials in the HTTP headers for Basic Authentication.", "en-US", ex);
            }
        }
    }
}
