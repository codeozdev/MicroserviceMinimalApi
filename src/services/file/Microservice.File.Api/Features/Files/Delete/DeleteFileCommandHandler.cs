using MediatR;
using Microsoft.Extensions.FileProviders;
using Shared;

namespace Microservice.File.Api.Features.Files.Delete;

public class DeleteFileCommandHandler(IFileProvider fileProvider) : IRequestHandler<DeleteFileCommand, ServiceResult>
{
    public Task<ServiceResult> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        // Silinecek dosyanın fiziksel yol bilgisi alınır
        // Örn: wwwroot/files/dosya.jpg gibi
        var fileInfo = fileProvider.GetFileInfo(Path.Combine("files", request.FileName));

        // Dosya mevcut değilse 404 Not Found hatası döndürülür
        if (!fileInfo.Exists)
        {
            return Task.FromResult(ServiceResult.ErrorAsNotFound());
        }

        // Dosya varsa fiziksel olarak silinir
        System.IO.File.Delete(fileInfo.PhysicalPath!);

        // Silme işlemi başarılıysa 204 No Content döndürülür
        return Task.FromResult(ServiceResult.SuccessAsNoContent());
    }
}