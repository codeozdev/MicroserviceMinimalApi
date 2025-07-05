using MediatR;
using Refit;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails; // ItemGroup kismina FrameworkReference kismini ekledik

namespace Shared;

// Qury veya Command kisminda daha kisa yazmak adina bunlari tanimladik  
// IRequest<ServiceResult<CreateCategoryResponse>>; boyle uzun uzun yazmak yerine IRequestByServiceResult<CreateCategoryResponse> olarak yazacagiz
public interface IRequestByServiceResult<T> : IRequest<ServiceResult<T>>; // generic versiyonu

public interface IRequestByServiceResult : IRequest<ServiceResult>;      // non-generic versiyonu


public class ServiceResult
{
    [JsonIgnore] public HttpStatusCode Status { get; set; }
    public ProblemDetails? Fail { get; set; }
    [JsonIgnore] public bool IsSuccess => Fail == null;
    [JsonIgnore] public bool IsFail => !IsSuccess;


    // Static factory methods
    public static ServiceResult SuccessAsNoContent()
    {
        return new ServiceResult
        {
            Status = HttpStatusCode.NoContent
        };
    }

    public static ServiceResult ErrorAsNotFound()
    {
        return new ServiceResult
        {
            Status = HttpStatusCode.NoContent,
            Fail = new ProblemDetails
            {
                Title = "Not Found",
                Detail = "The requested resource was not found."
            }
        };
    }

    public static ServiceResult Error(ProblemDetails problemDetails, HttpStatusCode status)
    {
        return new ServiceResult
        {
            Status = status,
            Fail = problemDetails
        };
    }

    public static ServiceResult Error(string title, string description, HttpStatusCode status)
    {
        return new ServiceResult
        {
            Status = status,
            Fail = new ProblemDetails()
            {
                Title = title,
                Detail = description,
                Status = status.GetHashCode()
            }
        };
    }

    public static ServiceResult Error(string title, HttpStatusCode status)
    {
        return new ServiceResult
        {
            Status = status,
            Fail = new ProblemDetails()
            {
                Title = title,
                Status = status.GetHashCode()
            }
        };
    }

    public static ServiceResult ErrorFromProblemDetails(ApiException exception)
    {
        if (string.IsNullOrEmpty(exception.Content))
        {
            return new ServiceResult()
            {
                Fail = new ProblemDetails()
                {
                    Title = exception.Message
                },
                Status = exception.StatusCode
            };
        }

        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(exception.Content,
            new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });


        return new ServiceResult()
        {
            Fail = problemDetails,
            Status = exception.StatusCode
        };
    }

    public static ServiceResult ErrorFromValidation(IDictionary<string, object?> errors)
    {
        return new ServiceResult
        {
            Status = HttpStatusCode.BadRequest,
            Fail = new ProblemDetails()
            {
                Title = "Validation errors occured",
                Detail = "Please check the errors property for more details",
                Extensions = errors,
                Status = HttpStatusCode.BadRequest.GetHashCode()
            }
        };
    }
}

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; set; }
    [JsonIgnore] public string? UrlAsCreated { get; set; }


    // Static factory methods (200)
    public static ServiceResult<T> SuccessAsOk(T data)
    {
        return new ServiceResult<T>
        {
            Data = data,
            Status = HttpStatusCode.OK,
        };
    }

    // Static factory methods (201)
    public static ServiceResult<T> SuccessAsCreated(T data, string url)
    {
        return new ServiceResult<T>()
        {
            Data = data,
            Status = HttpStatusCode.Created,
            UrlAsCreated = url,
        };
    }

    public new static ServiceResult<T> Error(ProblemDetails problemDetails, HttpStatusCode status)
    {
        return new ServiceResult<T>
        {
            Status = status,
            Fail = problemDetails
        };
    }

    public new static ServiceResult<T> Error(string title, string description, HttpStatusCode status)
    {
        return new ServiceResult<T>
        {
            Status = status,
            Fail = new ProblemDetails()
            {
                Title = title,
                Detail = description,
                Status = status.GetHashCode()
            }
        };
    }

    public new static ServiceResult<T> Error(string title, HttpStatusCode status)
    {
        return new ServiceResult<T>
        {
            Status = status,
            Fail = new ProblemDetails()
            {
                Title = title,
                Status = status.GetHashCode()
            }
        };

    }

    public new static ServiceResult<T> ErrorFromProblemDetails(ApiException exception)
    {
        if (string.IsNullOrEmpty(exception.Content))
        {
            return new ServiceResult<T>()
            {
                Fail = new ProblemDetails()
                {
                    Title = exception.Message
                },
                Status = exception.StatusCode
            };
        }

        // apiden gelen json içeriğini ProblemDetails nesnesine ceviriyoruz
        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(exception.Content,
            new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });


        return new ServiceResult<T>()
        {
            Fail = problemDetails,
            Status = exception.StatusCode
        };
    }

    public new static ServiceResult<T> ErrorFromValidation(IDictionary<string, object?> errors)
    {
        return new ServiceResult<T>
        {
            Status = HttpStatusCode.BadRequest,
            Fail = new ProblemDetails()
            {
                Title = "Validation errors occured",
                Detail = "Please check the errors property for more details",
                Extensions = errors,
                Status = HttpStatusCode.BadRequest.GetHashCode()
            }
        };
    }
}


/*
  Sen new ServiceResult<T>() diyerek bu sınıftan yeni bir nesne (örnek) oluşturuyorsun.
  İçine Data, Status, UrlAsCreated gibi property değerlerini set ediyorsun.
  Sonra bu nesneyi return ile metot çağrısını yapan yere geri gönderiyorsun. 
  var yeniAraba = new Araba() { Marka = "BMW" };
 */



/*
  3 farkli Error'u su sekilde kullaniyoruz

  // Sadece başlık
   var result1 = ServiceResult.Error("Not Found");
   
   // Başlık ve açıklama
   var result2 = ServiceResult.Error("Validation Error", "Name is required");
   
   // Başlık, açıklama ve özel status
   var result3 = ServiceResult.Error("Unauthorized", "Access denied", HttpStatusCode.Unauthorized);

 */