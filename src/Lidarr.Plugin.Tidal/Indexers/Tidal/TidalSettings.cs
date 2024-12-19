using FluentValidation;
using NzbDrone.Core.Annotations;
using NzbDrone.Core.Validation;
using NzbDrone.Core.Validation.Paths;
using NzbDrone.Plugin.Tidal;

namespace NzbDrone.Core.Indexers.Tidal
{
    public class TidalIndexerSettingsValidator : AbstractValidator<TidalIndexerSettings>
    {
        public TidalIndexerSettingsValidator()
        {
            RuleFor(x => x.ConfigPath).IsValidPath();
        }
    }

    public class TidalIndexerSettings : IIndexerSettings
    {
        private static readonly TidalIndexerSettingsValidator Validator = new TidalIndexerSettingsValidator();

        [FieldDefinition(0, Label = "Tidal URL", HelpText = "Use this to sign into Tidal.")]
        public string TidalUrl { get => TidalAPI.Instance?.Client?.GetPkceLoginUrl() ?? ""; set { } }

        [FieldDefinition(0, Label = "Redirect Url", Type = FieldType.Textbox)]
        public string RedirectUrl { get; set; } = "";

        [FieldDefinition(1, Label = "Config Path", Type = FieldType.Textbox, HelpText = "This is the directory where you account's information is stored so that it can be reloaded later.")]
        public string ConfigPath { get; set; } = "";

        [FieldDefinition(2, Type = FieldType.Number, Label = "API Requests Per Second", HelpText = "Limits how many requests can be sent to Tidal every second, helps to prevent rate-limiting from Tidal.")]
        public int RequestsPerSecond { get; set; } = 5;

        [FieldDefinition(3, Type = FieldType.Number, Label = "Early Download Limit", Unit = "days", HelpText = "Time before release date Lidarr will download from this indexer, empty is no limit", Advanced = true)]
        public int? EarlyReleaseLimit { get; set; }

        // this is hardcoded so this doesn't need to exist except that it's required by the interface
        public string BaseUrl { get; set; } = "";

        public NzbDroneValidationResult Validate()
        {
            // a very lazy way to update this when changed
            // it's not very expensive as all this does is set a variable
            TidalAPI.Instance?.Client?.SetRateLimit(RequestsPerSecond);

            return new NzbDroneValidationResult(Validator.Validate(this));
        }
    }
}
