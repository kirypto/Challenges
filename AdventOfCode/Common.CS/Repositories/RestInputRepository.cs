using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode.Common.Interfaces;
using Microsoft.Extensions.Logging;
using static System.Environment;
using static kirypto.AdventOfCode.Common.Services.IO.DailyProgramLogger;

namespace kirypto.AdventOfCode.Common.Repositories;

public class RestInputRepository : IInputRepository {
    private const string _SESSION_TOKEN_VAR = "AocSessionToken";
    private readonly int _day;
    private readonly string _fetchCode;
    private readonly HttpClient _httpClient;
    public RestInputRepository(int day, string fetchCode) {
        _day = day;
        _fetchCode = fetchCode;
        string sessionToken = GetEnvironmentVariable(_SESSION_TOKEN_VAR);
        if (string.IsNullOrWhiteSpace(sessionToken)) {
            throw new ArgumentException($"{_SESSION_TOKEN_VAR} environment var must be set to fetch inputs.");
        }
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Cookie", $"session={sessionToken}");
    }

    public string Fetch() {
        Logger.LogInformation($"Fetching {_fetchCode} input from website for day {_day}");

        switch (_fetchCode) {
            case "real":
                HttpResponseMessage real = _httpClient
                        .GetAsync($"https://adventofcode.com/2024/day/{_day}/input")
                        .Result;
                real.EnsureSuccessStatusCode();
                return real.Content.ReadAsStringAsync().Result;
            case "example":
                HttpResponseMessage example = _httpClient
                        .GetAsync($"https://adventofcode.com/2024/day/{_day}")
                        .Result;
                example.EnsureSuccessStatusCode();
                string result = example.Content.ReadAsStringAsync().Result;

                Match match = Regex.Match(result, @"example[^\n]+</p>[^<]+<pre><code>((.|\n)+?)</code></pre>");
                if (!match.Success) {
                    throw new ApplicationException("Regex didn't match, and you should have known better.");
                }
                return match.Groups[1].Value;
            default:
                throw new ArgumentException($"Unknown fetch code {_fetchCode}");
        }
    }
}
