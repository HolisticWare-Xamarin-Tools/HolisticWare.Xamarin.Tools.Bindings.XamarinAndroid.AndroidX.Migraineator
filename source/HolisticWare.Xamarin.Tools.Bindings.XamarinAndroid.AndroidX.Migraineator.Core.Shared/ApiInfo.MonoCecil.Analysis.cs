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
            public void Analyse()
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

                Parallel.Invoke
                (
                    () =>
                    {
                        GetTypesAndroidRegistered(allTypes);
                    },
                    () =>
                    {
                        GetTypes(allTypes);
                    }
                );

                return;
            }

            private void GetTypesAndroidRegistered(IEnumerable<TypeDefinition> allTypes)
            {
                List<
                        (
                            string ManagedClass, 
                            string ManagedNamespace, 
                            string JNIPackage, 
                            string JNIType
                        )
                    > info = null;

                info = new List<
                                    (
                                        string ManagedClass,
                                        string ManagedNamespace,
                                        string JNIPackage,
                                        string JNIType
                                    )
                                >();

                foreach (TypeDefinition t in allTypes)
                {
                    foreach (CustomAttribute attr in t.CustomAttributes)
                    {
                        if (attr.AttributeType.FullName.Equals("Android.Runtime.RegisterAttribute"))
                        {
                            string jniType = attr.ConstructorArguments[0].Value.ToString();

                            int lastSlash = jniType.LastIndexOf('/');

                            string jniClass = jniType.Substring(lastSlash + 1).Replace('$', '.');
                            string jniPkg = jniType.Substring(0, lastSlash).Replace('/', '.');

                            string mngdClass = GetTypeName(t);
                            string mngdNs = GetNamespace(t);

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

                this.TypesAndroidRegistered = info;
            }

            private void GetTypes(IEnumerable<TypeDefinition> allTypes)
            {
                List<
                        (
                            string ManagedClass,
                            string ManagedNamespace,
                            string JNIPackage,
                            string JNIType
                        )
                    > info = null;

                info = new List<
                                    (
                                        string ManagedClass,
                                        string ManagedNamespace,
                                        string JNIPackage,
                                        string JNIType
                                    )
                                >();

                foreach (TypeDefinition t in allTypes)
                {
                    foreach (CustomAttribute attr in t.CustomAttributes)
                    {
                        if (attr.AttributeType.FullName.Equals("Android.Runtime.RegisterAttribute"))
                        {
                            continue;
                        }
                    }
                    string mngdClass = GetTypeName(t);
                    string mngdNs = GetNamespace(t);

                    info.Add(
                                 (
                                     ManagedClass: mngdClass,
                                     ManagedNamespace: mngdNs,
                                     JNIPackage: "managed type",
                                     JNIType: "managed type"
                                 )
                             );
                }

                this.TypesManaged = info;
            }
        }
    }
}
