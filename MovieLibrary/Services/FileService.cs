using System;
using Microsoft.Extensions.Logging;

namespace MovieLibrary.Services;

public class FileService : IFileService
{
    private readonly ILogger<IFileService> _logger;

    string filePath = $"{AppContext.BaseDirectory}/data/movies.csv";

    public List<Movie> movieList;
    public bool FileRead { get; set;}
    public int PageSize { get; set; }
    public int PageCount { get; set; }
    public FileService(ILogger<IFileService> logger)
    {
        _logger = logger;
    }

    public void Read()
    {
        if (FileRead) return;

        string filePath = $"{AppContext.BaseDirectory}/data/movies.csv";

        if (!File.Exists(filePath))
        {
            _logger.LogError("File does not exist: {File}", filePath);
        }

        // read data from file and add columns to corresponding lists
        try
        {
            movieList = new List<Movie>();
            StreamReader sr = new StreamReader(filePath);
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                var movie = new Movie();
                // check quote(") first, it indicates a comma in movie title
                int index = line.IndexOf('"');
                if (index == -1)
                {
                    // means no quote and no comma in the movie title
                    // separate movie id movie title and movie genre with comma(,)
                    string[] movieDetails = line.Split(',');

                    // first array contains movie id
                    movie.Id = UInt64.Parse(movieDetails[0]);

                    // second array contains movie title
                    movie.Title = movieDetails[1];

                    // third array contains movie genres, replace'|' with ','
                    movie.Genres = movieDetails[2].Replace("|", ", ");
                }
                else
                {
                    // quote means comma in movie title,locate the index of quote
                    // add number to movie id 
                    movie.Id = UInt64.Parse(line.Substring(0, index - 1));
                    // remove movie id and first quote from line
                    line = line.Substring(index + 1);
                    // locate the next quote
                    index = line.IndexOf('"');
                    // extract the movie title
                    movie.Title = line.Substring(0, index);
                    // remove title and last comma from the line
                    line = line.Substring(index + 2);

                    // replace '|' with ','
                    movie.Genres = line.Replace("|", ", ");

                }

                movieList.Add(movie);
                
            }
            // close file when finished
            sr.Close();
            FileRead = true;
            SetPageCount();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            movieList = null; //reset movie list
        }

       
        _logger.LogInformation("Movies in file {Count}", movieList.Count);
    }

    public void SetPageCount()
    {
        if (this.movieList == null || !this.movieList.Any()) return;

        PageCount = (int)Math.Ceiling((double)movieList.Count / PageSize);
    }

    public void ShowMovieList(int pageNum = 1)
    {
        var firstIndex = (pageNum - 1) * PageSize;
        var lastIndex = pageNum * PageSize - 1;

        if (lastIndex > movieList.Count - 1)
            lastIndex = movieList.Count - 1;

        Console.WriteLine("********************************");
        Console.WriteLine($"{movieList.Count} movies found. Total {PageCount} pages. Displaying page {pageNum}. Movie [No {firstIndex +1}] to [No {lastIndex+1}]");
        Console.WriteLine("--------------------------------");
        for (var i= firstIndex;i<= lastIndex;i++)
        {
            Console.WriteLine($"No-{i+1} {movieList[i].Title} {movieList[i].Genres}");
        }
        Console.WriteLine("--------------------------------");
    }

    public void Write()
    {
        // add movie, prompt user for movie title and movie genre
        Console.WriteLine("Please enter the movie title");
        string movieTitle = Console.ReadLine();

        // check for duplicate movie title
        //List<string> LowerCaseMovieTitles = MovieTitles.ConvertAll(t => t.ToLower());
        //if (LowerCaseMovieTitles.Contains(movieTitle.ToLower()))
        if(movieList != null && movieList.Any(x=>string.Equals(x.Title, movieTitle, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("The title of movie has already been entered");
            _logger.LogInformation("This is a duplicate movie title {Title}", movieTitle);
        }
        else
        {
            // generate movie ID
            // movie id should be treated like a primary key
            // and auto-generate the next by getting the max value
            UInt64 movieId = movieList.Any() ? movieList.Max(x => x.Id) + 1 : 1;

            //  create a list to hold genres
            List<string> genres = new List<string>();
            string genre;

            do
            {
                Console.WriteLine("Please enter genre (or q to quit)");
                genre = Console.ReadLine();

                if (genre != "q" && genre.Length > 0)
                {
                    genres.Add(genre);
                }
            } while (genre != "q");

            if (genres.Count == 0)
            {
                genres.Add("No genres listed");
            }

            // delimit genres with '|'
            string genresType = string.Join("|", genres);

            // wrap the title with quotes if there is a comma in it
            movieTitle = movieTitle.IndexOf(',') != -1 ? $"\"{movieTitle}\"" : movieTitle;
            Console.WriteLine($"{movieId},{movieTitle},{genresType}");


            StreamWriter sw = new StreamWriter(filePath, true);
            sw.WriteLine($"{movieId},{movieTitle},{genresType}");
            sw.Close();

            // add movie details to lists
            if (movieList != null)
            {
                var movie = new Movie();
                movie.Id = movieId;
                movie.Title = movieTitle;
                movie.Genres = genresType;
                movieList.Add(movie);
                SetPageCount();
            }

            _logger.LogInformation("Movie id {Id} added", movieId);
        }
    }
}