using System;
using System.IO;
using System.IO.Packaging;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace ShapeDatasetGeneratorWPF.Helper
{
    public static class InitializeExtension
    {
        public static void LoadViewFromUri(this UserControl userControl, string baseUri)
        {
            try
            {
                ParserContext parserContext;
                var stream = GetStream(baseUri, out parserContext);
                typeof(XamlReader).GetMethod("LoadBaml", BindingFlags.NonPublic | BindingFlags.Static)
                    .Invoke(null, new object[] {stream, parserContext, userControl, true});
            }
            catch (Exception)
            {
                // log
            }
        }

        public static void LoadViewFromUri(this Window window, string baseUri)
        {
            try
            {
                ParserContext parserContext;
                var stream = GetStream(baseUri, out parserContext);
                typeof(XamlReader).GetMethod("LoadBaml", BindingFlags.NonPublic | BindingFlags.Static)
                    .Invoke(null, new object[] {stream, parserContext, window, true});
            }
            catch (Exception)
            {
                //log
            }
        }

        private static Stream GetStream(string baseUri, out ParserContext parserContext)
        {
            var resourceLocater = new Uri(baseUri, UriKind.Relative);
            var exprCa = (PackagePart) typeof(Application)
                .GetMethod("GetResourceOrContentPart", BindingFlags.NonPublic | BindingFlags.Static)
                .Invoke(null, new object[] {resourceLocater});
            var stream = exprCa.GetStream();
            var uri = new Uri(
                (Uri) typeof(BaseUriHelper)
                    .GetProperty("PackAppBaseUri", BindingFlags.Static | BindingFlags.NonPublic)
                    .GetValue(null, null), resourceLocater);
            parserContext = new ParserContext
            {
                BaseUri = uri
            };
            return stream;
        }
    }
}