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
using Newtonsoft.Json;

namespace IBM.WatsonDeveloperCloud.NaturalLanguageClassifier.v1.Model
{
    /// <summary>
    /// List of available classifiers.
    /// </summary>
    public class ClassifierList : BaseModel
    {
        /// <summary>
        /// The classifiers available to the user. Returns an empty array if no classifiers are available.
        /// </summary>
        /// <value>The classifiers available to the user. Returns an empty array if no classifiers are available.</value>
        [JsonProperty("classifiers", NullValueHandling = NullValueHandling.Ignore)]
        public List<Classifier> Classifiers { get; set; }
    }

}
