using Neuroglia.AsyncApi.Generation;

/// <summary>
/// Exports AsyncAPI documents to YAML files.
/// This service is responsible for generating and exporting AsyncAPI documents in YAML format.
/// It retrieves documents from the configured provider and serializes them using the specified serializer.
/// </summary>
public class AsyncApiYamlExporter : IHostedService
{
    // Dependencies for document generation and serialization
    private readonly IAsyncApiDocumentProvider _documentProvider;
    private readonly IYamlSerializer _yamlSerializer;
    private readonly ILogger<AsyncApiYamlExporter> _logger;
    // Output directory for exported YAML files
    private readonly string _outputDirectory = Path.Combine(AppContext.BaseDirectory, "../../../asyncapi");

    // IDs and versions to export (can be customized)
    private readonly string[] _ids = { "Light Measurement API", "light-measurement-api", "Light Measurement API V3", "light-measurement-api-v3" };
    private readonly string[] _versions = { "1.0.0" };

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncApiYamlExporter"/> class.
    /// </summary>
    public AsyncApiYamlExporter(
        IAsyncApiDocumentProvider documentProvider,
        IYamlSerializer yamlSerializer,
        ILogger<AsyncApiYamlExporter> logger)
    {
        _documentProvider = documentProvider;
        _yamlSerializer = yamlSerializer;
        _logger = logger;
    }

    /// <summary>
    /// Starts the service to export AsyncAPI documents.
    /// This method is called when the application starts and iterates over the configured IDs and versions,
    /// retrieving and serializing each AsyncAPI document to YAML format.
    /// </summary>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Ensure output directory exists
        Directory.CreateDirectory(_outputDirectory);

        // Iterate over all configured IDs and versions
        foreach (var id in _ids)
        {
            foreach (var version in _versions)
            {
                try
                {
                    // Retrieve the AsyncAPI document
                    var doc = await _documentProvider.GetDocumentAsync(id, version ?? string.Empty);
                    if (doc is null)
                    {
                        _logger.LogWarning("No AsyncAPI document found for ID '{Id}' and version '{Version}'", id, version);
                        continue;
                    }

                    // Serialize the document to YAML
                    if (!TrySerializeDocument(doc, out var yaml))
                    {
                        _logger.LogWarning("Unsupported AsyncAPI document type '{DocType}' for ID '{Id}' and version '{Version}'",
                            doc.GetType().FullName, id, version);
                        continue;
                    }

                    // Build output file path
                    var fileName = $"{SanitizeFileName(id)}_{version ?? "default"}.yaml";
                    var filePath = Path.Combine(_outputDirectory, fileName);

                    // Fix YAML formatting issues (v3 uses 'availableScopes' instead of 'scopes')
                    if (doc is Neuroglia.AsyncApi.v3.V3AsyncApiDocument)
                        yaml = yaml.Replace("scopes:", "availableScopes:");

                    // Write YAML to file
                    await File.WriteAllTextAsync(filePath, yaml, cancellationToken);
                    _logger.LogInformation("AsyncAPI document saved to {FilePath}", filePath);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to export AsyncAPI document for ID '{Id}' and version '{Version}'", id, version);
                }
            }
        }
    }

    /// <summary>
    /// Stops the service gracefully.
    /// This method is called when the application is shutting down.
    /// It currently does nothing but can be extended for cleanup if needed.
    /// </summary>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    // Serializes the AsyncAPI document to YAML using the injected serializer
    private bool TrySerializeDocument(object doc, out string yaml)
    {
        yaml = string.Empty;

        using var ms = new MemoryStream();

        switch (doc)
        {
            case Neuroglia.AsyncApi.v2.V2AsyncApiDocument v2Doc:
                _logger.LogInformation("Serializing V2 AsyncAPI document: Title='{Title}', Version='{Version}', Channels={Count}",
                    v2Doc.Info?.Title, v2Doc.Info?.Version, v2Doc.Channels?.Count);
                _yamlSerializer.Serialize(v2Doc, ms, v2Doc.GetType());
                break;

            case Neuroglia.AsyncApi.v3.V3AsyncApiDocument v3Doc:
                _logger.LogInformation("Serializing V3 AsyncAPI document: Title='{Title}', Version='{Version}', Channels={Count}",
                    v3Doc.Info?.Title, v3Doc.Info?.Version, v3Doc.Channels?.Count);
                _yamlSerializer.Serialize(v3Doc, ms, v3Doc.GetType());
                break;

            default:
                return false;
        }

        ms.Position = 0;
        using var reader = new StreamReader(ms);
        yaml = reader.ReadToEnd();
        return true;
    }

    // Utility to sanitize file names for output
    private static string SanitizeFileName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "default";

        var invalidChars = Path.GetInvalidFileNameChars();
        foreach (var c in invalidChars)
        {
            name = name.Replace(c.ToString(), "");
        }

        return name;
    }
}
