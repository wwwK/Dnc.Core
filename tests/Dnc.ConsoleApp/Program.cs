﻿using Dnc.ConsoleApp.Models;
using Dnc.Data;
using Dnc.Dispatcher;
using Dnc.Extensions;
using Dnc.FaultToleranceProcessors;
using Dnc.ObjectId;
using Dnc.Output;
using Dnc.SeedWork;
using Dnc.Spiders;
using Dnc.Structures;
using Microsoft.Extensions.DependencyInjection;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dnc.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var fx = Framework
                .Construct<DefaultFrameworkConstruction>()
                .UseConsoleOutputHelper()
                .UseXiCiAgentGetter()
                .UseMemoryAgentPool()
                .UsePuppeteerHtmlDownloader()
                .UseDefaultCompiler()
                .UseDownloader()
                .UseRedis(opt =>
                {
                    opt.Host = "140.143.88.100";
                    opt.Port = 6379;
                    opt.InstanceName = "Dnc.Core";
                    opt.Password = "p@ssw0rd";
                })
                .UseFaultToleranceProcessor()
                .UseMailSender();
            fx.Build();

            var sp = fx.ServiceProvider;
            var faultToleranceProcessor = sp.GetRequiredService<IFaultToleranceProcessor>();
            //var testService = sp.GetRequiredService<ITestService>();
            #region ConsoleOutputHelper.
            var outputHelper = sp.GetService<IConsoleOutputHelper>() as IConsoleOutputHelper;
            //outputHelper.OutputImage(@"C:\Users\Administrator\Pictures\timg (3).jpg");
            //Enumerable.Range(1, 10).ToList()
            //    .ForEach(i => Task.Factory.StartNew(() =>
            //    {
            //        Thread.Sleep(500);
            //        outputHelper.Debug("Server error.");
            //        outputHelper.Info("Server error.");
            //        outputHelper.Warning("Server error.");
            //        outputHelper.Error("Server error.");
            //    }));
            #endregion

            #region Compiler.
            //            var compiler = sp.GetRequiredService<ICompiler>();
            //            var script = @"  public class HelloWorld
            //    {
            //        public string String { get; set; }
            //        public HelloWorld(string str)
            //        {
            //            String = str;
            //        }
            //    }
            //new HelloWorld(Arg1).String";
            //            var rt = compiler.RunAsync<string>(script,
            //                  new Arg<string>() { Arg1 = "gainorloss" },
            //                  nameSpace: "Dnc.ConsoleApp",
            //                  references: typeof(HelloWorld).Assembly)
            //                  .ConfigureAwait(false)
            //                  .GetAwaiter()
            //                  .GetResult();
            #endregion

            #region Downloader.
            //var downloader = sp.GetRequiredService<IDownloader>();
            //var result = downloader.DownloadRemoteImageAsync("https://goss3.vcg.com/creative/vcg/800/version23/VCG41471562191.jpg", "c://")
            //      .ConfigureAwait(false)
            //      .GetAwaiter()
            //      .GetResult();

            //PriceSpiderAsync(sp)
            //    .ConfigureAwait(false)
            //   .GetAwaiter();
            #endregion

            #region Spider.
            //DangdangCategoryGetterAsync(sp)
            //    .ConfigureAwait(false)
            //    .GetAwaiter();
            TianMaoPriceGetterAsync(sp)
                .Wait();
            #endregion

            #region sort.
            //var items = new int[] { 6, 3, 2, 7, 9, 10 };
            //items.QuickSort(0, 5);
            //items.BubbleSort(); 
            #endregion

            #region alarmer.
            //var alarmer = sp.GetRequiredService<IAlarmer>();
            //var isSuccess = alarmer.AlarmAdminUsingWechatAsync("您的服务器挂了", "库存同步调度失败请赶紧处理")
            //    .ConfigureAwait(false)
            //    .GetAwaiter()
            //    .GetResult(); 
            #endregion

            #region redis.
            var redis = sp.GetRequiredService<IRedis>();
            var mock = sp.GetRequiredService<IMockRepository>();
            //var val = redis.TryGetOrCreate("firstname", () => "gainorloss");
            //val = redis.TryGetOrCreate("firstname", () => "gainorloss"); 

            //var count = redis.Count<Models.User>(1, "isFocused");
            //var items_count = redis.Like(1, "focus", mock.CreateMultiple<Models.User>().ToArray());
            #endregion

            #region mock.
            //var mock = sp.GetRequiredService<IMockRepository>();
            //var hello = mock.Create<HelloWorld>();
            //var hellos = mock.CreateMultiple<HelloWorld>(); 
            #endregion

            #region object id generator.
            //var objectIdGenerator = sp.GetRequiredService<IObjectIdGenerator>();
            //Enumerable.Range(0, 10000).ToList()
            //    .ForEach(i =>
            //    {
            //        var uuid = objectIdGenerator.StringGuid();
            //        var intGuid = objectIdGenerator.IntGuid();
            //        var combinedGuid = objectIdGenerator.StringCombinedGuid();
            //        var snowflakeId = objectIdGenerator.IntSnowflakeId();
            //        Console.WriteLine($"{uuid},{intGuid},{combinedGuid},{snowflakeId}");
            //    });
            #endregion

            #region mail sender.
            //var mailer = sp.GetRequiredService<IMailSender>();
            //mailer.SendMailAsync("519564415@qq.com","this is a title","this is a content")
            //    .ConfigureAwait(false)
            //    .GetAwaiter();
            #endregion

            #region view engine.
            //var viewEngine = sp.GetRequiredService<IViewEngine>();
            //var rt = viewEngine.ParseAsync("<h1>@Model.FirstName</h1><p>@Model.LastName</p>", new { FirstName = "gainorloss", LastName = "gain" })
            //      .ConfigureAwait(false)
            //      .GetAwaiter()
            //      .GetResult();
            //outputHelper.Info(rt);
            #endregion

            #region fault tolerance processor.
            //var faultToleranceProcessor = sp.GetRequiredService<IFaultToleranceProcessor>();
            //faultToleranceProcessor.RetryAsync(async () =>
            //await Task.Run(() =>
            //     {
            //         var zero = 0;
            //         var a = 1 / zero;
            //     })).Wait(); 
            #endregion

            #region io queue.
            var ioQueue = sp.GetRequiredService<IQueue>();
            ioQueue.Enqueue(count =>
            Enumerable.Range(0, 100)
        .ToList()
        .ForEach(i => Console.WriteLine(i)), 100);
            #endregion

            Console.Read();
            Console.WriteLine("Hello World!");
        }

        private static async Task TianMaoPriceGetterAsync(IServiceProvider sp)
        {
            var urls = new List<string>
            {
                "https://detail.tmall.com/item.htm?spm=a220o.1000855.w5001-15421479553.143.7e9e412djTZhL1&id=528648187112&scene=taobao_shop",
                "https://detail.tmall.com/item.htm?spm=a220m.1000858.1000725.1.11e33090FTHStI&id=577658727978&user_id=859515618&cat_id=2&is_b=1&rn=cdc7c780329bc679c19ae6b8c5c081be",
                "https://detail.tmall.com/item.htm?spm=a220o.1000855.w5001-15421479553.144.7e9e412djTZhL1&id=45638137890&rn=1c166838ad2611e2f0ae4723c835c4c0&abbucket=10&scene=taobao_shop",
                "https://detail.tmall.com/item.htm?spm=a220o.1000855.w5001-15421479553.146.1f617840xeeCej&id=39248761854&rn=20828f401aa5df6381a396f3a50bb901&abbucket=20&scene=taobao_shop"
            };
            var ioQueue = sp.GetRequiredService<IQueue>();
            var agentGetter = sp.GetService<IAgentGetter>() as IAgentGetter;

            var proxies = await agentGetter.GetProxiesAsync<BaseAgentSpiderItem>();
            proxies = proxies.Where(proxy => agentGetter.VerifyProxyAsync(proxy.Host).Result).ToList();

            foreach (var url in urls)
            {
                var proxy = proxies.ElementAt(new Random().Next(0, proxies.Count));
                var agent = $"{proxy.AgentType}://{proxy.Host}:{proxy.Port}";
                ioQueue.Enqueue(async link => await GetTianMaoPriceAsync(sp, url, agent), url);
                //await GetTianMaoPriceAsync(sp, url);
            }
        }

        private static async Task GetTianMaoPriceAsync(IServiceProvider sp, string url, string agent)
        {
            var manager = sp.GetRequiredService<IAgentPool>();
            var downloader = sp.GetRequiredService<IHtmlDownloader>();
            var parser = sp.GetRequiredService<IHtmlParser>();
            var outputHelper = sp.GetService<IConsoleOutputHelper>() as IConsoleOutputHelper;

            var html = await downloader.DownloadHtmlContentAsync(url, async page => await page.WaitForSelectorAsync(".tm-price"), agent: agent);
            var h1 = await parser.GetElementAsync(html, ".tb-detail-hd h1");
            var span = await parser.GetElementAsync(html, ".tm-price");
            var price = span.TextContent.Trim();
            var title = h1.TextContent.Trim();
            var lis = await parser.GetElementsAsync(html, "#J_AttrUL li");

            var name = string.Empty;
            var isbn = string.Empty;
            foreach (var li in lis)
            {
                var content = li.TextContent.Trim();
                if (content.Contains("书名"))
                {
                    name = content.Split(':')[1];
                    continue;
                }
                if (content.Contains("ISBN编号"))
                {
                    isbn = content.Split(':')[1];
                    continue;
                }
            }
            Console.WriteLine($"{isbn}:【{name}】:{price}");
        }

        private static async Task PriceSpiderAsync(IServiceProvider sp)
        {
            var downloader = sp.GetRequiredService<IHtmlDownloader>();
            var parser = sp.GetRequiredService<IHtmlParser>();

            var url = "https://detail.tmall.com/item.htm?spm=a220m.1000858.1000725.1.46db30909Ff9yt&id=577658727978&skuId=4611686596086115882&user_id=859515618&cat_id=2&is_b=1&rn=b63ddc1ba00d1050fce9ff69fff2724d";
            var content = await downloader.DownloadHtmlContentAsync(url, async page =>
            {
                var closeBtn = await page.QuerySelectorAsync(".sufei-dialog-close");
                var box = await closeBtn.BoundingBoxAsync();
                var x = box.X + box.Width / 2;
                var y = box.Y + box.Height / 2;
                await page.Mouse.ClickAsync(x, y);
                await page.WaitForNavigationAsync();
            });
        }

        private static async Task TestAsync()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            var page = await browser.NewPageAsync();
            await page.GoToAsync("http://www.google.com");
        }

        private static async Task DangdangCategoryGetterAsync(IServiceProvider sp)
        {
            var manager = sp.GetRequiredService<IAgentPool>();
            var scheduler = sp.GetRequiredService<ScheduleCenter>();
            var downloader = sp.GetRequiredService<IHtmlDownloader>();
            var parser = sp.GetRequiredService<IHtmlParser>();
            var outputHelper = sp.GetService<IConsoleOutputHelper>() as IConsoleOutputHelper;
            var agentGetter = sp.GetService<IAgentGetter>() as IAgentGetter;

            //start a scheduler to get proxies.
            //await scheduler.CreateAndRunScheduleAsync("spider", "Dnc.ConsoleApp.Jobs.ProxyManagerJob", "*/10 * * ? * *", "Dnc.ConsoleApp.dll");
            //Thread.Sleep(10000);

            var isbns = new List<string>
            {
                "9787560099347",
                "9787313171115",
                "9787565600524",
                "9787111575184"
            };
            foreach (var isbn in isbns)
            {
                //var item = await manager.GetAgentAsync<BaseAgentSpiderItem>();
                //var agent = $"{item.AgentType}://{item.Host}:{item.Port}";
                var agent = "https://111.177.191.237:9999";

                var queryUrl = $"http://search.dangdang.com/?key={isbn}";
                var queryHtml = await downloader.DownloadHtmlContentAsync(queryUrl, agent: agent);
                var li = await parser.GetElementAsync(queryHtml, "#search_nature_rg ul li");
                var skuId = li.GetAttribute("sku");

                var itemUrl = $"http://product.dangdang.com/{skuId}.html";
                var itemHtml = await downloader.DownloadHtmlContentAsync(itemUrl, agent: agent);
                var spans = await parser.GetElementsAsync(itemHtml, ".clearfix .fenlei .lie");
                var categories = new List<string>();
                foreach (var span in spans)
                {
                    var categoryNodes = span.QuerySelectorAll("a");
                    var categoryNodeStrs = categoryNodes.Select(n => n.TextContent);
                    categories.Add(string.Join("&gt", categoryNodeStrs));
                }

                var category = string.Join(";", categories);
                outputHelper.Info(category, "当当营销分类");
            }
            scheduler.Shutdown();
            #region Obsolete.
            //var item= manager.GetProxyAsync<BaseProxySpiderItem>().Result;
            //var spider = sp.GetRequiredService<ISpider>();
            //spider.StartAsync("https://www.nuget.org/packages?q=dnc")
            //    .ConfigureAwait(false)
            //    .GetAwaiter(); 
            #endregion
        }
    }
}
