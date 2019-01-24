using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace Sample.MonoCecil.JniTypeMappings
{
    class Program
    {
        static void DumpMappings()
        {
            var asm = AssemblyDefinition.ReadAssembly($@".\files\AndroidSupport.Merged.dll"
            //, new ReaderParameters { AssemblyResolver = CreateAssemblyResolver() }
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

                        var mngdClass = GetTypeName(t);
                        var mngdNs = GetNamespace(t);

                        info.Add($"{jniPkg}, {jniClass}, {mngdNs}, {mngdClass}");
                    }
                }
            }

            System.IO.File.WriteAllLines("C:\\code\\support-mappings.csv", info);
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


        static void Main(string[] args)
        {
            DumpMappings();
            return;
        }
    }
}
