using MarcketPlace.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace MarcketPlace.Application.Contracts;

public interface IFileService
{
    Task<string> Upload(IFormFile arquivo, EUploadPath uploadPath);
}