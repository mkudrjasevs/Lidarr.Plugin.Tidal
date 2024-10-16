# Tidal for Lidarr
This plugin provides a Tidal indexer and downloader client for Lidarr.

## Installation
This requires your Lidarr setup to be using the `plugins` branch. My docker-compose is setup like the following.
```yml
  lidarr:
    image: ghcr.io/hotio/lidarr:pr-plugins
    container_name: lidarr
    environment:
      - PUID:100
      - PGID:1001
      - TZ:Etc/UTC
    volumes:
      - /path/to/config/:/config
      - /path/to/downloads/:/downloads
      - /path/to/music:/music
    ports:
      - 8686:8686
    restart: unless-stopped
```

1. In Lidarr, go to `System -> Plugins`, paste `https://github.com/TrevTV/Lidarr.Plugin.Tidal` into the GitHub URL box, and press Install.
2. Go into the Indexer settings and press Add. In the modal, choose `Tidal` (under Other at the bottom).
3. If you have a specific ARL you want to use, paste it into the box, if you don't, the plugin will automatically pick one for you. Then press Save. It will load for awhile as it performs a lot of calls to Tidal.
4. Go into the Download Client settings and press Add. In the modal, choose `Tidal` (under Other at the bottom).
5. Go into the Profile settings and find the Delay Profiles. On each (by default there is only one), click the wrench on the right and toggle Tidal on.
6. Optional: To prevent Lidarr from downloading all track files into the base artist folder rather than into their own separate album folder, go into the Media Management settings and enable Rename Tracks. You can change the formats to your liking, but it helps to let each album have their own folder.

## Licensing
All of these libraries have been merged into the final plugin assembly due to (what I believe is) a bug in Lidarr's plugin system.
- [TidalSharp](https://github.com/TrevTV/TidalSharp) is licensed under the GPL-3.0 license. See [LICENSE](https://github.com/TrevTV/TidalSharp/blob/main/LICENSE) for the full license.
- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) is licensed under the MIT license. See [LICENSE](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md) for the full license.
- [TagLibSharp](https://github.com/mono/taglib-sharp) is licensed under the LGPL-2.1 license. See [COPYING](https://github.com/mono/taglib-sharp/blob/main/COPYING) for the full license.