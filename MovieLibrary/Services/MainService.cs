
using System;

namespace MovieLibrary.Services;

public class MainService : IMainService
{
    private readonly IFileService _fileService;

    public MainService(IFileService fileService)
    {
        _fileService = fileService;
    }
    public void Invoke()
    {
        throw new NotImplementedException();
    }
}