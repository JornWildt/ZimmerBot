using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;

namespace ZimmerBot.Core.AddOnHandling
{
  public static class AddOnLoader
  {
    static ILog Logger = LogManager.GetLogger(typeof(AddOnLoader));


    public static void InitializeAddOns()
    {
      Logger.Debug("Loading addons");
      foreach (Type t in GetAddOns())
      {
        Logger.DebugFormat("Initializing addon '{0}'", t);
        IZimmerBotAddOn addOn = Activator.CreateInstance(t) as IZimmerBotAddOn;
        addOn.Initialize();
      }
    }


    public static void ShutdownAddOns()
    {
      foreach (Type t in GetAddOns())
      {
        Logger.DebugFormat("Shutting down addon '{0}'", t);
        IZimmerBotAddOn addOn = Activator.CreateInstance(t) as IZimmerBotAddOn;
        addOn.Shutdown();
      }
    }


    private static IList<Type> _addOns = null;

    private static IList<Type> GetAddOns()
    {
      if (_addOns != null)
        return _addOns;

      Type addOnType = typeof(IZimmerBotAddOn);

      string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      Logger.DebugFormat("Scanning '{0}' for addons", path);

      // Search for all DLLs and load each of them
      foreach (string dll in Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories))
      {
        try
        {
          Logger.DebugFormat("Loading '{0}'", dll);
          Assembly loadedAssembly = Assembly.LoadFile(dll);
        }
        catch (ReflectionTypeLoadException ex)
        {
          Logger.Error("Failed to load addon", ex);
          foreach (Exception subex in ex.LoaderExceptions)
            Logger.Error("Got loader exception", subex);
        }
        catch (Exception ex)
        {
          Logger.Error("Failed to load addon", ex);
        }
      }

      // Find all AddOn types in the loaded assemblies
      _addOns =
        AppDomain.CurrentDomain.GetAssemblies()
          .Where(a => a.GetName().Name.EndsWith(".AddOn"))
          .SelectMany(a => a.GetTypes())
          .Where(t => addOnType.IsAssignableFrom(t) && t.IsClass).ToList();

      return _addOns;
    }
  }
}
