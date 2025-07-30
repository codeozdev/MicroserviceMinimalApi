using Shared;

namespace Microservice.File.Api.Features.Files.Upload;

public record UploadFileCommand(IFormFile File) : IRequestByServiceResult<UploadFileCommandResponse>;

// IFormFile bir interface, HTTP isteği ile gönderilen bir dosyayı temsil eder (yerlesik bir interface'dir).