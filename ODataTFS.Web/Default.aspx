<%@ Page Language="C#" AutoEventWireup="false" Inherits="System.Web.UI.Page" ViewStateEncryptionMode="Never" ViewStateMode="Disabled" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head>
    <title>Team Foundation Service OData API</title>
    <meta http-equiv="Content-type" content="application/xhtml+xml" />
    <meta content="text/html; charset=UTF-8" http-equiv="Content-Type" />
    <meta content="en" name="language" />
    <meta content="en" http-equiv="Content-Language" />
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <meta http-equiv="X-XSS-Protection" content="0" />
    <link rel="stylesheet" href="<%= this.ResolveUrl("~/site.css") %>" type="text/css" media="screen" charset="utf-8" />
    </head>
<body>
    <% 
        var baseUrl = this.Context.Request.Url.GetComponents(UriComponents.SchemeAndServer & ~UriComponents.Port, UriFormat.UriEscaped).ToString();
    %>
    <div id="container">
       <div id="header">
            <h1 style="float: left; margin-top:40px; padding: 5px 0px">Team Foundation Server OData API</h1>
            <a title="Open Data Proptocol OData" href="http://www.odata.org/" class="odata-logo">
                <img height="40" width="142" alt="OData" src="<%= this.ResolveUrl("~/OData-logo.png") %>" />
                </a>
            <div class="header-clearer">
            </div>
        </div>
        <div id="content">
           <br />
            
            <p>Last service update: 4/17/13</p>
            <p>View known issues, FAQ at: <a href="http://social.technet.microsoft.com/wiki/contents/articles/15039.odata-service-for-team-foundation-server-v2.aspx">OData Service TechNet Wiki article</a></p>
            
            <h2>Overview</h2>

            <p>The Team Foundation Server OData API is an implementation of the OData protocol built upon the existing Team Foundation 
                Server client objet model used to connect to Team Foundation Server. The API is subject to change as we get feedback from customers.</p>         
                        
            <p>To learn more about the OData protocol, you can browse the OData site at <a title="Open Data Proptocol OData" href="http://www.odata.org/" class="odata-logo">http://www.odata.org</a>.</p>

            <p>If you have questions or feedback about this service, please email <a href="mailto:TFSOData@Microsoft.com">TFSOData@Microsoft.com</a>. Please note that this project is provided &quot;as-is&quot;, with no guaranteed uptime and is not officially supported by Microsoft. But if you are having problems please let us know and we&#39;ll do our best to work with you.</p>

            <h3>See the Demo</h3>
            <p>There is a <a href="http://channel9.msdn.com/Blogs/briankel/OData-Service-for-Team-Foundation-Server-2010">video for Channel 9</a> which shows how to get started using the 
                v1 of the service. Most of the same concepts from that video still apply for 
                this version, but a revised video has not yet been created.</p>

            <h3>Samples:</h3>
            <p>Windows 8 client (see Nisha Singh's <a href="http://aka.ms/TFSDashboardApp">blog</a> entry).</p>
            <p>Windows Phone 8 app (download <a href="http://www.microsoft.com/en-us/download/details.aspx?id=36230">here</a>).</p>
            <p>OData service code (see Brian Keller's <a href="http://aka.ms/tfsodata">blog</a> entry). This version of the codebase can be used against on-premises deployments of Team Foundation Server 2010 and 2012, Team Foundation Service, and CodePlex.</p>

            <h3>Team Foundation Service authentication:</h3>

            <p>(Optional) In order to authenticate with Team Foundation Service, you will need to enable and configure 
                basic auth credentials on tfs.visualstudio.com:</p>
            <ul>
                <li>Navigate to the account that you want to use on <a href="https://tfs.visualstudio.com">https://tfs.visualstudio.com</a>. For example, you may have https://account.visualstudio.com.</li>
                <li>In the top-right corner, click on your account name and then select <strong>My Profile</strong></li>
                <li>Select the <strong>Credentials</strong> tab</li>
                <li>Click the '<strong>Enable alternate credentials and set password</strong>' link</li>
                <li>Enter a password. It is suggested that you choose a unique password here (not associated with any other accounts)</li>
                <li>Click <strong>Save Changes</strong></li>
            </ul>


            <p>To authenticate against the OData service, you need to send your basic auth credentials in the following domain\username and password format:</p>
            <ul>
                <li>account\username</li>
                <li>password</li>
                <li>Note: <strong>account</strong> is from <strong>account</strong>.visualstudio.com, 
                    <strong>username</strong> is from the Credentials tab under My Profile, and 
                    <strong>password</strong> is the password that you just created.</li>
            </ul>

            <h3>Getting Started:</h3>
            <p>In the following section you will find meaningful information about how to consume data from Team Foundation Server taking advantage of the OData API.</p>

            <h4>Collections</h4>
            <p>The main resources available are Builds, Changesets, Changes, 
                Builds, Build Definitions, Branches, Work Items, Attachments, Projects, Queries, Links, Area Paths, and Iteration Paths. A couple of sample queries are provided for each resource, although complete query options are provided further in this 
                page.</p>
            <p><strong>Case Sensitivity</strong>: Be aware that the OData resources are case-sensitive when making queries.</p>
            <p><strong>Page size defaults</strong>: the default page sizes returned by the OData 
                service are set to 20, although you can certainly use the top and skip 
                parameters to manually control that.</p>

            <table border="0" cellpadding="5" cellspacing="2">
                <tr>
                    <th>Resources</th>
                    <th>Path</th>
                </tr>
                <tr>
                    <td>Builds</td>
                    <td>
                        <a title="Builds" href="<%= baseUrl %>/DefaultCollection/Builds"><%= baseUrl %>/DefaultCollection/Builds</a><br /><br />
                        <a title="Builds" href="<%= baseUrl %>/DefaultCollection/Projects('projectName')/Builds"><%= baseUrl %>/DefaultCollection/Projects(&#39;projectName&#39;)/Builds</a><br /><br />
                    </td>
                </tr>
                <tr>
                    <td>Build Definitions</td>
                    <td>
                        <a title="Build Definitions" href="<%= baseUrl %>/DefaultCollection/Projects('projectName')/BuildDefinitions"><%= baseUrl %>/DefaultCollection/Projects(&#39;projectName&#39;)/BuildDefinitions</a><br /><br />
                    </td>
                </tr>
                <tr>
                    <td>Changesets</td>
                    <td>
                    <a title="Changesets" href="<%= baseUrl %>/DefaultCollection/Changesets"><%= baseUrl %>/DefaultCollection/Changesets</a><br /><br />
                    <a title="Changesets" href="<%= baseUrl %>/DefaultCollection/Projects('projectName')/Changesets"><%= baseUrl %>/DefaultCollection/Projects(&#39;projectName&#39;)/Changesets</a><br /><br />
                    <a title="Changesets" href="<%= baseUrl %>/DefaultCollection/Branches('path')/Changesets"><%= baseUrl %>/DefaultCollection/Branches(&#39;path&#39;)/Changesets</a><br /><br />
                    <a title="Changesets" href="<%= baseUrl %>/DefaultCollection/Builds(Project='prjName',Definition='BuildDef',Number='BuildNum')/Changesets"><%= baseUrl %>/DefaultCollection/Builds(Project=&#39;prjName&#39;,Definition=&#39;BuildDef&#39;,Number=&#39;BuildNum&#39;)/Changesets</a></td>
                </tr> 
                <tr>
                    <td>Changes</td>
                    <td>
                        <a title="Changes" href="<%= baseUrl %>/DefaultCollection/Changesets(1)/Changes"><%= baseUrl %>/DefaultCollection/Changesets(Id)/Changes</a>
                    </td>
                </tr>

                <tr>
                    <td>Branches</td>
                    <td>
                        <a title="Branches" href="<%= baseUrl %>/DefaultCollection/Branches"><%= baseUrl %>/DefaultCollection/Branches</a><br /><br />
                        <a title="Branches" href="<%= baseUrl %>/DefaultCollection/Projects('prjName')/Branches"><%= baseUrl %>/DefaultCollection/Projects(&#39;prjName&#39;)/Branches</a>
                    </td>
                </tr>   

                <tr>
                    <td>WorkItems</td>
                    <td>
                        <a title="WorkItems" href="<%= baseUrl %>/DefaultCollection/WorkItems"><%= baseUrl %>/DefaultCollection/WorkItems</a><br /><br />
                        <a title="WorkItems" href="<%= baseUrl %>/DefaultCollection/Builds(Project='prjName',Definition='BuildDef',Number='BuildNum')/WorkItems"><%= baseUrl %>/DefaultCollection/Builds(Project=&#39;prjName&#39;,Definition=&#39;BuildDef&#39;,Number=&#39;BuildNum&#39;)/WorkItems</a><br /><br />
                        <a title="WorkItems" href="<%= baseUrl %>/DefaultCollection/Changesets(1)/WorkItems"><%= baseUrl %>/DefaultCollection/Changesets(id)/WorkItems</a><br /><br />
                        <a title="WorkItems" href="<%= baseUrl %>/DefaultCollection/Projects('projectName')/WorkItems"><%= baseUrl %>/DefaultCollection/Projects(&#39;projectName&#39;)/WorkItems</a><br /><br />
                        <a title="WorkItems" href="<%= baseUrl %>/DefaultCollection/Queries('id')/WorkItems"><%= baseUrl %>/DefaultCollection/Queries(&#39;id&#39;)/WorkItems</a>
                    </td>
                </tr>

                <tr>
                    <td>Attachments</td>
                    <td>
                        <a title="Attachments" href="<%= baseUrl %>/DefaultCollection/WorkItems(1)/Attachments"><%= baseUrl %>/DefaultCollection/WorkItems(id)/Attachments</a>
                    </td>
                </tr>

                <tr>
                    <td>Links</td>
                    <td>
                        <a title="Links" href="<%= baseUrl %>/DefaultCollection/WorkItems(1)/Links"><%= baseUrl %>/DefaultCollection/WorkItems(id)/Links</a>
                    </td>
                </tr>

                <tr>
                    <td>Projects</td>
                    <td>
                        <a title="Projects" href="<%= baseUrl %>/DefaultCollection/Projects"><%= baseUrl %>/DefaultCollection/Projects</a>
                    </td>
                </tr>

                <tr>
                    <td>Queries</td>
                    <td>
                        <a title="Queries" href="<%= baseUrl %>/DefaultCollection/Queries"><%= baseUrl %>/DefaultCollection/Queries</a>
                    </td>
                </tr>

                <tr>
                    <td>AreaPaths</td>
                    <td>
                        <a title="AreaPaths" href="<%= baseUrl %>/DefaultCollection/AreaPaths"><%= baseUrl %>/DefaultCollection/AreaPaths</a><br /><br />
                        <a title="AreaPaths" href="<%= baseUrl %>/DefaultCollection/Projects('projectName')/AreaPaths"><%= baseUrl %>/DefaultCollection/Projects('projectName')/AreaPaths</a>
                    </td>
                </tr>

                <tr>
                    <td>IterationPaths</td>
                    <td>
                        <a title="IterationPaths" href="<%= baseUrl %>/DefaultCollection/IterationPaths"><%= baseUrl %>/DefaultCollection/IterationPaths</a><br /><br />
                        <a title="IterationPaths" href="<%= baseUrl %>/DefaultCollection/Projects('projectName')/IterationPaths"><%= baseUrl %>/DefaultCollection/Projects('projectName')/IterationPaths</a>
                    </td>
                </tr>
               
            </table>

            <h4>Individual Resources</h4>
            <table border="0" cellpadding="5" cellspacing="2">
                <tr>
                    <th>Resource</th>
                    <th>Path</th>
                    <th>Related Resources</th>
                    <th>Fields *</th>		
                </tr>
                <tr>
                    <td>Build</td>
                    <td>
                        <a title="Build" href="<%= baseUrl %>/DefaultCollection/Builds(Project='prjName',Definition='BuildDef',Number='BuildNum')"><%= baseUrl %>/DefaultCollection/Builds(Project='prjName',Definition='BuildDef',Number='BuildNum')</a>
                    </td>
                    <td>
                        WorkItems, Changesets</td>
                    <td>
                        <strong>Project</strong>, <strong>Definition</strong>,<strong> Number</strong>, Reason, Quality, Status, RequestedBy, 
                        RequestedFor, LastChangedBy, StartTime, FinishTime, LastChangedOn, 
                        BuildFinishied, DropLocation, Errors, Warnings</td>
                </tr>
                <tr>
                    <td>Build Definition</td>
                    <td>
                        <a title="Build Definition" href="<%= baseUrl %>/DefaultCollection/BuildDefinitions(Project='prjName',Definition='BuildDef')"><%= baseUrl %>/DefaultCollection/BuildDefinitions(Project='prjName',Definition='BuildDef')</a>
                    </td>
                    <td>
                        -</td>
                    <td>
                        <strong>Project</strong>, <strong>Definition</strong></td>
                </tr>
                <tr>
                    <td>Changeset</td>
                    <td>
                        <a title="Changeset" href="<%= baseUrl %>/DefaultCollection/Changesets(1)"><%= baseUrl %>/DefaultCollection/Changesets(id)</a>
                    <td>Changes, WorkItems</td>
                    <td><strong>Id</strong>, ArtifactUri, Comment, Committer, CreationDate, Owner, Branch, WebEditorUrl</td>
                </tr> 
                <tr>
                    <td>Change</td>
                    <td>
                        <a title="Change" href="<%= baseUrl %>/DefaultCollection/Changes(Changeset='id',Path='path')"><%= baseUrl %>/DefaultCollection/Changes(Changeset=&#39;id&#39;,Path=&#39;path&#39;)</a>
                    </td>
                    <td>-</td>
                    <td><strong>Changeset</strong>, <strong>Path</strong>, Collection, ChangeType, Type</td>
                </tr>

                <tr>
                    <td>Branch</td>
                    <td>
                        <a title="Branch" href="<%= baseUrl %>/DefaultCollection/Branches('path')"><%= baseUrl %>/DefaultCollection/Branches(&#39;path&#39;)</a>
                    </td>
                    <td>Changesets</td>
                    <td><strong>Path</strong>, Description, DateCreated</td>
                </tr>   

                <tr>
                    <td>WorkItem</td>
                    <td>
                        <a title="WorkItem" href="<%= baseUrl %>/DefaultCollection/WorkItems(1)"><%= baseUrl %>/DefaultCollection/WorkItems(id)</a>
                    </td>
                    <td>Attachments</td>
                    <td><strong>Id</strong>, AreaPath, IterationPath, Revision, Priority, Severity, StackRank, Project, 
                        AssignedTo, CreatedDate, CreatedBy, ChangedDate, ChangedBy, ResolvedBy, Title, 
                        State, Type, Reason, Description, ReproSteps, FoundInBuild, IntegratedInBuild, 
                        Blocked, OriginalEstimate, RemainingWork, StoryPoints, BacklogPriority, 
                        BusinessValue, Effort, Size, CompletedWork, AttachedFileCount, HyperLinkCount, 
                        RelatedLinkCount, 
                        WebEditorUrl</td>
                </tr>

                <tr>
                    <td>Attachment</td>
                    <td>
                        <a title="Attachment" href="<%= baseUrl %>/DefaultCollection/Attachments('WorkItemId-Index')"><%= baseUrl %>/DefaultCollection/Attachments(&#39;WorkItemId-Index&#39;)</a>
                    </td>
                    <td>-</td>
                    <td>Id, WorkItemId, Index, AttachedTime, CreationTime, LastWriteTime, Name, 
                        Extension, Comment, Length, Uri</td>
                </tr>

                <tr>
                    <td>Project</td>
                    <td>
                        <a title="Project" href="<%= baseUrl %>/DefaultCollection/Projects('name')"><%= baseUrl %>/DefaultCollection/Projects(&#39;name&#39;)</a>
                    </td>
                    <td>Changesets, Builds, BuildDefinitions, WorkItems, Queries, Branches, AreaPaths</td>
                    <td><strong>Name</strong>, Collection</td>
                </tr>

                <tr>
                    <td>Query</td>
                    <td>
                        <a title="Query" href="<%= baseUrl %>/DefaultCollection/Queries('id')"><%= baseUrl %>/DefaultCollection/Queries(&#39;id&#39;)</a>
                    </td>
                    <td>WorkItems</td>
                    <td><strong>Id</strong>, Name, Description, QueryText, Path, Project, QueryType</td>
                </tr>

                <tr>
                    <td>AreaPath</td>
                    <td>
                        <a title="AreaPaths" href="<%= baseUrl %>/DefaultCollection/AreaPaths('path')"><%= baseUrl %>/DefaultCollection/AreaPaths(&#39;path&#39;)</a><br /><br />
                    </td>
                    <td>SubAreas</td>
                    <td><strong>Path</strong>, Name</td>
                </tr>

                <tr>
                    <td>User</td>
                    <td>
                        <a title="User" href="<%= baseUrl %>/DefaultCollection/Users('user@email.com')"><%= baseUrl %>/DefaultCollection/Users(&#39;user@email.com&#39;)</a><br /><br />
                        <a title="User" href="<%= baseUrl %>/DefaultCollection/Users('domain:user')"><%= baseUrl %>/DefaultCollection/Users(&#39;domain:user&#39;)</a><br /><br />
                    </td>
                    <td>-</td>
                    <td><strong>UserId</strong>, DisplayName, Id</td>
                </tr>
            </table>
            <p class="note">* Id fields are displayed in <strong>bold</strong>.</p>
            
            <h4>Parameters Support</h4>
            <p>These are the allowed parameters for manipulating the data that comes out from the OData Service, due to the nature of the API 
                <em>$inlinecount</em> and <em>$expand</em> are not currently supported for this service.</p>
            <table border="0" cellpadding="5" cellspacing="2">
                <tr>
                    <th>Name</th>
                    <th>Description</th>
                    <th>Values</th>
                    <th>Example</th>		
                </tr>
                <tr>
                    <td>filter</td>
                    <td>Filter the results</td>
                    <td>See filtering support section</td>
                    <td><a title="filter" href="<%= baseUrl %>/DefaultCollection/Projects?$filter=Name%20eq%20'MyProject'"><%= baseUrl %>/DefaultCollection/Projects?$filter=Name eq &#39;MyProject&#39;</a><br /></td>
                </tr>
                <tr>
                    <td>count</td>
                    <td>Count the results</td>
                    <td>-</td>
                    <td><a title="count" href="<%= baseUrl %>/DefaultCollection/WorkItems/$count"><%= baseUrl %>/DefaultCollection/WorkItems/$count</a><br /></td>
                </tr>
                <tr>
                    <td>top/skip</td>
                    <td>Paging options</td>
                    <td>Integer values</td>
                    <td><a title="top/skip" href="<%= baseUrl %>/DefaultCollection/WorkItems?$top=5&$skip=10"><%= baseUrl %>/DefaultCollection/WorkItems?$top=5&amp;$skip=10</a><br /></td>
                </tr>
                <tr>
                    <td>orderby</td>
                    <td>Sort results</td>
                    <td>Resource field</td>
                    <td><a title="orderby" href="<%= baseUrl %>/DefaultCollection/Builds?$orderby=Reason%20asc"><%= baseUrl %>/DefaultCollection/Builds?$orderby=Reason asc</a><br /></td>
                </tr>
                <tr>
                    <td>select</td>
                    <td>Fields to return</td>
                    <td>Fields for the resource type</td>
                    <td><a title="select" href="<%= baseUrl %>/DefaultCollection/Changesets?$select=Committer,Owner"><%= baseUrl %>/DefaultCollection/Changesets?$select=Committer,Owner</a><br /></td>
                </tr>
                <tr>
                    <td>format</td>
                    <td>Format of response</td>
                    <td>Only 'json' supported</td>
                    <td><a title="format" href="<%= baseUrl %>/DefaultCollection/Projects?$format=json"><%= baseUrl %>/DefaultCollection/Projects?$format=json"</a><br /></td>
                </tr>
                <tr>
                    <td>callback</td>
                    <td>Specify callback method if json format requested</td>
                    <td>Javascript callback method</td>
                    <td><a title="format" href="<%= baseUrl %>/DefaultCollection/Projects?$format=json&$callback=method"><%= baseUrl %>/DefaultCollection/Projects?$format=json&$callback=method"</a><br /></td>
                </tr>
            </table>

            <h4>Filtering Support</h4>
            <p>These are the supported fields and operations while filtering out the data that comes out from the 
                service. All these items along with its corresponding operators can be used for filtering data.</p> 
            <table border="0" cellpadding="5" cellspacing="2">
                <tr>
                    <th>Entity</th>
                    <th>Supported Filter Operations</th>
                    <th>Example</th>		
                </tr>
                <tr>
                    <td>Build</td>
                    <td>
                        <ul>
                        <li>Users can filter for a specific value (<strong>eq</strong> operator) by Project, Definition, Number, Reason, Quality, Status and RequestedFor.</li>
                        <li>Users can filter for a different value (<strong>ne</strong> operator) and range (<strong>eq</strong>, 
                            <strong>gt</strong>, <strong>lt</strong> operators) by RequestedBy, StartTime, FinishTime and BuildFinished.</li>
                        <li>Only logical <strong>And</strong> operator is supported.</li>
                        </ul>
                    </td>
                    <td><a title="Builds" href="<%= baseUrl %>/DefaultCollection/Builds?$filter=Definition%20eq%20'buildDef'%20and%20RequestedBy%20ne%20'johndoe'"><%= baseUrl %>/DefaultCollection/Builds?$filter=Definition eq &#39;buildDef&#39; and RequestedBy ne &#39;johndoe&#39;</a></td>
                </tr>
                <tr>
                    <td>Changeset</td>
                    <td>
                        <ul>
                            <li>Users can filter for a specific value (<strong>eq</strong> operator) by Committer and for a range of values (<strong>eq</strong>, 
                                <strong>gt</strong> and <strong>lt</strong> operators) by CreationDate.</li>
                            <li>Users can filter for a specific and a different value (<strong>eq</strong> and 
                                <strong>ne</strong> operators) by ArtifactUri, Comment and Owner.</li>
                            <li>Only logical <strong>And</strong> operator is supported.</li>
                        </ul>
                    </td>
                    <td>
                        <a title="Builds" href="<%= baseUrl %>/DefaultCollection/Changesets?$filter=Committer%20eq%20'johndoe'%20and%20ArtifactUri%20ne%20'https://tfsserver/artifact'"><%= baseUrl %>/DefaultCollection/Changesets?$filter=Committer eq &#39;johndoe&#39; and ArtifactUri ne &#39;https://tfsserver/artifact&#39;</a>
                    </td>
                </tr>
                <tr>
                    <td>Change</td>
                    <td>
                        <ul>
                            <li>Change collections are only browsable relative to a Changeset.</li>
                        </ul>
                    </td>
                    <td>
                        <a title="Changes" href="<%= baseUrl %>/DefaultCollection/Changesets(10)/Changes?$filter=Type%20eq%20'file'"><%= baseUrl %>/DefaultCollection/Changesets(10)/Changes?$filter=Type eq &#39;file&#39;</a>
                    </td>
                </tr>
                <tr>
                    <td>Branch</td>
                    <td>
                    <ul>
                        <li>Users can filter for a specific value (<strong>eq</strong> operator) by Committer and for a range of values (<strong>eq</strong>, 
                            <strong>gt</strong> and <strong>lt</strong> operators) by CreationDate.</li>
                        <li>Users can filter for a specific and a different value (<strong>eq</strong> and 
                            <strong>ne</strong> operators) by ArtifactUri, Comment and Owner.</li>
                        <li>Only logical <strong>And</strong> operator is supported.</li>
                    </ul>
                    </td>
                    <td>
                        <a title="Branches" href="<%= baseUrl %>/DefaultCollection/Branches?$filter=Description%20eq%20'Release Branch'"><%= baseUrl %>/DefaultCollection/Branches?$filter=Description eq &#39;Release Branch&#39;</a>
                    </td>
                </tr>
                <tr>
                    <td>WorkItem</td>
                    <td>
                        <ul>
                        <li>Users can filter for a different (<strong>ne</strong> operator), a specific (<strong>eq</strong> operator), or a substring of a value (<strong>substringof
                            </strong>operator) by AreaPath, IterationPath, Priority, Severity, StackRank, Project, AssignedTo, CreatedBy, ChangedBy, ResolvedBy, Title, State, Type, Reason, Description, ReproSteps, FoundInBuild and IntegratedInBuild.</li>
                        <li>Users can filter for a different (<strong>ne</strong> operator), a specific (<strong>eq
                            </strong>operator) or a range (<strong>gt</strong>, <strong>lt</strong>, <strong>ge</strong>, 
                            <strong>le</strong> operators) of values by CreatedDate, ChangedDate and Revision.</li>
                        <li>Logical <strong>And</strong> and <strong>Or</strong> operators are supported.</li>
                        </ul>
                    </td>
                    <td>
                        <a title="WorkItems" href="<%= baseUrl %>/DefaultCollection/WorkItems?$filter=Project%20eq%20'myProject' or substringof('fixed', State)%20eq%20true"><%= baseUrl %>/DefaultCollection/WorkItems?$filter=Project eq &#39;myProject&#39; or substringof(&#39;fixed&#39;, State) eq true</a>
                    </td>
                </tr>
                <tr>
                    <td>Attachment</td>
                    <td><ul><li>Attachment collections are only browsable relative to a WorkItem.</li></ul></td>
                    <td>
                        <a title="Attachments" href="<%= baseUrl %>/DefaultCollection/WorkItems(5)/Attachments?$filter=Extension%20eq%20'JPG'"><%= baseUrl %>/DefaultCollection/WorkItems(5)/Attachments?$filter=Extension eq &#39;JPG&#39;</a>
                    </td>
                </tr>
                <tr>
                    <td>Project</td>
                    <td>All valid OData operations.</td>
                    <td>
                        <a title="Projects" href="<%= baseUrl %>/DefaultCollection/Projects?$filter=Name%20eq%20'myProject'"><%= baseUrl %>/DefaultCollection/Projects?$filter=Name eq &#39;myProject&#39;</a>
                    </td>
                </tr>
                <tr>
                    <td>Query</td>
                    <td>All valid OData operations.</td>
                    <td>
                        <a title="Queries" href="<%= baseUrl %>/DefaultCollection/Queries?$filter=Name%20eq%20'All Work Items'"><%= baseUrl %>/DefaultCollection/Queries?$filter=Name eq &#39;All Work Items&#39;</a>
                    </td>
                </tr>
                <tr>
                    <td>AreaPath</td>
                    <td>All valid OData operations.</td>
                    <td>
                        <a title="AreaPaths" href="<%= baseUrl %>/DefaultCollection/AreaPaths?$filter=Name%20eq%20'Area 1'"><%= baseUrl %>/DefaultCollection/AreaPaths?$filter=Name eq &#39;Area 1&#39;</a>
                    </td>
                </tr>
                <tr>
                    <td>BuildDefinition</td>
                    <td>All valid OData operations.</td>
                    <td>
                        <a title="BuildDefinitions" href="<%= baseUrl %>/DefaultCollection/BuildDefinitions?$filter=Project%20eq%20'MyProject'"><%= baseUrl %>/DefaultCollection/AreaPaths?$filter=Project eq &#39;MyProject&#39;</a>
                    </td>
                </tr>
            </table>

            <h4>Write Operations Support</h4>
            <p>The following entities allow write or update operations as specified by the <a href="http://www.odata.org/developers/protocols/operations#CreatingnewEntries">OData specification</a>.
            <table border="0" cellpadding="5" cellspacing="2">
                <tr>
                    <th>Entity</th>
                    <th>Supported Operations</th>
                </tr>
                <tr>
                    <td>WorkItem</td>
                    <td>
                        <ul>
                            <li>Creating an entity through an HTTP POST operation to the main collection.</li>
                            <li>Updating an entity through an HTTP PUT operation to an individual resource.</li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td>Attachment</td>
                    <td>
                        <ul>
                            <li>Creating an entity through an HTTP POST operation to the main collection.</li>
                        </ul>
                    </td>
                </tr>
            </table>
<h4>Service Operations Support</h4>
            <p>The following custom operations are supported by the service, as specified by the <a href="http://www.odata.org/developers/protocols/operations#InvokingServiceOperations">OData specification</a>.
            <table border="0" cellpadding="5" cellspacing="2">
                <tr>
                    <th>Operation</th>
                    <th>Method</th>
                    <th>Request Body</th>
                    <th>Description</th>
                </tr>
                <tr>
                    <td>TFSCollectionName/TriggerBuild</td>
                    <td>POST</td>
                    <td>
                        Project=<i>prjName</i>&Definition=<i>buildDef</i>
                    </td>
                    <td>
                        Triggers a new build using the Build Definition <i>buildDef</i> belonging to the Team Project 
                        <i>prjName</i> inside the TFS Project Collection named <i>TFSCollectionName</i>.
                    </td>
                </tr>
            </table>
        </div>
        <div id="footer">
            <span>&copy; <%= DateTime.Now.Year %> Microsoft Corporation. All rights reserved.</span>
        </div>
    </div>
</body>
</html>
