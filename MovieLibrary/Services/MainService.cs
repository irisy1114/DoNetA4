
using System;
using Microsoft.Extensions.Logging;

namespace MovieLibrary.Services
{

    public class MainService : IMainService
    {
        private readonly IFileService _fileService;
        private readonly int pageSize = 10;
        private readonly IRepository _jsonRepository;
        public MainService(IFileService fileService, IRepository jsonRepository)
        {
            _fileService = fileService;
            _jsonRepository = jsonRepository;
        }
        public void Invoke()
        {
            // set page size
            _fileService.PageSize = pageSize;
            //read file first
            _fileService.Read();

            _jsonRepository.Write();

            string choice;
            do
            {
                // prompt for user choice
                Console.WriteLine("********************************");
                Console.WriteLine("1. List all movies in the file");
                Console.WriteLine("2. Add Movie");
                Console.WriteLine("Enter any other key to quit");
                Console.WriteLine("********************************");
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        // show first page by default
                        _fileService.ShowMovieList(1);
                        // list all movies efficiently with separate pages to display
                        PaginationPrompt(_fileService.PageCount);
                        break;
                    case "2":
                        _fileService.Write();
                        break;

                }
            } while (choice == "2");

        }

        private void PaginationPrompt(int pageCount)
        {
            Console.WriteLine($"Please enter page number between 1 and {pageCount} to view more. Or enter 0 to exit.");
            var userInput = Console.ReadLine();
            var pageNumber = 1;
            if (!Int32.TryParse(userInput, out pageNumber))
            {
                Console.WriteLine($"Please enter a valid number");
                PaginationPrompt(pageCount);
            }

            if (pageNumber < 0 || pageNumber > pageCount)
            {
                //Console.WriteLine($"Please enter a number between 1 and {pageCount}");
                PaginationPrompt(pageCount);
            }

            if (pageNumber == 0)
            {
                Invoke();
                return;
            }

            _fileService.ShowMovieList(pageNumber);
            PaginationPrompt(pageCount);

        }
    }
}