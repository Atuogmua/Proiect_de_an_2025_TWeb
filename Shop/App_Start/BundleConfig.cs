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

   
               // Tiny Slider Scripts
               bundles.Add(new ScriptBundle("~/bundles/tiny-slider/js").Include("~/Scripts/tiny-slider.js"));

               // General Scripts
               bundles.Add(new ScriptBundle("~/bundles/main/js").Include("~/Scripts/custom.js"));

               // jQuery
               bundles.Add(new ScriptBundle("~/bundles/jquery/js").Include(
                         "~/Scripts/jquery-3.3.1.min.js"));

               // jQuery Validation
               bundles.Add(new ScriptBundle("~/bundles/validation/js").Include(
                   "~/Scripts/jquery.validate.min.js"));

               // Unobtrusive           
               bundles.Add(new ScriptBundle("~/bundles/unobtrusive/js").Include(
                         "~/Scripts/jquery.unobtrusive-ajax.min.js"));

               // Pace
               bundles.Add(new ScriptBundle("~/bundles/pace/js").Include(
                         "~/Scripts/pace.min.js"));
               
               //DataTables
               bundles.Add(new ScriptBundle("~/bundles/datatables/js").Include(
                   "~/Vendors/datatables/datatables.min.js"));
          }
     }
}
