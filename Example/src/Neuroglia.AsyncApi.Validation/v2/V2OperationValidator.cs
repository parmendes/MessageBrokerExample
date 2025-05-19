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
/// Represents the service used to validate <see cref="V2OperationDefinition"/>s
/// </summary>
public class V2OperationValidator
    : AbstractValidator<V2OperationDefinition>
{

    /// <summary>
    /// Initializes a new <see cref="V2OperationValidator"/>
    /// </summary>
    public V2OperationValidator()
    {
        this.RuleForEach(o => o.Tags)
            .SetValidator(new V2TagValidator());
        this.RuleForEach(o => o.Traits)
            .SetValidator(new V2OperationTraitValidator());
        this.RuleFor(o => o.Message!)
            .SetValidator(new V2MessageValidator());
        this.RuleForEach(o => o.Message!.OneOf)
            .SetValidator(new V2MessageValidator())
            .When(o => o.Message != null);
    }

}
