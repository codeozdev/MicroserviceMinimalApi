using Asp.Versioning.Builder;
using Microservice.File.Api.Features.Files.Delete;
using Microservice.File.Api.Features.Files.Upload;

namespace Microservice.File.Api.Features.Files;

public static class FileEndpointExt
{
    public static void AddFileGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/files").WithTags("files").WithApiVersionSet(apiVersionSet)
            .UploadFileGroupItemEndpoint()
            .DeleteFileGroupItemEndpoint();
    }
}