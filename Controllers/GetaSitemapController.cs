﻿using System.IO.Compression;
using System.Reflection;
using System.Web.Mvc;
using Geta.SEO.Sitemaps.Entities;
using Geta.SEO.Sitemaps.Repositories;
using log4net;

namespace Geta.SEO.Sitemaps.Controllers
{
    public class GetaSitemapController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISitemapRepository sitemapRepository = new SitemapRepository();

        public ActionResult Index()
        {
            SitemapData sitemapData = sitemapRepository.GetSitemapData(Request.Url.ToString());

            if (sitemapData == null || sitemapData.Data == null)
            {
                Log.Error(string.Format("Xml sitemap data not found for request url: {0}", Request.Url));
                return new HttpNotFoundResult();
            }

            Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);
            Response.AppendHeader("Content-Encoding", "gzip");

            return new FileContentResult(sitemapData.Data, "text/xml");
        }
    }
}