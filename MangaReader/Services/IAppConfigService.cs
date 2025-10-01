namespace MangaReader.Services
{
    public interface IAppConfigService
    {
        string? GetBasePath();
        string? GetCoversFolderName();
        string? GetMangasFolderName();
        string? GetChaptersFolderName();
        string? GetAvatarsFolderName();
        string? GetMailUsername();
        string? GetMailPassword();
        string? GetMailHost();
        string? GetMailPort();
    }
}
