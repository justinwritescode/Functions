// /*
//  * AcceptContentType.cs
//  *
//  *   Created: 2022-11-11-01:18:23
//  *   Modified: 2022-11-11-01:18:24
//  *
//  *   Author: Justin Chase <justin@justinwritescode.com>
//  *
//  *   Copyright © 2022-2023 Justin Chase, All Rights Reserved
//  *      License: MIT (https://opensource.org/licenses/MIT)
//  */

// namespace JustinWritesCode.AzureFunctions;
// using MediaTypeNames = System.Net.Mime.MediaTypeNames;

// [GenerateEnumerationClass("ContentType")]
// public enum ContentTypeEnum
// {
//     [ComponentModelDisplay(Name = MediaTypeNames.Application.Octet, ShortName = "???", Description = "An unknown content type")]
//     Unknown = 0,
//     [ComponentModelDisplay(Name = MediaTypeNames.Application.Json, ShortName = "json", Description = "JSON")]
//     Json,
//     [ComponentModelDisplay(Name = $"{MediaTypeNames.Application.Json}+plain-text", ShortName = "json+plaintext", Description = "JSON with a plain-text payload")]
//     JsonPlainText,
//     [ComponentModelDisplay(Name = MediaTypeNames.Application.Xml, ShortName = "xml", Description = "XML")]
//     Xml,
//     [ComponentModelDisplay(Name = MediaTypeNames.Text.Html, ShortName = "html", Description = "HTML")]
//     Html,
//     [ComponentModelDisplay(Name = MediaTypeNames.Text.Plain, ShortName = "plain", Description = "Plain text")]
//     Text,
//     [ComponentModelDisplay(Name = MediaTypeNames.Application.Octet, ShortName = "byte[]", Description = "Binary data")]
//     Binary
// }

// public partial class AcceptContentType : ContentType
// {
//     public AcceptContentType() : base(0, "Unknown") {  }
// }

// public partial class RequestContentType : ContentType
// {
//     public RequestContentType() : base(0, "Unknown") { }
// }

// public partial class ContentType
// {
// }

// public partial struct HttpMethod
// {
//     public const string GET = "GET";
//     public const string POST = "POST";
//     public const string PUT = "PUT";
//     public const string DELETE = "DELETE";
//     public const string PATCH = "PATCH";
//     public const string OPTIONS = "OPTIONS";
//     public const string HEAD = "HEAD";
//     public const string TRACE = "TRACE";
//     public const string CONNECT = "CONNECT";
// }

// public partial class AcceptContentType
// {
//     public static implicit operator string(AcceptContentType acceptContentType) => acceptContentType.ToString();
//     public static implicit operator AcceptContentType(string strValue) => Parse<AcceptContentType>(strValue);
// }

// public partial class RequestContentType
// {
//     public static implicit operator string(RequestContentType acceptContentType) => acceptContentType.ToString();
//     public static implicit operator RequestContentType(string strValue) => Parse<RequestContentType>(strValue);
// }

// public static partial class AcceptContentTypeEnumExtensions
// {

// }
