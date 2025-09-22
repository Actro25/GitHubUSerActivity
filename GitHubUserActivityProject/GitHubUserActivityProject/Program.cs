using System;
using System.Text;
using System.Text.Json;
using System.Net.Http;
class Program
{
    static async Task Main(string[] args)
    {
        string name = "";
        while (name != "exit")
        {
            name = Console.ReadLine();
            if (!String.IsNullOrWhiteSpace(name))
            {
                await GItResp(name);
            }
        }


    }

    private static async Task GItResp(string name)
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://api.github.com");
        client.DefaultRequestHeaders.Add("User-Agent", "CSharpApp");
        HttpResponseMessage response = await client.GetAsync($"users/{name}/events");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<JsonElement>(content);
            foreach (var item in data.EnumerateArray())
            {
                var temp = item.GetProperty("type").GetString();
                switch (temp)
                {
                    case "PushEvent":
                        Console.WriteLine($"- Pushed commit to {item.GetProperty("repo").GetProperty("name").GetString()}");
                        break;
                    case "CreateEvent":
                        Console.WriteLine($"- Create in {item.GetProperty("repo").GetProperty("name").GetString()}");
                        break;
                    case "IssueCommentEvent":
                        Console.WriteLine($"- Comment issue in {item.GetProperty("repo").GetProperty("name").GetString()}");
                        break;
                    case "IssuesEvent":
                        Console.WriteLine($"- Create issue in {item.GetProperty("repo").GetProperty("name").GetString()}");
                        break;
                }
            }
        }
        Console.ReadKey();
        Console.Clear();
    }
}