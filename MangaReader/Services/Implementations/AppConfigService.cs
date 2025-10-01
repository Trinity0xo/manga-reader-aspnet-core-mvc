namespace MangaReader.Services.Implementations
{
    public class AppConfigService : IAppConfigService
    {
        private readonly IConfiguration _config;

        public AppConfigService(IConfiguration config)
        {
            _config = config;
        }

        public string? GetBasePath()
        {
            return _config["FileSettings:BasePath"];
        }

        public string? GetMangasFolderName()
        {
            return _config["FileSettings:Folders:Mangas"];
        }

        public string? GetCoversFolderName()
        {
            return _config["FileSettings:Folders:Covers"];
        }

        public string? GetChaptersFolderName()
        {
            return _config["FileSettings:Folders:Chapters"];
        }

        public string? GetAvatarsFolderName()
        {
            return _config["FileSettings:Folders:Avatars"];
        }

        public string? GetMailUsername()
        {
            return _config["Mail:Username"];
        }

        public string? GetMailPassword()
        {
            return _config["Mail:Password"];
        }

        public string? GetMailHost()
        {
            return _config["Mail:Host"];
        }

        public string? GetMailPort()
        {
            return _config["Mail:Port"];
        }
    }
}
