using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Osca.Models.Osca
{

	public class AnnouncementResult
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public DateTime Modified { get; set; }
		public DateTime Created { get; set; }
		/// <summary>
		/// müsste eigentlich eine UUID (GUID) sein
		/// </summary>
		public Guid GUID { get; set; }
		public string Body { get; set; }
	}

	public class AnnouncementList
	{
		[JsonProperty("results")]
		public List<AnnouncementResult> Results { get; set; }
	}

	/// <summary>
	/// https://osca.hs-osnabrueck.de/lms/{courseId}/_api/web/lists/getByTitle('Ank%C3%BCndigungen')/items
	/// mit "application/json;odata=verbose" in nem AcceptHeader
	/// </summary>
	public class AnnouncementRootObject
	{
		[JsonProperty("d")]
		public AnnouncementList ListObject { get; set; }
	}

}