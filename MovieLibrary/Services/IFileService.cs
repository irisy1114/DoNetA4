namespace MovieLibrary.Services
{

    public interface IFileService
    {
        void Read();
        void Write();
        void ShowMovieList(int pageNum);
        int PageSize { get; set; }
        int PageCount { get; set; }
        bool FileRead { get; set; }
    }
}