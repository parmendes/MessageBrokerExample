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

namespace Neuroglia.AsyncApi;

/// <summary>
/// Represents an <see cref="Attribute"/> used to mark a class as an Async Api to generate a new <see cref="IAsyncApiDocument"/> for
/// </summary>
/// <param name="title">The <see cref="AsyncApiAttribute"/>'s title</param>
/// <param name="version">The <see cref="AsyncApiAttribute"/>'s version</param>
public abstract class AsyncApiAttribute(string title, string version)
    : Attribute
{

    /// <summary>
    /// Gets/sets the generated <see cref="IAsyncApiDocument"/>'s id
    /// </summary>
    public virtual string? Id { get; set; }

    /// <summary>
    /// Gets the generated <see cref="IAsyncApiDocument"/>'s title
    /// </summary>
    public virtual string Title { get; } = title;

    /// <summary>
    /// Gets the generated <see cref="IAsyncApiDocument"/>'s version
    /// </summary>
    public virtual string Version { get; } = version;

    /// <summary>
    /// Gets/sets the generated <see cref="IAsyncApiDocument"/>'s description
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// Gets/sets the generated <see cref="IAsyncApiDocument"/>'s terms of service <see cref="Uri"/>
    /// </summary>
    public virtual string? TermsOfServiceUrl { get; set; }

    /// <summary>
    /// Gets/sets the generated <see cref="IAsyncApiDocument"/>'s contact name
    /// </summary>
    public virtual string? ContactName { get; set; }

    /// <summary>
    /// Gets/sets the generated <see cref="IAsyncApiDocument"/>'s contact url
    /// </summary>
    public virtual string? ContactUrl { get; set; }

    /// <summary>
    /// Gets/sets the generated <see cref="IAsyncApiDocument"/>'s contact email
    /// </summary>
    public virtual string? ContactEmail { get; set; }

    /// <summary>
    /// Gets/sets the generated <see cref="IAsyncApiDocument"/>'s license name
    /// </summary>
    public virtual string? LicenseName { get; set; }

    /// <summary>
    /// Gets/sets the generated <see cref="IAsyncApiDocument"/>'s license <see cref="Uri"/>
    /// </summary>
    public virtual string? LicenseUrl { get; set; }

}
