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

namespace Neuroglia.AsyncApi.AspNetCore.UI.Models.v3;

/// <summary>
/// Holds the data used to render a <see cref="V3OperationTraitDefinition"/> view
/// </summary>
/// <param name="Document">The <see cref="V3AsyncApiDocument"/> that defines the <see cref="V3OperationTraitDefinition"/> to render</param>
/// <param name="Name">The name of the <see cref="V3OperationTraitDefinition"/> to render</param>
/// <param name="Definition">The <see cref="V3OperationTraitDefinition"/> to render</param>
/// <param name="Index">The index of the <see cref="V3OperationTraitDefinition"/> within its parent <see cref="V3OperationDefinition"/>, if any</param>
public record V3OperationTraitDefinitionViewModel(V3AsyncApiDocument Document, string Name, V3OperationTraitDefinition Definition, int? Index = null)
    : V3AsyncApiDocumentViewModel(Document)
{



}
