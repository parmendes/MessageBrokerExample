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
/// Represents the default implementation of the <see cref="IV2TagDefinitionBuilder"/>
/// </summary>
/// <remarks>
/// Initializes a new <see cref="V2TagDefinitionBuilder"/>
/// </remarks>
/// <param name="validators">An <see cref="IEnumerable{T}"/> containing the services used to validate <see cref="V2TagDefinition"/>s</param>
public class V2TagDefinitionBuilder(IEnumerable<IValidator<V2TagDefinition>> validators)
    : IV2TagDefinitionBuilder
{

    /// <summary>
    /// Gets the services used to validate <see cref="V2TagDefinition"/>s
    /// </summary>
    protected virtual IEnumerable<IValidator<V2TagDefinition>> Validators { get; } = validators;

    /// <summary>
    /// Gets the <see cref="V2TagDefinition"/> to configure
    /// </summary>
    protected virtual V2TagDefinition Tag { get; } = new();

    /// <inheritdoc/>
    public virtual IV2TagDefinitionBuilder WithName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        Tag.Name = name;
        return this;
    }

    /// <inheritdoc/>
    public virtual IV2TagDefinitionBuilder WithDescription(string? description)
    {
        Tag.Description = description;
        return this;
    }

    /// <inheritdoc/>
    public virtual IV2TagDefinitionBuilder WithExternalDocumentation(Uri uri, string? description = null)
    {
        ArgumentNullException.ThrowIfNull(uri);
        Tag.ExternalDocs = new() { Url = uri, Description = description };
        return this;
    }

    /// <inheritdoc/>
    public virtual V2TagDefinition Build()
    {
        var validationResults = Validators.Select(v => v.Validate(Tag));
        if (!validationResults.All(r => r.IsValid)) throw new ValidationException(validationResults.Where(r => !r.IsValid).SelectMany(r => r.Errors));
        return Tag;
    }

}
