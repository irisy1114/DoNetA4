
using System;
using Microsoft.Extensions.DependencyInjection;
using MovieLibrary.Services;


namespace MovieLibrary;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var startup = new Startup();
            var serviceProvider = startup.ConfigureServices();
            var service = serviceProvider.GetService<IMainService>();

            service?.Invoke();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
        }

    }
}


