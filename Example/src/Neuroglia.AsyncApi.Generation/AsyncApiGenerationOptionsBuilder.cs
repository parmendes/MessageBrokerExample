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

namespace Neuroglia.AsyncApi.Generation;

/// <summary>
/// Represents the default implementation of the <see cref="IAsyncApiGenerationOptionsBuilder"/> interface
/// </summary>
/// <param name="options">The <see cref="AsyncApiGenerationOptions"/> to configure</param>
public class AsyncApiGenerationOptionsBuilder(AsyncApiGenerationOptions options)
    : IAsyncApiGenerationOptionsBuilder
{

    /// <summary>
    /// Initializes a new <see cref="AsyncApiGenerationOptionsBuilder"/>
    /// </summary>
    public AsyncApiGenerationOptionsBuilder() : this(new()) { }

    /// <summary>
    /// Gets the <see cref="AsyncApiGenerationOptions"/> to configure
    /// </summary>
    protected AsyncApiGenerationOptions Options { get; } = options;

    /// <inheritdoc/>
    public virtual IAsyncApiGenerationOptionsBuilder WithMarkupType(Type markupType)
    {
        ArgumentNullException.ThrowIfNull(markupType);
        Options.MarkupTypes.Add(markupType);
        return this;
    }

    /// <inheritdoc/>
    public virtual IAsyncApiGenerationOptionsBuilder WithMarkupType<TMarkup>() => WithMarkupType(typeof(TMarkup));

    /// <inheritdoc/>
    public virtual IAsyncApiGenerationOptionsBuilder UseDefaultV2DocumentConfiguration(Action<IV2AsyncApiDocumentBuilder> configurationAction)
    {
        ArgumentNullException.ThrowIfNull(configurationAction);
        Options.DefaultV2DocumentConfiguration = configurationAction;
        return this;
    }

    /// <inheritdoc/>
    public virtual IAsyncApiGenerationOptionsBuilder UseDefaultV3DocumentConfiguration(Action<IV3AsyncApiDocumentBuilder> configurationAction)
    {
        ArgumentNullException.ThrowIfNull(configurationAction);
        Options.DefaultV3DocumentConfiguration = configurationAction;
        return this;
    }

    /// <inheritdoc/>
    public virtual AsyncApiGenerationOptions Build() => Options;


}
