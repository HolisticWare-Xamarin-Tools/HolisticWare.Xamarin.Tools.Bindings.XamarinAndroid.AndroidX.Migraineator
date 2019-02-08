using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Mono.Cecil;
using Mono.Cecil.Rocks;

using Core.Text;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core.Generated;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core
{
    public partial class ApiInfo
    {
        public partial class MonoCecilData
        {
            AssemblyDefinition asm = null;
            public void Analyse()
            {
                asm = AssemblyDefinition.ReadAssembly
                                              (
                                                  file_name,
                                                  new ReaderParameters
                                                  {
                                                      AssemblyResolver = CreateAssemblyResolver()
                                                  }
                                              );

                Initialize();

                GetTypes();

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

            private void GetTypes()
            {
                Console.WriteLine("Scraping ....");

                foreach (TypeDefinition t in asm.MainModule.Types)
                {

                    string managed_class = GetTypeName(t);
                    string managed_namespace = GetNamespace(t);

                    Console.WriteLine($"        Class                : {managed_class}");
                    Console.WriteLine($"            FullName         : {t.FullName}");
                    Console.WriteLine($"            Managed Namespace: {managed_namespace}");

                    if (t.HasCustomAttributes)
                    {
                        // 
                        foreach (CustomAttribute attr in t.CustomAttributes)
                        {
                            if (attr.AttributeType.FullName.Equals("Android.Runtime.RegisterAttribute"))
                            {
                                string jni_type = attr.ConstructorArguments[0].Value.ToString();

                                int lastSlash = jni_type.LastIndexOf('/');

                                string jni_class = jni_type.Substring(lastSlash + 1).Replace('$', '.');
                                string jni_package = jni_type.Substring(0, lastSlash).Replace('/', '.');

                                this.TypesAndroidRegistered.Add
                                                                (
                                                                    (
                                                                        ManagedClass: managed_class,
                                                                        ManagedNamespace: managed_namespace,
                                                                        JNIPackage: jni_package,
                                                                        JNIType: jni_type
                                                                    )
                                                                );

                            }
                        }
                    }

                    this.TypesNotAndroidRegistered.Add
                            (
                                (
                                    ManagedClass: managed_class,
                                    ManagedNamespace: managed_namespace,
                                    JNIPackage: "managed - not Android registered",
                                    JNIType: "managed - not Android registered"
                                )
                            );
                }

                return;
            }

            private void Initialize()
            {
                this.TypesAndroidRegistered =
                                                new List<
                                                            (
                                                                string ManagedClass,
                                                                string ManagedNamespace,
                                                                string JNIPackage,
                                                                string JNIType
                                                            )
                                                        >();

                this.TypesNotAndroidRegistered =
                                                new List<
                                                            (
                                                                string ManagedClass,
                                                                string ManagedNamespace,
                                                                string JNIPackage,
                                                                string JNIType
                                                            )
                                                        >();

                return;
            }
        }
    }
}
