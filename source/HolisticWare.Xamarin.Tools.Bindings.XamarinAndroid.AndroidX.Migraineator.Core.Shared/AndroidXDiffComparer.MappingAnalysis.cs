using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;

using Core.Text;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core.Generated;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core
{
    public partial class AndroidXDiffComparer
    {
        public
            (
                List<string> namespaces,
                List<string> namespaces_new_suspicious,
                List<string> namespaces_old_suspicious,
                List<string> classes
            )
                                    Analyse(ApiInfo api_info)
        {
            List<string> namespaces = new List<string>();
            List<string> namespaces_new_suspicious = new List<string>();
            List<string> namespaces_old_suspicious = new List<string>();

            List<string> classes = new List<string>();

            foreach (Namespace n in api_info.Assembly.Namespaces.Namespace)
            {
                string namespace_name = n.Name;
                namespaces.Add(namespace_name);

                if (namespace_name.StartsWith("Androidx."))
                {
                    namespaces_new_suspicious.Add(namespace_name);
                }
                else if (namespace_name.StartsWith("Android."))
                {
                    namespaces_old_suspicious.Add(namespace_name);
                }

                try
                {
                    if (n.Classes != null)
                    {
                        foreach (Class c in n?.Classes.Class)
                        {
                            string class_name = c?.Name;
                            string class_name_fq = $"{n.Name}.{class_name}";
                            classes.Add($"{class_name_fq},                                 ,{class_name}");
                        }
                    }
                }
                catch
                {
                    throw;
                }

            }

            return
                (
                    namespaces,
                    namespaces_new_suspicious,
                    namespaces_old_suspicious,
                    classes
                );
        }

        public List<(string ClassName, string NamespaceOld, string NamespaceNew)>
            MappingApiInfoMatertial()
        {
            List<(string ClassName, string NamespaceName)> classes_material = null;

            classes_material = new List<(string ClassName, string NamespaceName)>();

            foreach (Namespace n in ApiInfoDataNew.Assembly.Namespaces.Namespace)
            {
                string namespace_name = n.Name;

                if (namespace_name.StartsWith("Xamarin.Google.Android.Material"))
                {
                    try
                    {
                        if (n.Classes != null)
                        {
                            foreach (Class c in n?.Classes.Class)
                            {
                                string class_name = c?.Name;
                                classes_material.Add
                                                (
                                                    (
                                                        ClassName: class_name,
                                                        NamespaceName: namespace_name
                                                    )
                                                );
                            }
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            List<(string ClassName, string NamespaceOld, string NamespaceNew)> classes_material_mapping;
            classes_material_mapping = new List<(string ClassName, string NamespaceOld, string NamespaceNew)>();

            return classes_material_mapping;
        }
    }
}
