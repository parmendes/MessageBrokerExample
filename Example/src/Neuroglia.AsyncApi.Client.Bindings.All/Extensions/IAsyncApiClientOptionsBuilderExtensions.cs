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

using Neuroglia.AsyncApi.Client.Configuration;

namespace Neuroglia.AsyncApi.Client.Bindings;

/// <summary>
/// Defines extensions for <see cref="IAsyncApiClientOptionsBuilder"/>s
/// </summary>
public static class IAsyncApiClientOptionsBuilderExtensions
{

    /// <summary>
    /// Adds and configures the handlers for all default AsyncAPI bindings
    /// </summary>
    /// <param name="builder">The <see cref="IAsyncApiClientOptionsBuilder"/> to configure</param>
    /// <returns>The configured <see cref="IAsyncApiClientOptionsBuilder"/></returns>
    public static IAsyncApiClientOptionsBuilder AddAllBindingHandlers(this IAsyncApiClientOptionsBuilder builder)
    {
        builder.AddAmqpBindingHandler();
        builder.AddHttpBindingHandler();
        builder.AddKafkaBindingHandler();
        builder.AddMqttBindingHandler();
        builder.AddNatsBindingHandler();
        builder.AddPulsarBindingHandler();
        builder.AddRedisBindingHandler();
        builder.AddSolaceBindingHandler();
        builder.AddStompBindingHandler();
        builder.AddWebSocketBindingHandler();
        return builder;
    }

}
