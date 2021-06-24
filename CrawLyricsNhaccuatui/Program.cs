using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuppeteerSharp;


namespace ScreenShot
{
    class Program
    {
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
                Console.WriteLine("Navigating Google Maps");
                await page.GoToAsync("https://www.google.com/maps/");

                //Console.WriteLine("Generating PDF");
                //await page.PdfAsync(Path.Combine(Directory.GetCurrentDirectory(), "google.pdf"));


                await page.WaitForSelectorAsync(".searchbox input");
                await page.FocusAsync(".searchbox input");
                await page.Keyboard.TypeAsync("Công ty chuyển phát nhanh và kho vận Pcs Post, Lê Quang Đạo, Mỹ Đình 1, Từ Liêm, Hà Nội");
                await page.ClickAsync(".searchbox-searchbutton");
                await page.WaitForSelectorAsync(".section-layout .S9kvJb");
                await page.ClickAsync(".S9kvJb");
                Console.WriteLine("Waiting input ...");
                await page.WaitForSelectorAsync(".sbib_b input");
                await page.FocusAsync(".sbib_b input");
                await page.Keyboard.TypeAsync("Đại Học Kiến Trúc - Trần Phú (Hà Đông), Nguyễn Trãi, Văn Quán, Hà Đông, Hanoi");
                await page.Keyboard.PressAsync("Enter", null);

                await page.WaitForNavigationAsync();
                await page.WaitForTimeoutAsync(5000);

                Console.WriteLine("screen shoot");
                await page.ScreenshotAsync("C:\\users\\admin\\documents\\file\\screenshot.png");

                Console.WriteLine("export completed");

                if (!args.Any(arg => arg == "auto-exit"))
                {
                    Console.ReadLine();
                }
            }
        }
    }
}
