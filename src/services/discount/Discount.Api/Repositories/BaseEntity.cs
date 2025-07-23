using MongoDB.Bson.Serialization.Attributes;

namespace Discount.Api.Repositories;

public class BaseEntity
{
    [BsonElement("id")] public Guid Id { get; set; }
}