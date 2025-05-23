# StreetLightsApi

## Propósito da Aplicação
Esta aplicação simula uma API de gestão de medições de luminosidade de candeeiros de rua, servindo como exemplo de integração de eventos assíncronos (event-driven) usando o padrão AsyncAPI. O objetivo é demonstrar como descrever, documentar e expor operações de publicação e consumo de eventos (por exemplo, medições de luz) sobre uma infraestrutura de mensageria (RabbitMQ/AMQP), promovendo interoperabilidade e documentação interativa.

## Bibliotecas Instaladas e Suas Finalidades
- **Neuroglia.AsyncApi.AspNetCore**: Permite a geração automática de documentação AsyncAPI (v2 e v3) a partir do código C# (code-first), expondo endpoints e UI interativa para APIs orientadas a eventos.
- **Neuroglia.AsyncApi.Bindings.Amqp**: Fornece suporte a bindings AMQP (RabbitMQ) para descrever canais, operações e mensagens de acordo com o protocolo AMQP.
- **JsonSchema.Net.Generation**: Usada para geração e manipulação de schemas JSON, essenciais para descrever os payloads dos eventos.
- **MQTTnet** (presente nas dependências): Biblioteca para comunicação MQTT, caso seja necessário suportar este protocolo.
- **Microsoft.AspNetCore.Razor**: Suporte a Razor Pages para UI e documentação interativa.

## Estrutura da Aplicação e Uso da Neuroglia
A aplicação está dividida em:
- **Services/**: Contém as classes principais de domínio, incluindo:
  - `LightMeasurementProducer`: Produtor de eventos de medições de luz.
  - `LightMeasurementConsumer`: Consumidor de eventos.
  - `LightMeasurementApi`: Classe raiz que agrega e expõe as operações da API.
  - `LightMeasuredChannel` e `LightMeasurementInfrastructure`: Definem detalhes de canais e infraestrutura de mensageria.
- **Program.cs**: Configura os serviços, gera a especificação AsyncAPI (v2 e v3) e expõe a UI.
- **Messages/**: Define os tipos de mensagens/eventos trocados.

A Neuroglia é usada para:
- Gerar a especificação AsyncAPI automaticamente a partir do código (code-first), usando tanto anotações (attributes) como configuração programática (builder).
- Expor endpoints e UI interativa para consulta da documentação e contratos da API de eventos.

## Criação da Especificação AsyncAPI

### Usando Anotações (Attributes)
- As anotações (attributes) são usadas diretamente nas classes e métodos para marcar canais, operações (publish/subscribe), mensagens e schemas. Exemplos: `[Channel]`, `[SubscribeOperation]`, `[PublishOperation]`, `[Message]`.
- Permitem que a Neuroglia reflita sobre o código e gere automaticamente partes da especificação, reduzindo a configuração manual.

### Usando o Builder
- O builder é usado em `Program.cs` para configurar detalhes avançados e customizados da especificação, como:
  - Definição de servidores (por exemplo, RabbitMQ/AMQP), incluindo URL, protocolo e descrição.
  - Configuração de bindings AMQP (RabbitMQ), detalhando exchanges, queues, delivery mode, routing keys e versões de binding, tanto ao nível de canal como de operação.
  - Definição de esquemas de segurança OAuth2, incluindo flows, scopes e requisitos de autenticação para publicação e consumo de eventos.
  - Criação de canais, operações e componentes adicionais, permitindo adicionar detalhes que não são facilmente expressos apenas com attributes, como exemplos, configurações de infraestrutura e requisitos de segurança específicos por operação.
- O builder complementa e sobrepõe a geração automática baseada em attributes, garantindo flexibilidade total e documentação precisa da infraestrutura e segurança da API.

## UI de Documentação AsyncAPI
A Neuroglia expõe uma UI interativa para consulta da especificação AsyncAPI gerada. Por padrão, pode ser acedida em:

```
http://localhost:5000/asyncapi
```

> O endereço pode variar conforme a configuração do host/porta da aplicação.

---

Esta documentação permite explorar os canais, operações, mensagens e detalhes de segurança da API, facilitando a integração e o desenvolvimento orientado a eventos.