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

namespace Neuroglia.AsyncApi.Validation.v2;

/// <summary>
/// Represents the service used to validate the <see cref="V2ApiInfo"/>
/// </summary>
public class V2ApiInfoValidator
    : AbstractValidator<V2ApiInfo>
{

    /// <summary>
    /// Initializes a new <see cref="V2ApiInfoValidator"/>
    /// </summary>
    public V2ApiInfoValidator()
    {
        this.RuleFor(i => i.Title)
            .NotEmpty();
        this.RuleFor(i => i.Version)
            .NotEmpty();
        this.RuleFor(i => i.License!)
            .SetValidator(new V2LicenseValidator());
    }

}
