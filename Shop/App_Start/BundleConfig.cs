using System.Web.Optimization;

namespace Shop
{
    public class BundleConfig
    {
       public static void RegisterBundles(BundleCollection bundles)
          {
               // Bootstrap style
               bundles.Add(new StyleBundle("~/bundles/bootstrap/css").Include("~/Content/bootstrap.min.css", new CssRewriteUrlTransform()));
               
               
               // Font Awesome
               bundles.Add(new StyleBundle("~/bundles/font-awesome/css").Include("~/Content/font-awesome.min.css", new CssRewriteUrlTransform()));

               // General Style
               bundles.Add(new StyleBundle("~/bundles/main/css").Include("~/Content/style.css", new CssRewriteUrlTransform()));

               // Slider Style
               bundles.Add(new StyleBundle("~/bundles/tiny-slider/css").Include("~/Content/tiny-slider.css", new CssRewriteUrlTransform()));


               // Bootstrap
               bundles.Add(new ScriptBundle("~/bundles/bootstrap/js").Include("~/Scripts/bootstrap.min.js"));

               // Bootstrap bundles
               //bundles.Add(new ScriptBundle("~/bundles/bootstrap-bundle/js").Include("~/Scripts/bootstrap.bundle.min.js"));

               // Tiny Slider Scripts
               bundles.Add(new ScriptBundle("~/bundles/tiny-slider/js").Include("~/Scripts/tiny-slider.js"));

               // General Scripts
               bundles.Add(new ScriptBundle("~/bundles/main/js").Include("~/Scripts/custom.js"));


          }
     }
}
