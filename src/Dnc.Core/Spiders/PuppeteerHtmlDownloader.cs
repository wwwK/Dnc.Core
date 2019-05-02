﻿using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dnc.Spiders
{
    public class PuppeteerHtmlDownloader
        : IHtmlDownloader
    {
        #region Static ctor.
        static PuppeteerHtmlDownloader()
        {
            new BrowserFetcher()
               .DownloadAsync(BrowserFetcher.DefaultRevision)
               .Wait();
        }
        #endregion

        #region Methods for getting html content.
        public async Task<string> DownloadHtmlContentAsync(string url, Func<Page, Task> beforeGetContentHandler = null, string agent = null)
        {
            return await GetHtmlContentUsingPuppeteerAsync(url, beforeGetContentHandler, agent);
        }
        #endregion

        #region Helper.
        private async Task<string> GetHtmlContentUsingPuppeteerAsync(string url, Func<Page, Task> beforeGetContentHandler = null, string agent = null)
        {
            var option = new LaunchOptions()
            {
                Headless = false
            };

            var args = new List<string>()
            {
                "--no-sandbox",
                "--disable-setuid-sandbox"
            };

            if (!string.IsNullOrEmpty(agent))
                args.Add($"--proxy-server={agent}");

            option.Args = args.ToArray();

            using (var browser = await Puppeteer.LaunchAsync(option))
            {
                using (var page = await browser.NewPageAsync())
                {
                    await page.GoToAsync(url);
                    if (beforeGetContentHandler != null)
                    {
                        await beforeGetContentHandler.Invoke(page);//control browser before get content.
                    }
                    var html = await page.GetContentAsync();
                    return html;
                }
            }
        }
        #endregion
    }
}
