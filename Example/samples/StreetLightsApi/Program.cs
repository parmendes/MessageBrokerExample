// This file configures and starts the ASP.NET Core application for the StreetLightsApi sample.
// It registers all required services, sets up AsyncAPI document generation (code-first) for both AsyncAPI v2 and v3,
// configures security schemes, channel and operation bindings, and exposes the AsyncAPI UI and API endpoints.
// The configuration ensures modularity, proper RabbitMQ infrastructure documentation, and best practices for maintainability.

// Copyright © 2021-Present Neuroglia SRL. All rights reserved.
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

using Neuroglia.AsyncApi.Bindings;
using Neuroglia.AsyncApi.Bindings.Amqp;
using StreetLightsApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Register core services for Razor Pages (with runtime compilation), AsyncAPI UI, HTTP client, and DI for API components
builder.Services.AddRazorPages().AddRazorRuntimeCompilation(); // Enables Razor Pages and hot reload for development
builder.Services.AddAsyncApiUI(); // Adds the AsyncAPI UI middleware for interactive documentation
builder.Services.AddHttpClient(); // Registers the default HTTP client for DI
builder.Services.AddTransient<Neuroglia.Data.Schemas.Json.IJsonSchemaResolver, Neuroglia.Data.Schemas.Json.JsonSchemaResolver>(); // Registers JSON schema resolver for AsyncAPI
builder.Services.AddTransient<LightMeasurementProducer>(); // Registers the producer service for DI
builder.Services.AddTransient<LightMeasurementConsumer>(); // Registers the consumer service for DI
builder.Services.AddTransient<LightMeasurementApi>(); // Registers the root API class for DI

// Register AsyncAPI document generation using service markup (code-first)
builder.Services.AddAsyncApiGeneration(options =>
    options.WithMarkupType<LightMeasurementApi>() // Use LightMeasurementApi as the root for code-first AsyncAPI generation
        // Configure AsyncAPI v2 document
        .UseDefaultV2DocumentConfiguration(asyncApi =>
        {
            // Define AMQP channel bindings (for documentation only)
            var amqpChannelBindings = new ChannelBindingDefinitionCollection();
            amqpChannelBindings.Add(new AmqpChannelBindingDefinition());

            // Define AMQP operation bindings (for documentation only)
            var amqpBinding = new AmqpOperationBindingDefinition
            {
                Cc = new Neuroglia.EquatableList<string>(new[] { "light.measured" }), // Routing key for the operation
                DeliveryMode = AmqpDeliveryMode.Persistent, // Persistent delivery mode for reliability
                Priority = 0, // Default priority
            };
            var amqpBindings = new OperationBindingDefinitionCollection();
            amqpBindings.Add(amqpBinding);

            asyncApi
                // Set terms of service and server configuration
                .WithTermsOfService(new Uri("https://www.websitepolicies.com/blog/sample-terms-service-template"))
                .WithServer("mosquitto", server => server
                    .WithUrl(new Uri("amqp://localhost:5672")) // RabbitMQ server URL
                    .WithProtocol(AsyncApiProtocol.Amqp) // Use AMQP protocol
                    .WithDescription("RabbitMQ server for light measurement events")
                    .WithSecurityRequirement("oauth2") // Require OAuth2 security at the server level
                )
                // Register channel and operation bindings for AMQP
                .WithChannelBindingComponent("amqp", amqpChannelBindings)
                .WithOperationBindingComponent("amqp", amqpBindings)
                // Register OAuth2 security scheme in components (for documentation and UI)
                .WithSecurityScheme("oauth2", scheme => scheme
                    .WithType(SecuritySchemeType.OAuth2)
                    .WithDescription("OAuth2 authentication for the API")
                    .WithParameterName("oauth2") // Parameter name for OAuth2 token
                    .WithOAuthFlows(oauth => oauth
                        .WithImplicitFlow(flow => flow
                            .WithAuthorizationUrl(new Uri("https://your-auth-server.com/token")) // Auth server URL
                            .WithTokenUrl(new Uri("https://your-auth-server.com/token")) // Token URL for OAuth2
                            .WithScopes(new Dictionary<string, string>
                            {
                                { "read:lights", "Read street lights" },
                                { "write:lights", "Write street lights" }
                            })
                        )
                    )
                );
        })
        // Configure AsyncAPI v3 document
        .UseDefaultV3DocumentConfiguration(asyncApi =>
        {
            asyncApi
                // Set terms of service and server configuration
                .WithTermsOfService(new Uri("https://www.websitepolicies.com/blog/sample-terms-service-template"))
                .WithServer("mosquitto", server => server
                    .WithHost("amqp://localhost:5672") // RabbitMQ server host
                    .WithProtocol(AsyncApiProtocol.Amqp) // Use AMQP protocol
                    .WithDescription("RabbitMQ server for light measurement events")
                    .WithSecurityRequirement(security => security.Use("#/components/securitySchemes/OAuth2")) // Require OAuth2 security at the server level
                )
                // Register channel and operation bindings for AMQP
                .WithChannelBindingsComponent("amqp", new ChannelBindingDefinitionCollection())
                .WithOperationBindingsComponent("http", bindings => bindings
                    .WithBinding(new HttpOperationBindingDefinition()
                    {
                        Method = Neuroglia.AsyncApi.Bindings.Http.HttpMethod.POST,
                        Type = HttpBindingOperationType.Request,
                        BindingVersion = "0.3.0",
                    })
                )
                .WithOperationBindingsComponent("amqp", new OperationBindingDefinitionCollection())
                // Register OAuth2 security scheme in components (for documentation and UI)
                .WithSecuritySchemeComponent("OAuth2", scheme => scheme
                    .WithType(SecuritySchemeType.OAuth2)
                    .WithOAuthFlows(oauth => oauth
                        .WithImplicitFlow(flow => flow
                            .WithAuthorizationUrl(new Uri("https://your-auth-server.com/token")) // Auth server URL
                            .WithScopes(new Dictionary<string, string>
                            {
                                { "read:lights", "Read street lights" },
                                { "write:lights", "Write street lights" }
                            })
                        )
                    )
                )
                .WithSecuritySchemeComponent("X509", scheme => scheme
                    .WithType(SecuritySchemeType.X509)
                    .WithDescription("X.509 certificate authentication for secure communication")
                )
                .WithOperation("publishLightMeasured", op => op
                    .WithChannel("light.measured") // Channel name for publishing
                    .WithSecurity(security => security.Use("#/components/securitySchemes/X509"))
                )
                ;
        })
);

builder.Services.AddHostedService<AsyncApiYamlExporter>();

// Build and configure the HTTP request pipeline
var app = builder.Build();
app.UseStaticFiles(); // Serve static files (e.g., AsyncAPI UI, CSS, JS)
app.UseRouting(); // Enable endpoint routing
app.UseAuthorization(); // Enable authorization middleware
app.MapAsyncApiDocuments(); // Map AsyncAPI document endpoints
app.MapRazorPages(); // Map Razor Pages endpoints

app.Run(); // Start the application
