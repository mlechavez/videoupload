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
                new StyleBundle("~/united", "https://maxcdn.bootstrapcdn.com/bootswatch/3.3.7/united/bootstrap.min.css")
                        .Include("~/Content/modified/bootstrap.css"));

            bundles.Add(
                new StyleBundle("~/fontawesome", "https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css")
                        .Include("~/Content/font-awesome.css"));

            bundles.Add(new StyleBundle("~/gfont", "https://fonts.googleapis.com/css?family=Raleway:200,300,500"));
            
            bundles.Add(
                new StyleBundle("~/css")
                        .Include(                    
                            "~/Content/PagedList.css",
                            "~/Content/main.css",
                            "~/Content/print.css"));

            bundles.Add(
                new StyleBundle("~/cust-layout")
                        .Include(
                            "~/Content/cust-layout.css"));

            bundles.Add(
                new ScriptBundle("~/jquery", "https://code.jquery.com/jquery-3.2.1.min.js")
                        .Include("~/scripts/jquery-{version}.js"));

            bundles.Add(
                new ScriptBundle("~/jbootstrap", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js")
                        .Include("~/scripts/bootstrap.js"));

            bundles.Add(
                new ScriptBundle("~/jcookies", "https://cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.min.js")
                        .Include("~/scripts/jquery.cookie.js"));

            bundles.Add(
                new ScriptBundle("~/tinymce", "https://cloud.tinymce.com/stable/tinymce.js?apiKey=5p70a8d0ew9m48r9xmcngtz3t0itd0czyhvrsiimy476qizj")
                    .Include("~/scripts/tinymce.js"));

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
                    .Include("~/scripts/videos/contactus.js"));
            
            BundleTable.EnableOptimizations = true;
        }
    }
}