namespace MangaReader.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IAppConfigService _configService;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FileService> _logger;

        public FileService(IAppConfigService configService, IWebHostEnvironment env, ILogger<FileService> logger)
        {
            _configService = configService;
            _env = env;
            _logger = logger;
        }

        private string BuildPath(params string[] paths)
        {
            return Path.Combine(paths);
        }

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                _logger.LogInformation("Created new folder: {Path}", path);
            }
        }

        private string GenerateUniqueFileName(string extension)
        {
            return $"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}{extension}";
        }

        public async Task<string?> UploadMangaCoverAsync(IFormFile file, string mangaFolderName)
        {
            try
            {
                if (file == null || file.Length == 0) return null;

                string? basePath = _configService.GetBasePath();
                string? coversFolderName = _configService.GetCoversFolderName();

                string newFileName = GenerateUniqueFileName(Path.GetExtension(file.FileName));
                string targetPath = BuildPath(_env.WebRootPath, basePath, mangaFolderName, coversFolderName);

                EnsureDirectoryExists(targetPath);

                string filePath = BuildPath(targetPath, newFileName);

                await using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                _logger.LogInformation("Cover uploaded: {FilePath}", filePath);
                return newFileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading cover");
                return null;
            }
        }

        public void DeleteMangaCover(string fileName, string mangaFolderName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName)) return;

                string? basePath = _configService.GetBasePath();
                string? coversFolderName = _configService.GetCoversFolderName();
                string filePath = BuildPath(_env.WebRootPath, basePath, mangaFolderName, coversFolderName, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    _logger.LogInformation("Cover deleted: {FilePath}", filePath);
                }
                else
                {
                    _logger.LogWarning("Cover not found: {FilePath}", filePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting cover");
            }
        }

        public async Task<List<string>?> UploadChapterPagesAsync(List<IFormFile> files, string mangaFolderName, string chapterFolderName)
        {

            try
            {
                List<string> uploadedFileNames = [];
                if (files == null || files.Count == 0) return null;

                string? basePath = _configService.GetBasePath();
                string? chaptersFolderName = _configService.GetChaptersFolderName();
                string targetPath = BuildPath(_env.WebRootPath, basePath, mangaFolderName, chaptersFolderName, chapterFolderName);

                EnsureDirectoryExists(targetPath);

                foreach (var file in files)
                {
                    if (file == null || file.Length == 0) continue;

                    string newFileName = GenerateUniqueFileName(Path.GetExtension(file.FileName));
                    string filePath = BuildPath(targetPath, newFileName);

                    await using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);

                    _logger.LogInformation("Page uploaded: {FilePath}", filePath);
                    uploadedFileNames.Add(newFileName);
                }

                return uploadedFileNames;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading cover");
                return null;
            }
        }

        public async Task<List<string>?> UpdateChapterPagesAsync(List<IFormFile> newFiles, List<string> oldFileNames,string mangaFolderName, string chapterFolderName)
        {

            try
            {
                List<string> uploadedFileNames = [];
                if (newFiles == null || newFiles.Count == 0)
                    return null;

                string? basePath = _configService.GetBasePath();
                string? chaptersFolderName = _configService.GetChaptersFolderName();
                string targetPath = BuildPath(_env.WebRootPath, basePath, mangaFolderName, chaptersFolderName, chapterFolderName);

                EnsureDirectoryExists(targetPath);

                if (oldFileNames != null && oldFileNames.Count > 0)
                {
                    foreach (var oldFile in oldFileNames)
                    {
                        string oldFilePath = BuildPath(targetPath, oldFile);
                        if (File.Exists(oldFilePath))
                        {
                            File.Delete(oldFilePath);
                            _logger.LogInformation("Old page deleted: {OldFilePath}", oldFilePath);
                        }
                    }
                }

                foreach (var file in newFiles)
                {
                    if (file == null || file.Length == 0) continue;

                    string newFileName = GenerateUniqueFileName(Path.GetExtension(file.FileName));
                    string filePath = BuildPath(targetPath, newFileName);

                    await using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);

                    _logger.LogInformation("Page uploaded: {FilePath}", filePath);
                    uploadedFileNames.Add(newFileName);
                }

                return uploadedFileNames;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating pages");
                return null;
            }
        }


        public void DeleteChapterPages(string mangaFolderName, string chapterFolderName)
        {
            try
            {
                string? basePath = _configService.GetBasePath();
                string? chaptersFolderName = _configService.GetChaptersFolderName();
                string chapterPath = BuildPath(_env.WebRootPath, basePath, mangaFolderName, chaptersFolderName, chapterFolderName);

                if (Directory.Exists(chapterPath))
                {
                    Directory.Delete(chapterPath, true);
                    _logger.LogInformation("Deleted chapter folder: {ChapterPath}", chapterPath);
                }
                else
                {
                    _logger.LogWarning("Chapter folder not found: {ChapterPath}", chapterPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting chapter");
            }
        }

        public void DeleteMangaRelativeFiles(string mangaFolderName)
        {
            try
            {
                string? basePath = _configService.GetBasePath();
                string mangaPath = BuildPath(_env.WebRootPath, basePath, mangaFolderName);
                if (Directory.Exists(mangaPath))
                {
                    Directory.Delete(mangaPath, true);
                    _logger.LogInformation("Deleted manga folder: {MangaPath}", mangaPath);
                }
                else
                {
                    _logger.LogWarning("Manga folder not found: {MangaPath}", mangaPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting manga folder");
            }
        }

        public void RenameMangaFolder(string oldMangaFolderName, string newMangaFolderName)
        {
            try
            {
                string? basePath = _configService.GetBasePath();
                var oldPath = Path.Combine(_env.WebRootPath, basePath, oldMangaFolderName);
                var newPath = Path.Combine(_env.WebRootPath, basePath, newMangaFolderName);

                if (Directory.Exists(oldPath))
                {
                    Directory.Move(oldPath, newPath);
                    _logger.LogInformation("Manga folder renamed: {OldPath} -> {NewPath}", oldPath, newPath);
                }
                else
                {
                    _logger.LogWarning("Manga folder not found: {OldPath}", oldPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when renaming manga folder");
            }
        }

        public void RenameChapterFolder(string mangaFolderName, string oldChapterFolderName, string newChapterFolderName)
        {
            try
            {
                string? basePath = _configService.GetBasePath();
                string? chaptersFolderName = _configService.GetChaptersFolderName();
                var oldPath = Path.Combine(_env.WebRootPath, basePath, mangaFolderName, chaptersFolderName, oldChapterFolderName);
                var newPath = Path.Combine(_env.WebRootPath, basePath, mangaFolderName, chaptersFolderName, newChapterFolderName);

                if (Directory.Exists(oldPath))
                {
                    Directory.Move(oldPath, newPath);
                    _logger.LogInformation("Chapter folder renamed: {OldPath} -> {NewPath}", oldPath, newPath);
                }
                else
                {
                    _logger.LogWarning("Chapter folder not found: {OldPath}", oldPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when renaming chapter folder");
            }
        }

        public async Task<string?> UploadUserAvatar(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0) return null;

                string? basePath = _configService.GetBasePath();
                string? avatarsFolderName = _configService.GetAvatarsFolderName();

                string newFileName = GenerateUniqueFileName(Path.GetExtension(file.FileName));
                string targetPath = BuildPath(_env.WebRootPath, basePath, avatarsFolderName);

                EnsureDirectoryExists(targetPath);

                string filePath = BuildPath(targetPath, newFileName);

                await using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                _logger.LogInformation("Avatar uploaded: {FilePath}", filePath);
                return newFileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading avatar");
                return null;
            }
        }

        public void DeleteUserAvatar(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName)) return;

                string? basePath = _configService.GetBasePath();
                string? avatarsFolderName = _configService.GetAvatarsFolderName();
                string filePath = BuildPath(_env.WebRootPath, basePath, avatarsFolderName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    _logger.LogInformation("Avatar deleted: {FilePath}", filePath);
                }
                else
                {
                    _logger.LogWarning("Avatar not found: {FilePath}", filePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting avatar");
            }
        }
    }
}
