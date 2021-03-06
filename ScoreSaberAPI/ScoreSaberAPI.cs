﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ScoreSaber
{
    public class ScoreSaberAPI
    {
        private const string Url = "https://scoresaber.com";
        private readonly HttpClient client;
        public ScoreSaberAPI(HttpClient client)
        {
            this.client = client;
        }

        public string GetProfileUrl(ulong userProfile) => Url + "/u/" + userProfile;

        public async Task<UserInfo> GetUserData(ulong userProfile)
        {
            var uri = new Uri(GetProfileUrl(userProfile));
            Console.WriteLine("GetUserData, uri: " + uri);
            var response = await client.GetAsync(uri);
            Console.WriteLine("Response: " + response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("UserData obtained!");
                var s = await response.Content.ReadAsStringAsync();
                return new UserInfo(s);
            }
            return default;
        }
    }
}