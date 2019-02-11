using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Retinalyze
{
    class Program
    {
        static void Main(string[] args)
        {
            Run().GetAwaiter().GetResult();
        }

        private static async Task Run()
        {
            Console.WriteLine("Username: ");
            var username = Console.ReadLine();
            Console.WriteLine("Password: ");
            var password = Console.ReadLine();

            using (var http = new HttpClient())
            {
                var client = new Client(http);
                var token = await client.TokenAsync(username, password);

                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

                using (var stream = new MemoryStream(File.ReadAllBytes("test.jpg")))
                {
                    var init = await client.InitializeAsync(0, new FileParameter(stream, "file.jpg", "image/jpg"), "R", "horusdec200");

                    Console.WriteLine(init.Id);
                }

                Console.WriteLine("Complete");
                Console.ReadLine();
            }
        }
    }

}
