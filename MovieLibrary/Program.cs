using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;


namespace MovieLibrary
{
    class Program
    {
        public static void Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection
                .AddLogging(x => x.AddConsole())
                .BuildServiceProvider();
            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
            logger.LogInformation("Program started");

            string filePath = $"{AppContext.BaseDirectory}/data/movies.csv";

            if (!File.Exists(filePath))
            {
                logger.LogError("File does not exist: {File}", filePath);
            }
            else
            {
                string choice;
                do
                {
                    // prompt for user choice incomplete
                    Console.WriteLine("1. List all movies in the file");
                    Console.WriteLine("2. Add Movie");
                    Console.WriteLine("Enter any other key to quit");
                   
                    // input choice
                    choice = Console.ReadLine();
                    logger.LogInformation("User choice: {choice}", choice);

                    // create parallel lists of movie details include movie ID, movie title and movie genres
                    List<UInt64> MovieIds = new List<UInt64>();
                    List<string> MovieTitles = new List<string>();
                    List<string> MovieGenres = new List<string>();

                    // read data from file and add columns to corresponding lists
                    try
                    {
                        StreamReader sr = new StreamReader(filePath);
                        sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            
                            // check quote(") first, it indicates a comma in movie title
                            int index = line.IndexOf('"');
                            if (index == -1)
                            {
                                // means no quote and no comma in the movie title
                                // separate movie id movie title and movie genre with comma(,)
                                string[] movieDetails = line.Split(',');

                                // first array contains movie id
                                MovieIds.Add(UInt64.Parse(movieDetails[0]));

                                // second array contains movie title
                                MovieTitles.Add(movieDetails[1]);

                                // third array contains movie genres, replace'|' with ','
                                MovieGenres.Add(movieDetails[2].Replace("|", ", "));
                            }
                            else
                            {
                                // quote means comma in movie title,locate the index of quote
                                // and number to movie id 
                                MovieIds.Add(UInt64.Parse(line.Substring(0, index - 1)));
                                // remove movie id and first quote from line
                                line = line.Substring(index + 1);
                                // locate the next quote
                                index = line.IndexOf('"');
                                // extract the movie title
                                MovieTitles.Add(line.Substring(0, index));
                                // remove title and last comma from the line
                                line = line.Substring(index + 2);

                                // replace '|' with ','
                                MovieGenres.Add(line.Replace("|", ", "));

                            }
                        }
                        // close file when finished
                        sr.Close();
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e.Message);
                    }
                    logger.LogInformation("Movies in file {Count}", MovieIds.Count);

                    if (choice == "1")
                    {
                        // list all movies efficiently with separate pages to display
                        for (int i = 0; i < MovieIds.Count; i++)
                        {
                            
                        }

                    }

                    else if (choice == "2")
                    {
                        // add movie, prompt user for movie title and movie genre
                        Console.WriteLine("Please enter the movie title");
                        string movieTitle = Console.ReadLine();

                        // check for duplicate movie title
                        List<string> LowerCaseMovieTitles = MovieTitles.ConvertAll(t => t.ToLower());
                        if (LowerCaseMovieTitles.Contains(movieTitle.ToLower()))
                        {
                            Console.WriteLine("The title of movie has already been entered");
                            logger.LogInformation("This is a duplicate movie title {Title}", movieTitle);
                        }
                        else
                        {
                            // generate movie ID
                            // movie id should be treated like a primary key
                            // and auto-generate the next by getting the max value
                            UInt64 movieId = MovieIds.Max() + 1;

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
                            MovieIds.Add(movieId);
                            MovieTitles.Add(movieTitle);
                            MovieGenres.Add(genresType);

                            logger.LogInformation("Movie id {Id} added", movieId);
                        }
                    }

                } while (choice == "1" || choice == "2");
            }

            logger.LogInformation("The end of the program");

        }

        public class PaginationFilter
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }

            public PaginationFilter()
            {
                this.PageNumber = 1;
                this.PageSize = 10;
            }

            public PaginationFilter(int pageNumber, int pageSize)
            {
                this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
                this.PageSize = pageSize > 10 ? 10 : pageSize;
            }
        }
    }
}

