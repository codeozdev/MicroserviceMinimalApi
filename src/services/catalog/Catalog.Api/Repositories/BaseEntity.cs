using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Api.Repositories;

public class BaseEntity
{
    [BsonElement("id")] public Guid Id { get; set; }
}