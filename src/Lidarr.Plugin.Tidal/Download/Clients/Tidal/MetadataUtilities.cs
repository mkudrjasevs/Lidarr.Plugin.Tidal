using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace NzbDrone.Core.Download.Clients.Tidal
{
    internal static class MetadataUtilities
    {
        // TODO: implement this
        /*public static string GetFilledTemplate(string template, string ext, JToken TidalPage, JToken TidalAlbumPage)
        {
            var releaseDate = DateTime.Parse(TidalPage["DATA"]!["PHYSICAL_RELEASE_DATE"]!.ToString(), CultureInfo.InvariantCulture);
            return GetFilledTemplate_Internal(template,
                TidalPage["DATA"]!["SNG_TITLE"]!.ToString(),
                TidalPage["DATA"]!["ALB_TITLE"]!.ToString(),
                TidalAlbumPage["DATA"]!["ART_NAME"]!.ToString(),
                TidalPage["DATA"]!["ART_NAME"]!.ToString(),
                TidalAlbumPage["DATA"]!["ARTISTS"]!.Select(a => a["ART_NAME"]!.ToString()).ToArray(),
                TidalPage["DATA"]!["ARTISTS"]!.Select(a => a["ART_NAME"]!.ToString()).ToArray(),
                $"{(int)TidalPage["DATA"]!["TRACK_NUMBER"]!:00}",
                TidalAlbumPage["SONGS"]!["total"]!.ToString(),
                releaseDate.Year.ToString(CultureInfo.InvariantCulture),
                ext);
        }*/
        public static string GetFilledTemplate(string template, string ext, object tidalPage, object tidalAlbumPage) { return ""; }

        private static string GetFilledTemplate_Internal(string template, string title, string album, string albumArtist, string artist, string[] albumArtists, string[] artists, string track, string trackCount, string year, string ext)
        {
            StringBuilder t = new(template);
            ReplaceC("%title%", title);
            ReplaceC("%album%", album);
            ReplaceC("%albumartist%", albumArtist);
            ReplaceC("%artist%", artist);
            ReplaceC("%albumartists%", string.Join("; ", albumArtists));
            ReplaceC("%artists%", string.Join("; ", artists));
            ReplaceC("%track%", track);
            ReplaceC("%trackcount%", trackCount);
            ReplaceC("%ext%", ext);
            ReplaceC("%year%", year);

            return t.ToString();

            void ReplaceC(string o, string r)
            {
                t.Replace(o, CleanPath(r));
            }
        }

        public static string CleanPath(string str)
        {
            var invalid = Path.GetInvalidFileNameChars();
            for (var i = 0; i < invalid.Length; i++)
            {
                var c = invalid[i];
                str = str.Replace(c, '_');
            }
            return str;
        }
    }
}
