using Shared;

namespace Microservice.File.Api.Features.Files.Delete;

public record DeleteFileCommand(string FileName) : IRequestByServiceResult;
