using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Osca.Models.Osca;
using Osca.Services.Osca.Magic;

namespace Osca.Services.Osca
{
    /// <summary>
    /// Zuständig für Anfragen an das Osca-App-Backend
    /// </summary>
    public class OscaAppService
    {
        private static readonly string OscaUrl = "https://osca-bew.hs-osnabrueck.de/scripts/mgrqispi.dll";

        private static LoginResult AccessTokens;

        public async Task<LoginResult> Login(string username, string password)
        {
            var loginResult = new LoginResult();
            var client = new HttpClient();
            try
            {
                var response = await client.PostAsync(OscaUrl, CreateLoginContent(username, password));
                loginResult.Cookie = GetCookie(response.Headers);
                loginResult.SessionId = GetSessionId(response.Headers);
                AccessTokens = loginResult;
                return loginResult;
            }
            catch (WebException e)
            {
                // Diese Exception kommt bei einem RequestTimeOut
                Console.WriteLine(e);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public FormUrlEncodedContent CreateLoginContent(string username, string password)
        {
            var content = new Dictionary<string, string>
            {
                { "usrname", username },
                { "pass", password },
                { "menuno", "002298" },
                { "APPNAME", "CampusNet" },
                { "clino", "000000000000001" },
                { "persono", "00000000" },
                { "platform", "" },
                { "PRGNAME", "LOGINCHECK" },
                { "ARGUMENTS", "clino,usrname,pass,menuno,persno,browser,platform" },
            };

            return new FormUrlEncodedContent(content);
        }

        private string GetSessionId(HttpResponseHeaders headers)
        {
            var refreshUrl = headers.GetValues("REFRESH").FirstOrDefault();
            var regex = new Regex("ARGUMENTS=-N(.*?),");
            var result = regex.Match(refreshUrl).Value;
            result = result.Replace("ARGUMENTS=-N", "");
            result = result.Remove(result.Length - 1);
            return result;
        }

        private string GetCookie(HttpResponseHeaders headers)
        {
            var cookie = headers.GetValues("Set-cookie").FirstOrDefault();
            // Manchmal sendet der Server die Cookies mit Leerzeichen drin.
            // Aber der Server akzeptiert die Cookies nicht mit einem Leerzeichen darin.
            // => Also Leerzeichen entfernen
            return cookie.Replace(" ", "");
        }

        public async Task<List<StudentExam>> GetExams(CancellationToken cancellationToken = default)
        {
            var url = OscaUrlBuilder.ExamsUrl(AccessTokens.SessionId, OscaUrlBuilder.PersonType.Student);
            var result = await GetParsed<ExamMessage>(url, cancellationToken);
            return result.StudentExam;
        }

        public async Task<List<Message>> GetMessages(CancellationToken cancellationToken = default)
        {
            var url = OscaUrlBuilder.MessagesUrl(AccessTokens.SessionId);
            var result = await GetParsed<MessageMessage>(url, cancellationToken);
            return result.Message.OrderByDescending(m => m.Date).ToList(); ;
        }

        public async Task<string> GetMaterialsAsString(CancellationToken cancellationToken = default)
        {
            var url = OscaUrlBuilder.GetMaterialsUrl(AccessTokens.SessionId, "368793939484652", "TIMETABLE");
            var client = HttpClientWithCookie();
            var result = await client.GetAsync(url, cancellationToken);
            return await result.Content.ReadAsStringAsync();
        }

        public async Task<List<EventInfo>> GetEventInfo(StudentEvent e, CancellationToken cancellationToken = default)
        {
            var url = OscaUrlBuilder.EventInfoUrl(AccessTokens.SessionId, e.CourseID);
            var result = await GetParsed<EventInfoMessage>(url, cancellationToken);
            return result.EventInfo;
        }

        public async Task<List<Appointment>> GetAppointments(CancellationToken cancellationToken = default)
        {
            var url = OscaUrlBuilder.AppointmentsUrl(AccessTokens.SessionId);
            var result = await GetParsed<AppointmentMessage>(url, cancellationToken);
            return result.Appointment;
        }

        public async Task<List<StudentEvent>> GetEvents(CancellationToken cancellationToken = default)
        {
            var url = OscaUrlBuilder.EventsUrl(AccessTokens.SessionId, OscaUrlBuilder.PersonType.Student);
            var result = await GetParsed<EventMessage>(url, cancellationToken);
            return result.StudentEvent;
        }

        /// <summary>
        /// Gibt "STD" oder "DOZ" zurück
        /// </summary>
        /// <returns>The person type.</returns>
        public async Task<string> GetPersonType(CancellationToken cancellationToken = default)
        {
            var result = await GetParsed<PersonMessage>(OscaUrlBuilder.PersonTypeUrl(AccessTokens.SessionId), cancellationToken);
            return result?.Person.Actortype;
        }

        private async Task<T> GetParsed<T>(string url, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                var client = HttpClientWithCookie();
                var content = await client.GetAsync(url, cancellationToken);
                var result = await Parse<T>(content.Content);
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                // todo hier was sinnvolles tun
                return null;
            }
        }

        private static async Task<T> Parse<T>(HttpContent httpContent)
        {
#if false
            var downloadedString = await httpContent.ReadAsStringAsync();
            Debug.Print(downloadedString);
#endif
            using (var content = await httpContent.ReadAsStreamAsync())
            {
                var deserializer = new XmlSerializer(typeof(T));
                var result = (T)deserializer.Deserialize(content);
                return result;
            }
        }

        private HttpClient HttpClientWithCookie()
        {
            var handler = new HttpClientHandler
            {
                //UseCookies = Device.RuntimePlatform != Device.macOS // auf macOs muss das false sein
            };
            var httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("Cookie", AccessTokens.Cookie);
            return httpClient;
        }
    }

    public class LoginResult
    {
        public string Cookie { get; set; }

        public string SessionId { get; set; }

        public override string ToString() => $"Cookie:{Cookie}, SessionID:{SessionId}";
    }
}