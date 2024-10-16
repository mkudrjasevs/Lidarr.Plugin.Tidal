using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NLog;
using NzbDrone.Common.Http;
using NzbDrone.Core.IndexerSearch.Definitions;
using NzbDrone.Plugin.Tidal;
using TagLib.Ogg;

namespace NzbDrone.Core.Indexers.Tidal
{
    public class TidalRequestGenerator : IIndexerRequestGenerator
    {
        public TidalIndexerSettings Settings { get; set; }
        public Logger Logger { get; set; }

        public virtual IndexerPageableRequestChain GetRecentRequests()
        {
            var pageableRequests = new IndexerPageableRequestChain();

            // TODO: implement this

            /*Dictionary<string, string> data = new()
            {
                { "gateway_input", new JObject()
                    {
                        ["PAGE"] = "channels/explore",
                        ["VERSION"] = "2.3",
                        ["SUPPORT"] = new JObject()
                        {
                            ["grid"] = new JArray()
                            {
                                "channel",
                                "album"
                            },
                            ["horizontal-grid"] = new JArray()
                            {
                                "album"
                            }
                        },
                        ["LANG"] = "us"
                    }.ToString(Newtonsoft.Json.Formatting.None)
                }
            };

            var url = TidalAPI.Instance!.GetGWUrl("page.get", data);
            var req = new IndexerRequest(url, HttpAccept.Json);
            req.HttpRequest.Method = System.Net.Http.HttpMethod.Post;
            req.HttpRequest.Cookies.Add("sid", TidalAPI.Instance.Client.SID);

            pageableRequests.Add(new[]
            {
                req
            });*/

            return pageableRequests;
        }

        public IndexerPageableRequestChain GetSearchRequests(AlbumSearchCriteria searchCriteria)
        {
            var chain = new IndexerPageableRequestChain();

            chain.AddTier(GetRequests($"{searchCriteria.ArtistQuery} {searchCriteria.AlbumQuery}"));

            return chain;
        }

        public IndexerPageableRequestChain GetSearchRequests(ArtistSearchCriteria searchCriteria)
        {
            var chain = new IndexerPageableRequestChain();

            chain.AddTier(GetRequests(searchCriteria.ArtistQuery));

            return chain;
        }

        private IEnumerable<IndexerRequest> GetRequests(string searchParameters)
        {
            var data = new Dictionary<string, string>()
            {
                ["query"] = searchParameters,
                ["limit"] = "1000",
                ["types"] = "albums,tracks",
                ["offset"] = "0",
            };

            var url = TidalAPI.Instance!.GetAPIUrl("search", data);
            var req = new IndexerRequest(url, HttpAccept.Json);
            req.HttpRequest.Method = System.Net.Http.HttpMethod.Get;
            req.HttpRequest.Headers.Add("Authorization", $"{TidalAPI.Instance.Client.ActiveUser.TokenType} {TidalAPI.Instance.Client.ActiveUser.AccessToken}");
            yield return req;
        }
    }
}
