using MediatR;
using Microsoft.Extensions.FileProviders;
using Shared;
using System.Net;

namespace Microservice.File.Api.Features.Files.Upload;

public class UploadFileCommandHandler(IFileProvider fileProvider) : IRequestHandler<UploadFileCommand, ServiceResult<UploadFileCommandResponse>>
{
    public async Task<ServiceResult<UploadFileCommandResponse>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        // Eğer dosya boşsa (0 byte), geçersiz dosya hatası döndür
        if (request.File.Length == 0)
        {
            return ServiceResult<UploadFileCommandResponse>.Error("Invalid file", "The provided file is empty or null", HttpStatusCode.BadRequest);
        }

        // Dosyaya benzersiz bir isim ver (.jpg, .png gibi uzantı korunur)
        var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";

        // Dosyanın kaydedileceği tam yolu oluştur
        // fileProvider.GetFileInfo("files").PhysicalPath => wwwroot/files dizinini döner
        var uploadPath = Path.Combine(
            fileProvider.GetFileInfo("files").PhysicalPath!,
            newFileName
        );

        // Upload path'e bir dosya oluştur ve kullanıcının gönderdiği verileri bu dosyaya yaz
        await using var stream = new FileStream(uploadPath, FileMode.Create);
        await request.File.CopyToAsync(stream, cancellationToken); // kullanici yuklerken tarayici kapatirsa cancellationToken devreye girer ve buradaki operasyonu anlik olarak durdurur


        // Cevap için DTO oluştur (yeni dosya adı, fiziksel yol, orijinal ad)
        var response = new UploadFileCommandResponse(
            newFileName,
            $"files/{newFileName}",
            request.File.FileName
        );

        return ServiceResult<UploadFileCommandResponse>.SuccessAsCreated(response, response.FilePath);
    }
}


// using neden kullaniyoruz? kendi islemi bittikten sonra dispose olacak neden onemli bu eger 5gb bir dosya kaydediyorsak using kullanmadigimiz durumda bu bellekte yer kaplar garbage collector bunu silene kadar bekler ve bu da performans sorununa neden olur. Bu nedenle using kullaniyoruz
// memoryde fazla yer kaplayan seyler varsa using kullanabiliriz