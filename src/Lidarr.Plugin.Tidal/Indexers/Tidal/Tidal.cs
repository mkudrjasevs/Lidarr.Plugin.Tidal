using System;
using NLog;
using NzbDrone.Common.Cache;
using NzbDrone.Common.Http;
using NzbDrone.Core.Configuration;
using NzbDrone.Core.Download.Clients.Tidal;
using NzbDrone.Core.Parser;
using NzbDrone.Plugin.Tidal;

namespace NzbDrone.Core.Indexers.Tidal
{
    public class Tidal : HttpIndexerBase<TidalIndexerSettings>
    {
        public override string Name => "Tidal";
        public override string Protocol => nameof(TidalDownloadProtocol);
        public override bool SupportsRss => false;
        public override bool SupportsSearch => true;
        public override int PageSize => 100;
        public override TimeSpan RateLimit => new TimeSpan(0);

        private readonly ITidalProxy _TidalProxy;

        public Tidal(ITidalProxy TidalProxy,
            IHttpClient httpClient,
            IIndexerStatusService indexerStatusService,
            IConfigService configService,
            IParsingService parsingService,
            Logger logger)
            : base(httpClient, indexerStatusService, configService, parsingService, logger)
        {
            _TidalProxy = TidalProxy;
        }

        public override IIndexerRequestGenerator GetRequestGenerator()
        {
            if (string.IsNullOrEmpty(Settings.Arl))
            {
                // TODO: auto grab token
                Settings.Arl = "";
                return null;
            }

            TidalAPI.Instance?.CheckAndSetToken(Settings.Arl);

            return new TidalRequestGenerator()
            {
                Settings = Settings,
                Logger = _logger
            };
        }

        public override IParseIndexerResponse GetParser()
        {
            return new TidalParser()
            {
                Settings = Settings
            };
        }
    }
}
