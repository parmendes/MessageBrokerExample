
    const schema = {
  "asyncapi": "2.6.0",
  "info": {
    "title": "RabbitMQ AsyncAPI Example",
    "version": "1.0.0",
    "description": "AsyncAPI definition for RabbitMQ messaging."
  },
  "servers": {
    "rabbitmq": {
      "url": "localhost:15672",
      "protocol": "amqp",
      "description": "RabbitMQ server."
    }
  },
  "channels": {
    "weather.signedup": {
      "description": "Channel for user signup events.",
      "publish": {
        "operationId": "publishUserSignedUp",
        "summary": "Publish a user signup event.",
        "message": {
          "contentType": "application/json",
          "payload": {
            "type": "object",
            "properties": {
              "userId": {
                "type": "string",
                "description": "The ID of the user.",
                "x-parser-schema-id": "<anonymous-schema-2>"
              },
              "email": {
                "type": "string",
                "description": "The email of the user.",
                "x-parser-schema-id": "<anonymous-schema-3>"
              },
              "timestamp": {
                "type": "string",
                "format": "date-time",
                "description": "The time the event occurred.",
                "x-parser-schema-id": "<anonymous-schema-4>"
              }
            },
            "x-parser-schema-id": "<anonymous-schema-1>"
          },
          "x-parser-message-name": "<anonymous-message-1>"
        }
      }
    }
  },
  "x-parser-spec-parsed": true,
  "x-parser-api-version": 3,
  "x-parser-spec-stringified": true
};
    const config = {"show":{"sidebar":true},"sidebar":{"showOperations":"byDefault"}};
    const appRoot = document.getElementById('root');
    AsyncApiStandalone.render(
        { schema, config, }, appRoot
    );
  