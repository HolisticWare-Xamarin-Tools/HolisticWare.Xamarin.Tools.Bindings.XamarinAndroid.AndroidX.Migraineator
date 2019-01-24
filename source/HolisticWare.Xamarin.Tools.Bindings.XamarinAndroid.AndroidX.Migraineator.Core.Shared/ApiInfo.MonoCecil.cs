using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using Core.Text;

using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core.Generated;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core
{
    public partial class ApiInfo
    {
        public partial class MonoCecil
        {
            public MonoCecil(string path)
            {
                this.file_name = path;
                fs = new FileStream(file_name, FileMode.Open, FileAccess.Read);
                xml_doc = XDocument.Load(fs);

                return;
            }

            string file_name = null;
            FileStream fs = null;
            XDocument xml_doc = null;

        }

        public void Dump()
        {
            AssemblyDefinition asm = AssemblyDefinition.ReadAssembly
                                                                  (
                                                                      @"C:\code\AndroidSupport.Merged.dll",
                                                                      new ReaderParameters
                                                                      {
                                                                          //AssemblyResolver = CreateAssemblyResolver()
                                                                      }
                                                                  );

            IEnumerable<TypeDefinition> allTypes;
            allTypes = asm.MainModule.GetAllTypes();

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

            File.WriteAllLines($@"binaries\support-mappings.csv", info);

            return;
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
    }
}
