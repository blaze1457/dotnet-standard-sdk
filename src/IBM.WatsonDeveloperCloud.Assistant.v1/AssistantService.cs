/**
* Copyright 2018 IBM Corp. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/

using System.Collections.Generic;
using System.Text;
using IBM.WatsonDeveloperCloud.Assistant.v1.Model;
using IBM.WatsonDeveloperCloud.Http;
using IBM.WatsonDeveloperCloud.Service;
using Newtonsoft.Json;
using System;

namespace IBM.WatsonDeveloperCloud.Assistant.v1
{
    public partial class AssistantService : WatsonService, IAssistantService
    {
        const string SERVICE_NAME = "assistant";
        const string URL = "https://gateway.watsonplatform.net/assistant/api";
        private string _versionDate;
        public string VersionDate
        {
            get { return _versionDate; }
            set { _versionDate = value; }
        }

        public AssistantService() : base(SERVICE_NAME, URL)
        {
            if(!string.IsNullOrEmpty(this.Endpoint))
                this.Endpoint = URL;
        }

        public AssistantService(string userName, string password, string versionDate) : this()
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            this.SetCredential(userName, password);
            if(string.IsNullOrEmpty(versionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            VersionDate = versionDate;
        }

        public AssistantService(IClient httpClient) : this()
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));

            this.Client = httpClient;
        }

        /// <summary>
        /// Get response to user input. Get a response to a user's input.    There is no rate limit for this operation.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="request">The message to be sent. This includes the user's input, along with optional intents, entities, and context from the last response. (optional)</param>
        /// <param name="nodesVisitedDetails">Whether to include additional diagnostic information about the dialog nodes that were visited during processing of the message. (optional, default to false)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="MessageResponse" />MessageResponse</returns>
        public MessageResponse Message(string workspaceId, MessageRequest messageRequest = null, bool? nodesVisitedDetails = null, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            MessageResponse result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .PostAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/message");
                request.WithArgument("version", VersionDate);
                if (nodesVisitedDetails != null)
                    request.WithArgument("nodes_visited_details", nodesVisitedDetails);
                request.WithBody<MessageRequest>(messageRequest);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<MessageResponse>().Result;
                if(result == null)
                    result = new MessageResponse();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }
        /// <summary>
        /// Create workspace. Create a workspace based on component objects. You must provide workspace components defining the content of the new workspace.    This operation is limited to 30 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="properties">The content of the new workspace.    The maximum size for this data is 50MB. If you need to import a larger workspace, consider importing the workspace without intents and entities and then adding them separately. (optional)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="Workspace" />Workspace</returns>
        public Workspace CreateWorkspace(CreateWorkspace properties = null, Dictionary<string, object> customData = null)
        {

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            Workspace result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .PostAsync($"{this.Endpoint}/v1/workspaces");
                request.WithArgument("version", VersionDate);
                request.WithBody<CreateWorkspace>(properties);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<Workspace>().Result;
                if(result == null)
                    result = new Workspace();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Delete workspace. Delete a workspace from the service instance.    This operation is limited to 30 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="BaseModel" />BaseModel</returns>
        public BaseModel DeleteWorkspace(string workspaceId, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            BaseModel result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .DeleteAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}");
                request.WithArgument("version", VersionDate);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<BaseModel>().Result;
                if(result == null)
                    result = new BaseModel();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Get information about a workspace. Get information about a workspace, optionally including all workspace content.    With **export**=`false`, this operation is limited to 6000 requests per 5 minutes. With **export**=`true`, the limit is 20 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="export">Whether to include all element content in the returned data. If **export**=`false`, the returned data includes only information about the element itself. If **export**=`true`, all content, including subelements, is included. (optional, default to false)</param>
        /// <param name="includeAudit">Whether to include the audit properties (`created` and `updated` timestamps) in the response. (optional, default to false)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="WorkspaceExport" />WorkspaceExport</returns>
        public WorkspaceExport GetWorkspace(string workspaceId, bool? export = null, bool? includeAudit = null, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            WorkspaceExport result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .GetAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}");
                request.WithArgument("version", VersionDate);
                if (export != null)
                    request.WithArgument("export", export);
                if (includeAudit != null)
                    request.WithArgument("include_audit", includeAudit);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<WorkspaceExport>().Result;
                if(result == null)
                    result = new WorkspaceExport();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// List workspaces. List the workspaces associated with a Watson Assistant service instance.    This operation is limited to 500 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="pageLimit">The number of records to return in each page of results. (optional, default to 100)</param>
        /// <param name="includeCount">Whether to include information about the number of records returned. (optional, default to false)</param>
        /// <param name="sort">The attribute by which returned results will be sorted. To reverse the sort order, prefix the value with a minus sign (`-`). Supported values are `name`, `updated`, and `workspace_id`. (optional)</param>
        /// <param name="cursor">A token identifying the page of results to retrieve. (optional)</param>
        /// <param name="includeAudit">Whether to include the audit properties (`created` and `updated` timestamps) in the response. (optional, default to false)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="WorkspaceCollection" />WorkspaceCollection</returns>
        public WorkspaceCollection ListWorkspaces(long? pageLimit = null, bool? includeCount = null, string sort = null, string cursor = null, bool? includeAudit = null, Dictionary<string, object> customData = null)
        {

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            WorkspaceCollection result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .GetAsync($"{this.Endpoint}/v1/workspaces");
                request.WithArgument("version", VersionDate);
                if (pageLimit != null)
                    request.WithArgument("page_limit", pageLimit);
                if (includeCount != null)
                    request.WithArgument("include_count", includeCount);
                if (!string.IsNullOrEmpty(sort))
                    request.WithArgument("sort", sort);
                if (!string.IsNullOrEmpty(cursor))
                    request.WithArgument("cursor", cursor);
                if (includeAudit != null)
                    request.WithArgument("include_audit", includeAudit);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<WorkspaceCollection>().Result;
                if(result == null)
                    result = new WorkspaceCollection();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Update workspace. Update an existing workspace with new or modified data. You must provide component objects defining the content of the updated workspace.    This operation is limited to 30 request per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="properties">Valid data defining the new and updated workspace content.    The maximum size for this data is 50MB. If you need to import a larger amount of workspace data, consider importing components such as intents and entities using separate operations. (optional)</param>
        /// <param name="append">Whether the new data is to be appended to the existing data in the workspace. If **append**=`false`, elements included in the new data completely replace the corresponding existing elements, including all subelements. For example, if the new data includes **entities** and **append**=`false`, all existing entities in the workspace are discarded and replaced with the new entities.    If **append**=`true`, existing elements are preserved, and the new elements are added. If any elements in the new data collide with existing elements, the update request fails. (optional, default to false)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="Workspace" />Workspace</returns>
        public Workspace UpdateWorkspace(string workspaceId, UpdateWorkspace properties = null, bool? append = null, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            Workspace result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .PostAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}");
                request.WithArgument("version", VersionDate);
                if (append != null)
                    request.WithArgument("append", append);
                request.WithBody<UpdateWorkspace>(properties);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<Workspace>().Result;
                if(result == null)
                    result = new Workspace();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }
        /// <summary>
        /// Create intent. Create a new intent.    This operation is limited to 2000 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="body">The content of the new intent.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="Intent" />Intent</returns>
        public Intent CreateIntent(string workspaceId, CreateIntent body, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (body == null)
                throw new ArgumentNullException(nameof(body));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            Intent result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .PostAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/intents");
                request.WithArgument("version", VersionDate);
                request.WithBody<CreateIntent>(body);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<Intent>().Result;
                if(result == null)
                    result = new Intent();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Delete intent. Delete an intent from a workspace.    This operation is limited to 2000 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="intent">The intent name.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="BaseModel" />BaseModel</returns>
        public BaseModel DeleteIntent(string workspaceId, string intent, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(intent))
                throw new ArgumentNullException(nameof(intent));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            BaseModel result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .DeleteAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/intents/{intent}");
                request.WithArgument("version", VersionDate);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<BaseModel>().Result;
                if(result == null)
                    result = new BaseModel();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Get intent. Get information about an intent, optionally including all intent content.    With **export**=`false`, this operation is limited to 6000 requests per 5 minutes. With **export**=`true`, the limit is 400 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="intent">The intent name.</param>
        /// <param name="export">Whether to include all element content in the returned data. If **export**=`false`, the returned data includes only information about the element itself. If **export**=`true`, all content, including subelements, is included. (optional, default to false)</param>
        /// <param name="includeAudit">Whether to include the audit properties (`created` and `updated` timestamps) in the response. (optional, default to false)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="IntentExport" />IntentExport</returns>
        public IntentExport GetIntent(string workspaceId, string intent, bool? export = null, bool? includeAudit = null, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(intent))
                throw new ArgumentNullException(nameof(intent));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            IntentExport result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .GetAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/intents/{intent}");
                request.WithArgument("version", VersionDate);
                if (export != null)
                    request.WithArgument("export", export);
                if (includeAudit != null)
                    request.WithArgument("include_audit", includeAudit);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<IntentExport>().Result;
                if(result == null)
                    result = new IntentExport();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// List intents. List the intents for a workspace.    With **export**=`false`, this operation is limited to 2000 requests per 30 minutes. With **export**=`true`, the limit is 400 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="export">Whether to include all element content in the returned data. If **export**=`false`, the returned data includes only information about the element itself. If **export**=`true`, all content, including subelements, is included. (optional, default to false)</param>
        /// <param name="pageLimit">The number of records to return in each page of results. (optional, default to 100)</param>
        /// <param name="includeCount">Whether to include information about the number of records returned. (optional, default to false)</param>
        /// <param name="sort">The attribute by which returned results will be sorted. To reverse the sort order, prefix the value with a minus sign (`-`). Supported values are `name`, `updated`, and `workspace_id`. (optional)</param>
        /// <param name="cursor">A token identifying the page of results to retrieve. (optional)</param>
        /// <param name="includeAudit">Whether to include the audit properties (`created` and `updated` timestamps) in the response. (optional, default to false)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="IntentCollection" />IntentCollection</returns>
        public IntentCollection ListIntents(string workspaceId, bool? export = null, long? pageLimit = null, bool? includeCount = null, string sort = null, string cursor = null, bool? includeAudit = null, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            IntentCollection result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .GetAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/intents");
                request.WithArgument("version", VersionDate);
                if (export != null)
                    request.WithArgument("export", export);
                if (pageLimit != null)
                    request.WithArgument("page_limit", pageLimit);
                if (includeCount != null)
                    request.WithArgument("include_count", includeCount);
                if (!string.IsNullOrEmpty(sort))
                    request.WithArgument("sort", sort);
                if (!string.IsNullOrEmpty(cursor))
                    request.WithArgument("cursor", cursor);
                if (includeAudit != null)
                    request.WithArgument("include_audit", includeAudit);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<IntentCollection>().Result;
                if(result == null)
                    result = new IntentCollection();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Update intent. Update an existing intent with new or modified data. You must provide component objects defining the content of the updated intent.    This operation is limited to 2000 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="intent">The intent name.</param>
        /// <param name="body">The updated content of the intent.    Any elements included in the new data will completely replace the equivalent existing elements, including all subelements. (Previously existing subelements are not retained unless they are also included in the new data.) For example, if you update the user input examples for an intent, the previously existing examples are discarded and replaced with the new examples specified in the update.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="Intent" />Intent</returns>
        public Intent UpdateIntent(string workspaceId, string intent, UpdateIntent body, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(intent))
                throw new ArgumentNullException(nameof(intent));
            if (body == null)
                throw new ArgumentNullException(nameof(body));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            Intent result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .PostAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/intents/{intent}");
                request.WithArgument("version", VersionDate);
                request.WithBody<UpdateIntent>(body);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<Intent>().Result;
                if(result == null)
                    result = new Intent();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }
        /// <summary>
        /// Create user input example. Add a new user input example to an intent.    This operation is limited to 1000 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="intent">The intent name.</param>
        /// <param name="body">The content of the new user input example.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="Example" />Example</returns>
        public Example CreateExample(string workspaceId, string intent, CreateExample body, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(intent))
                throw new ArgumentNullException(nameof(intent));
            if (body == null)
                throw new ArgumentNullException(nameof(body));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            Example result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .PostAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/intents/{intent}/examples");
                request.WithArgument("version", VersionDate);
                request.WithBody<CreateExample>(body);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<Example>().Result;
                if(result == null)
                    result = new Example();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Delete user input example. Delete a user input example from an intent.    This operation is limited to 1000 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="intent">The intent name.</param>
        /// <param name="text">The text of the user input example.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="BaseModel" />BaseModel</returns>
        public BaseModel DeleteExample(string workspaceId, string intent, string text, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(intent))
                throw new ArgumentNullException(nameof(intent));
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            BaseModel result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .DeleteAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/intents/{intent}/examples/{text}");
                request.WithArgument("version", VersionDate);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<BaseModel>().Result;
                if(result == null)
                    result = new BaseModel();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Get user input example. Get information about a user input example.    This operation is limited to 6000 requests per 5 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="intent">The intent name.</param>
        /// <param name="text">The text of the user input example.</param>
        /// <param name="includeAudit">Whether to include the audit properties (`created` and `updated` timestamps) in the response. (optional, default to false)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="Example" />Example</returns>
        public Example GetExample(string workspaceId, string intent, string text, bool? includeAudit = null, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(intent))
                throw new ArgumentNullException(nameof(intent));
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            Example result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .GetAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/intents/{intent}/examples/{text}");
                request.WithArgument("version", VersionDate);
                if (includeAudit != null)
                    request.WithArgument("include_audit", includeAudit);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<Example>().Result;
                if(result == null)
                    result = new Example();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// List user input examples. List the user input examples for an intent.    This operation is limited to 2500 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="intent">The intent name.</param>
        /// <param name="pageLimit">The number of records to return in each page of results. (optional, default to 100)</param>
        /// <param name="includeCount">Whether to include information about the number of records returned. (optional, default to false)</param>
        /// <param name="sort">The attribute by which returned results will be sorted. To reverse the sort order, prefix the value with a minus sign (`-`). Supported values are `name`, `updated`, and `workspace_id`. (optional)</param>
        /// <param name="cursor">A token identifying the page of results to retrieve. (optional)</param>
        /// <param name="includeAudit">Whether to include the audit properties (`created` and `updated` timestamps) in the response. (optional, default to false)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="ExampleCollection" />ExampleCollection</returns>
        public ExampleCollection ListExamples(string workspaceId, string intent, long? pageLimit = null, bool? includeCount = null, string sort = null, string cursor = null, bool? includeAudit = null, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(intent))
                throw new ArgumentNullException(nameof(intent));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            ExampleCollection result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .GetAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/intents/{intent}/examples");
                request.WithArgument("version", VersionDate);
                if (pageLimit != null)
                    request.WithArgument("page_limit", pageLimit);
                if (includeCount != null)
                    request.WithArgument("include_count", includeCount);
                if (!string.IsNullOrEmpty(sort))
                    request.WithArgument("sort", sort);
                if (!string.IsNullOrEmpty(cursor))
                    request.WithArgument("cursor", cursor);
                if (includeAudit != null)
                    request.WithArgument("include_audit", includeAudit);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<ExampleCollection>().Result;
                if(result == null)
                    result = new ExampleCollection();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Update user input example. Update the text of a user input example.    This operation is limited to 1000 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="intent">The intent name.</param>
        /// <param name="text">The text of the user input example.</param>
        /// <param name="body">The new text of the user input example.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="Example" />Example</returns>
        public Example UpdateExample(string workspaceId, string intent, string text, UpdateExample body, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(intent))
                throw new ArgumentNullException(nameof(intent));
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));
            if (body == null)
                throw new ArgumentNullException(nameof(body));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            Example result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .PostAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/intents/{intent}/examples/{text}");
                request.WithArgument("version", VersionDate);
                request.WithBody<UpdateExample>(body);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<Example>().Result;
                if(result == null)
                    result = new Example();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }
        /// <summary>
        /// Create counterexample. Add a new counterexample to a workspace. Counterexamples are examples that have been marked as irrelevant input.    This operation is limited to 1000 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="body">The content of the new counterexample.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="Counterexample" />Counterexample</returns>
        public Counterexample CreateCounterexample(string workspaceId, CreateCounterexample body, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (body == null)
                throw new ArgumentNullException(nameof(body));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            Counterexample result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .PostAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/counterexamples");
                request.WithArgument("version", VersionDate);
                request.WithBody<CreateCounterexample>(body);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<Counterexample>().Result;
                if(result == null)
                    result = new Counterexample();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Delete counterexample. Delete a counterexample from a workspace. Counterexamples are examples that have been marked as irrelevant input.    This operation is limited to 1000 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="text">The text of a user input counterexample (for example, `What are you wearing?`).</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="BaseModel" />BaseModel</returns>
        public BaseModel DeleteCounterexample(string workspaceId, string text, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            BaseModel result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .DeleteAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/counterexamples/{text}");
                request.WithArgument("version", VersionDate);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<BaseModel>().Result;
                if(result == null)
                    result = new BaseModel();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Get counterexample. Get information about a counterexample. Counterexamples are examples that have been marked as irrelevant input.    This operation is limited to 6000 requests per 5 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="text">The text of a user input counterexample (for example, `What are you wearing?`).</param>
        /// <param name="includeAudit">Whether to include the audit properties (`created` and `updated` timestamps) in the response. (optional, default to false)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="Counterexample" />Counterexample</returns>
        public Counterexample GetCounterexample(string workspaceId, string text, bool? includeAudit = null, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            Counterexample result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .GetAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/counterexamples/{text}");
                request.WithArgument("version", VersionDate);
                if (includeAudit != null)
                    request.WithArgument("include_audit", includeAudit);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<Counterexample>().Result;
                if(result == null)
                    result = new Counterexample();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// List counterexamples. List the counterexamples for a workspace. Counterexamples are examples that have been marked as irrelevant input.    This operation is limited to 2500 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="pageLimit">The number of records to return in each page of results. (optional, default to 100)</param>
        /// <param name="includeCount">Whether to include information about the number of records returned. (optional, default to false)</param>
        /// <param name="sort">The attribute by which returned results will be sorted. To reverse the sort order, prefix the value with a minus sign (`-`). Supported values are `name`, `updated`, and `workspace_id`. (optional)</param>
        /// <param name="cursor">A token identifying the page of results to retrieve. (optional)</param>
        /// <param name="includeAudit">Whether to include the audit properties (`created` and `updated` timestamps) in the response. (optional, default to false)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="CounterexampleCollection" />CounterexampleCollection</returns>
        public CounterexampleCollection ListCounterexamples(string workspaceId, long? pageLimit = null, bool? includeCount = null, string sort = null, string cursor = null, bool? includeAudit = null, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            CounterexampleCollection result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .GetAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/counterexamples");
                request.WithArgument("version", VersionDate);
                if (pageLimit != null)
                    request.WithArgument("page_limit", pageLimit);
                if (includeCount != null)
                    request.WithArgument("include_count", includeCount);
                if (!string.IsNullOrEmpty(sort))
                    request.WithArgument("sort", sort);
                if (!string.IsNullOrEmpty(cursor))
                    request.WithArgument("cursor", cursor);
                if (includeAudit != null)
                    request.WithArgument("include_audit", includeAudit);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<CounterexampleCollection>().Result;
                if(result == null)
                    result = new CounterexampleCollection();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Update counterexample. Update the text of a counterexample. Counterexamples are examples that have been marked as irrelevant input.    This operation is limited to 1000 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="text">The text of a user input counterexample (for example, `What are you wearing?`).</param>
        /// <param name="body">The text of the counterexample.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="Counterexample" />Counterexample</returns>
        public Counterexample UpdateCounterexample(string workspaceId, string text, UpdateCounterexample body, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));
            if (body == null)
                throw new ArgumentNullException(nameof(body));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            Counterexample result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .PostAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/counterexamples/{text}");
                request.WithArgument("version", VersionDate);
                request.WithBody<UpdateCounterexample>(body);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<Counterexample>().Result;
                if(result == null)
                    result = new Counterexample();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }
        /// <summary>
        /// Create entity. Create a new entity.    This operation is limited to 1000 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="properties">The content of the new entity.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="Entity" />Entity</returns>
        public Entity CreateEntity(string workspaceId, CreateEntity properties, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            Entity result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .PostAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/entities");
                request.WithArgument("version", VersionDate);
                request.WithBody<CreateEntity>(properties);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<Entity>().Result;
                if(result == null)
                    result = new Entity();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Delete entity. Delete an entity from a workspace.    This operation is limited to 1000 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="entity">The name of the entity.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="BaseModel" />BaseModel</returns>
        public BaseModel DeleteEntity(string workspaceId, string entity, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(entity))
                throw new ArgumentNullException(nameof(entity));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            BaseModel result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .DeleteAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/entities/{entity}");
                request.WithArgument("version", VersionDate);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<BaseModel>().Result;
                if(result == null)
                    result = new BaseModel();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Get entity. Get information about an entity, optionally including all entity content.    With **export**=`false`, this operation is limited to 6000 requests per 5 minutes. With **export**=`true`, the limit is 200 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="entity">The name of the entity.</param>
        /// <param name="export">Whether to include all element content in the returned data. If **export**=`false`, the returned data includes only information about the element itself. If **export**=`true`, all content, including subelements, is included. (optional, default to false)</param>
        /// <param name="includeAudit">Whether to include the audit properties (`created` and `updated` timestamps) in the response. (optional, default to false)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="EntityExport" />EntityExport</returns>
        public EntityExport GetEntity(string workspaceId, string entity, bool? export = null, bool? includeAudit = null, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(entity))
                throw new ArgumentNullException(nameof(entity));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            EntityExport result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .GetAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/entities/{entity}");
                request.WithArgument("version", VersionDate);
                if (export != null)
                    request.WithArgument("export", export);
                if (includeAudit != null)
                    request.WithArgument("include_audit", includeAudit);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<EntityExport>().Result;
                if(result == null)
                    result = new EntityExport();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// List entities. List the entities for a workspace.    With **export**=`false`, this operation is limited to 1000 requests per 30 minutes. With **export**=`true`, the limit is 200 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="export">Whether to include all element content in the returned data. If **export**=`false`, the returned data includes only information about the element itself. If **export**=`true`, all content, including subelements, is included. (optional, default to false)</param>
        /// <param name="pageLimit">The number of records to return in each page of results. (optional, default to 100)</param>
        /// <param name="includeCount">Whether to include information about the number of records returned. (optional, default to false)</param>
        /// <param name="sort">The attribute by which returned results will be sorted. To reverse the sort order, prefix the value with a minus sign (`-`). Supported values are `name`, `updated`, and `workspace_id`. (optional)</param>
        /// <param name="cursor">A token identifying the page of results to retrieve. (optional)</param>
        /// <param name="includeAudit">Whether to include the audit properties (`created` and `updated` timestamps) in the response. (optional, default to false)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="EntityCollection" />EntityCollection</returns>
        public EntityCollection ListEntities(string workspaceId, bool? export = null, long? pageLimit = null, bool? includeCount = null, string sort = null, string cursor = null, bool? includeAudit = null, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            EntityCollection result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .GetAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/entities");
                request.WithArgument("version", VersionDate);
                if (export != null)
                    request.WithArgument("export", export);
                if (pageLimit != null)
                    request.WithArgument("page_limit", pageLimit);
                if (includeCount != null)
                    request.WithArgument("include_count", includeCount);
                if (!string.IsNullOrEmpty(sort))
                    request.WithArgument("sort", sort);
                if (!string.IsNullOrEmpty(cursor))
                    request.WithArgument("cursor", cursor);
                if (includeAudit != null)
                    request.WithArgument("include_audit", includeAudit);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<EntityCollection>().Result;
                if(result == null)
                    result = new EntityCollection();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Update entity. Update an existing entity with new or modified data. You must provide component objects defining the content of the updated entity.    This operation is limited to 1000 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="entity">The name of the entity.</param>
        /// <param name="properties">The updated content of the entity. Any elements included in the new data will completely replace the equivalent existing elements, including all subelements. (Previously existing subelements are not retained unless they are also included in the new data.) For example, if you update the values for an entity, the previously existing values are discarded and replaced with the new values specified in the update.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="Entity" />Entity</returns>
        public Entity UpdateEntity(string workspaceId, string entity, UpdateEntity properties, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(entity))
                throw new ArgumentNullException(nameof(entity));
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            Entity result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .PostAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/entities/{entity}");
                request.WithArgument("version", VersionDate);
                request.WithBody<UpdateEntity>(properties);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<Entity>().Result;
                if(result == null)
                    result = new Entity();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }
        /// <summary>
        /// Add entity value. Create a new value for an entity.    This operation is limited to 1000 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="entity">The name of the entity.</param>
        /// <param name="properties">The new entity value.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="Value" />Value</returns>
        public Value CreateValue(string workspaceId, string entity, CreateValue properties, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(entity))
                throw new ArgumentNullException(nameof(entity));
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            Value result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .PostAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/entities/{entity}/values");
                request.WithArgument("version", VersionDate);
                request.WithBody<CreateValue>(properties);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<Value>().Result;
                if(result == null)
                    result = new Value();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Delete entity value. Delete a value from an entity.    This operation is limited to 1000 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="entity">The name of the entity.</param>
        /// <param name="value">The text of the entity value.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="BaseModel" />BaseModel</returns>
        public BaseModel DeleteValue(string workspaceId, string entity, string value, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(entity))
                throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            BaseModel result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .DeleteAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}");
                request.WithArgument("version", VersionDate);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<BaseModel>().Result;
                if(result == null)
                    result = new BaseModel();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Get entity value. Get information about an entity value.    This operation is limited to 6000 requests per 5 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="entity">The name of the entity.</param>
        /// <param name="value">The text of the entity value.</param>
        /// <param name="export">Whether to include all element content in the returned data. If **export**=`false`, the returned data includes only information about the element itself. If **export**=`true`, all content, including subelements, is included. (optional, default to false)</param>
        /// <param name="includeAudit">Whether to include the audit properties (`created` and `updated` timestamps) in the response. (optional, default to false)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="ValueExport" />ValueExport</returns>
        public ValueExport GetValue(string workspaceId, string entity, string value, bool? export = null, bool? includeAudit = null, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(entity))
                throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            ValueExport result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .GetAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}");
                request.WithArgument("version", VersionDate);
                if (export != null)
                    request.WithArgument("export", export);
                if (includeAudit != null)
                    request.WithArgument("include_audit", includeAudit);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<ValueExport>().Result;
                if(result == null)
                    result = new ValueExport();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// List entity values. List the values for an entity.    This operation is limited to 2500 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="entity">The name of the entity.</param>
        /// <param name="export">Whether to include all element content in the returned data. If **export**=`false`, the returned data includes only information about the element itself. If **export**=`true`, all content, including subelements, is included. (optional, default to false)</param>
        /// <param name="pageLimit">The number of records to return in each page of results. (optional, default to 100)</param>
        /// <param name="includeCount">Whether to include information about the number of records returned. (optional, default to false)</param>
        /// <param name="sort">The attribute by which returned results will be sorted. To reverse the sort order, prefix the value with a minus sign (`-`). Supported values are `name`, `updated`, and `workspace_id`. (optional)</param>
        /// <param name="cursor">A token identifying the page of results to retrieve. (optional)</param>
        /// <param name="includeAudit">Whether to include the audit properties (`created` and `updated` timestamps) in the response. (optional, default to false)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="ValueCollection" />ValueCollection</returns>
        public ValueCollection ListValues(string workspaceId, string entity, bool? export = null, long? pageLimit = null, bool? includeCount = null, string sort = null, string cursor = null, bool? includeAudit = null, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(entity))
                throw new ArgumentNullException(nameof(entity));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            ValueCollection result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .GetAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/entities/{entity}/values");
                request.WithArgument("version", VersionDate);
                if (export != null)
                    request.WithArgument("export", export);
                if (pageLimit != null)
                    request.WithArgument("page_limit", pageLimit);
                if (includeCount != null)
                    request.WithArgument("include_count", includeCount);
                if (!string.IsNullOrEmpty(sort))
                    request.WithArgument("sort", sort);
                if (!string.IsNullOrEmpty(cursor))
                    request.WithArgument("cursor", cursor);
                if (includeAudit != null)
                    request.WithArgument("include_audit", includeAudit);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<ValueCollection>().Result;
                if(result == null)
                    result = new ValueCollection();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Update entity value. Update an existing entity value with new or modified data. You must provide component objects defining the content of the updated entity value.    This operation is limited to 1000 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="entity">The name of the entity.</param>
        /// <param name="value">The text of the entity value.</param>
        /// <param name="properties">The updated content of the entity value.    Any elements included in the new data will completely replace the equivalent existing elements, including all subelements. (Previously existing subelements are not retained unless they are also included in the new data.) For example, if you update the synonyms for an entity value, the previously existing synonyms are discarded and replaced with the new synonyms specified in the update.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="Value" />Value</returns>
        public Value UpdateValue(string workspaceId, string entity, string value, UpdateValue properties, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(entity))
                throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            Value result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .PostAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}");
                request.WithArgument("version", VersionDate);
                request.WithBody<UpdateValue>(properties);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<Value>().Result;
                if(result == null)
                    result = new Value();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }
        /// <summary>
        /// Add entity value synonym. Add a new synonym to an entity value.    This operation is limited to 1000 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="entity">The name of the entity.</param>
        /// <param name="value">The text of the entity value.</param>
        /// <param name="body">The new synonym.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="Synonym" />Synonym</returns>
        public Synonym CreateSynonym(string workspaceId, string entity, string value, CreateSynonym body, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(entity))
                throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));
            if (body == null)
                throw new ArgumentNullException(nameof(body));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            Synonym result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .PostAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}/synonyms");
                request.WithArgument("version", VersionDate);
                request.WithBody<CreateSynonym>(body);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<Synonym>().Result;
                if(result == null)
                    result = new Synonym();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Delete entity value synonym. Delete a synonym from an entity value.    This operation is limited to 1000 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="entity">The name of the entity.</param>
        /// <param name="value">The text of the entity value.</param>
        /// <param name="synonym">The text of the synonym.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="BaseModel" />BaseModel</returns>
        public BaseModel DeleteSynonym(string workspaceId, string entity, string value, string synonym, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(entity))
                throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(synonym))
                throw new ArgumentNullException(nameof(synonym));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            BaseModel result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .DeleteAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}/synonyms/{synonym}");
                request.WithArgument("version", VersionDate);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<BaseModel>().Result;
                if(result == null)
                    result = new BaseModel();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Get entity value synonym. Get information about a synonym of an entity value.    This operation is limited to 6000 requests per 5 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="entity">The name of the entity.</param>
        /// <param name="value">The text of the entity value.</param>
        /// <param name="synonym">The text of the synonym.</param>
        /// <param name="includeAudit">Whether to include the audit properties (`created` and `updated` timestamps) in the response. (optional, default to false)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="Synonym" />Synonym</returns>
        public Synonym GetSynonym(string workspaceId, string entity, string value, string synonym, bool? includeAudit = null, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(entity))
                throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(synonym))
                throw new ArgumentNullException(nameof(synonym));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            Synonym result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .GetAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}/synonyms/{synonym}");
                request.WithArgument("version", VersionDate);
                if (includeAudit != null)
                    request.WithArgument("include_audit", includeAudit);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<Synonym>().Result;
                if(result == null)
                    result = new Synonym();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// List entity value synonyms. List the synonyms for an entity value.    This operation is limited to 2500 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="entity">The name of the entity.</param>
        /// <param name="value">The text of the entity value.</param>
        /// <param name="pageLimit">The number of records to return in each page of results. (optional, default to 100)</param>
        /// <param name="includeCount">Whether to include information about the number of records returned. (optional, default to false)</param>
        /// <param name="sort">The attribute by which returned results will be sorted. To reverse the sort order, prefix the value with a minus sign (`-`). Supported values are `name`, `updated`, and `workspace_id`. (optional)</param>
        /// <param name="cursor">A token identifying the page of results to retrieve. (optional)</param>
        /// <param name="includeAudit">Whether to include the audit properties (`created` and `updated` timestamps) in the response. (optional, default to false)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="SynonymCollection" />SynonymCollection</returns>
        public SynonymCollection ListSynonyms(string workspaceId, string entity, string value, long? pageLimit = null, bool? includeCount = null, string sort = null, string cursor = null, bool? includeAudit = null, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(entity))
                throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            SynonymCollection result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .GetAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}/synonyms");
                request.WithArgument("version", VersionDate);
                if (pageLimit != null)
                    request.WithArgument("page_limit", pageLimit);
                if (includeCount != null)
                    request.WithArgument("include_count", includeCount);
                if (!string.IsNullOrEmpty(sort))
                    request.WithArgument("sort", sort);
                if (!string.IsNullOrEmpty(cursor))
                    request.WithArgument("cursor", cursor);
                if (includeAudit != null)
                    request.WithArgument("include_audit", includeAudit);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<SynonymCollection>().Result;
                if(result == null)
                    result = new SynonymCollection();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Update entity value synonym. Update an existing entity value synonym with new text.    This operation is limited to 1000 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="entity">The name of the entity.</param>
        /// <param name="value">The text of the entity value.</param>
        /// <param name="synonym">The text of the synonym.</param>
        /// <param name="body">The updated entity value synonym.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="Synonym" />Synonym</returns>
        public Synonym UpdateSynonym(string workspaceId, string entity, string value, string synonym, UpdateSynonym body, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(entity))
                throw new ArgumentNullException(nameof(entity));
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));
            if (string.IsNullOrEmpty(synonym))
                throw new ArgumentNullException(nameof(synonym));
            if (body == null)
                throw new ArgumentNullException(nameof(body));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            Synonym result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .PostAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/entities/{entity}/values/{value}/synonyms/{synonym}");
                request.WithArgument("version", VersionDate);
                request.WithBody<UpdateSynonym>(body);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<Synonym>().Result;
                if(result == null)
                    result = new Synonym();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }
        /// <summary>
        /// Create dialog node. Create a new dialog node.    This operation is limited to 500 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="properties">A CreateDialogNode object defining the content of the new dialog node.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="DialogNode" />DialogNode</returns>
        public DialogNode CreateDialogNode(string workspaceId, CreateDialogNode properties, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            DialogNode result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .PostAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/dialog_nodes");
                request.WithArgument("version", VersionDate);
                request.WithBody<CreateDialogNode>(properties);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<DialogNode>().Result;
                if(result == null)
                    result = new DialogNode();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Delete dialog node. Delete a dialog node from a workspace.    This operation is limited to 500 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="dialogNode">The dialog node ID (for example, `get_order`).</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="BaseModel" />BaseModel</returns>
        public BaseModel DeleteDialogNode(string workspaceId, string dialogNode, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(dialogNode))
                throw new ArgumentNullException(nameof(dialogNode));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            BaseModel result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .DeleteAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/dialog_nodes/{dialogNode}");
                request.WithArgument("version", VersionDate);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<BaseModel>().Result;
                if(result == null)
                    result = new BaseModel();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Get dialog node. Get information about a dialog node.    This operation is limited to 6000 requests per 5 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="dialogNode">The dialog node ID (for example, `get_order`).</param>
        /// <param name="includeAudit">Whether to include the audit properties (`created` and `updated` timestamps) in the response. (optional, default to false)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="DialogNode" />DialogNode</returns>
        public DialogNode GetDialogNode(string workspaceId, string dialogNode, bool? includeAudit = null, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(dialogNode))
                throw new ArgumentNullException(nameof(dialogNode));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            DialogNode result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .GetAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/dialog_nodes/{dialogNode}");
                request.WithArgument("version", VersionDate);
                if (includeAudit != null)
                    request.WithArgument("include_audit", includeAudit);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<DialogNode>().Result;
                if(result == null)
                    result = new DialogNode();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// List dialog nodes. List the dialog nodes for a workspace.    This operation is limited to 2500 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="pageLimit">The number of records to return in each page of results. (optional, default to 100)</param>
        /// <param name="includeCount">Whether to include information about the number of records returned. (optional, default to false)</param>
        /// <param name="sort">The attribute by which returned results will be sorted. To reverse the sort order, prefix the value with a minus sign (`-`). Supported values are `name`, `updated`, and `workspace_id`. (optional)</param>
        /// <param name="cursor">A token identifying the page of results to retrieve. (optional)</param>
        /// <param name="includeAudit">Whether to include the audit properties (`created` and `updated` timestamps) in the response. (optional, default to false)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="DialogNodeCollection" />DialogNodeCollection</returns>
        public DialogNodeCollection ListDialogNodes(string workspaceId, long? pageLimit = null, bool? includeCount = null, string sort = null, string cursor = null, bool? includeAudit = null, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            DialogNodeCollection result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .GetAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/dialog_nodes");
                request.WithArgument("version", VersionDate);
                if (pageLimit != null)
                    request.WithArgument("page_limit", pageLimit);
                if (includeCount != null)
                    request.WithArgument("include_count", includeCount);
                if (!string.IsNullOrEmpty(sort))
                    request.WithArgument("sort", sort);
                if (!string.IsNullOrEmpty(cursor))
                    request.WithArgument("cursor", cursor);
                if (includeAudit != null)
                    request.WithArgument("include_audit", includeAudit);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<DialogNodeCollection>().Result;
                if(result == null)
                    result = new DialogNodeCollection();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// Update dialog node. Update an existing dialog node with new or modified data.    This operation is limited to 500 requests per 30 minutes. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="dialogNode">The dialog node ID (for example, `get_order`).</param>
        /// <param name="properties">The updated content of the dialog node.    Any elements included in the new data will completely replace the equivalent existing elements, including all subelements. (Previously existing subelements are not retained unless they are also included in the new data.) For example, if you update the actions for a dialog node, the previously existing actions are discarded and replaced with the new actions specified in the update.</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="DialogNode" />DialogNode</returns>
        public DialogNode UpdateDialogNode(string workspaceId, string dialogNode, UpdateDialogNode properties, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));
            if (string.IsNullOrEmpty(dialogNode))
                throw new ArgumentNullException(nameof(dialogNode));
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            DialogNode result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .PostAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/dialog_nodes/{dialogNode}");
                request.WithArgument("version", VersionDate);
                request.WithBody<UpdateDialogNode>(properties);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<DialogNode>().Result;
                if(result == null)
                    result = new DialogNode();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }
        /// <summary>
        /// List log events in all workspaces. List the events from the logs of all workspaces in the service instance.    If **cursor** is not specified, this operation is limited to 40 requests per 30 minutes. If **cursor** is specified, the limit is 120 requests per minute. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="filter">A cacheable parameter that limits the results to those matching the specified filter. You must specify a filter query that includes a value for `language`, as well as a value for `workspace_id` or `request.context.metadata.deployment`. For more information, see the [documentation](https://console.bluemix.net/docs/services/conversation/filter-reference.html#filter-query-syntax).</param>
        /// <param name="sort">The attribute by which returned results will be sorted. To reverse the sort order, prefix the value with a minus sign (`-`). Supported values are `name`, `updated`, and `workspace_id`. (optional)</param>
        /// <param name="pageLimit">The number of records to return in each page of results. (optional, default to 100)</param>
        /// <param name="cursor">A token identifying the page of results to retrieve. (optional)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="LogCollection" />LogCollection</returns>
        public LogCollection ListAllLogs(string filter, string sort = null, long? pageLimit = null, string cursor = null, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(filter))
                throw new ArgumentNullException(nameof(filter));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            LogCollection result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .GetAsync($"{this.Endpoint}/v1/logs");
                request.WithArgument("version", VersionDate);
                if (!string.IsNullOrEmpty(filter))
                    request.WithArgument("filter", filter);
                if (!string.IsNullOrEmpty(sort))
                    request.WithArgument("sort", sort);
                if (pageLimit != null)
                    request.WithArgument("page_limit", pageLimit);
                if (!string.IsNullOrEmpty(cursor))
                    request.WithArgument("cursor", cursor);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<LogCollection>().Result;
                if(result == null)
                    result = new LogCollection();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }

        /// <summary>
        /// List log events in a workspace. List the events from the log of a specific workspace.    If **cursor** is not specified, this operation is limited to 40 requests per 30 minutes. If **cursor** is specified, the limit is 120 requests per minute. For more information, see **Rate limiting**.
        /// </summary>
        /// <param name="workspaceId">Unique identifier of the workspace.</param>
        /// <param name="sort">The attribute by which returned results will be sorted. To reverse the sort order, prefix the value with a minus sign (`-`). Supported values are `name`, `updated`, and `workspace_id`. (optional)</param>
        /// <param name="filter">A cacheable parameter that limits the results to those matching the specified filter. For more information, see the [documentation](https://console.bluemix.net/docs/services/conversation/filter-reference.html#filter-query-syntax). (optional)</param>
        /// <param name="pageLimit">The number of records to return in each page of results. (optional, default to 100)</param>
        /// <param name="cursor">A token identifying the page of results to retrieve. (optional)</param>
        /// <param name="customData">Custom data object to pass data including custom request headers.</param>
        /// <returns><see cref="LogCollection" />LogCollection</returns>
        public LogCollection ListLogs(string workspaceId, string sort = null, string filter = null, long? pageLimit = null, string cursor = null, Dictionary<string, object> customData = null)
        {
            if (string.IsNullOrEmpty(workspaceId))
                throw new ArgumentNullException(nameof(workspaceId));

            if(string.IsNullOrEmpty(VersionDate))
                throw new ArgumentNullException("versionDate cannot be null.");

            LogCollection result = null;

            try
            {
                var request = this.Client.WithAuthentication(this.UserName, this.Password)
                                .GetAsync($"{this.Endpoint}/v1/workspaces/{workspaceId}/logs");
                request.WithArgument("version", VersionDate);
                if (!string.IsNullOrEmpty(sort))
                    request.WithArgument("sort", sort);
                if (!string.IsNullOrEmpty(filter))
                    request.WithArgument("filter", filter);
                if (pageLimit != null)
                    request.WithArgument("page_limit", pageLimit);
                if (!string.IsNullOrEmpty(cursor))
                    request.WithArgument("cursor", cursor);
                if (customData != null)
                    request.WithCustomData(customData);
                result = request.As<LogCollection>().Result;
                if(result == null)
                    result = new LogCollection();
                result.CustomData = request.CustomData;
            }
            catch(AggregateException ae)
            {
                throw ae.Flatten();
            }

            return result;
        }
    }
}
