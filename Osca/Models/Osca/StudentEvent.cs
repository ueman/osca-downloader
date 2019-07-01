using System.Collections.Generic;
using System.Xml.Serialization;
using SQLite;

namespace Osca.Models.Osca
{
	[XmlRoot(ElementName = "studentEvent", Namespace = "http://datenlotsen.de")]
	public class StudentEvent
	{
		/// <summary>
		/// Angeblich die Kurs-Id, ist aber nicht eindeutig
		/// </summary>
		/// <value>The course identifier.</value>
		[XmlElement(ElementName = "courseID", Namespace = "http://datenlotsen.de")]
		public string CourseID { get; set; }

		/// <summary>
		/// Das hier ist zumindest unique im gegensatz zu <see cref="CourseID"/>
		/// </summary>
		/// <value>The course data identifier.</value>
		[PrimaryKey]
		[XmlElement(ElementName = "courseDataID", Namespace = "http://datenlotsen.de")]
		public string CourseDataID { get; set; }

		[XmlElement(ElementName = "courseNumber", Namespace = "http://datenlotsen.de")]
		public string CourseNumber { get; set; }

		[XmlElement(ElementName = "courseName", Namespace = "http://datenlotsen.de")]
		public string CourseName { get; set; }

		[XmlElement(ElementName = "eventType", Namespace = "http://datenlotsen.de")]
		public string EventType { get; set; }

		[XmlElement(ElementName = "eventCategory", Namespace = "http://datenlotsen.de")]
		public string EventCategory { get; set; }

		[XmlElement(ElementName = "semesterID", Namespace = "http://datenlotsen.de")]
		public string SemesterID { get; set; }

		[XmlElement(ElementName = "semesterName", Namespace = "http://datenlotsen.de")]
		public string SemesterName { get; set; }

		[XmlElement(ElementName = "creditPoints", Namespace = "http://datenlotsen.de")]
		public string CreditPoints { get; set; }

		[XmlElement(ElementName = "hoursPerWeek", Namespace = "http://datenlotsen.de")]
		public string HoursPerWeek { get; set; }

		[XmlElement(ElementName = "smallGroups", Namespace = "http://datenlotsen.de")]
		public string SmallGroups { get; set; }

		[XmlElement(ElementName = "courseLanguage", Namespace = "http://datenlotsen.de")]
		public string CourseLanguage { get; set; }

		[XmlElement(ElementName = "facultyName", Namespace = "http://datenlotsen.de")]
		public string FacultyName { get; set; }

		[XmlElement(ElementName = "maxStudents", Namespace = "http://datenlotsen.de")]
		public string MaxStudents { get; set; }

		[XmlElement(ElementName = "instructorsString", Namespace = "http://datenlotsen.de")]
		public string InstructorsString { get; set; }

		[XmlElement(ElementName = "moduleName", Namespace = "http://datenlotsen.de")]
		public string ModuleName { get; set; }

		[XmlElement(ElementName = "moduleNumber", Namespace = "http://datenlotsen.de")]
		public string ModuleNumber { get; set; }

		[XmlElement(ElementName = "listener", Namespace = "http://datenlotsen.de")]
		public string Listener { get; set; }

		[XmlElement(ElementName = "acceptedStatus", Namespace = "http://datenlotsen.de")]
		public string AcceptedStatus { get; set; }

		[XmlElement(ElementName = "materialPresent", Namespace = "http://datenlotsen.de")]
		public string MaterialPresent { get; set; }

		[XmlElement(ElementName = "infoPresent", Namespace = "http://datenlotsen.de")]
		public string InfoPresent { get; set; }
	}

	[XmlRoot(ElementName = "Message", Namespace = "http://datenlotsen.de")]
	public class EventMessage
	{
		[XmlElement(ElementName = "studentEvent", Namespace = "http://datenlotsen.de")]
		public List<StudentEvent> StudentEvent { get; set; }
		[XmlAttribute(AttributeName = "mgns1", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Mgns1 { get; set; }
	}
}
