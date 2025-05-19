﻿// Copyright © 2021-Present Neuroglia SRL. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License"),
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Neuroglia.AsyncApi.Bindings.Sqs;

/// <summary>
/// Represents an SQS identifier
/// </summary>
[DataContract]
public record SqsIdentifier
{

    /// <summary>
    /// Gets/sets the endpoint's arn
    /// </summary>
    [DataMember(Order = 1, Name = "arn"), JsonPropertyOrder(1), JsonPropertyName("arn"), YamlMember(Order = 1, Alias = "arn")]
    public virtual string? Arn { get; set; }

    /// <summary>
    /// Gets/sets the endpoint's name
    /// </summary>
    [DataMember(Order = 2, Name = "name"), JsonPropertyOrder(2), JsonPropertyName("name"), YamlMember(Order = 2, Alias = "name")]
    public virtual string? Name { get; set; }

}