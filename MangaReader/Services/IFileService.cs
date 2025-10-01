namespace MangaReader.Services
{
    public interface IFileService
    {
        Task<string?> UploadMangaCoverAsync(IFormFile file, string mangaFolderName);
        void DeleteMangaCover(string fileName, string mangaFolderName);
        Task<List<string>?> UploadChapterPagesAsync(List<IFormFile> files, string mangaFolderName, string chapterFolderName);
        Task<List<string>?> UpdateChapterPagesAsync(List<IFormFile> newFiles, List<string> oldFileNames, string mangaFolderName, string chapterFolderName);
        void DeleteChapterPages(string mangaFolderName, string chapterFolderName);
        void DeleteMangaRelativeFiles(string mangaFolderName);
        void RenameMangaFolder(string oldMangaFolderName, string newMangaFolderName);
        void RenameChapterFolder(string mangaFolderName, string oldChapterFolderName, string newChapterFolderName);
        Task<string?> UploadUserAvatar(IFormFile file);
        void DeleteUserAvatar(string fileName);
    }
}
