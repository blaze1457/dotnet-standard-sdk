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

namespace IBM.WatsonDeveloperCloud.VisualRecognition.v3.Model
{
    /// <summary>
    /// DetectedFaces.
    /// </summary>
    public class DetectedFaces : BaseModel
    {
        /// <summary>
        /// Number of images processed for the API call.
        /// </summary>
        /// <value>Number of images processed for the API call.</value>
        [JsonProperty("images_processed", NullValueHandling = NullValueHandling.Ignore)]
        public long? ImagesProcessed { get; set; }
        /// <summary>
        /// The array of images.
        /// </summary>
        /// <value>The array of images.</value>
        [JsonProperty("images", NullValueHandling = NullValueHandling.Ignore)]
        public List<ImageWithFaces> Images { get; set; }
        /// <summary>
        /// Information about what might cause less than optimal output. For example, a request sent with a corrupt .zip file and a list of image URLs will still complete, but does not return the expected output. Not returned when there is no warning.
        /// </summary>
        /// <value>Information about what might cause less than optimal output. For example, a request sent with a corrupt .zip file and a list of image URLs will still complete, but does not return the expected output. Not returned when there is no warning.</value>
        [JsonProperty("warnings", NullValueHandling = NullValueHandling.Ignore)]
        public List<WarningInfo> Warnings { get; set; }
    }

}
