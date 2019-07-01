using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace Osca.Models.Osca
{
	public class SharepointFile
	{
		public bool Exists { get; set; }
		public string Length { get; set; }
		public int Level { get; set; }
		public int MajorVersion { get; set; }
		public int MinorVersion { get; set; }
		public string Name { get; set; }
		public string ServerRelativeUrl { get; set; }
		public DateTime TimeCreated { get; set; }
		public DateTime TimeLastModified { get; set; }
	}

	public class FileRootObject
	{
		[JsonProperty("d")]
		public SharepointFile File { get; set; }
	}
}
