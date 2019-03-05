using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;

using Core.Text;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Generated;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator
{
    public partial class ApiComparer
    {
        public
            (
                List<string> namespaces,
                List<string> namespaces_new_suspicious,
                List<string> namespaces_old_suspicious,
                List<(string ClassName, string ClassNameFullyQualified)> classes
            )
                                    Analyse(ApiInfo api_info)
        {
            List<(string PackageJava, string NamespaceManaged)> namespaces_from_attributes = null;
            namespaces_from_attributes = new List<(string PackageJava, string NamespaceManaged)>();

            foreach (Generated.Attribute a in api_info.XmlSerializerAPI.ApiInfo.Assembly.Attributes.Attribute)
            {
                string attribute_name = a.Name;
                if (attribute_name.Contains("Android.Support"))
                {

                }
            }

            List<string> namespaces = new List<string>();
            List<string> namespaces_new_suspicious = new List<string>();
            List<string> namespaces_old_suspicious = new List<string>();

            List<(string ClassName, string ClassNameFullyQualified)> classes = null;

            classes = new List<(string ClassName, string ClassNameFullyQualified)>();


            foreach (Namespace n in api_info.XmlSerializerAPI.ApiInfo.Assembly.Namespaces.Namespace)
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

                int ClassNameLength = 0;
                int ClassNameFullyQualifiedLength = 0;
                try
                {
                    if (n.Classes != null)
                    {
                        foreach (Class c in n?.Classes.Class)
                        {
                            string class_name = c?.Name;
                            if (class_name.Length > ClassNameLength)
                            {
                                ClassNameLength = class_name.Length;
                            }

                            string class_name_fq = $"{n.Name}.{class_name}";
                            if (class_name_fq.Length > ClassNameFullyQualifiedLength)
                            {
                                ClassNameFullyQualifiedLength = class_name_fq.Length;
                            }
                            classes.Add((ClassName: class_name, ClassNameFullyQualified: class_name_fq));
                        }
                    }
                }
                catch
                {
                    throw;
                }

                try
                {
                    //if (n.Interfaces != null)
                    {
                        //foreach (Interface i in n?.Interfaces.Interface)
                        {
                            //string interface_name = i?.Name;
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

        public 
            List<(string ClassName, string NamespaceOld, string NamespaceNew)>
                                    MappingApiInfoMatertial()
        {
            List<(string ClassName, string NamespaceName)> classes_material = null;

            classes_material = new List<(string ClassName, string NamespaceName)>();

            foreach (Namespace n in ApiInfoDataNew.XmlSerializerAPI.ApiInfo.Assembly.Namespaces.Namespace)
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

            foreach ( (string ClassName, string NamespaceName) cn in classes_material)
            {
                string namespace_name_new = cn.NamespaceName;

                foreach (Namespace n in ApiInfoDataOld.XmlSerializerAPI.ApiInfo.Assembly.Namespaces.Namespace)
                {
                    string namespace_name_old = n.Name;

                    try
                    {
                        if (n.Classes != null)
                        {
                            foreach (Class c in n?.Classes.Class)
                            {
                                string class_name_old = c?.Name;
                                if (cn.ClassName == class_name_old)
                                {
                                    classes_material_mapping.Add
                                        (
                                            (
                                                ClassName: class_name_old,
                                                NamespaceOld: namespace_name_old,
                                                NamespaceNew: namespace_name_new
                                            )
                                        );
                                }
                            }
                        }
                    }
                    catch
                    {
                        throw;
                    }

                }
            }

            this.DumpToFileXamarinMaterialMappings(classes_material_mapping);

            return classes_material_mapping;
        }

        private void DumpToFileXamarinMaterialMappings
                                        (
                                            List
                                                <
                                                    (
                                                        string ClassName, 
                                                        string NamespaceOld, 
                                                        string NamespaceNew
                                                    )
                                                > classes_material_mapping
                                        )
        {
            int n = classes_material_mapping.Count();

            int length_class = 0;
            int length_namepsace_new = 0;
            int length_namepsace_old = 0;

            for (int i = 0; i < n; i++)
            {
                int lci = classes_material_mapping[i].ClassName.Length;
                if (classes_material_mapping[i].ClassName.Length > length_class)
                {
                    length_class = lci;
                }
                int lnoi = classes_material_mapping[i].NamespaceOld.Length;
                if (classes_material_mapping[i].NamespaceOld.Length > length_namepsace_old)
                {
                    length_namepsace_old = lnoi;
                }
                int lnni = classes_material_mapping[i].NamespaceNew.Length;
                if (classes_material_mapping[i].NamespaceNew.Length > length_namepsace_new)
                {
                    length_namepsace_new = lnni;
                }
            }

            int padding = 3;
            string fmt0 = "{0,-" + (length_class + padding) + "}";
            string fmt1 = ",{1,-" + (length_namepsace_old + padding) + "}";
            string fmt2 = ",{2}";

            string fmt = fmt0 + fmt1 + fmt2;

            string file_content = null;
            for (int i = 0; i < n; i++)
            {
                string c = classes_material_mapping[i].ClassName;
                string no = classes_material_mapping[i].NamespaceOld;
                string nn = classes_material_mapping[i].NamespaceNew;

                file_content += String.Format(fmt, c + ",", no + ",", nn);
                file_content += Environment.NewLine;
            }

            System.IO.File.WriteAllText("mapping-xamarin-android-support-to-androidx.csv", file_content);

            return;
        }
    }
}
