using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Osca.Models.Osca
{
	public class FileLink
	{
		[JsonProperty("__deferred")]
		public FileUrl FileUrl { get; set; }
	}

	public class FileUrl
	{
		[JsonProperty("uri")]
		public string Url { get; set; }
	}

	public class FileResult
	{
		public FileLink File { get; set; }
		/// <summary>
		/// Laut https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.filesystemobjecttype.aspx
		/// 0 wenn es eine Datei ist
		/// </summary>
		/// <value>The type of the file system object.</value>
		public int FileSystemObjectType { get; set; }
		public int Id { get; set; }
		public int ID { get; set; }
		public string ContentTypeId { get; set; }
		public DateTime Created { get; set; }
		public int AuthorId { get; set; }
		public DateTime Modified { get; set; }
		public int EditorId { get; set; }
		public object OData__CopySource { get; set; }
		public object CheckoutUserId { get; set; }
		public string OData__UIVersionString { get; set; }
		public string GUID { get; set; }
		public object Title { get; set; }
	}

	public class FileList
	{
		[JsonProperty("results")]
		public List<FileResult> Results { get; set; }
	}

	public class FileListRootObject
	{
		[JsonProperty("d")]
		public FileList FileList { get; set; }
	}
}
