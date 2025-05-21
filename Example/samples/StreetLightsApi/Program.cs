using System;
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

// Register core services
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddAsyncApiUI();
builder.Services.AddHttpClient();
builder.Services.AddTransient<Neuroglia.Data.Schemas.Json.IJsonSchemaResolver, Neuroglia.Data.Schemas.Json.JsonSchemaResolver>();
builder.Services.AddTransient<LightMeasurementProducer>();
builder.Services.AddTransient<LightMeasurementConsumer>();
builder.Services.AddTransient<LightMeasurementApi>();

// Register AsyncAPI document generation using service markup (code-first)
builder.Services.AddAsyncApiGeneration(options =>
    options.WithMarkupType<LightMeasurementApi>()
        .UseDefaultV2DocumentConfiguration(asyncApi =>
        {
            var amqpChannelBindings = new ChannelBindingDefinitionCollection();
            amqpChannelBindings.Add(new AmqpChannelBindingDefinition());

            var amqpBinding = new AmqpOperationBindingDefinition
            {
                Cc = new Neuroglia.EquatableList<string>(new[] { "light.measured" }),
                DeliveryMode = AmqpDeliveryMode.Persistent,
                Priority = 0,
            };
            var amqpBindings = new OperationBindingDefinitionCollection();
            amqpBindings.Add(amqpBinding);

            asyncApi
                .WithTermsOfService(new Uri("https://www.websitepolicies.com/blog/sample-terms-service-template"))
                .WithServer("mosquitto", server => server
                    .WithUrl(new Uri("amqp://localhost:5672"))
                    .WithProtocol(Neuroglia.AsyncApi.AsyncApiProtocol.Amqp)
                    .WithDescription("RabbitMQ server for light measurement events")
                    .WithSecurityRequirement("oauth2")
                )
                .WithChannelBindingComponent("amqp", amqpChannelBindings)
                .WithOperationBindingComponent("amqp", amqpBindings)
                .WithSecurityScheme("oauth2", scheme => scheme
                    .WithType(SecuritySchemeType.OAuth2)
                    .WithDescription("OAuth2 authentication for the API")
                    .WithAuthorizationScheme("Bearer")
                    .WithOAuthFlows(oauth => oauth
                        .WithClientCredentialsFlow(flow => flow
                            .WithAuthorizationUrl(new Uri("https://your-auth-server.com/token"))
                            .WithScope("api:read", "Read access to API")
                            .WithScope("api:write", "Write access to API")
                        )
                    )
                );
        })
        .UseDefaultV3DocumentConfiguration(asyncApi =>
        {
            var amqpChannelBindings = new ChannelBindingDefinitionCollection();
            amqpChannelBindings.Add(new AmqpChannelBindingDefinition
            {
                Queue = new AmqpQueueDefinition
                {
                    Name = LightMeasurementInfrastructure.QueueName,
                    Durable = LightMeasurementInfrastructure.QueueDurable,
                    AutoDelete = LightMeasurementInfrastructure.QueueAutoDelete,
                },
                Exchange = new AmqpExchangeDefinition
                {
                    Name = LightMeasurementInfrastructure.ExchangeName,
                    Type = AmqpExchangeType.Direct,
                    Durable = LightMeasurementInfrastructure.ExchangeDurable,
                    AutoDelete = LightMeasurementInfrastructure.ExchangeAutoDelete,
                },
                BindingVersion = "0.3.0"
            });

            var amqpOperationBindings = new OperationBindingDefinitionCollection();
            amqpOperationBindings.Add(new AmqpOperationBindingDefinition
            {
                BindingVersion = "0.3.0"
            });

            asyncApi
                .WithTermsOfService(new Uri("https://www.websitepolicies.com/blog/sample-terms-service-template"))
                .WithServer("mosquitto", server => server
                    .WithHost("amqp://localhost:5672")
                    .WithProtocol(AsyncApiProtocol.Amqp)
                    .WithDescription("RabbitMQ server for light measurement events")
                    .WithSecurityRequirement(security => security.Use("#/components/securitySchemes/oauth2"))
                )
                .WithChannelBindingsComponent("amqp", amqpChannelBindings)
                .WithOperationBindingsComponent("amqp", amqpOperationBindings)
                .WithSecuritySchemeComponent("oauth2", scheme => scheme
                    .WithType(SecuritySchemeType.OAuth2)
                    .WithDescription("OAuth2 authentication for the API")
                    .WithAuthorizationScheme("Bearer")
                    .WithOAuthFlows(oauth => oauth
                        .WithClientCredentialsFlow(flow => flow
                            .WithAuthorizationUrl(new Uri("https://your-auth-server.com/token"))
                            .WithScope("api:read", "Read access to API")
                            .WithScope("api:write", "Write access to API")
                        )
                    )
                );
        })
        
);

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapAsyncApiDocuments();
app.MapRazorPages();

app.Run();
