using System.Globalization;
using System.Reflection;
using WPFLocalizeExtension.Extensions;

namespace ShapeDatasetGeneratorWPF
{
    public static class LocalizationProvider
    {
        public static T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":Resources:" +
                                                     key);
        }

        public static T GetLocalizedValueForTargetCulture<T>(string key, CultureInfo targetCultureInfo)
        {
            return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + ":Resources:" + key,
                targetCultureInfo);
        }
    }
}
