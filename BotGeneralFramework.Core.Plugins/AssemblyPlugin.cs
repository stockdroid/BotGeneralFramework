namespace BotGeneralFramework.Core.Plugins;
using BotGeneralFramework.Collections.Core;
using BotGeneralFramework.Core.Plugins.Exceptions;
using BotGeneralFramework.Interfaces.Core;
using System.Reflection;
using System.Runtime.Loader;

/// <summary>
/// Describes a plugin compiled in .net bytecode
/// </summary>
public sealed class AssemblyPlugin: IPlugin
{
  private readonly string _pluginName;
  private readonly string _pluginFullName;
  private readonly Type _pluginType;
  private readonly Assembly _assembly;
  private readonly IPlugin _plugin;

  public AssemblyPlugin(Assembly assemblyFile, string pluginName) {
    // later used to identify the type
    _assembly = assemblyFile;
    _pluginName = pluginName
                  ?? throw new ArgumentNullException(nameof(pluginName));

    // identify the plugin type
    _pluginFullName = $"BotGeneralFramework.Plugins.{_pluginName}";
    _pluginType = _assembly.GetType(_pluginFullName)
                  ?? throw new PluginTypeNotFoundException(_pluginFullName);

    // instantiate the plugin
    _plugin = Activator.CreateInstance(_pluginType) as IPlugin
              ?? throw new PluginMalformedException(_pluginName);

    // export useful attributes
    _plugin.Exports.Add("assembly", _assembly);
    _plugin.Exports.Add("typeName", _pluginFullName);
  }

  public string Author => _plugin.Author;
  public string Name => _plugin.Name;
  public string Version => _plugin.Version;
  public ExportCollection Exports => _plugin.Exports;

  public bool Activate(IApp app) => _plugin.Activate(app);
  public bool Deactivate()
  {
    if (!_plugin.Deactivate()) return false;
    
    // used to unload assembly
    var assemblyContext = AssemblyLoadContext.GetLoadContext(_assembly);
    if (assemblyContext is null) return false;
    
    // unload assembly
    assemblyContext.Unload();
    return true;
  }

  public static AssemblyPlugin? Import(string pluginsPath, string pluginName) {
    // later used to locate assembly files
    string pluginBundle = Path.Combine(pluginsPath, pluginName);
    FileInfo assemblyFile = new(Path.Combine(pluginBundle, $"{pluginName}.dll"));
    if (!assemblyFile.Exists) return null;

    // load the given assembly
    string pluginDeps = Path.Combine(pluginBundle, "dependencies");
    Assembly assembly = Assembly.LoadFrom(assemblyFile.FullName);

    AssemblyLoadContext? assemblyContext = AssemblyLoadContext.GetLoadContext(assembly);
    if (assemblyContext is null) return null;

    // add dependency resolver
    assemblyContext.Resolving += (context, assemblyName) => {
      if (assemblyName.Name is null) return null;

      FileInfo assemblyInfo = new(Path.Combine(pluginBundle, $"{assemblyName.Name}.dll"));
      if (!assemblyInfo.Exists) return null;

      return context.LoadFromAssemblyPath(assemblyInfo.FullName);
    };

    return new(assembly, pluginName);
  }
}