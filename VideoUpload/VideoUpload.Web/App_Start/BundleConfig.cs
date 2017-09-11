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

            //Layout page
            bundles.Add(
                new StyleBundle("~/bootstrap", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css")
                        .Include("~/Content/bootstrap.css"));            
            
            bundles.Add(
                new StyleBundle("~/css")
                        .Include(                    
                            "~/Content/PagedList.css",
                            "~/Content/main1.css",
                            "~/Content/print.css"));

            bundles.Add(
                new StyleBundle("~/cust-layout")
                        .Include(
                            "~/Content/cust-layout.css"));
            

            bundles.Add(
                new ScriptBundle("~/js")
                    .Include("~/scripts/videos/timeZoneCookie.js"));

            
            //Upload page
            bundles.Add(
                new ScriptBundle("~/upload")
                    .Include("~/scripts/videos/upload.js"));
            //Edit video page
            bundles.Add(
                new ScriptBundle("~/post-edit")
                    .Include("~/scripts/videos/edit.js"));

            //Post page
            bundles.Add(
                new ScriptBundle("~/details")
                    .Include("~/scripts/videos/details.js"));
            //Watch page
            bundles.Add(
                new ScriptBundle("~/watch")
                    .Include("~/scripts/videos/watch.js"));
            //Contact page
            bundles.Add(
                new ScriptBundle("~/contactus")
                    .Include("~/scripts/home/contactus.js"));
            
            BundleTable.EnableOptimizations = true;
        }
    }
}