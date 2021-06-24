using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PuppeteerSharp;
using System.Collections;


namespace PuppeteerCrawlNhacuatui
{
    class Program
    {
        public class Data
        {
            public string Title { get; set; }
            public string Url { get; set; }
        }
        public class Song
        {
            public string Lyric { get; set; }
        }
        public static async Task Main(string[] args)
        {
            var options = new LaunchOptions
            {
                Headless = false
            };


            //Console.WriteLine("Downloading chromium");
            //Download the Chromium revision if it does not already exist
            //await new BrowserFetcher().DownloadAsync();

            using (var browser = await Puppeteer.LaunchAsync(options))
            using (var page = await browser.NewPageAsync())
            {
                Console.WriteLine("Navigating Nhaccuatui");
                await page.GoToAsync("https://www.nhaccuatui.com/bai-hat/top-20.html");

                var jsCode = @"() => {
                    const selectors = Array.from(document.querySelectorAll('.name_song'));
                    return selectors.map( t=> {return { title: t.innerHTML, url: t.href}});
                }";

                 
                var results = await page.EvaluateFunctionAsync<Data[]>(jsCode);

                          
                foreach (var result in results)
                {                    
                    await page.GoToAsync(result.Url);

                    #region "comment vo van"
                    //var jsScript = @"()=>{
                    //    let lyric = document.getElementsByClassName('pd_lyric trans')[0]
                    //    .innerHTML.replace(/\<br\>/g, "");
                    //    return lyric;           
                    //}";
                    //var jsScript = @"()=>{
                    //    document.getElementsByClassName('pd_lyric trans')[0].innerHTML.replace(/\<br\>/g, '');
                    //}";
                    //var jsXpath = "//*[@id=\"divLyric\"]";
                    //await page.WaitForXPathAsync(jsXpath);
                    #endregion

                    string fileName = @"C:\Users\Admin\Desktop\FileStore\text.rtf";
                    string fileNameImage = @"C:\Users\Admin\Desktop\FileStore\image.txt";
                    var arrEl = await page.EvaluateExpressionAsync("document.querySelector(\"#divLyric\").innerHTML.replace(/\\<br\\>/g, \"\").trim()");
                    string getRawUrlImage = await page.EvaluateExpressionAsync<string>("document.querySelector(\"#coverImageflashPlayer\").style.background");
                    int lastIndexPostion = getRawUrlImage.LastIndexOf("jpg") - 2;
                    string fixUrlImage = getRawUrlImage.Substring(5, lastIndexPostion);

                    Task task2 = new Task(
                            () => {
                                Console.WriteLine(result.Title);                             
                                try
                                {
                                    
                                    Console.WriteLine(fixUrlImage);
                                    using (StreamWriter writer = new StreamWriter(fileName, append: true))
                                    {
                                        string tam = result.Title.ToUpper();
                                        Console.WriteLine("Writting ...");
                                        writer.WriteLine(tam + '\n' + arrEl);
                                    }

                                    
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                    );
                    Task task3 = new Task(
                        () =>
                        {
                            using (StreamWriter writer = new StreamWriter(fileNameImage, append: true))
                            {
                                writer.WriteLine(result.Title);
                                writer.WriteLine(fixUrlImage);
                            }
                        }
                    );
                    task2.Start();
                    task3.Start();
                    //Console.WriteLine(result.Title);

                    //string fileName = @"C:\Users\Admin\Desktop\FileStore\text.rtf";
                    //string fileNameImage = @"C:\Users\Admin\Desktop\FileStore\image.txt";

                    //try
                    //{
                    //    var arrEl = await page.EvaluateExpressionAsync("document.querySelector(\"#divLyric\").innerHTML.replace(/\\<br\\>/g, \"\").trim()");
                    //    string getRawUrlImage = await page.EvaluateExpressionAsync<string>("document.querySelector(\"#coverImageflashPlayer\").style.background");
                    //    int lastIndexPostion = getRawUrlImage.LastIndexOf("jpg")-2;
                    //    string fixUrlImage = getRawUrlImage.Substring(5, lastIndexPostion);
                    //    Console.WriteLine(fixUrlImage);
                    //    using (StreamWriter writer = new StreamWriter(fileName, append: true))
                    //    {
                    //        string tam = result.Title.ToUpper();
                    //        Console.WriteLine("Writting ...");
                    //        writer.WriteLine(tam+'\n'+arrEl);
                    //    }
                        
                    //    using (StreamWriter writer = new StreamWriter(fileNameImage, append: true))
                    //    {
                    //        writer.WriteLine(result.Title);
                    //        writer.WriteLine (fixUrlImage);
                    //    }
                    //}
                    //catch (Exception e)
                    //{
                    //    Console.WriteLine(e.Message);
                    //}
                    
                }
                if (!args.Any(arg => arg == "auto-exit"))
                {
                    Console.ReadLine();
                }
            }
        }
    }
}