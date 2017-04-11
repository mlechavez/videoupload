using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace VideoUpload.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            bundles.Add(
                new StyleBundle("~/bundles/css")
                .Include(
                    "~/Content/bootstrap.min.css",
                    "~/Content/font-awesome.min.css",
                    "~/Content/main.css",
                    "~/Content/print.css"));

            bundles.Add(new StyleBundle("~/bundles/gfont", "https://fonts.googleapis.com/css?family=Raleway:200,300,500"));

            bundles.Add(
                new ScriptBundle("~/bundles/scripts")
                .Include(
                    "~/scripts/jquery-{version}.min.js",
                    "~/scripts/jquery.cookie-1.4.1.min.js",                    
                    "~/scripts/bootstrap.min.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}