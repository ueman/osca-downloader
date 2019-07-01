using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using Osca.Models.News;
using Osca.Services.Database;
using SQLite;
using Osca.Services.FilePath;

namespace Osca.Services.RssNews
{
    public class NewsService
    {
        private static readonly HttpClient _client = new HttpClient();

        private readonly SQLiteAsyncConnection _connection;

        public NewsService()
        {
            _connection = DatabaseService.Instance.GetConnection();
        }

        public async Task<List<RssItem>> LoadNews()
        {
            try
            {
                var feed = await FeedReader.ReadAsync(NewsUrls.HochschulNews);
                return feed.Items
                           .AsParallel()
                           .Select(i => i.SpecificItem)
                           .Cast<Rss20FeedItem>()
                           .Select(ToItem)
                           .ToList();
            }
            catch (Exception e)
            {
                // TODO besseres errorhandling
                Debug.WriteLine(e);
            }
            return new List<RssItem>();
        }

        public static string GetImageStoragePath()
        {
            var path = Path.Combine(PathService.OutputPath, "images");
            Console.WriteLine($"Image storage path: {path}");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        private static RssItem ToItem(Rss20FeedItem item)
        {
            return new RssItem
            {
                Title = item.Title,
                Link = item.Link,
                Description = item.Description,
                Author = item.Author,
                Comments = item.Comments,
                ImageUrl = item.Enclosure.Url,
                Guid = item.Guid,
                PublishingDateString = item.PublishingDateString,
                PublishingDate = item.PublishingDate,
                Content = item.Content
            };
        }

        public async Task DownloadAndSaveImages(List<RssItem> rssItems)
        {
            try
            {
                var downloadTasks = rssItems.Select(item => DownloadAndSaveImage(_client, item)).ToList();

                // wir können hier parallel arbeiten,
                // da verschiedene Bilder in verschiedene Dateien geschrieben werden
                await Task.WhenAll(downloadTasks);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                // TODO besseres Fehlerhandling
                // andererseits wen interessieren diese News und die Bilder?
            }
        }

        public async Task DownloadAndSaveImage(HttpClient client, RssItem rssItem)
        {
            var response = await client.GetAsync(rssItem.ImageUrl);

            var pathToImageFile = Path.Combine(GetImageStoragePath(), rssItem.FileName);

            using (var imageStream = await response.Content.ReadAsStreamAsync())
            {
                using (var imageFile = File.Create(pathToImageFile))
                {
                    imageStream.CopyTo(imageFile);
                }
            }
        }

        public async Task<List<RssItem>> GetNewsFromDb()
        {
            var items = await _connection.Table<RssItem>()
                                   .OrderByDescending(r => r.PublishingDate)
                                   .ToListAsync();
            return items;
        }
    }
}
