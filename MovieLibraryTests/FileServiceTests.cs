using System;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using MovieLibrary.Services;
using Xunit;

namespace MovieLibrary.Tests;

public class FileServiceTests
{
    private readonly FileService _service;

    public FileServiceTests()
    {
        Mock<ILogger<IFileService>> mockLogger = new();
        _service = new FileService(mockLogger.Object);


    }

    [Fact]
    public void FileService_SetPageCount_Zero_Success()
    {
        _service.movieList = new();
        _service.SetPageCount();
        Assert.Equal(0, _service.PageCount);
    }

    [Fact]
    public void FileService_SetPageCount_Five_Success()
    {
        _service.movieList = new();
        for (int i = 0; i < 50; i++)
        {
            Movie movie = new Movie();
            movie.Title = i.ToString();
            movie.Genres = i.ToString();
            movie.Id = (ulong) i;
            _service.movieList.Add(movie);
        }

        _service.pageSize = 10;
        _service.SetPageCount();
        Assert.Equal(5, _service.PageCount);
    }
}