using ScoreSaber;
using System;
using System.Net.Http;

namespace ScoreSaberCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ulong profileId = 2429129807113296;
            var api = new ScoreSaberAPI(new HttpClient());
            var task = api.GetUserData(profileId).ConfigureAwait(true);
            var userInfo = task.GetAwaiter().GetResult();
        }
    }
}
