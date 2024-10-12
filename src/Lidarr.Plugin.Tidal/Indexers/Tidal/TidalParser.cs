using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NzbDrone.Common.Http;
using NzbDrone.Core.Download.Clients.Tidal;
using NzbDrone.Core.Parser.Model;
using System.Collections.Concurrent;
using NzbDrone.Plugin.Tidal;
using System.Globalization;

namespace NzbDrone.Core.Indexers.Tidal
{
    public class TidalParser : IParseIndexerResponse
    {
        public TidalIndexerSettings Settings { get; set; }

        public IList<ReleaseInfo> ParseResponse(IndexerResponse response)
        {
            var torrentInfos = new List<ReleaseInfo>();

            // TODO: implement this

            /*TidalSearchResponse jsonResponse = null;
            if (response.HttpRequest.Url.FullUri.Contains("method=page.get", StringComparison.InvariantCulture)) // means we're asking for a channel and need to parse it accordingly
            {
                var task = GenerateSearchResponseFromChannelData(response.Content);
                task.Wait();
                jsonResponse = task.Result;
            }
            else
                jsonResponse = new HttpResponse<TidalSearchResponseWrapper>(response.HttpResponse).Resource.Results;

            var tasks = jsonResponse.Data.Select(result => ProcessResultAsync(result)).ToArray();

            Task.WaitAll(tasks);

            foreach (var task in tasks)
            {
                if (task.Result != null)
                    torrentInfos.AddRange(task.Result);
            }*/

            return torrentInfos
                .OrderByDescending(o => o.Size)
                .ToArray();
        }

        private async Task<IList<ReleaseInfo>> ProcessResultAsync(object result) // TODO: result should be an album object, but i dont have that implemented yet obviously
        {
            var torrentInfos = new List<ReleaseInfo>();

            // TODO: process results
            await Task.Delay(100);
            /*var albumPage = await TidalAPI.Instance.Client.GWApi.GetAlbumPage(long.Parse(result.AlbumId, CultureInfo.InvariantCulture));

            var missing = albumPage["SONGS"]!["data"]!.Count(d => d["FILESIZE"]!.ToString() == "0");
            if (Settings.HideAlbumsWithMissing && missing > 0)
                return null; // return null if missing any tracks

            var size128 = albumPage["SONGS"]!["data"]!.Sum(d => d["FILESIZE_MP3_128"]!.Value<long>());
            var size320 = albumPage["SONGS"]!["data"]!.Sum(d => d["FILESIZE_MP3_320"]!.Value<long>());
            var sizeFlac = albumPage["SONGS"]!["data"]!.Sum(d => d["FILESIZE_FLAC"]!.Value<long>());

            // MP3 128
            torrentInfos.Add(ToReleaseInfo(result, 1, size128));

            // MP3 320
            if (TidalAPI.Instance.Client.GWApi.ActiveUserData["USER"]!["OPTIONS"]!["web_hq"]!.Value<bool>())
            {
                torrentInfos.Add(ToReleaseInfo(result, 3, size320));
            }

            // FLAC
            if (TidalAPI.Instance.Client.GWApi.ActiveUserData["USER"]!["OPTIONS"]!["web_lossless"]!.Value<bool>())
            {
                torrentInfos.Add(ToReleaseInfo(result, 9, sizeFlac));
            }*/

            return torrentInfos;
        }

        private static ReleaseInfo ToReleaseInfo(object x, int bitrate, long size) // TODO: x should be an album object, but i dont have that implemented yet obviously
        {
            return null;
            /*var publishDate = DateTime.UtcNow;
            var year = 0;
            if (DateTime.TryParse(x.DigitalReleaseDate, out var digitalReleaseDate))
            {
                publishDate = digitalReleaseDate;
                year = publishDate.Year;
            }
            else if (DateTime.TryParse(x.PhysicalReleaseDate, out var physicalReleaseDate))
            {
                publishDate = physicalReleaseDate;
                year = publishDate.Year;
            }

            // TODO: correct url
            var url = $"https://Tidal.com/album/{x.AlbumId}";

            var result = new ReleaseInfo
            {
                Guid = $"Tidal-{x.AlbumId}-{bitrate}",
                Artist = x.ArtistName,
                Album = x.AlbumTitle,
                DownloadUrl = url,
                InfoUrl = url,
                PublishDate = publishDate,
                DownloadProtocol = nameof(TidalDownloadProtocol)
            };

            string format;
            switch (bitrate)
            {
                case 9:
                    result.Codec = "FLAC";
                    result.Container = "Lossless";
                    format = "FLAC";
                    break;
                case 3:
                    result.Codec = "MP3";
                    result.Container = "320";
                    format = "MP3 320";
                    break;
                case 1:
                    result.Codec = "MP3";
                    result.Container = "128";
                    format = "MP3 128";
                    break;
                default:
                    throw new NotImplementedException();
            }

            result.Size = size;
            result.Title = $"{x.ArtistName} - {x.AlbumTitle}";

            if (year > 0)
            {
                result.Title += $" ({year})";
            }

            if (x.Explicit)
            {
                result.Title += " [Explicit]";
            }

            result.Title += $" [{format}] [WEB]";

            return result;*/
        }
    }
}
