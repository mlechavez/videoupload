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
            bundles.Add(
                new StyleBundle("~/bundles/css")
                .Include("~/Content/bootstrap.min.css"));

            bundles.Add(
                new ScriptBundle("~/bundles/scripts")
                .Include("~/Content/jquery-{version}.min.js"));
        }
    }
}