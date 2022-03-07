using MovieLibrary.Model;
using Newtonsoft.Json;

namespace MovieLibrary
{
    public class JsonRepository : IRepository
    {
        public string Write()
        {
            Movie movie = new Movie();
            movie.Id = UInt64.Parse("1");
            movie.Title = "Toy Story (1995)";
            movie.Genres = "Adventure|Animation|Children|Comedy|Fantasy";

            string json = JsonConvert.SerializeObject(movie);

            Console.WriteLine(json);

            return json;
        }

        public void Read(string json)
        {
            Movie m = JsonConvert.DeserializeObject<Movie>(json);

            Console.WriteLine(m.Id);
            Console.WriteLine(m.Title);
            Console.WriteLine(m.Genres);
        }
    }
}