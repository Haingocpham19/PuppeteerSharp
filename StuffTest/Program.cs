using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace StuffTest
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var options = new LaunchOptions
            {
                Headless = true
            };
            Console.WriteLine("Downloading chromium");
            //Download the Chromium revision if it does not already exist
            await new BrowserFetcher().DownloadAsync();

            //Console.WriteLine("Navigating google");
            using (var browser = await Puppeteer.LaunchAsync(options))
            using (var page = await browser.NewPageAsync())
            {
                Console.WriteLine("Navigating Amazon");
                await page.GoToAsync("https://www.amazon.com/s?i=specialty-aps&bbn=16225007011&rh=n%3A16225007011%2Cn%3A13896617011&ref=nav_em__nav_desktop_sa_intl_computers_tablets_0_2_6_4");


                await page.WaitForTimeoutAsync(2000);
               
                Console.WriteLine("List Product");
                int countProduct = await page.EvaluateExpressionAsync<int>("document.querySelectorAll('.s-result-item').length-3");
                for (int i = 0; i <= countProduct; i++)
                {
                    var str = await page.EvaluateExpressionAsync($"Array.from(document.querySelectorAll('.s-result-item .rush-component a'))[{i}].href;");
                    Console.WriteLine(str); 
                }
                if (!args.Any(arg => arg == "auto-exit"))
                {
                    Console.ReadLine();
                }
            }
        }
    }
}
