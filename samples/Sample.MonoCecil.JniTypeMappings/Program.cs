using System;
using System.Collections.Generic;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace Sample.MonoCecil.JniTypeMappings
{
    class Program
    {
        static void DumpMappings()
        {
            string path = Environment.CurrentDirectory;

            var asm = AssemblyDefinition.ReadAssembly
                                                (
                                                    $@"./bin/Debug/netcoreapp2.2/Files/AndroidSupport.Merged.dll", 
                                                    new ReaderParameters 
                                                        { 
                                                            AssemblyResolver = CreateAssemblyResolver() 
                                                        }
                                                );

            var allTypes = asm.MainModule.GetAllTypes();

            var info = new List<string>();

            foreach (var t in allTypes)
            {
                foreach (var attr in t.CustomAttributes)
                {
                    if (attr.AttributeType.FullName.Equals("Android.Runtime.RegisterAttribute"))
                    {
                        var jniType = attr.ConstructorArguments[0].Value.ToString();

                        var lastSlash = jniType.LastIndexOf('/');

                        var jniClass = jniType.Substring(lastSlash + 1).Replace('$', '.');
                        var jniPkg = jniType.Substring(0, lastSlash).Replace('/', '.');

                        if (jniPkg.StartsWith("mono."))
                        {
                            continue;
                        }

                        var mngdClass = GetTypeName(t);
                        var mngdNs = GetNamespace(t);

                        info.Add($"{jniPkg}, {jniClass}, {mngdNs}, {mngdClass}");
                    }
                }
            }

            System.IO.File.WriteAllLines("./support-mappings.csv", info);
        }

        static string GetNamespace(TypeDefinition typeDef)
        {
            var td = typeDef;
            var ns = typeDef.Namespace;

            while (string.IsNullOrEmpty(ns))
            {
                if (td.DeclaringType == null)
                    break;
                ns = td.DeclaringType.Namespace;
                td = td.DeclaringType;
            }

            return ns;
        }

        static string GetTypeName(TypeDefinition typeDef)
        {
            var td = typeDef;
            var tn = typeDef.Name;

            while (td.DeclaringType != null)
            {
                tn = td.DeclaringType.Name + "." + tn;
                td = td.DeclaringType;
            }

            return tn;
        }

        static IAssemblyResolver CreateAssemblyResolver()
        {
            var VsInstallRoot = "C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Enterprise\\";
            var TargetFrameworkVerison = "v9.0";

            var resolver = new DefaultAssemblyResolver();
            if (!string.IsNullOrEmpty(VsInstallRoot) && Directory.Exists(VsInstallRoot))
            {
                resolver.AddSearchDirectory(Path.Combine(
                    VsInstallRoot,
                    @"Common7\IDE\ReferenceAssemblies\Microsoft\Framework\MonoAndroid\" + TargetFrameworkVerison
                    ));
            }
            else
            {
                resolver.AddSearchDirectory(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    @"Reference Assemblies\Microsoft\Framework\MonoAndroid\" + TargetFrameworkVerison
                ));
            }
            return resolver;
        }

        static void Main(string[] args)
        {
            DumpMappings();
            return;
        }
    }
}
