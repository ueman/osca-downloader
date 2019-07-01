using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using SQLite;

namespace Osca.Models.Osca
{
	[XmlRoot(ElementName = "studentExam", Namespace = "http://datenlotsen.de")]
	public class StudentExam : IEquatable<StudentExam>
	{
		[PrimaryKey]
		[XmlElement(ElementName = "examID", Namespace = "http://datenlotsen.de")]
		public string ExamID { get; set; }

		/// <summary>
		/// Name der Prüfung, also Abschlussarbeit/Kolloquium/Prüfungsleistung
		/// </summary>
		/// <value>The name of the exam.</value>
		[XmlElement(ElementName = "examName", Namespace = "http://datenlotsen.de")]
		public string ExamName { get; set; }

		/// <summary>
		/// Name des Moduls
		/// </summary>
		/// <value>The context.</value>
		[XmlElement(ElementName = "context", Namespace = "http://datenlotsen.de")]
		public string Context { get; set; }

		/// <summary>
		/// Modul, Kurs oder so
		/// </summary>
		/// <value>The type of the context.</value>
		[XmlElement(ElementName = "contextType", Namespace = "http://datenlotsen.de")]
		public string ContextType { get; set; }

		[XmlElement(ElementName = "subject", Namespace = "http://datenlotsen.de")]
		public string Subject { get; set; }

		/// <summary>
		/// Im Format 20.11.2017
		/// </summary>
		/// <value>The begin date.</value>
		[XmlElement(ElementName = "beginDate", Namespace = "http://datenlotsen.de")]
		public string BeginDate { get; set; }

		/// <summary>
		/// Im Format 20.11.2017
		/// </summary>
		/// <value>The due date.</value>
		[XmlElement(ElementName = "dueDate", Namespace = "http://datenlotsen.de")]
		public string DueDate { get; set; }

		/// <summary>
		/// Im Format 12:00
		/// </summary>
		/// <value>The time from.</value>
		[XmlElement(ElementName = "timeFrom", Namespace = "http://datenlotsen.de")]
		public string TimeFrom { get; set; }

		/// <summary>
		/// Im Format 12:00
		/// </summary>
		/// <value>The time to.</value>
		[XmlElement(ElementName = "timeTo", Namespace = "http://datenlotsen.de")]
		public string TimeTo { get; set; }

		/// <summary>
		/// Vereinigt <see cref="BeginDate"/> und <see cref="TimeFrom"/> in einem DateTime
		/// </summary>
		/// <value>The start date.</value>
		[XmlIgnore]
		public DateTime? StartDate
		{
			get
			{
				try
				{
					var completeStartTime = $"{BeginDate} {TimeFrom}";
					if (string.IsNullOrWhiteSpace(completeStartTime))
					{
						return null;
					}
					var date = DateTime.ParseExact(completeStartTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
					return date;
				}
				catch
				{
					return null;
				}
			}
			// muss da sein, damit es in die Datenbank geschrieben wird
#pragma warning disable RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter
			set { }
#pragma warning restore RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter
		}


		/// <summary>
		/// Vereinigt <see cref="DueDate"/> und <see cref="TimeTo"/> in einem DateTime
		/// </summary>
		/// <value>The end date.</value>
		[XmlIgnore]
		public DateTime? EndDate
		{
			get
			{
				try
				{
					if (string.IsNullOrEmpty(DueDate))
					{
						var completeEndTime = $"{BeginDate} {TimeTo}";
						if (string.IsNullOrWhiteSpace(completeEndTime))
						{
							return null;
						}
						var date = DateTime.ParseExact(completeEndTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
						return date;
					}
					else if (string.IsNullOrWhiteSpace(TimeTo))
					{
						if (string.IsNullOrWhiteSpace(DueDate))
						{
							return null;
						}
						var date = DateTime.ParseExact(DueDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
						return date;
					}
					else
					{
						var completeEndTime = $"{DueDate} {TimeTo}";
						if (string.IsNullOrWhiteSpace(completeEndTime))
						{
							return null;
						}
						var date = DateTime.ParseExact(completeEndTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
						return date;
					}
				}
				catch
				{
					return null;
				}
			}
			// muss da sein, damit es in die Datenbank geschrieben wird
#pragma warning disable RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter
			set { }
#pragma warning restore RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter
		}

		/// <summary>
		/// Note als Zahl, zB. 1,7
		/// </summary>
		/// <value>The grade.</value>
		[XmlElement(ElementName = "grade", Namespace = "http://datenlotsen.de")]
		public string Grade { get; set; }

		public decimal? GradeAsNumber
		{
			get
			{
				if (Grade.Contains(","))
				{
					return decimal.Parse(Grade.Replace(",", "."));
				}
				return null;
			}
			// muss da sein, damit es in die Datenbank geschrieben wird
#pragma warning disable RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter
			set { }
#pragma warning restore RECS0029 // Warns about property or indexer setters and event adders or removers that do not use the value parameter
		}

		/// <summary>
		/// Ausgeschriebene Note
		/// </summary>
		/// <value>The grade description.</value>
		[XmlElement(ElementName = "gradeDescription", Namespace = "http://datenlotsen.de")]
		public string GradeDescription { get; set; }

		[XmlElement(ElementName = "instructorString", Namespace = "http://datenlotsen.de")]
		public string InstructorString { get; set; }

		/// <summary>
		/// Bestanden, nicht bestanden usw als string
		/// </summary>
		/// <value>The status.</value>
		[XmlElement(ElementName = "status", Namespace = "http://datenlotsen.de")]
		public string Status { get; set; }

		/// <summary>
		/// Typ des Status
		/// 1 = bestanden
		/// 2 = ??? (ich vermute noch nicht benotet)
		/// 3 = nicht bestanden
		/// 4 = entschuldigt
		/// </summary>
		/// <value>The status system.</value>
		[XmlElement(ElementName = "statusSystem", Namespace = "http://datenlotsen.de")]
		public int StatusSystem { get; set; }

		[XmlElement(ElementName = "semesterID", Namespace = "http://datenlotsen.de")]
		public string SemesterID { get; set; }

		/// <summary>
		/// Name des Semester, "WiSe18/19"
		/// </summary>
		/// <value>The name of the semester.</value>
		[XmlElement(ElementName = "semesterName", Namespace = "http://datenlotsen.de")]
		public string SemesterName { get; set; }

		public override bool Equals(object obj)
		{
			return Equals(obj as StudentExam);
		}

		public bool Equals(StudentExam other)
		{
			return other != null &&
				   ExamID == other.ExamID &&
				   ExamName == other.ExamName &&
				   Context == other.Context &&
				   ContextType == other.ContextType &&
				   Subject == other.Subject &&
				   BeginDate == other.BeginDate &&
				   DueDate == other.DueDate &&
				   TimeFrom == other.TimeFrom &&
				   TimeTo == other.TimeTo &&
				   Grade == other.Grade &&
				   GradeDescription == other.GradeDescription &&
				   InstructorString == other.InstructorString &&
				   Status == other.Status &&
				   StatusSystem == other.StatusSystem &&
				   SemesterID == other.SemesterID &&
				   SemesterName == other.SemesterName;
		}

		public override int GetHashCode()
		{
			var hashCode = 175354043;
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ExamID);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ExamName);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Context);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ContextType);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Subject);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BeginDate);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DueDate);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TimeFrom);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TimeTo);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Grade);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(GradeDescription);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InstructorString);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Status);
			hashCode = hashCode * -1521134295 + StatusSystem.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SemesterID);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SemesterName);
			return hashCode;
		}
	}

	[XmlRoot(ElementName = "Message", Namespace = "http://datenlotsen.de")]
	public class ExamMessage
	{
		[XmlElement(ElementName = "studentExam", Namespace = "http://datenlotsen.de")]
		public List<StudentExam> StudentExam { get; set; }
		[XmlAttribute(AttributeName = "mgns1", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Mgns1 { get; set; }
	}
}
