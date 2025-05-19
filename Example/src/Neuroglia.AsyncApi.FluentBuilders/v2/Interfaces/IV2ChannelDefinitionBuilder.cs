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

namespace Neuroglia.AsyncApi.FluentBuilders.v2;

/// <summary>
/// Defines the fundamentals of a service used to build <see cref="V2ChannelDefinition"/>s
/// </summary>
public interface IV2ChannelDefinitionBuilder
{

    /// <summary>
    /// Configures the <see cref="V2ChannelDefinition"/> to build to use the specified description
    /// </summary>
    /// <param name="description">The <see cref="V2ChannelDefinition"/>'s description</param>
    /// <returns>The configured <see cref="IV2ChannelDefinitionBuilder"/></returns>
    IV2ChannelDefinitionBuilder WithDescription(string? description);

    /// <summary>
    /// Adds a new <see cref="V2ParameterDefinition"/> to the <see cref="V2ChannelDefinition"/> to build
    /// </summary>
    /// <param name="name">The name of the <see cref="V2ParameterDefinition"/> to add</param>
    /// <param name="setup">An <see cref="Action{T}"/> used to setup the <see cref="V2ParameterDefinition"/> to add</param>
    /// <returns>The configured <see cref="IV2ChannelDefinitionBuilder"/></returns>
    IV2ChannelDefinitionBuilder WithParameter(string name, Action<IV2ParameterDefinitionBuilder> setup);

    /// <summary>
    /// Adds the specified <see cref="IChannelBindingDefinition"/> to the <see cref="V2ChannelDefinition"/> to build
    /// </summary>
    /// <param name="binding">The <see cref="IChannelBindingDefinition"/> to add</param>
    /// <returns>The configured <see cref="IV2ChannelDefinitionBuilder"/></returns>
    IV2ChannelDefinitionBuilder WithBinding(IChannelBindingDefinition binding);

    /// <summary>
    /// Defines and configures an operation of the <see cref="V2ChannelDefinition"/> to build
    /// </summary>
    /// <param name="type">The <see cref="V2OperationDefinition"/>'s type</param>
    /// <param name="setup">An <see cref="Action{T}"/> used to setup the Subscribe <see cref="V2OperationDefinition"/></param>
    /// <returns>The configured <see cref="IV2ChannelDefinitionBuilder"/></returns>
    IV2ChannelDefinitionBuilder WithOperation(V2OperationType type, Action<IV2OperationDefinitionBuilder> setup);

    /// <summary>
    /// Defines and configures the Subscribe operation of the <see cref="V2ChannelDefinition"/> to build
    /// </summary>
    /// <param name="setup">An <see cref="Action{T}"/> used to setup the Subscribe <see cref="V2OperationDefinition"/></param>
    /// <returns>The configured <see cref="IV2ChannelDefinitionBuilder"/></returns>
    IV2ChannelDefinitionBuilder WithSubscribeOperation(Action<IV2OperationDefinitionBuilder> setup);

    /// <summary>
    /// Defines and configures the Publish operation of the <see cref="V2ChannelDefinition"/> to build
    /// </summary>
    /// <param name="setup">An <see cref="Action{T}"/> used to setup the Publish <see cref="V2OperationDefinition"/></param>
    /// <returns>The configured <see cref="IV2ChannelDefinitionBuilder"/></returns>
    IV2ChannelDefinitionBuilder WithPublishOperation(Action<IV2OperationDefinitionBuilder> setup);

    /// <summary>
    /// Builds a new <see cref="V2ChannelDefinition"/>
    /// </summary>
    /// <returns>A new <see cref="V2ChannelDefinition"/></returns>
    V2ChannelDefinition Build();

}
