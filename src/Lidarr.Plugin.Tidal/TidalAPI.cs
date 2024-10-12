using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Linq;

namespace NzbDrone.Plugin.Tidal
{
    public class TidalAPI
    {
        public static TidalAPI Instance { get; private set; } = new("");

        internal TidalAPI(string arl)
        {
            Instance = this;
            _client = null;
            CheckAndSetToken(arl);
        }

        // TODO: dynamic is temporary while things are set up
        public dynamic Client => _client;

        private dynamic _client;
        private string _apiToken => _client.GWApi.ActiveUserData["checkForm"]?.ToString() ?? "null";

        internal bool CheckAndSetToken(string token)
        {
            // TODO: set token

            return true;
        }
    }
}
