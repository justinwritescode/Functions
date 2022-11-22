namespace Backroom.Functions.Helpers.Api.Objects.Models;
public class ObjectPropertyDbModel
{
    public int Id { get; set; }
    public string ObjectId { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
