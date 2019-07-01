using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Osca.Models;
using Osca.Models.Misc;
using Osca.Services.Database;
using SQLite;

namespace Osca.Services.Statistics
{
    /*
Einige lustige Datenbankabfragen die man jetzt machen kann

-- Wochenstunden pro Semester
SELECT SemesterName, SUM(HoursPerWeek)
FROM StudentEvent
GROUP BY SemesterID

-- gibt alle Master Semester zurück
SELECT DISTINCT SemesterID, SemesterName
FROM StudentEvent
WHERE CourseNumber LIKE '%m%'

-- wie oft man entschuldigt war
SELECT Status, COUNT(GradeDescription) as Count
FROM StudentExam 
WHERE StatusSystem IS '4'

-- wie oft man durchgefallen ist
SELECT GradeDescription, COUNT(GradeDescription) as Count
FROM StudentExam 
WHERE StatusSystem IS '3'

-- bachelor notenanzahl
SELECT GradeAsNumber as GradeDescription, COUNT(GradeAsNumber) as Count
FROM StudentExam 
LEFT JOIN Semester ON StudentExam.SemesterID = Semester.Id
WHERE GradeAsNumber IS NOT NULL
AND Semester.Bachelor = 1
GROUP BY GradeAsNumber


-- noten und datum
SELECT GradeAsNumber as GradeDescription, Enddate
FROM StudentExam 
LEFT JOIN Semester ON StudentExam.SemesterID = Semester.Id
WHERE GradeAsNumber IS NOT NULL
AND Semester.Bachelor = 1
ORDER BY Enddate ASC

-- Durchschnittsnoten nach Semester, nicht nach Prüfungsordnung gewichte
SELECT AVG(GradeAsNumber), Semester.Id, Semester.Name
FROM StudentExam 
LEFT JOIN Semester ON StudentExam.SemesterID = Semester.Id
WHERE GradeAsNumber IS NOT NULL
AND Semester.Bachelor = 1
AND StudentExam.StatusSystem IS '1'
GROUP BY Semester.Id
*/
    public class StatisticsService
    {
        private readonly SQLiteAsyncConnection _connection;

        public StatisticsService()
        {
            _connection = DatabaseService.Instance.GetConnection();
        }

        /// <summary>
        /// Gibt die ungewichtete Durchschnittsnote an.
        /// Unbestandene und entschuldigte Kurse werden nicht mit einberechnet.
        /// Parameter gibt an ob Bachelor oder Master
        /// </summary>
        /// <returns>The average grade.</returns>
        public async Task<float> GetAverageGrade(UniDegreeType uniDegreeType)
        {
            var degreeRestriction = GetRestriction(uniDegreeType);

            var query = $@"
SELECT AVG(GradeAsNumber)
FROM StudentExam 
LEFT JOIN Semester ON StudentExam.SemesterID = Semester.Id
WHERE GradeAsNumber IS NOT NULL
{degreeRestriction}
AND StudentExam.StatusSystem IS '1'
";
            try
            {
                return await _connection.ExecuteScalarAsync<float>(query);
            }
            catch (NullReferenceException)
            {
                // Wird geworfen wenn es noch keine Noten gibt
                return 0f;
            }
        }

        /// <summary>
        /// Gibt an welche Notengruppen (also sehr gut umfasst 1.0 und 1.3) wie oft geschrieben worden sind.
        /// </summary>
        /// <returns>The grade info.</returns>
        public Task<List<Grade>> GetGradeInfo(UniDegreeType uniDegreeType)
        {
            var degreeRestriction = GetRestriction(uniDegreeType);

            var query = $@"
SELECT GradeDescription, COUNT(GradeDescription) as Count
FROM StudentExam 
LEFT JOIN Semester ON StudentExam.SemesterID = Semester.Id
WHERE GradeAsNumber IS NOT NULL
{degreeRestriction}
GROUP BY GradeDescription
";
            return _connection.QueryAsync<Grade>(query);
        }

        /// <summary>
        /// Gibt an welche Noten wie oft geschrieben worden sind.
        /// </summary>
        /// <returns>The grade info.</returns>
        public Task<List<Grade>> GetMoreDetailedGradeInfo(UniDegreeType uniDegreeType)
        {
            var degreeRestriction = GetRestriction(uniDegreeType);

            var query = $@"
SELECT GradeAsNumber as GradeDescription, COUNT(GradeAsNumber) as Count, Enddate
FROM StudentExam 
WHERE GradeAsNumber IS NOT NULL
{degreeRestriction}
GROUP BY GradeAsNumber
";
            return _connection.QueryAsync<Grade>(query);
        }

        /// <summary>
        /// Liste aller Noten im Bachelor
        /// </summary>
        /// <returns>The grade info.</returns>
        public Task<List<Grade>> GetGrades(UniDegreeType uniDegreeType)
        {
            var degreeRestriction = GetRestriction(uniDegreeType);

            var query = $@"
SELECT GradeAsNumber as GradeDescription, Enddate
FROM StudentExam 
LEFT JOIN Semester ON StudentExam.SemesterID = Semester.Id
WHERE GradeAsNumber IS NOT NULL
{degreeRestriction}
ORDER BY Enddate ASC
";
            return _connection.QueryAsync<Grade>(query);
        }

        public Task<List<KeyValue>> GetHoursPerWeekPerSemester(UniDegreeType uniDegreeType)
        {
            var degreeRestriction = "";

            switch (uniDegreeType)
            {
                case UniDegreeType.Bachelor:
                    degreeRestriction = "WHERE Semester.Bachelor = 1";
                    break;
                case UniDegreeType.Master:
                    degreeRestriction = "WHERE Semester.Master = 1";
                    break;
                case UniDegreeType.Both:
                    degreeRestriction = "";
                    break;
            }

            var query = $@"
SELECT SemesterName as Key, SUM(HoursPerWeek) as Value
FROM StudentEvent
LEFT JOIN Semester ON StudentEvent.SemesterID = Semester.Id
{degreeRestriction}
GROUP BY SemesterID			
";
            return _connection.QueryAsync<KeyValue>(query);
        }

        public Task<List<KeyValue>> GetAvgGradePerSemester(UniDegreeType uniDegreeType)
        {
            var degreeRestriction = GetRestriction(uniDegreeType);

            var query = $@"
SELECT AVG(GradeAsNumber) as Value, Semester.Id, Semester.Name as Key
FROM StudentExam 
LEFT JOIN Semester ON StudentExam.SemesterID = Semester.Id
WHERE GradeAsNumber IS NOT NULL
{degreeRestriction}
AND StudentExam.StatusSystem IS '1'
GROUP BY Semester.Id
ORDER BY Id ASC		
";
            return _connection.QueryAsync<KeyValue>(query);
        }

        private string GetRestriction(UniDegreeType uniDegreeType)
        {
            var degreeRestriction = "";

            switch (uniDegreeType)
            {
                case UniDegreeType.Bachelor:
                    degreeRestriction = "AND Semester.Bachelor = 1";
                    break;
                case UniDegreeType.Master:
                    degreeRestriction = "AND Semester.Master = 1";
                    break;
                case UniDegreeType.Both:
                    degreeRestriction = "";
                    break;
            }
            return degreeRestriction;
        }
    }
}
