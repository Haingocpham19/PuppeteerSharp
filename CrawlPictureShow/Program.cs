using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuppeteerSharp;
namespace CrawlPictureShow
{
    class Program
    {
        
        public class DataImage
        {
            public string Title { get; set; }
            public string ID { get; set; }

        }
        public static async Task Main(string[] args)
        {
            var options = new LaunchOptions
            {
                Headless = false
            };
            Console.WriteLine("Downloading chromium");
            //Download the Chromium revision if it does not already exist
            await new BrowserFetcher().DownloadAsync();

            //Console.WriteLine("Navigating google");
            using (var browser = await Puppeteer.LaunchAsync(options))
            using (var page = await browser.NewPageAsync())
            {
                Console.WriteLine("Navigating Amazon Detail");
                await page.GoToAsync("https://www.amazon.com/Lenovo-Processor-Graphics-Included-81X20005US/dp/B086226DDB/ref=sr_1_1?dchild=1&qid=1624499298&s=computers-intl-ship&sr=1-1&th=1");

                //Console.WriteLine("Generating PDF");
                //await page.PdfAsync(Path.Combine(Directory.GetCurrentDirectory(), "google.pdf"));

                #region Crawl Image Show
                await page.WaitForSelectorAsync(".imgTagWrapper");
                await page.ClickAsync("#imgTagWrapperId");
                var jsCode = @"() => {
                    let links = [];
                    const selectors = Array.from(document.querySelectorAll('.ivThumb'));
                        selectors.forEach(item => {
                          links.push({
                            title: item.innerText,
                            id: item.getAttribute('id')
                          });
                     });
                     return links;
                }";
                string fileNameImage = @"C:\Users\Admin\Desktop\FileStore\image.txt";
                DataImage[] arr = await page.EvaluateFunctionAsync<DataImage[]>(jsCode);
                for (int i = 2; i < arr.Length; i++)
                {              
                    Console.WriteLine(arr[i].ID);
                    string tam = "#" + arr[i].ID.ToString();
                    await page.ClickAsync(tam);
                    await page.WaitForTimeoutAsync(1000);
                    string getRawUrlImage = await page.EvaluateExpressionAsync<string>("document.querySelector(\"#ivLargeImage\").innerHTML");
                    int lastIndexPostion = getRawUrlImage.LastIndexOf("jpg")-7;
                    string fixUrlImage = getRawUrlImage.Substring(10, lastIndexPostion);
                    Console.WriteLine(fixUrlImage);
                    using (StreamWriter writer = new StreamWriter(fileNameImage, append: true))
                    {
                        writer.WriteLine(arr[i].ID);
                        writer.WriteLine(fixUrlImage);
                    }
                }
                #endregion
                #region Crawl Video Show
                await page.ClickAsync("#ivVideosTabHeading");

                #endregion

                Console.WriteLine("Completed");

                if (!args.Any(arg => arg == "auto-exit"))
                {
                    Console.ReadLine();
                }
            }
        }
    }
}
