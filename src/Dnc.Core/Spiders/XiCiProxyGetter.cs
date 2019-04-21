﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dnc.Spiders
{
    /// <summary>
    /// Agent getter from web url:https://www.xicidaili.com/nn/.
    /// </summary>
    public class XiCiAgentGetter
        : IAgentGetter
    {
        #region Private members.
        private readonly IHtmlDownloader _downloader;
        private readonly IHtmlParser _parser;
        #endregion

        #region Default ctor.
        public XiCiAgentGetter(IHtmlDownloader downloader, IHtmlParser parser)
        {
            _downloader = downloader;
            _parser = parser;
        }
        #endregion

        #region Methods for getting proxies.
        public async Task<IEnumerable<T>> GetProxiesAsync<T>(string url) where T : BaseAgentSpiderItem, new()
        {
            var html = await _downloader.DownloadHtmlContentAsync(url);
            if (string.IsNullOrEmpty(html))
                return null;

            var trs = await _parser.GetElementsAsync(html, "#ip_list tr");
            if (trs == null || trs.Length <= 1)
                return null;

            var proxies = new List<T>();
            for (int i = 1; i < trs.Length; i++)
            {
                var tr = trs[i];

                var tds = tr.QuerySelectorAll("td");

                if (tds == null || tds.Length != 10)
                    continue;

                var host = tds[1].TextContent;
                var port = tds[2].TextContent;
                var address = tds[3].TextContent;
                var anonymous = tds[4].TextContent.Equals("高匿");
                var agentType = tds[5].TextContent;
                var speed = tds[6].QuerySelector(".bar").GetAttribute("title");
                var connectionTime = tds[7].QuerySelector(".bar").GetAttribute("title");
                var aliveTime = tds[8].TextContent;
                var verifyTime = tds[9].TextContent;

                var instance = new T
                {
                    Host = host,
                    Port = Convert.ToInt32(port),
                    Address = address,
                    Anonymous = anonymous,
                    AgentType = agentType,
                    VerifyTime = Convert.ToDateTime(verifyTime)
                };
                proxies.Add(instance);
            }
            return proxies;
        } 
        #endregion
    }
}
