using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Practices.Prism.Modularity;

namespace MediaVF.Services.Modularity
{

    public class ChildDomainModuleInfoLoader : MarshalByRefObject
    {
        public List<ModuleInfo> GetModuleInfos(List<string> loadedAssemblies, Dictionary<string, Dictionary<string, List<ModuleData>>> moduleDirectories)
        {
            List<ModuleInfo> moduleInfos = new List<ModuleInfo>();

            loadedAssemblies.ForEach(loadedAssembly => Assembly.ReflectionOnlyLoadFrom(loadedAssembly));

            // handle failure of resolution of dependent assemblies in ReflectionOnlyAssemblyResolve event handler
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(OnReflectionOnlyAssemblyResolve);

            // get module infos in each directory
            foreach (string moduleDirectory in moduleDirectories.Keys)
            {
                // get all dll files in directory
                IEnumerable<FileInfo> assemblyFiles = Directory.GetFiles(moduleDirectory, "*.dll").Select(fileName => new FileInfo(fileName));

                // perform reflection-only load for those assemblies not already loaded AND in the list of assemblies for active modules
                IEnumerable<FileInfo> assembliesToLoad =
                    assemblyFiles.Where(assemblyFile =>
                        !AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().Any(loadedAssembly =>
                            Path.GetFileName(loadedAssembly.Location) == assemblyFile.Name));

                // get module types out of reflection-only loaded assemblies and create ModuleInfos
                foreach (FileInfo assemblyToLoad in assembliesToLoad)
                {
                    // load into reflection context
                    Assembly assembly = Assembly.ReflectionOnlyLoadFrom(assemblyToLoad.FullName);

                    // check that this assembly is a module assembly
                    if (moduleDirectories[moduleDirectory].ContainsKey(assembly.FullName))
                    {
                        List<ModuleData> modules = moduleDirectories[moduleDirectory][assembly.FullName];

                        // get exported types from assembly
                        Type[] exportedTypes = assembly.GetExportedTypes();

                        // get module types with active module records
                        Dictionary<Type, ModuleData> moduleTypes = exportedTypes.Where(type => type.GetInterface(typeof(IModule).Name) != null &&
                                                                                    modules.Any(m => m.Class == type.FullName))
                                                                      .ToDictionary(t => t,
                                                                                    t => modules.First(m => m.Class == t.FullName));

                        // get module infos from assembly
                        IEnumerable<ModuleInfo> assemblyModuleInfos = moduleTypes.Select(moduleTypeAndData => GetModuleInfo(moduleTypeAndData.Key, moduleTypeAndData.Value));

                        // add module infos to catalog
                        foreach (ModuleInfo moduleInfo in assemblyModuleInfos)
                            if (moduleInfo != null)
                                moduleInfos.Add(moduleInfo);
                    }
                }
            }

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= new ResolveEventHandler(OnReflectionOnlyAssemblyResolve);

            return moduleInfos;
        }

        /// <summary>
        /// Handle attempted resolution of dependent assemblies
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private Assembly OnReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs e)
        {
            // check already loaded assemblies
            Assembly loadedAssembly =
                AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().FirstOrDefault(asm => string.Equals(asm.FullName, e.Name, StringComparison.OrdinalIgnoreCase));
            if (loadedAssembly != null)
                return loadedAssembly;

            // get the assembly name
            AssemblyName dependentAssemblyName = new AssemblyName(e.Name);

            // check the requesting assembly's directory
            string localDirectoryPath = Path.Combine(Path.GetDirectoryName(e.RequestingAssembly.Location), dependentAssemblyName.Name + ".dll");
            if (File.Exists(localDirectoryPath))
                return Assembly.ReflectionOnlyLoadFrom(localDirectoryPath);

            return Assembly.ReflectionOnlyLoad(dependentAssemblyName.FullName);
        }

        /// <summary>
        /// Gets module info for a given module type
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        private ModuleInfo GetModuleInfo(Type moduleType, ModuleData moduleData)
        {
            // create module info
            ModuleInfo moduleInfo = new ExtendedModuleInfo()
            {
                ModuleType = moduleType.AssemblyQualifiedName,
                Ref = moduleType.Assembly.CodeBase,
                ModuleID = moduleData.ModuleID
            };

            // get module attribute off type
            CustomAttributeData moduleAttributeData =
                moduleType.GetCustomAttributesData().FirstOrDefault(attribute => attribute.Constructor.DeclaringType.Equals(typeof(ModuleAttribute)));
            if (moduleAttributeData != null)
            {
                // get module name
                CustomAttributeNamedArgument moduleNameArg = moduleAttributeData.NamedArguments.FirstOrDefault(arg => arg.MemberInfo.Name == "ModuleName");
                if (moduleNameArg != null)
                    moduleInfo.ModuleName = moduleNameArg.TypedValue.Value as string;

                // get on demand flag and set initialization mode based on it
                CustomAttributeNamedArgument onDemandArg = moduleAttributeData.NamedArguments.FirstOrDefault(arg => arg.MemberInfo.Name == "OnDemand");
                if (onDemandArg != null && (bool)onDemandArg.TypedValue.Value)
                    moduleInfo.InitializationMode = InitializationMode.OnDemand;
                else
                    moduleInfo.InitializationMode = InitializationMode.WhenAvailable;
            }

            // if a name was not provided for the module from its custom attribute, set the name from the value in the db
            if (string.IsNullOrEmpty(moduleInfo.ModuleName))
                moduleInfo.ModuleName = moduleData.ModuleName;

            // get module dependency attributes off type
            IEnumerable<CustomAttributeData> moduleDependencyAttributeDataCollection =
                moduleType.GetCustomAttributesData().Where(attribute => attribute.Constructor.DeclaringType.Equals(typeof(ModuleDependencyAttribute)));

            // add dependencies to module info
            foreach (CustomAttributeData moduleDependencyAttributeData in moduleDependencyAttributeDataCollection)
                moduleInfo.DependsOn.Add(moduleDependencyAttributeData.ConstructorArguments[0].Value as string);

            return moduleInfo;
        }
    }
}
