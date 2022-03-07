using MovieLibrary.Model;

namespace MovieLibrary
{
    public interface IRepository
    {
        string Write();
        void Read(string json);
    }
}