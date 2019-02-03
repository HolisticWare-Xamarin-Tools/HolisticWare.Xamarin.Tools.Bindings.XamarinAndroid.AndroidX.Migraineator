using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
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
        public partial class MonoCecilData
        {
            public MonoCecilData(string path)
            {
                this.file_name = path;

                Console.WriteLine("Mono.Cecil initialized for API scraping");

                return;
            }

            string file_name = null;
            FileStream fs = null;
            XDocument xml_doc = null;

            public static IAssemblyResolver CreateAssemblyResolver()
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

            public new List <
                                (
                                    string ManagedClass,
                                    string ManagedNamespace,
                                    string JNIPackage,
                                    string JNIType
                                )
                            >   Types
            {
                get;
                protected set;
            }

            public void AnalyseAPI()
            {
                AssemblyDefinition asm = AssemblyDefinition.ReadAssembly
                                                                      (
                                                                          file_name,
                                                                          new ReaderParameters
                                                                          {
                                                                              AssemblyResolver = CreateAssemblyResolver()
                                                                          }
                                                                      );

                IEnumerable<TypeDefinition> allTypes;
                allTypes = asm.MainModule.GetAllTypes();

                var info = new List <
                                        (
                                            string ManagedClass,
                                            string ManagedNamespace,
                                            string JNIPackage,
                                            string JNIType
                                        )
                                    >();

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

                           info.Add(
                                        (
                                            ManagedClass: mngdClass,
                                            ManagedNamespace: mngdNs,
                                            JNIPackage: jniPkg,
                                            JNIType: jniType
                                        )
                                    );
                        }
                    }
                }

                this.Types = info;

                return;
            }

            public void DumpAPI(string filename_base)
            {
                string path = Path.Combine
                    (
                        new string[]
                        {
                            Environment.CurrentDirectory,
                            "..",
                            "output"
                        }
                    );
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string path_output = Path.Combine(path, "MonoCecil");
                if (!Directory.Exists(path_output))
                {
                    Directory.CreateDirectory(path_output);
                }

                string filename = null;

                filename = Path.Combine(path_output, $"API.{filename_base}.Types.csv");
                this.DumpAPITypes(filename);

                return;
            }

            private void DumpAPITypes(string filename)
            {
                List<string> dump = new List<string>();
                StringBuilder sb = new StringBuilder();

                foreach
                (
                    (
                        string ManagedClass,
                        string ManagedNamespace,
                        string JNIPackage,
                        string JNIType
                    ) typ in this.Types
                )
                {
                    sb.AppendLine($"{typ.ManagedClass},{typ.ManagedNamespace},{typ.JNIPackage},{typ.JNIType}");
                }

                File.WriteAllText($@"{filename}", sb.ToString());

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
}
