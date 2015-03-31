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

namespace Microsoft.Samples.DPE.ODataTFS.Web.Infrastructure
{
    using System;
    using System.Globalization;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    public class TFSInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var uriMatch = (UriTemplateMatch)request.Properties["UriTemplateMatchResults"];
            if (uriMatch.RelativePathSegments.Count > 0)
            {
                var collection = uriMatch.RelativePathSegments[0];
                request.Properties.Add("TfsCollectionName", collection);
                request.Properties["MicrosoftDataServicesRootUri"] = new Uri(uriMatch.BaseUri, collection);
            }

            if (request.Properties.ContainsKey("UriTemplateMatchResults"))
            {
                //note: body of this "if" copied over from OData Toolkit as this project
                //uses a dispatch inspector, and only one can be assigned at a time. ODataService<>
                //implementation does provide JSONP support, but we effectively remove that functionality
                //by creating this inspector. Therefore, the functionality was merged into this one
                //dispatch inspector for now.
                var match = (UriTemplateMatch)request.Properties["UriTemplateMatchResults"];
                var format = match.QueryParameters["$format"];

                match.QueryParameters["$filter"] = 
                    this.FixSubstringsinFilter(match.QueryParameters["$filter"]);

                if ("json".Equals(format, StringComparison.InvariantCultureIgnoreCase))
                {
                    // strip out $format from the query options to avoid an error
                    // due to use of a reserved option (starts with "$")
                    match.QueryParameters.Remove("$format");

                    // replace the Accept header so that the Data Services runtime 
                    // assumes the client asked for a JSON representation
                    var httpmsg = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
                    httpmsg.Headers["Accept"] = "application/json";

                    var callback = match.QueryParameters["$callback"];

                    if (!string.IsNullOrEmpty(callback))
                    {
                        match.QueryParameters.Remove("$callback");
                        return callback;
                    }
                }
            }
            else
            {
                return null;
            }

            return null;
        }

        private string FixSubstringsinFilter(string filterParam)
        {
            if (string.IsNullOrEmpty(filterParam))
            { return string.Empty; }

            string updatedFilterParam = filterParam;

            Regex substrRegEx = new Regex(@"substringof\([^,]*,[^,]*\)");
            Regex substrEqualityRegEx = new Regex(@"^\s(ne|eq)\s(true|false)");

            MatchCollection matches = substrRegEx.Matches(filterParam);
            foreach (Match m in matches)
            {
                string sub = filterParam.Substring(m.Index + m.Length);
                if (!substrEqualityRegEx.IsMatch(sub))
                {
                    updatedFilterParam = filterParam.Replace(m.Value, m.Value + " eq true");
                }
            }

            return updatedFilterParam;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            //Below code handles reformatting reply if json format was originally requested

            if (correlationState == null || !(correlationState is string))
                return;

            // If we have a JSONP callback then buffer the response, wrap it with the
            // callback call and then re-create the response message
            var callback = (string)correlationState;

            var reader = reply.GetReaderAtBodyContents();
            reader.ReadStartElement();

            var content = Encoding.UTF8.GetString(reader.ReadContentAsBase64());
            content = string.Format(CultureInfo.InvariantCulture, "{0}({1});", callback, content);

            var newReply = Message.CreateMessage(MessageVersion.None, string.Empty, new JsonBodyWriter2(content));
            newReply.Properties.CopyProperties(reply.Properties);
            reply = newReply;

            // change response content type to text/javascript if the JSON (only done when wrapped in a callback)
            var replyProperties =
                (HttpResponseMessageProperty)reply.Properties[HttpResponseMessageProperty.Name];

            replyProperties.Headers["Content-Type"] =
                replyProperties.Headers["Content-Type"].Replace("application/json", "text/javascript");
        }
    }
}