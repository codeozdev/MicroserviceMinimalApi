namespace Discount.Api.Options;

public class MongoOption
{
    public string DatabaseName { get; set; } = null!;
    public string ConnectionString { get; set; } = null!;
}

// Database bağlamak için yardımcı bir sınıf OptionExtensions içerisinde ekledik.