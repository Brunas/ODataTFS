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
    using System.Globalization;
    using System.Linq;
    using System.Xml;
    using Microsoft.Samples.DPE.ODataTFS.Model.Entities;
    using Microsoft.TeamFoundation.Server;

    public static class EntityTranslator
    {
        public static Project ToModel(this ProjectInfo projectInfo, string collectionName)
        {
            if (projectInfo == null)
            {
                throw new ArgumentNullException("projectInfo");
            }

            return new Project()
            {
                Collection = collectionName,
                Name = projectInfo.Name,
            };
        }

        public static AreaPath ToModel(this XmlNode node, IEnumerable<AreaPath> subAreas)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            var pathElements = node.Attributes["Path"].Value.Trim('\\').Split('\\');
            if (pathElements != null && pathElements.Length > 1 && pathElements.ElementAt(1).Equals("Area"))
            {
                var parsedAreaPath = pathElements.ToList();
                parsedAreaPath.RemoveAt(1);
                pathElements = parsedAreaPath.ToArray();
            }

            return new AreaPath() { Name = node.Attributes["Name"].Value, Path = EncodePath(string.Join("\\", pathElements)), SubAreas = subAreas };
        }

        public static IterationPath ToModel(this XmlNode node, IEnumerable<IterationPath> subIterations)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            var pathElements = node.Attributes["Path"].Value.Trim('\\').Split('\\');
            if (pathElements != null && pathElements.Length > 1 && pathElements.ElementAt(1).Equals("Iteration"))
            {
                var parsedIterationPath = pathElements.ToList();
                parsedIterationPath.RemoveAt(1);
                pathElements = parsedIterationPath.ToArray();
            }

            var sDT = new DateTime();
            var fDT = new DateTime();
            if (node.Attributes["StartDate"] != null)
                sDT = DateTime.Parse(node.Attributes["StartDate"].Value);
            if (node.Attributes["FinishDate"] != null)
                fDT = DateTime.Parse(node.Attributes["FinishDate"].Value);

            return new IterationPath() { Name = node.Attributes["Name"].Value, 
                Path = EncodePath(string.Join("\\", pathElements)), StartDate = sDT, FinishDate = fDT, SubIterations = subIterations
            };
        }

        public static Build ToModel(this TeamFoundation.Build.Client.IBuildDetail buildDetail)
        {
            if (buildDetail == null)
            {
                throw new ArgumentNullException("buildDetail");
            }

            return new Build()
            {
                Project = buildDetail.TeamProject,
                Definition = buildDetail.BuildDefinition.Name,
                Number = buildDetail.BuildNumber,
                Reason = buildDetail.Reason.ToString(),
                Quality = buildDetail.Quality,
                Status = buildDetail.Status.ToString(),
                RequestedBy = buildDetail.RequestedBy,
                RequestedFor = buildDetail.RequestedFor,
                LastChangedBy = buildDetail.LastChangedBy,
                StartTime = buildDetail.StartTime,
                FinishTime = buildDetail.FinishTime,
                LastChangedOn = buildDetail.LastChangedOn,
                BuildFinished = buildDetail.BuildFinished,
                DropLocation = buildDetail.DropLocation,
                Errors = string.Join(Environment.NewLine, TeamFoundation.Build.Client.InformationNodeConverters.GetBuildErrors(buildDetail).Select(e => e.Message)),
                Warnings = string.Join(Environment.NewLine, TeamFoundation.Build.Client.InformationNodeConverters.GetBuildWarnings(buildDetail).Select(w => w.Message))
            };
        }

        public static BuildDefinition ToModel(this TeamFoundation.Build.Client.IBuildDefinition buildDefinition)
        {
            if (buildDefinition == null)
            {
                throw new ArgumentNullException("buildDefinition");
            }

            return new BuildDefinition()
            {
                Project = buildDefinition.TeamProject,
                Definition = buildDefinition.Name
            };
        }

        public static Change ToModel(this TeamFoundation.VersionControl.Client.Change tfsChange, string collectionName, int changesetId)
        {
            if (tfsChange == null)
            {
                throw new ArgumentNullException("tfsChange");
            }

            return new Change()
            {
                Collection = collectionName,
                Changeset = changesetId,
                Path = EncodePath(tfsChange.Item.ServerItem),
                Type = tfsChange.Item.ItemType.ToString(),
                ChangeType = tfsChange.ChangeType.ToString()
            };
        }

        public static Changeset ToModel(this TeamFoundation.VersionControl.Client.Changeset tfsChangeset, Uri changesetWebEditorUrl)
        {
            if (tfsChangeset == null)
            {
                throw new ArgumentNullException("tfsChangeset");
            }

            if (changesetWebEditorUrl == null)
            {
                throw new ArgumentNullException("changesetWebEditorUrl");
            }

            return new Changeset()
            {
                ArtifactUri = tfsChangeset.ArtifactUri.ToString(),
                Comment = tfsChangeset.Comment,
                Committer = tfsChangeset.Committer,
                CreationDate = tfsChangeset.CreationDate,
                Id = tfsChangeset.ChangesetId,
                Owner = tfsChangeset.Owner,
                WebEditorUrl = changesetWebEditorUrl.ToString()
            };
        }

        public static WorkItem ToModel(this TeamFoundation.WorkItemTracking.Client.WorkItem tfsWorkItem, Uri workItemWebEditorUrl)
        {
            if (tfsWorkItem == null)
            {
                throw new ArgumentNullException("tfsWorkItem");
            }

            if (workItemWebEditorUrl == null)
            {
                throw new ArgumentNullException("workItemWebEditorUrl");
            }

            WorkItem wi = new WorkItem();
            double parsedDouble; int parsedInt;

            //Base information to provide in addition to WorkItem fields
            wi.Id = tfsWorkItem.Id;
            wi.Project = tfsWorkItem.Project.Name;
            wi.Type = tfsWorkItem.Type.Name;
            wi.WebEditorUrl = workItemWebEditorUrl.ToString();

            //Base fields
            wi.AreaPath = tfsWorkItem.AreaPath;
            wi.IterationPath = tfsWorkItem.IterationPath;
            wi.Revision = tfsWorkItem.Revision;
            wi.Priority = tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Common.Priority") ?? tfsWorkItem.Fields.GetFieldValueByReference("CodeStudio.Rank"); //CodeStudio.Rank is for codeplex support
            wi.Severity = tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Common.Severity");
            double.TryParse(tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Scheduling.RemainingWork"), out parsedDouble);
            wi.RemainingWork = parsedDouble;
            wi.AssignedTo = tfsWorkItem.Fields[TeamFoundation.WorkItemTracking.Client.CoreField.AssignedTo].Value.ToString();
            wi.CreatedDate = tfsWorkItem.CreatedDate;
            wi.CreatedBy = tfsWorkItem.CreatedBy;
            wi.ChangedDate = tfsWorkItem.ChangedDate;
            wi.ChangedBy = tfsWorkItem.ChangedBy;
            wi.ResolvedBy = tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Common.ResolvedBy");
            wi.Title = tfsWorkItem.Title;
            wi.State = tfsWorkItem.State;
            wi.Reason = tfsWorkItem.Reason;
            double.TryParse(tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Scheduling.CompletedWork"), out parsedDouble);
            wi.CompletedWork = parsedDouble;
            wi.Description = tfsWorkItem.Description;
            wi.ReproSteps = tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.TCM.ReproSteps");
            wi.FoundInBuild = tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Build.FoundIn");
            wi.IntegratedInBuild = tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Build.IntegrationBuild");
            wi.AttachedFileCount = tfsWorkItem.AttachedFileCount;
            wi.HyperLinkCount = tfsWorkItem.HyperLinkCount;
            wi.RelatedLinkCount = tfsWorkItem.RelatedLinkCount;

            //Agile
            wi.Risk = tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Common.Risk");
            double.TryParse(tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Scheduling.StoryPoints"), out parsedDouble);
            wi.StoryPoints = parsedDouble;
            double.TryParse(tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Common.StackRank"), out parsedDouble);
            wi.StackRank = parsedDouble;

            //Agile and CMMI
            double.TryParse(tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Scheduling.OriginalEstimate"), out parsedDouble);
            wi.OriginalEstimate = parsedDouble;

            //Scrum
            double.TryParse(tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Common.BacklogPriority"), out parsedDouble);
            wi.BacklogPriority = parsedDouble;
            int.TryParse(tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Common.BusinessValue"), out parsedInt);
            wi.BusinessValue = parsedInt;
            double.TryParse(tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Scheduling.Effort"), out parsedDouble);
            wi.Effort = parsedDouble;

            //Scrum and CMMI
            wi.Blocked = tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.CMMI.Blocked");

            //CMMI
            double.TryParse(tfsWorkItem.Fields.GetFieldValueByReference("Microsoft.VSTS.Scheduling.Size"), out parsedDouble);
            wi.Size = parsedDouble;

            return wi;
        }

        public static Attachment ToModel(this TeamFoundation.WorkItemTracking.Client.Attachment tfsAttachment, int workItemId, int index)
        {
            if (tfsAttachment == null)
            {
                throw new ArgumentNullException("tfsAttachment");
            }

            return new Attachment
            {
                Id = string.Format(CultureInfo.InvariantCulture, "{0}-{1}", workItemId, index),
                WorkItemId = workItemId,
                Index = index,
                Name = tfsAttachment.Name,
                Extension = tfsAttachment.Extension,
                Comment = tfsAttachment.Comment,
                Length = tfsAttachment.Length,
                AttachedTime = tfsAttachment.AttachedTime,
                CreationTime = tfsAttachment.CreationTime,
                LastWriteTime = tfsAttachment.LastWriteTime,
                Uri = tfsAttachment.Uri.ToString()
            };
        }

        public static Query ToModel(this TeamFoundation.WorkItemTracking.Client.QueryDefinition tfsQueryItem, string path)
        {
            if (tfsQueryItem == null)
            {
                throw new ArgumentNullException("tfsQueryItem");
            }

            return new Query
            {
                Id = tfsQueryItem.Id.ToString(),
                Name = tfsQueryItem.Name,
                QueryText = tfsQueryItem.QueryText,
                Project = tfsQueryItem.Project.Name,
                Path = path,
                QueryType = tfsQueryItem.QueryType.ToString()
            };
        }

        public static Branch ToModel(this TeamFoundation.VersionControl.Client.BranchObject tfsBranch)
        {
            if (tfsBranch == null)
            {
                throw new ArgumentNullException("tfsBranch");
            }

            return new Branch
            {
                Path = EncodePath(tfsBranch.Properties.RootItem.Item),
                Description = tfsBranch.Properties.Description,
                DateCreated = tfsBranch.DateCreated
            };
        }

        public static User ToModel(this TeamFoundation.Framework.Client.TeamFoundationIdentity tfsIdentity, string userName)
        {
            return new User
            {
                DisplayName = tfsIdentity.DisplayName,
                UserName = userName,
                Id = tfsIdentity.TeamFoundationId.ToString()
            };
        }

        public static Link ToModel(this TeamFoundation.WorkItemTracking.Client.Link tfsLink, TeamFoundation.WorkItemTracking.Client.WorkItemStore witStore, int workItemId)
        {
            if (tfsLink == null)
            {
                throw new ArgumentNullException("tfsLink");
            }
            if (witStore == null)
            {
                throw new ArgumentNullException("witStore");
            }

            var rLinkTypes = witStore.RegisteredLinkTypes;
            var wiTypes = witStore.WorkItemLinkTypes;

            Link newLink = new Link();
            newLink.SourceWorkItemId = workItemId;
            newLink.BaseLinkType = tfsLink.BaseType.ToString();
            newLink.ArtifactLinkType = tfsLink.ArtifactLinkType.Name;
            newLink.Comment = tfsLink.Comment;

            switch (tfsLink.BaseType)
            {
                case TeamFoundation.WorkItemTracking.Client.BaseLinkType.ExternalLink:
                    var exLink = tfsLink as TeamFoundation.WorkItemTracking.Client.ExternalLink;
                    newLink.LinkedArtifactUri = exLink.LinkedArtifactUri;
                    break;

                case TeamFoundation.WorkItemTracking.Client.BaseLinkType.Hyperlink:
                    var hyperLink = tfsLink as TeamFoundation.WorkItemTracking.Client.Hyperlink;
                    newLink.Location = hyperLink.Location;
                    break;

                case TeamFoundation.WorkItemTracking.Client.BaseLinkType.RelatedLink:
                    var relLink = tfsLink as TeamFoundation.WorkItemTracking.Client.RelatedLink;
                    newLink.LinkTypeEnd = relLink.LinkTypeEnd.Name;
                    newLink.RelatedWorkItemId = relLink.RelatedWorkItemId;
                    break;

                case TeamFoundation.WorkItemTracking.Client.BaseLinkType.WorkItemLink:
                    var wiLink = tfsLink as TeamFoundation.WorkItemTracking.Client.WorkItemLink;
                    //newLink.AddedBy = wiLink.AddedBy;
                    //newLink.AddedDate = wiLink.AddedDate;
                    //newLink.ChangedDate = wiLink.ChangedDate.Value;
                    //newLink.RemovedBy = wiLink.RemovedBy;
                    //newLink.RemovedDate = wiLink.RemovedDate;
                    //newLink.TargetWorkItemId = wiLink.TargetId;
                    break;
            }

            return newLink;
        }

        public static TeamFoundation.WorkItemTracking.Client.WorkItem ToEntity(this WorkItem workItemModel, Microsoft.TeamFoundation.WorkItemTracking.Client.Project project)
        {
            if (workItemModel == null)
            {
                throw new ArgumentNullException("workItemModel");
            }

            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            var workItemTypes = project.WorkItemTypes;
            var type = workItemTypes[workItemModel.Type];
            var workItemEntity = new TeamFoundation.WorkItemTracking.Client.WorkItem(type);

            workItemEntity.UpdateFromModel(workItemModel);

            return workItemEntity;
        }

        public static TeamFoundation.WorkItemTracking.Client.Attachment ToEntity(this Attachment attachmentModel, string path)
        {
            if (attachmentModel == null)
            {
                throw new ArgumentNullException("attachmentModel");
            }

            return new TeamFoundation.WorkItemTracking.Client.Attachment(path, attachmentModel.Comment);
        }

        public static void UpdateFromModel(this TeamFoundation.WorkItemTracking.Client.WorkItem workItemEntity, WorkItem workItemModel)
        {
            if (workItemEntity == null)
            {
                throw new ArgumentNullException("workItemEntity");
            }

            if (workItemModel == null)
            {
                throw new ArgumentNullException("workItemModel");
            }

            if (!string.IsNullOrWhiteSpace(workItemModel.AreaPath))
            {
                workItemEntity.AreaPath = workItemModel.AreaPath;
            }

            if (workItemEntity.Type.FieldDefinitions.TryGetByName("Assigned To") != null &&
                !string.IsNullOrWhiteSpace(workItemModel.AssignedTo))
            {
                workItemEntity.Fields["Assigned To"].Value = workItemModel.AssignedTo;
            }

            if (workItemEntity.Type.FieldDefinitions.TryGetByName("Backlog Priority") != null)
            {
                workItemEntity.Fields["Backlog Priority"].Value = workItemModel.BacklogPriority;
            }

            if (workItemEntity.Type.FieldDefinitions.TryGetByName("Blocked") != null)
            {
                workItemEntity.Fields["Blocked"].Value = workItemModel.Blocked;
            }

            if (workItemEntity.Type.FieldDefinitions.TryGetByName("Business Value") != null)
            {
                workItemEntity.Fields["Business Value"].Value = workItemModel.BusinessValue;
            }

            if (workItemEntity.Type.FieldDefinitions.TryGetByName("Completed Work") != null)
            {
                workItemEntity.Fields["Completed Work"].Value = workItemModel.CompletedWork;
            }

            if (!string.IsNullOrWhiteSpace(workItemModel.Description))
            {
                workItemEntity.Description = workItemModel.Description;
            }

            if (workItemEntity.Type.FieldDefinitions.TryGetByName("Effort") != null)
            {
                workItemEntity.Fields["Effort"].Value = workItemModel.Effort;
            }

            if (workItemEntity.Type.FieldDefinitions.TryGetByName("Found In") != null &&
                !string.IsNullOrWhiteSpace(workItemModel.FoundInBuild))
            {
                workItemEntity.Fields["Found In"].Value = workItemModel.FoundInBuild;
            }

            if (workItemEntity.Type.FieldDefinitions.TryGetByName("Integration Build") != null &&
                !string.IsNullOrWhiteSpace(workItemModel.IntegratedInBuild))
            {
                workItemEntity.Fields["Integration Build"].Value = workItemModel.IntegratedInBuild;
            }

            if (!string.IsNullOrWhiteSpace(workItemModel.IterationPath))
            {
                workItemEntity.IterationPath = workItemModel.IterationPath;
            }

            if (workItemEntity.Type.FieldDefinitions.TryGetByName("Original Estimate") != null)
            {
                workItemEntity.Fields["Original Estimate"].Value = workItemModel.OriginalEstimate;
            }

            if (!string.IsNullOrWhiteSpace(workItemModel.Priority))
            {
                int priority;
                if (int.TryParse(workItemModel.Priority, NumberStyles.Integer, CultureInfo.InvariantCulture, out priority))
                {
                    if (workItemEntity.Type.FieldDefinitions.TryGetByName("Priority") != null)
                    {
                        workItemEntity.Fields["Priority"].Value = priority;
                    }

                    // For CodePlex TFS
                    if (workItemEntity.Type.FieldDefinitions.TryGetByName("Rank") != null)
                    {
                        workItemEntity.Fields["Rank"].Value = GetPriorityDescription(priority);
                    }
                }
                else
                {
                    // For CodePlex TFS
                    if (workItemEntity.Type.FieldDefinitions.TryGetByName("Rank") != null)
                    {
                        workItemEntity.Fields["Rank"].Value = workItemModel.Priority;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(workItemModel.Reason))
            {
                workItemEntity.Reason = workItemModel.Reason;
            }

            if (workItemEntity.Type.FieldDefinitions.TryGetByName("Remaining Work") != null)
            {
                workItemEntity.Fields["Remaining Work"].Value = workItemModel.RemainingWork;
            }

            if (workItemEntity.Type.FieldDefinitions.TryGetByName("Repro Steps") != null &&
                !string.IsNullOrWhiteSpace(workItemModel.ReproSteps))
            {
                workItemEntity.Fields["Repro Steps"].Value = workItemModel.ReproSteps;
            }

            if (workItemEntity.Type.FieldDefinitions.TryGetByName("Risk") != null &&
                !string.IsNullOrWhiteSpace(workItemModel.Risk))
            {
                workItemEntity.Fields["Risk"].Value = workItemModel.Risk;
            }

            if (workItemEntity.Type.FieldDefinitions.TryGetByName("Severity") != null &&
                !string.IsNullOrWhiteSpace(workItemModel.Severity))
            {
                workItemEntity.Fields["Severity"].Value = workItemModel.Severity;
            }

            if (workItemEntity.Type.FieldDefinitions.TryGetByName("Size") != null)
            {
                workItemEntity.Fields["Size"].Value = workItemModel.Size;
            }

            if (workItemEntity.Type.FieldDefinitions.TryGetByName("Stack Rank") != null)
            {
                workItemEntity.Fields["Stack Rank"].Value = workItemModel.StackRank;
            }

            if (workItemEntity.Type.FieldDefinitions.TryGetByName("Story Points") != null)
            {
                workItemEntity.Fields["Story Points"].Value = workItemModel.StoryPoints;
            }

            if (!string.IsNullOrWhiteSpace(workItemModel.State))
            {
                workItemEntity.State = workItemModel.State;
            }

            if (!string.IsNullOrWhiteSpace(workItemModel.Title))
            {
                workItemEntity.Title = workItemModel.Title;
            }
        }

        public static string DecodePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return path;
            }

            return path.Replace('>', '/').Replace('<', '\\');
        }

        public static string EncodePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return path;
            }

            return path.Replace('/', '>').Replace('\\', '<');
        }

        private static string GetPriorityDescription(int priority)
        {
            switch (priority)
            {
                case 1: return "High";
                case 2: return "Medium";
                default:
                    return "Low";
            }
        }

        private static string GetFieldValueByReference(this TeamFoundation.WorkItemTracking.Client.FieldCollection fields, string referenceName)
        {
            var field = fields
                            .Cast<TeamFoundation.WorkItemTracking.Client.Field>()
                            .SingleOrDefault(f => f.ReferenceName.Equals(referenceName, StringComparison.OrdinalIgnoreCase));

            return (field != null) && (field.Value != null)
                ? field.Value.ToString()
                : null;
        }

        private static void SetFieldValueByReference(this TeamFoundation.WorkItemTracking.Client.FieldCollection fields, string referenceName, object value)
        {
            var field = fields
                            .Cast<TeamFoundation.WorkItemTracking.Client.Field>()
                            .SingleOrDefault(f => f.ReferenceName.Equals(referenceName, StringComparison.OrdinalIgnoreCase));

            if (field != null)
            {
                field.Value = value;
            }
        }
    }
}
