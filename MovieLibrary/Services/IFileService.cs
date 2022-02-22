namespace MovieLibrary.Services;

public interface IFileService
{
    void Read(int pageSize);
    void Write();
    void ShowMovieList(int pageNum);

    int PageCount { get; set; }
}