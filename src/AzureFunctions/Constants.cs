/* 
 * Constants.cs
 * 
 *   Created: 2022-11-18-03:40:10
 *   Modified: 2022-11-18-03:40:10
 * 
 *   Author: Justin Chase <justin@justinwritescode.com>
 *   
 *   Copyright © 2022 Justin Chase, All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */ 

namespace JustinWritesCode.AzureFunctions;

public static class Constants
{
	public static class HttpHeaderNames
	{
		public const string AcceptHeaderName = "Accept";
		public const string ContentTypeHeaderName = "Content-Type";
		public const string ContentLengthHeaderName = "Content-Length";
		public const string ContentEncodingHeaderName = "Content-Encoding";
		public const string ContentDispositionHeaderName = "Content-Disposition";
		public const string ContentLanguageHeaderName = "Content-Language";
		public const string ContentRangeHeaderName = "Content-Range";
	}
}
