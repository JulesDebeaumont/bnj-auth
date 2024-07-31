using Server.Db;
using Server.Models;
using MimeTypes;

namespace Server.Services;

public class FileStorageService
{
    private readonly IConfiguration _config;
    private readonly MissionDevContext _dbContext;

    public FileStorageService(IConfiguration config, MissionDevContext dbContext)
    {
        _config = config;
        _dbContext = dbContext;
    }

    public async Task<FileStorageEditFileResponse> WriteFileToStorageAsync(IFormFile formfile, string relativePath)
    {
        var storageResponse = new FileStorageEditFileResponse();
        var filePath = Path.Combine(GetMainPathStorage(_config), relativePath);
        using (var stream = File.Create(filePath))
        {
            await formfile.CopyToAsync(stream);
        }
        storageResponse.IsSuccess = true;
        storageResponse.RelativePathFromStorage = relativePath;
        return storageResponse;
    }

    public async Task<FileStorageEditFileResponse> WriteUserFileToStorageAsync(IFormFile formfile, string userId)
    {
        var storageResponse = new FileStorageEditFileResponse();
        if (formfile.Length == 0)
        {
            storageResponse.Errors.Add("File is tempty");
            return storageResponse;
        }
        if (!UserFileOptions.PermittedExtensions.Contains(Path.GetExtension(formfile.FileName)))
        {
            storageResponse.Errors.Add("File extension is not accepted");
            return storageResponse;
        }
        if (formfile.Length > UserFileOptions.MaxFileLength)
        {
            storageResponse.Errors.Add($"File is too big ( size > {UserFileOptions.MaxFileLength})");
            return storageResponse;
        }
        var randomFilename = Path.GetRandomFileName();
        var mimeType = MimeTypeMap.GetMimeType(formfile.FileName);
        var relativePath = Path.Combine(UserFileOptions.Folder, randomFilename);
        var responseWrite = await WriteFileToStorageAsync(formfile, relativePath);
        if (!responseWrite.IsSuccess)
        {
            return responseWrite;
        }
        var userFile = new UserFile()
        {
            UserId = userId,
            Filename = Path.GetFileName(formfile.FileName),
            StorageFilename = randomFilename,
            CreatedAt = DateTime.Now,
            MimeType = mimeType
        };
        _dbContext.UserFiles.Add(userFile);
        await _dbContext.SaveChangesAsync();
        responseWrite.ResourceId = userFile.Id;
        return responseWrite;
    }

    public async Task<FileStorageEditFileResponse> WriteProjectFileToStorageAsync(IFormFile formfile, int projectId, string userId)
    {
        var storageResponse = new FileStorageEditFileResponse();
        if (formfile.Length == 0)
        {
            storageResponse.Errors.Add("File is tempty");
            return storageResponse;
        }
        if (!ProjectFileOptions.PermittedExtensions.Contains(Path.GetExtension(formfile.FileName)))
        {
            storageResponse.Errors.Add("File extension is not accepted");
            return storageResponse;
        }
        if (formfile.Length > ProjectFileOptions.MaxFileLength)
        {
            storageResponse.Errors.Add($"File is too big ( size > {ProjectFileOptions.MaxFileLength})");
            return storageResponse;
        }
        var randomFilename = Path.GetRandomFileName();
        var mimeType = MimeTypeMap.GetMimeType(formfile.FileName);
        var relativePath = Path.Combine(ProjectFileOptions.Folder, randomFilename);
        var responseWrite = await WriteFileToStorageAsync(formfile, relativePath);
        if (!responseWrite.IsSuccess)
        {
            return responseWrite;
        }
        var projectfile = new ProjectFile()
        {
            ProjectId = projectId,
            Filename = Path.GetFileName(formfile.FileName),
            StorageFilename = randomFilename,
            CreatedAt = DateTime.Now,
            MimeType = mimeType,
            UserId = userId
        };
        _dbContext.ProjectFiles.Add(projectfile);
        await _dbContext.SaveChangesAsync();
        responseWrite.ResourceId = projectfile.Id;
        return responseWrite;
    }

    public FileStorageEditFileResponse EraseFileFromStorage(string fileRelativePath)
    {
        var storageResponse = new FileStorageEditFileResponse();
        var filePath = Path.Combine(GetMainPathStorage(_config), fileRelativePath);
        if (!File.Exists(filePath))
        {
            storageResponse.Errors.Add("File does not exist");
            return storageResponse;
        }
        File.Delete(filePath);
        storageResponse.IsSuccess = true;
        return storageResponse;
    }

    public FileStorageEditFileResponse EraseUserFileFromStorage(UserFile userFile)
    {
        var relativePathInStorage = Path.Combine(UserFileOptions.Folder, userFile.StorageFilename);
        var responseEraseFile = EraseFileFromStorage(relativePathInStorage);
        if (!responseEraseFile.IsSuccess)
        {
            return responseEraseFile;
        }
        _dbContext.UserFiles.Remove(userFile);
        _dbContext.SaveChanges();
        return responseEraseFile;
    }

    public FileStorageEditFileResponse EraseProjectFileFromStorage(ProjectFile projectFile)
    {
        var relativePathInStorage = Path.Combine(ProjectFileOptions.Folder, projectFile.StorageFilename);
        var responseEraseFile = EraseFileFromStorage(relativePathInStorage);
        if (!responseEraseFile.IsSuccess)
        {
            return responseEraseFile;
        }
        _dbContext.ProjectFiles.Remove(projectFile);
        _dbContext.SaveChanges();
        return responseEraseFile;
    }

    public async Task<FileStorageGetFileResponse> GetFileFromStorageAsync(string fileRelativePath)
    {
        var storageResponse = new FileStorageGetFileResponse();
        var filePath = Path.Combine(GetMainPathStorage(_config), fileRelativePath);
        if (!File.Exists(filePath))
        {
            storageResponse.Errors.Add("File does not exist");
            return storageResponse;
        }
        storageResponse.IsSuccess = true;
        storageResponse.FileBytes = await File.ReadAllBytesAsync(filePath);
        return storageResponse;
    }

    public async Task<FileStorageGetFileResponse> GetUserFileFromStorageAsync(UserFile userfile)
    {
        var relativePathInStorage = Path.Combine(UserFileOptions.Folder, userfile.StorageFilename);
        var getFileResponse = await GetFileFromStorageAsync(relativePathInStorage);
        return getFileResponse;
    }

    public async Task<FileStorageGetFileResponse> GetProjectFileFromStorageAsync(ProjectFile projectFile)
    {
        var relativePathInStorage = Path.Combine(ProjectFileOptions.Folder, projectFile.StorageFilename);
        var getFileResponse = await GetFileFromStorageAsync(relativePathInStorage);
        return getFileResponse;
    }

    private static string GetMainPathStorage(IConfiguration config)
    {
        return config["MissionDevPathMainStorage"];
    }

    public static void EnsureStorageDirectoryAreCreated(IConfiguration config)
    {
        var storageFolders = new List<string>() {
            UserFileOptions.Folder,
            ProjectFileOptions.Folder
        };
        foreach (var folder in storageFolders)
        {
            Directory.CreateDirectory(Path.Combine(GetMainPathStorage(config), folder));
        }
    }

    private static class UserFileOptions
    {
        public readonly static string Folder = "UserFile";
        public readonly static string[] PermittedExtensions = [".pdf", ".csv", ".docx", ".json", ".png", ".jpeg"];
        public readonly static long MaxFileLength = 5L * 1024L * 1024L; // 5Mb
    }

    private static class ProjectFileOptions
    {
        public readonly static string Folder = "ProjectFile";
        public readonly static string[] PermittedExtensions = [".pdf", ".csv", ".docx", ".png", ".jpeg", ".jpg"];
        public readonly static long MaxFileLength = 5L * 1024L * 1024L; // 5Mb
    }
}
