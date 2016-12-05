using prototypeMovieAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace prototypemovieclient
{

    public static class Menu
    {
        public static string FrontEnd()
        {
            Menu.DisplayBox("Cinema Listings App", 11);

            string[] menuMain = new string[]
            {
                "[C] Local Cinema Index",
                "[M] Now Showing",
                "[F] Find Venue",
                "[S] Search By Title",
                "[G] Search By Genre",
                "[X] Quit App"
            };

            foreach (string option in menuMain)
            {
                Console.WriteLine(option);
            }

            // User button input taken as ConsoleKeyInfo object - Key property is always Upper(case) value
            Console.Write("\n\t");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Press a key to select an option: ");
            ConsoleKeyInfo choice = Console.ReadKey();
            Console.ResetColor();

            return (choice.Key.ToString());
        }

        public static string UserInput(string query)
        {
            string choice = "Go!";
            do
            {
                Console.Write("\n" + query + " ");
                choice = Console.ReadLine();
            } while (choice == "");
            return choice;
        }

        public static void DisplayBox(string title, int colour)
        {
            Console.ForegroundColor = (ConsoleColor)colour;
            Console.Write("_______________________________________\n\n\t");
            Console.WriteLine(title + "\n_______________________________________\n");
            Console.ResetColor();
        }
    }

    class Client
    {
        // GET to list all Cinemas
        static async Task GetAllCinemasAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:3692/");                             // base URL for API Controller i.e. RESTFul service

                    // add an Accept header for JSON
                    client.DefaultRequestHeaders.
                        Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // GET ../api/Cinemas
                    // get all Cinema venues
                    HttpResponseMessage response = await client.GetAsync("Cinemas/");                  // async call, await suspends until result available            
                    if (response.IsSuccessStatusCode)                                                   // 200..299
                    {
                        // read result
                        Menu.DisplayBox("Local Cinema Index", 11);

                        var venues = await response.Content.ReadAsAsync<IEnumerable<Cinema>>();
                        foreach (var v in venues)
                        {
                            Console.Write("Cinema: " + v.Name + " " + v.Website + " " + v.PhoneNumber + " ");
                            Console.WriteLine(" Screening: " + v.Movies.Title);
                        }
                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode + " " + response.ReasonPhrase);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // GET to display a single Cinema
        static async Task GetCinemaAsync(string id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:3692/");                             // base URL for API Controller i.e. RESTFul service

                    // add an Accept header for JSON
                    client.DefaultRequestHeaders.
                        Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // GET ../api/Cinemas/id
                    // get a particular Cinema
                    HttpResponseMessage response = await client.GetAsync("Cinemas/" + id);                  // async call, await suspends until result available            
                    if (response.IsSuccessStatusCode)                                                   // 200..299
                    {
                        // read result
                        Menu.DisplayBox("Cinema Details", 11);

                        var v = await response.Content.ReadAsAsync<Cinema>();

                        Console.Write("Cinema: " + v.Name + " " + v.Website + " " + v.PhoneNumber + " ");
                        Console.WriteLine(" Screening: " + v.Movies.Title);

                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode + " " + response.ReasonPhrase);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // GET to list all Movies
        static async Task GetAllMoviesAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:3692/");                             // base URL for API Controller i.e. RESTFul service

                    // add an Accept header for JSON
                    client.DefaultRequestHeaders.
                        Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // GET ../api/Movies
                    // get all movie screenings
                    HttpResponseMessage response = await client.GetAsync("Movies/");                  // async call, await suspends until result available            
                    if (response.IsSuccessStatusCode)                                                   // 200..299
                    {
                        // read result into iterable IEnumerable
                        Menu.DisplayBox("Movies Showing This Week", 6);
                        var screenings = await response.Content.ReadAsAsync<IEnumerable<Movie>>();
                        foreach (var s in screenings)
                        {
                            string cert = s.Certification.ToString().Substring("IFCO".Length);
                            string showtime = s.ShowTime.ToString().Remove(5);
                            Console.Write("Movie: " + s.Title + " " + cert + " " + showtime + " ");
                            Console.WriteLine(" Now showing at {0} Cinema{1}", s.Cinemas.Count, (s.Cinemas.Count == 1 ? "" : "s"));
                        }
                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode + " " + response.ReasonPhrase);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // GET to list all Cinemas where a Movie is Showing
        static async Task GetMovieScreenings(string id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:3692/");                             // base URL for API Controller i.e. RESTFul service

                    // add an Accept header for JSON
                    client.DefaultRequestHeaders.
                        Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // GET ../api/Movies/Screenings/id
                    // get single movie screenings
                    HttpResponseMessage response = await client.GetAsync("Movies/Screenings/" + id);                  // async call, await suspends until result available            
                    if (response.IsSuccessStatusCode)                                                   // 200..299
                    {
                        // read result into iterable IEnumerable

                        Console.WriteLine("\nThis movie is currently showing at ");
                        var venues = await response.Content.ReadAsAsync<IEnumerable<Cinema>>();
                        foreach (var v in venues)
                        {
                            Console.WriteLine(v.Name + " - book online at " + v.Website + ". Phone booking:" + v.PhoneNumber + ". Tickets " + v.TicketPrice);
                        }
                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode + " " + response.ReasonPhrase);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // GET to display a single Movie
        static async Task GetMovieAsync(string id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:3692/");                             // base URL for API Controller locally *adjust for Azure*

                    // add an Accept header for JSON
                    client.DefaultRequestHeaders.
                        Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // GET ../api/Movies/id
                    // get a particular Movie
                    HttpResponseMessage response = await client.GetAsync("Movies/" + id);                  // async call, await suspends until result available            
                    if (response.IsSuccessStatusCode)                                                   // 200..299
                    {
                        // read result
                        var s = await response.Content.ReadAsAsync<Movie>();
                        Menu.DisplayBox(s.Title, 13);
                        string cert = "Certificate: " + s.Certification.ToString().Substring("IFCO".Length);
                        string showtime = s.ShowTime.ToString().Remove(5);
                        Console.WriteLine(cert + "   Genre: " + s.Genre.ToString());
                        Console.WriteLine("\n" + s.Description + " \n");
                        Console.Write("Now showing at {0} Cinema{1}. ", s.Cinemas.Count, (s.Cinemas.Count == 1 ? "" : "s"));
                        Console.WriteLine("Program starts " + showtime + ". Running Time: " + s.RunTime + " mins.");
                        Console.WriteLine("Today's performance: " + s.MovieNow(s.Genre));
                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode + " " + response.ReasonPhrase);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            // call screenings before returning
            GetMovieScreenings(id).Wait();
        }

        // GET to find a movie by genre
        static async Task GetMoviesByGenreAsync(Genre id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:3692/");                             // base URL for API Controller *CHANGE FOR AZURE"

                    // add an Accept header for JSON
                    client.DefaultRequestHeaders.
                        Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string g = ((int)id).ToString();
                    HttpResponseMessage response = await client.GetAsync("Movies/Genre/" + g);                              
                    if (response.IsSuccessStatusCode)                                                   // 200..299
                    {
                        // read result into iterable IEnumerable
                        Menu.DisplayBox("Movies by Genre: " + id.ToString(), 6);                        // also =Enum.GetName(typeof(Genre), g) or 
                        var gens = await response.Content.ReadAsAsync<IEnumerable<Movie>>();
                        foreach (var s in gens)
                        {
                            string cert = s.Certification.ToString().Substring("IFCO".Length);
                            string showtime = s.ShowTime.ToString().Remove(5);
                            Console.Write("Movie: " + s.Title + " " + cert + " " + showtime + " ");
                            Console.WriteLine(" Now showing at {0} Cinema{1}", s.Cinemas.Count, (s.Cinemas.Count == 1 ? "" : "s"));
                        }
                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode + " " + response.ReasonPhrase);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        static void Main()
        {
            bool IsUsing = true;
            do
            {
                string input = Menu.FrontEnd();
                RunAsync(input).Wait();
                if (input == "X")
                {
                    IsUsing = false;
                }
            } while (IsUsing);
        }

        static async Task RunAsync(string path)
        {
            switch (path)
            {
                case "C":
                    Console.Clear();
                    GetAllCinemasAsync().Wait();
                    break;

                case "M":
                    Console.Clear();
                    GetAllMoviesAsync().Wait();
                    break;

                case "F":
                    Console.Clear();
                    string cSearch = Menu.UserInput("Please enter a search term for the cinema:");
                    GetCinemaAsync(cSearch).Wait();
                    break;

                case "S":
                    Console.Clear();
                    string mSearch = Menu.UserInput("Please enter a search term for the movie title:");
                    GetMovieAsync(mSearch).Wait();
                    break;

                case "G":
                    Console.Clear();
                    string gSearch = Menu.UserInput("Please enter a search term for the movie title:");
                    Genre gs = (Genre)(Int32.Parse(gSearch));                       // enum input as underlying enumerator - 
                    GetMoviesByGenreAsync(gs).Wait();                               // GET method will convert to string for uri
                    break;

                case "X":
                    Console.Clear();
                    Console.WriteLine("Quitting application...");
                    break;
            }
            Console.ReadLine();
        }
    }
}
