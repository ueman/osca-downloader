namespace Osca.Models.Osca
{
	public class Semester
	{
		public string Name { get; set; }
		public string Id { get; set; }
		public bool Master { get; set; }
		public bool Bachelor { get; set; }

		public static string CreateSemesterView()
		{
			return @"
CREATE VIEW IF NOT EXISTS Semester AS
SELECT SemesterName as Name, SemesterID AS Id, SUM(Master) as Master, SUM(Bachelor) as Bachelor
FROM (
	SELECT DISTINCT SemesterName, SemesterID, CourseNumber LIKE '%M%' AS Master, CourseNumber LIKE '%B%' AS Bachelor
	FROM StudentEvent
	WHERE CourseNumber LIKE '%M%'
	OR CourseNumber LIKE '%B%'
) as temp
GROUP BY SemesterName
ORDER BY SemesterID ASC			
";
		}
	}
}
