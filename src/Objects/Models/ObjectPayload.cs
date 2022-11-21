// 
// ObjectPayload.cs
// 
//   Created: 2022-11-02-01:15:56
//   Modified: 2022-11-02-01:16:38
// 
//   Author: Justin Chase <justin@justinwritescode.com>
//   
//   Copyright © 2022 Justin Chase, All Rights Reserved
//      License: MIT (https://opensource.org/licenses/MIT)
// 

namespace JustinWritesCode.Functions.Objects.Models;

public record ObjectPayload(string ObjectId, object Value) : Payloads.Payload<object>(Value)
{
    public override string ToString() => ObjectId;
    public string ObjectId {get;init;}
}
public record SetObjectPropertyValuePayload(string ObjectId, string Key, string Value, object Object) : ObjectPayload(ObjectId, Object)
{
    public string Key {get; init;}
    public string Value { get; init; }
}

public record GetObjectPropertyPayload(string ObjectId, string Key, object Object) : ObjectPayload(ObjectId, Object)
{   public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
