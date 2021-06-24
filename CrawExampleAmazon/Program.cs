using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace CrawlExampleAmazon
{
        
        class Program
        {
            public static void SetForcegroundColorRed()
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            public static void SetForcegroundColorDefault()
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            public class Data
            {
                public string BackGround { get; set; }
            }

        public static async Task Main(string[] args)
            {
                var options = new LaunchOptions
                {
                    Headless = true
                };

                //Console.WriteLine("Downloading chromium");
                //Download the Chromium revision if it does not already exist
                //await new BrowserFetcher().DownloadAsync();

                //Console.WriteLine("Navigating google");
                using (var browser = await Puppeteer.LaunchAsync(options))
                using (var page = await browser.NewPageAsync()) 
                {
                #region Chua co gi het ne` =.=
                //Console.WriteLine("Navigating Amazon");
                //await page.GoToAsync("");

                //var results = await page.WaitForXPathAsync("//*[@id=\"search\"]/div[1]/div/div[1]/div/span[3]/div[2]/div[1]/div");
                //var nameProductBack = await page.EvaluateExpressionAsync("document.querySelector(\"#search > div.s-desktop-width-max.s-opposite-dir > div > div.s-matching-dir.sg-col-16-of-20.sg-col.sg-col-8-of-12.sg-col-12-of-16 > div > span:nth-child(4) > div.s-main-slot.s-result-list.s-search-results.sg-row > div:nth-child(1) > div > span > div > div > div:nth-child(3) > h2\").innerText");
                //var urlProduct = await page.EvaluateExpressionAsync("document.querySelector(\"#search > div.s-desktop-width-max.s-opposite-dir > div > div.s-matching-dir.sg-col-16-of-20.sg-col.sg-col-8-of-12.sg-col-12-of-16 > div > span:nth-child(4) > div.s-main-slot.s-result-list.s-search-results.sg-row > div:nth-child(1) > div > span > div > div > span > a\").href");

                //Console.WriteLine(nameProductBack);
                //Console.ForegroundColor = ConsoleColor.Green;
                //Console.WriteLine("Goto Url" + urlProduct);
                //Console.ForegroundColor = ConsoleColor.White;

                //await page.GoToAsync(urlProduct.ToString());

                //Console.WriteLine("==========*=============");
                #endregion

                Console.WriteLine("Navigating Amazon Detail");
                await page.GoToAsync("https://www.amazon.com/Lenovo-Processor-Graphics-Included-81X20005US/dp/B086226DDB/ref=sr_1_1?dchild=1&qid=1624500390&s=computers-intl-ship&sr=1-1");

                #region Get Product Tilte
                string nameProduct = await page.EvaluateExpressionAsync<string>("document.querySelector(\"#productTitle\").innerText");
                SetForcegroundColorRed();
                Console.WriteLine("Name Product");
                SetForcegroundColorDefault();
                Console.WriteLine(nameProduct);
                #endregion

                #region  Get Rate Number
                    string[] arrTextSplitTextRateStar = await page.EvaluateExpressionAsync<string[]>("document.querySelector(\"#acrPopover\").innerText.trim().split(\" \")");
                    float rateNumber = float.Parse(arrTextSplitTextRateStar[0]);
                    SetForcegroundColorRed();
                    Console.WriteLine("Rate Number Star");
                    SetForcegroundColorDefault();
                    Console.WriteLine(rateNumber);
                #endregion

                #region Get Number Customer Ratings
                    string[] arrTextSplitCustomerRating = await page.EvaluateExpressionAsync<string[]>("document.querySelector(\"#acrCustomerReviewText\").innerText.trim().split(\" \")");
                    string result;
                    if (arrTextSplitCustomerRating[0].Length > 3)
                    {
                        string tam = "";
                        string[] arrListStr = arrTextSplitCustomerRating[0].Split(new char[] { ',' });
                        foreach (string item in arrListStr)
                        {
                            tam += item;
                        }
                        result = tam;
                    }
                    else
                    {
                        result = arrTextSplitCustomerRating[0];
                    }
                    SetForcegroundColorRed();
                    Console.WriteLine("Customer Rating Number");
                    SetForcegroundColorDefault();
                    Console.WriteLine(result);
                #endregion
                #region Get Image/Video
                var jsCode = @"() => {
                    const selectors = document.querySelectorAll('#ivThumbImage');
                     for (i = 0; i < x.length; i++) {
                        const url = selectors[i].style.background;   
                    }
                    return url;
                 }";
                string[] arrEx = await page.EvaluateExpressionAsync<string[]>(jsCode);
                foreach (var item in arrEx)
                {
                    Console.WriteLine(item);
                }
                #endregion

                if (!args.Any(arg => arg == "auto-exit"))
                    {
                        Console.ReadLine();
                    }
                }
            }
        }
    
}
