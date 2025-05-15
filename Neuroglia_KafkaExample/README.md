# Kafka Producer/Consumer Example (.NET 8.0)

This project demonstrates a simple Kafka producer and consumer using ASP.NET Core WebAPI and Confluent.Kafka.

## Features
- **Producer**: HTTP endpoint to send messages to a Kafka topic.
- **Consumer**: Background service that listens to the same topic and logs received messages.

## Prerequisites
- .NET 8.0 SDK
- Kafka broker running locally (default: `localhost:9092`)

## Usage
1. Start your local Kafka broker.
2. Run the API:
   ```powershell
   dotnet run
   ```
3. Send a POST request to `/produce` with a JSON body:
   ```json
   { "message": "Hello Kafka!" }
   ```
4. The consumer will log received messages to the console.

## Configuration
- Kafka settings can be changed in `appsettings.json`.

## Packages
- [Confluent.Kafka](https://www.nuget.org/packages/Confluent.Kafka)

---
This is a minimal example. Extend as needed for production use.
