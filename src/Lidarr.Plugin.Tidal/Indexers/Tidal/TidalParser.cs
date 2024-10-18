using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NzbDrone.Common.Http;
using NzbDrone.Core.Parser.Model;
using NzbDrone.Plugin.Tidal;
using TidalSharp.Data;

namespace NzbDrone.Core.Indexers.Tidal
{
    public class TidalParser : IParseIndexerResponse
    {
        public TidalIndexerSettings Settings { get; set; }

        public IList<ReleaseInfo> ParseResponse(IndexerResponse response)
        {
            var torrentInfos = new List<ReleaseInfo>();
            var content = new HttpResponse<TidalSearchResponse>(response.HttpResponse).Content;

            var jsonResponse = JObject.Parse(content).ToObject<TidalSearchResponse>();
            var releases = jsonResponse.AlbumResults.Items.Select(result => ProcessAlbumResult(result)).ToArray();

            foreach (var task in releases)
            {
                torrentInfos.Add(task);
            }

            foreach (var track in jsonResponse.TrackResults.Items)
            {
                // make sure the album hasn't already been processed before doing this
                if (!jsonResponse.AlbumResults.Items.Any(a => a.Id == track.Album.Id))
                {
                    var processTrackTask = ProcessTrackAlbumResultAsync(track);
                    processTrackTask.Wait();
                    torrentInfos.Add(processTrackTask.Result);
                }
            }

            return torrentInfos
                .OrderByDescending(o => o.Size)
                .ToArray();
        }

        private ReleaseInfo ProcessAlbumResult(TidalSearchResponse.Album result)
        {
            var quality = Enum.Parse<AudioQuality>(result.AudioQuality);
            return ToReleaseInfo(result, quality, 0);
        }

        private async Task<ReleaseInfo> ProcessTrackAlbumResultAsync(TidalSearchResponse.Track result)
        {
            var album = (await TidalAPI.Instance.Client.API.GetAlbum(result.Album.Id)).ToObject<TidalSearchResponse.Album>(); // track albums hold much less data so we get the full one
            var quality = Enum.Parse<AudioQuality>(album.AudioQuality);
            return ToReleaseInfo(album, quality, 0);
        }

        private static ReleaseInfo ToReleaseInfo(TidalSearchResponse.Album x, AudioQuality bitrate, long size)
        {
            var publishDate = DateTime.UtcNow;
            var year = 0;
            if (DateTime.TryParse(x.ReleaseDate, out var digitalReleaseDate))
            {
                publishDate = digitalReleaseDate;
                year = publishDate.Year;
            }

            var url = x.Url;

            var result = new ReleaseInfo
            {
                Guid = $"Tidal-{x.Id}-{bitrate}",
                Artist = x.Artists.First().Name,
                Album = x.Title,
                DownloadUrl = url,
                InfoUrl = url,
                PublishDate = publishDate,
                DownloadProtocol = nameof(TidalDownloadProtocol)
            };

            string format;
            switch (bitrate)
            {
                // TODO: no clue if this is right (or if it even matters)
                case AudioQuality.LOW:
                    result.Codec = "AAC";
                    result.Container = "96";
                    format = "M4A (AAC) 96kbps";
                    break;
                case AudioQuality.HIGH:
                    result.Codec = "AAC";
                    result.Container = "320";
                    format = "M4A (AAC) 320kbps";
                    break;
                case AudioQuality.LOSSLESS:
                    result.Codec = "FLAC";
                    result.Container = "Lossless";
                    format = "M4A (FLAC) Lossless";
                    break;
                case AudioQuality.HI_RES:
                    result.Codec = "FLAC";
                    result.Container = "Hi-Res";
                    format = "M4A (FLAC) Hi-Res";
                    break;
                case AudioQuality.HI_RES_LOSSLESS:
                    result.Codec = "FLAC";
                    result.Container = "Hi-Res Lossless";
                    format = "M4A (FLAC) Hi-Res Lossless";
                    break;
                default:
                    throw new NotImplementedException();
            }

            result.Size = size;
            result.Title = $"{x.Artists.First().Name} - {x.Title}";

            if (year > 0)
            {
                result.Title += $" ({year})";
            }

            if (x.Explicit)
            {
                result.Title += " [Explicit]";
            }

            result.Title += $" [{format}] [WEB]";

            return result;
        }
    }
}
