using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Core.Text;

using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Generated;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator
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
                            >   TypesAndroidRegistered
            {
                get;
                protected set;
            }


            public new List<
                                (
                                    string ManagedClass,
                                    string ManagedNamespace,
                                    string JNIPackage,
                                    string JNIType
                                )
                            > TypesNotAndroidRegistered
            {
                get;
                protected set;
            }

        }
    }
}
