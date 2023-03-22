using AutoMapper;
using Azure.Storage.Blobs;
using MarcketPlace.Application.Contracts;
using MarcketPlace.Application.Notification;
using MarcketPlace.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace MarcketPlace.Application.Services;

public class FileService : BaseService, IFileService
{
    public FileService(IMapper mapper, INotificator notificator) : base(mapper, notificator)
    {
    }

    public async Task<string> Upload(IFormFile arquivo, EUploadPath uploadPath)
    {
        var connectionString =
            "DefaultEndpointsProtocol=https;AccountName=judostorages;AccountKey=0nMIJ5xl5lRlaxwgBBazOikwZ6S8Li1BPbWGcte23dd/DURmobkYhc4yPh4qPpDPF0IwTVvaNPBM+AStEfuHaQ==;EndpointSuffix=core.windows.net";

        var fileName = GenerateNewFileName(arquivo.FileName);

        BlobContainerClient container = new BlobContainerClient(connectionString, "bob");
        BlobClient blob = container.GetBlobClient(fileName);
        await blob.UploadAsync(arquivo.OpenReadStream());

        return blob.Uri.AbsoluteUri;
    }

    private static string GenerateNewFileName(string name)
    {
        var newFileName = $"{Guid.NewGuid()}_{name}".ToLower();
        newFileName = newFileName.Replace("-", "");

        return newFileName;
    }
}