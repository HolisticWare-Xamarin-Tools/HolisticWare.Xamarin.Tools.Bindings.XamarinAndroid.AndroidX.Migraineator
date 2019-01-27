using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;

using Core.Text;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core.Generated;
using System.Collections.ObjectModel;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core
{
    public partial class ApiComparer
    {
        public List<(string OldAndroidSupport, string NewAndroidX)> MappingsArtifacts
        {
            get;
            protected set;
        }

        public List<(string OldAndroidSupport, string NewAndroidX)> MappingsNamespaces
        {
            get;
            protected set;
        }

        public List<(string OldAndroidSupport, string NewAndroidX)> MappingsClasses
        {
            get;
            protected set;
        }

        private string content_mappings_android_support_artifacts_to_androidx_artifacts;

        private string content_mappings_namespaces;
        private string content_mappings_classes;


        public ApiComparer()
        {
            return;
        }

        string path_mappings = null;

        //-------------------------------------------------------------------------------------------------------------------
        CharacterSeparatedValues csv_mappings_androidx_artifacts_with_old_packagenames;
        IEnumerable<string[]> map_androidx_artifacts_with_old_packagenames;
        IEnumerable
            <
                (
                    string AndroidXArtifact,
                    string OldPackageName
                )
            > map_typed_androidx_artifacts_with_old_packagenames;
        public
            List
            <
                (
                    string AndroidXArtifact,
                    string OldPackageName
                )
            > MapAndroidXArtifactToOldPackageName
        {
            get
            {
                return map_typed_androidx_artifacts_with_old_packagenames.ToList();
            }
        }

        public
            IEnumerable
                <
                    (
                        string AndroidXArtifact,
                        string OldPackageName
                    )
                >
                MapAndroidXArtifactToOldPackagename(IEnumerable<string[]> untyped_data)
        {
            foreach (string[] row in untyped_data)
            {
                yield return
                        (
                            AndroidXArtifact: row[0],
                            OldPackageName: row[2]
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------



        public void ModifyApiInfo
                                (
                                    string content_previous,
                                    ApiInfo api_info_androidx
                                )
        {
            foreach (Namespace n in api_info_androidx.XmlSerializerAPI.ApiInfo.Assembly.Namespaces.Namespace)
            {
                string namespace_name = n.Name; 

                if (namespace_name.StartsWith("Android.Support."))
                {
                    var shiet =
                        from map in MapNamespacesAndroisSupportToAndroidX
                            where map.ManagedNamespaceXamarinAndroidSupport == namespace_name
                            select map.ManagedNamespaceXamarinAndroidX
                            ;

                    List<string> replacements = shiet.ToList();

                    foreach(string replacement in  replacements)
                    {
                        Console.WriteLine($"Replacing:");
                        Console.WriteLine($"         {namespace_name}");
                        Console.WriteLine($"   with");
                        Console.WriteLine($"         {replacement}");
                        content_previous = content_previous.Replace(n.Name, replacement);
                    }
                }

                /*
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
                */
            }

            string file = "../../../../X/AndroidSupportComponents-AndroidX-binderate/output/AndroidSupport.AndroidX.api-info.previous.xml";
            System.IO.File.WriteAllText(file, content_previous);

            return;
        }

        //-------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------
        CharacterSeparatedValues csv_mappings_classes_android_support_to_androidx;
        IEnumerable<string[]> map_classes_android_support_to_androidx;
        IEnumerable
            <
                (
                    string AndroidSupportClass,
                    string AndroidXClass
                )
            > map_typed_classes_android_support_to_androidx;
        public
            List
            <
                (
                    string AndroidSupportClass,
                    string AndroidXClass
                )
            > MapClassesAndroidSupportToAndroidX
        {
            get
            {
                return map_typed_classes_android_support_to_androidx.ToList();
            }
        }





        public 
            List <
                            (
                                string ClassName,
                                string AndroidSupportClass,
                                string AndroidXClass,
                                string AndroidSupportClassFullyQualified,
                                string AndroidXClassFullyQualified,
                                // formatting space
                                string PackageAndroidSupport,
                                string PackageAndroidX,
                                string ManagedNamespaceXamarinAndroidSupport,
                                string ManagedNamespaceXamarinAndroidX
                            )
                        >
            Merge
                (
                    ReadOnlyCollection  <
                                            (
                                                string AndroidSupportClass, 
                                                string AndroidXClass
                                            )
                                        > google_class_mappings,
                    ApiInfo api_info_old_android_support, 
                    ApiInfo api_info_new_androidx
                )
        {
            //--------------------------------------------------------------------------------------------
            // Data structures (tuples) for analysis
            List<
                    (
                        string PackageAndroidSupport,
                        string PackageAndroidX,
                        string ManagedNamespaceXamarinAndroidSupport,
                        string ManagedNamespaceXamarinAndroidX
                    )
                >
                    packages_namespaces;

            List<
                    (
                        string ClassName,
                        string AndroidSupportClass,
                        string AndroidXClass,
                        string AndroidSupportClassFullyQualified,
                        string AndroidXClassFullyQualified,
                        // formatting space
                        string PackageAndroidSupport,
                        string PackageAndroidX,
                        string ManagedNamespaceXamarinAndroidSupport,
                        string ManagedNamespaceXamarinAndroidX
                    )
                >
                    results_found;

            List<
                    (
                        string ClassName,
                        string AndroidSupportClass,
                        string AndroidXClass
                    )
                >
                    results_not_found_at_all;

            List<
                    (
                        string ClassName,
                        string AndroidSupportClass,
                        string AndroidXClass
                    )
                >
                    results_not_found_in_xas;

            List<
                    (
                        string ClassName,
                        string AndroidSupportClass,
                        string AndroidXClass
                    )
                >
                    results_not_found_in_xax;
            //--------------------------------------------------------------------------------------------



            //--------------------------------------------------------------------------------------------
            // Initialization
            results_found =
                new
                    List<
                            (
                                string ClassName,
                                string AndroidSupportClass,
                                string AndroidXClass,
                                string AndroidSupportClassFullyQualified,
                                string AndroidXClassFullyQualified,
                                // formatting space
                                string PackageAndroidSupport,
                                string PackageAndroidX,
                                string ManagedNamespaceXamarinAndroidSupport,
                                string ManagedNamespaceXamarinAndroidX
                            )
                        >();

            packages_namespaces =
                new
                    List<
                            (
                                string PackageAndroidSupport,
                                string PackageAndroidX,
                                string ManagedNamespaceXamarinAndroidSupport,
                                string ManagedNamespaceXamarinAndroidX
                            )
                        >();

            results_not_found_at_all =
                new
                    List<
                            (
                                string ClassName,
                                string AndroidSupportClass,
                                string AndroidXClass
                            )
                        >();
            results_not_found_in_xas =
                new
                    List<
                            (
                                string ClassName,
                                string AndroidSupportClass,
                                string AndroidXClass
                            )
                        >();
            results_not_found_in_xax =
                new
                    List<
                            (
                                string ClassName,
                                string AndroidSupportClass,
                                string AndroidXClass
                            )
                        >();
            //--------------------------------------------------------------------------------------------



            foreach
            (
                (
                    string AndroidSupportClass,
                    string AndroidXClass
                ) gm
                in google_class_mappings
            )
            {
                string gm_as_cn_fq = gm.AndroidSupportClass;
                string gm_ax_cn_fq = gm.AndroidXClass;

                if (gm_as_cn_fq == "Support Library class" && gm_ax_cn_fq == "Android X class")
                {
                    // header ignore
                    continue;
                }

                //--------------------------------------------------------------------------------------------
                // Item declarations/definitions for Data Structures (tuples)
                (
                    string ClassName,
                    string AndroidSupportClass,
                    string AndroidXClass,
                    string AndroidSupportClassFullyQualified,
                    string AndroidXClassFullyQualified,
                    // formatting space
                    string PackageAndroidSupport,
                    string PackageAndroidX,
                    string ManagedNamespaceXamarinAndroidSupport,
                    string ManagedNamespaceXamarinAndroidX
                )
                    result_found;


                (
                    string PackageAndroidSupport,
                    string PackageAndroidX,
                    string ManagedNamespaceXamarinAndroidSupport,
                    string ManagedNamespaceXamarinAndroidX
                )
                    package_namespace;

                (
                    string ClassName,
                    string AndroidSupportClass,
                    string AndroidXClass
                )
                    result_not_found_in_xas;
                (
                    string ClassName,
                    string AndroidSupportClass,
                    string AndroidXClass
                )
                    result_not_found_in_xax;
                (
                    string ClassName,
                    string AndroidSupportClass,
                    string AndroidXClass
                )
                    result_not_found_at_all;


                var as_c = from as_class in api_info_old_android_support.XmlDocumentAPI.Classes
                           where 
                                (
                                    gm_as_cn_fq == $"{as_class.ManagedNamespace.ToLower()}.{as_class.ClassName}"
                                )
                           select 
                                    as_class
                           ;

                var ax_c = from ax_class in api_info_new_androidx.XmlDocumentAPI.Classes
                           where
                                (
                                    gm_ax_cn_fq == $"{ax_class.ManagedNamespace.ToLower()}.{ax_class.ClassName}"
                                )
                           select
                                    ax_class
                           ;

                int n_results_as_c = as_c.Count();
                int n_results_ax_c = ax_c.Count();

                string cn = null;
                string class_name_fq_xamarin_android_support = null;
                string class_name_fq_xamarin_androidx = null;
                string ns_xamarin_android_support = null;
                string ns_xamarin_androidx = null;

                int gm_as_cn_packagename_end = gm_as_cn_fq.LastIndexOf('.');
                int gm_ax_cn_packagename_end = gm_ax_cn_fq.LastIndexOf('.');

                // packagenames
                string gm_as_pn = gm.AndroidSupportClass.Substring(0, gm_as_cn_packagename_end);
                string gm_ax_pn = gm.AndroidXClass.Substring(0, gm_ax_cn_packagename_end);

                // classnames
                string gm_as_cn = gm.AndroidSupportClass.Replace($"{gm_as_pn}.", "");
                string gm_ax_cn = gm.AndroidXClass.Replace($"{gm_ax_pn}.", "");

                if (gm_as_cn != gm_ax_cn)
                {
                    // Google changed class name too
                    cn = "ClassName differs by Google!";
                }
                else
                {
                    cn = gm_ax_cn;
                }

                if (n_results_as_c == 1 && n_results_ax_c == 1)
                {
                    // found exact match
                    if (as_c.First().ClassName == ax_c.First().ClassName)
                    {
                        cn = as_c.First().ClassName;
                        ns_xamarin_android_support = $"{as_c.First().ManagedNamespace}";
                        ns_xamarin_androidx = $"{ax_c.First().ManagedNamespace}";
                        class_name_fq_xamarin_android_support = $"{ns_xamarin_android_support}.{cn}";
                        class_name_fq_xamarin_androidx = $"{ns_xamarin_androidx}.{cn}";
                    }
                }
                else if (n_results_as_c == 0 || n_results_ax_c == 0)
                {
                    // not found
                    if (n_results_as_c == 0 && n_results_ax_c >= 1)
                    {
                        result_not_found_in_xas =
                            (
                                ClassName: cn,
                                AndroidSupportClass: gm_as_cn_fq,
                                AndroidXClass: gm_ax_cn_fq
                            );
                        results_not_found_in_xas.Add(result_not_found_in_xas);
                    }
                    if (n_results_ax_c == 0 && n_results_as_c >= 1)
                    {
                        result_not_found_in_xax =
                            (
                                ClassName: cn,
                                AndroidSupportClass: gm_as_cn_fq,
                                AndroidXClass: gm_ax_cn_fq
                            );
                        results_not_found_in_xax.Add(result_not_found_in_xax);
                    }
                    if (n_results_as_c == 0 && n_results_ax_c == 0)
                    {
                        result_not_found_at_all =
                            (
                                ClassName: cn,
                                AndroidSupportClass: gm_as_cn_fq,
                                AndroidXClass: gm_ax_cn_fq
                            );
                        results_not_found_at_all.Add(result_not_found_at_all);
                    }
                }
                else if (n_results_as_c > 1 || n_results_ax_c > 1)
                {
                    // found ambiguous (more results)
                    throw new InvalidProgramException("mc++ ambiugity");
                }
                else
                {
                    throw new InvalidProgramException("mc++ nothing covered");
                }

                result_found =
                            (
                                ClassName: cn,
                                AndroidSupportClass: gm_as_cn_fq,
                                AndroidXClass: gm_ax_cn_fq,
                                AndroidSupportClassFullyQualified: class_name_fq_xamarin_android_support,
                                AndroidXClassFullyQualified: class_name_fq_xamarin_androidx,
                                // formatting space
                                PackageAndroidSupport: gm_as_pn,
                                PackageAndroidX: gm_ax_pn,
                                ManagedNamespaceXamarinAndroidSupport: ns_xamarin_android_support,
                                ManagedNamespaceXamarinAndroidX: ns_xamarin_androidx
                            );
                package_namespace =
                            (
                                PackageAndroidSupport: gm_as_pn,
                                PackageAndroidX: gm_ax_pn,
                                ManagedNamespaceXamarinAndroidSupport: ns_xamarin_android_support,
                                ManagedNamespaceXamarinAndroidX: ns_xamarin_androidx
                            );

                results_found.Add(result_found);
                packages_namespaces.Add(package_namespace);

                //yield return
                //(
                //    AndroidSupportClass: gm_as_cn_fq,
                //    AndroidXClass: ax_cn_fq
                //);
            }

            var packages_namespaces_normalized = packages_namespaces.Distinct();

            this.Dump(results_found);
            this.Dump(packages_namespaces);
            this.Dump(results_not_found_at_all, "NotFoundAtAll");
            this.Dump(results_not_found_in_xas, "NotFoundInXamarinAndroidSupport");
            this.Dump(results_not_found_in_xax, "NotFoundInXamarinAndroidX");

            return results_found;
        }

        int padding = 3;

        private void Dump
            (
                List
                    <
                        (
                            string ClassName, 
                            string AndroidSupportClass, 
                            string AndroidXClass, 
                            string AndroidSupportClassFullyQualified, 
                            string AndroidXClassFullyQualified,
                            // formatting space
                            string PackageAndroidSupport,
                            string PackageAndroidX,
                            string ManagedNamespaceXamarinAndroidSupport, 
                            string ManagedNamespaceXamarinAndroidX
                        )
                    > 
                    results_found
            )
        {
            int length_cn = -1;
            int length_cn_max = -1;

            int length_gm_as_cn_fq = -1;
            int length_gm_as_cn_fq_max = -1;

            int length_gm_ax_cn_fq = -1;
            int length_gm_ax_cn_fq_max = -1;

            int? length_as_cn_fq = -1;
            int length_as_cn_fq_max = -1;

            int? length_ax_cn_fq = -1;
            int length_ax_cn_fq_max = -1;

            int length_as_pn = -1;
            int length_as_pn_max = -1;

            int length_ax_pn = -1;
            int length_ax_pn_max = -1;

            int length_as_ns = -1;
            int length_as_ns_max = -1;

            int length_ax_ns = -1;
            int length_ax_ns_max = -1;

            foreach
                (
                    (
                        string ClassName,
                        string AndroidSupportClass,
                        string AndroidXClass,
                        string AndroidSupportClassFullyQualified,
                        string AndroidXClassFullyQualified,
                        // formatting space
                        string PackageAndroidSupport,
                        string PackageAndroidX,
                        string ManagedNamespaceXamarinAndroidSupport,
                        string ManagedNamespaceXamarinAndroidX
                    ) c
                    in results_found
                )
            {
                length_cn = c.ClassName.Length;
                if (length_cn > length_cn_max)
                {
                    length_cn_max = length_cn;
                }

                length_gm_as_cn_fq = c.AndroidSupportClass.Length;
                if (length_gm_as_cn_fq > length_gm_as_cn_fq_max)
                {
                    length_gm_as_cn_fq_max = length_gm_as_cn_fq;
                }

                length_gm_ax_cn_fq = c.AndroidXClass.Length;
                if (length_gm_ax_cn_fq > length_gm_ax_cn_fq_max)
                {
                    length_gm_ax_cn_fq_max = length_gm_ax_cn_fq;
                }

                length_as_cn_fq = c.AndroidSupportClassFullyQualified?.Length;
                if (length_as_cn_fq > length_as_cn_fq_max)
                {
                    length_as_cn_fq_max = length_as_cn_fq.Value;
                }

                length_ax_cn_fq = c.AndroidXClassFullyQualified?.Length;
                if (length_ax_cn_fq > length_ax_cn_fq_max)
                {
                    length_ax_cn_fq_max = length_ax_cn_fq.Value;
                }

                length_as_pn = c.PackageAndroidSupport.Length;
                if (length_as_pn > length_as_pn_max)
                {
                    length_as_pn_max = length_as_pn;
                }

                //length_ax_pn = c.PackageAndroidX.Length;
                //if (length_ax_pn > length_ax_pn_max)
                //{
                //    length_ax_pn_max = length_ax_pn;
                //}

            }
            string fmt =
                    "{0,-" + (length_cn_max + padding) + "}"
                    +
                    ",{1,-" + (length_gm_as_cn_fq_max + padding) + "}"
                    +
                    ",{2,-" + (length_gm_ax_cn_fq_max + padding) + "}"
                    +
                    ",{3,-" + (length_as_cn_fq_max + padding) + "}"
                    +
                    ",{4,-" + (length_ax_cn_fq_max + padding) + "}"
                    +
                    ",{5,-" + (length_as_pn_max + padding) + "}"
                    +
                    ",{6,-" + (length_ax_pn_max + padding) + "}"
                    +
                    ",{7,-" + (length_as_ns_max + padding) + "}"
                    +
                    ",{8}"
                        ;

            StringBuilder sb = new StringBuilder();
            foreach
                (
                    (
                        string ClassName,
                        string AndroidSupportClass,
                        string AndroidXClass,
                        string AndroidSupportClassFullyQualified,
                        string AndroidXClassFullyQualified,
                        // formatting space
                        string PackageAndroidSupport,
                        string PackageAndroidX,
                        string ManagedNamespaceXamarinAndroidSupport,
                        string ManagedNamespaceXamarinAndroidX
                    ) c
                    in results_found
                )
            {
                string cn = c.ClassName;
                string as_cn = c.AndroidSupportClass;
                string ax_cn = c.AndroidXClass;
                string xas_cn = c.AndroidSupportClassFullyQualified;
                string xax_cn = c.AndroidXClassFullyQualified;
                // formatting space
                string pn_as = c.PackageAndroidSupport;
                string pn_ax = c.PackageAndroidX;
                string ns_xas = c.ManagedNamespaceXamarinAndroidSupport;
                string ns_xax = c.ManagedNamespaceXamarinAndroidX;

                Console.WriteLine($"format = {fmt}");
                sb.AppendLine(string.Format(fmt, cn, as_cn, ax_cn, xas_cn, xax_cn, pn_as, pn_ax, ns_xas, ns_xax));
            }

            System.IO.File.WriteAllText($"API.Classes.csv", sb.ToString());

            return;
        }

        private void Dump
            (
                List
                    <
                        (
                            string PackageAndroidSupport,
                            string PackageAndroidX,
                            string ManagedNamespaceXamarinAndroidSupport,
                            string ManagedNamespaceXamarinAndroidX
                        )
                    >
                    results_found
            )
        {
            int length_gm_as_pn = -1;
            int length_gm_as_pn_max = -1;
            int length_gm_ax_pn = -1;
            int length_gm_ax_pn_max = -1;
            int? length_as_ns = -1;
            int? length_as_ns_max = -1;
            //int length_ax_ns = -1;
            //int length_ax_ns_max = -1;

            foreach
                (
                    (
                        string PackageAndroidSupport,
                        string PackageAndroidX,
                        string ManagedNamespaceXamarinAndroidSupport,
                        string ManagedNamespaceXamarinAndroidX
                    ) c
                    in results_found
                )
            {
                length_gm_as_pn = c.PackageAndroidSupport.Length;
                if (length_gm_as_pn > length_gm_as_pn_max)
                {
                    length_gm_as_pn_max = length_gm_as_pn;
                }
                length_gm_ax_pn = c.PackageAndroidX.Length;
                if (length_gm_ax_pn > length_gm_ax_pn_max)
                {
                    length_gm_ax_pn_max = length_gm_ax_pn;
                }
                length_as_ns = c.ManagedNamespaceXamarinAndroidSupport?.Length;
                if (length_as_ns > length_as_ns_max)
                {
                    length_as_ns_max = length_as_ns;
                }
                //length_ax_ns = c.ManagedNamespaceXamarinAndroidX.Length;
                //if (length_ax_ns > length_ax_ns_max)
                //{
                //    length_ax_ns_max = length_ax_ns;
                //}

            }

            string fmt =
                    "{0,-" + (length_gm_as_pn_max + padding) + "}"
                    +
                    ",{1,-" + (length_gm_ax_pn_max + padding) + "}"
                    +
                    ",{2,-" + (length_as_ns_max + padding) + "}"
                    +
                    ",{3}"
                        ;

            StringBuilder sb = new StringBuilder();
            foreach
                (
                    (
                        string PackageAndroidSupport,
                        string PackageAndroidX,
                        string ManagedNamespaceXamarinAndroidSupport,
                        string ManagedNamespaceXamarinAndroidX
                    ) c
                    in results_found
                )
            {
                string pn_as = c.PackageAndroidSupport;
                string pn_ax = c.PackageAndroidX;
                string ns_xas = c.ManagedNamespaceXamarinAndroidSupport;
                string ns_xax = c.ManagedNamespaceXamarinAndroidX;

                sb.AppendLine(string.Format(fmt, pn_as, pn_ax, ns_xas, ns_xax));
            }

            string[] lines = sb.ToString().Split
                                            (
                                                new string[] { Environment.NewLine }, 
                                                StringSplitOptions.RemoveEmptyEntries
                                            );

            IEnumerable<string> lines_normalized = lines.Distinct();
            StringBuilder sb_final = new StringBuilder();
            foreach (string line in lines_normalized)
            {
                if (line.EndsWith(",,"))
                {
                    continue;
                }
                sb_final.AppendLine(line);
            }

            System.IO.File.WriteAllText($"API.PackagesNamespaces.csv", sb_final.ToString());

            return;
        }

        private void Dump
                        (
                            List    <
                                        (
                                            string ClassName, 
                                            string AndroidSupportClass, 
                                            string AndroidXClass
                                        )
                                    > 
                                results_not_found_at_all,
                            string description
                        )
        {
            int length_cn = -1;
            int length_cn_max = -1;
            int length_gm_as_cn_fq = -1;
            int length_gm_as_cn_fq_max = -1;
            int length_ax_cn_fq = -1;
            int length_ax_cn_fq_max = -1;

            foreach 
                (
                    (
                        string ClassName,
                        string AndroidSupportClass,
                        string AndroidXClass
                    ) c
                    in results_not_found_at_all
                )
            {
                length_cn = c.ClassName.Length;
                length_gm_as_cn_fq = c.AndroidSupportClass.Length;
                length_ax_cn_fq = c.AndroidXClass.Length;

                if (length_cn > length_cn_max)
                {
                    length_cn = length_cn_max;
                }
                if (length_gm_as_cn_fq > length_gm_as_cn_fq_max)
                {
                    length_gm_as_cn_fq = length_gm_as_cn_fq_max;
                }
                //if (length_ax_cn_fq > length_ax_cn_fq_max)
                //{
                //    length_ax_cn_fq = length_ax_cn_fq_max;
                //}
            }

            string fmt =
                    "{0,-" + (length_cn + padding) + "}"
                    +
                    ",{1,-" + (length_gm_as_cn_fq + padding) + "}"
                    +
                    ",{2}"
                    ;
            StringBuilder sb = new StringBuilder();
            foreach
                (
                    (
                        string ClassName,
                        string AndroidSupportClass,
                        string AndroidXClass
                    ) c
                    in results_not_found_at_all
                )
            {
                string cn = c.ClassName;
                string as_cn = c.AndroidSupportClass;
                string ax_cn = c.AndroidXClass;

                sb.AppendLine(string.Format(fmt, cn, as_cn, ax_cn));

            }

            System.IO.File.WriteAllText($"API.Classes.{description}.csv", sb.ToString());

            return;
        }
        //-------------------------------------------------------------------------------------------------------------------


        public
            IEnumerable
                <
                    (
                        string AndroidSupportClass,
                        string AndroidXClass
                    )
                >
                MapClassesAndroidSupportClassToAndroidXClass(IEnumerable<string[]> untyped_data)
        {
            foreach (string[] row in untyped_data)
            {
                yield return
                        (
                            AndroidSupportClass: row[0],
                            AndroidXClass: row[2]
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------
        CharacterSeparatedValues csv_mappings_packages_android_support_to_androidx;
        IEnumerable<string[]> map_packages_android_support_to_androidx;
        IEnumerable
            <
                (
                    string AndroidSupportPackage,
                    string AndroidXPackage
                )
            > map_typed_packages_android_support_to_androidx;
        public
            List
            <
                (
                    string AndroidSupportPackage,
                    string AndroidXPackage
                )
            > MapPackagesAndroidSupportToAndroidX
        {
            get
            {
                return map_typed_packages_android_support_to_androidx.ToList();
            }
        }

        public
            IEnumerable
                <
                    (
                        string AndroidSupportPackage,
                        string AndroidXPackage
                    )
                >
                MapPackagesAndroidSupportPackageToAndroidXPackage(IEnumerable<string[]> untyped_data)
        {
            foreach (string[] row in untyped_data)
            {
                yield return
                        (
                            AndroidSupportPackage: row[0],
                            AndroidXPackage: row[2]
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------
        CharacterSeparatedValues csv_mappings_packages_android_support_to_xamarin_namespace;
        IEnumerable<string[]> map_packages_android_support_to_xamarin_namespace;
        IEnumerable
            <
                (
                    string AndroidSupportPackage,
                    string XamarinManagedNamespaceXamarinAndroidSupport
                )
            > map_typed_packages_android_support_to_xamarin_namespace;
        public
            List
            <
                (
                    string AndroidSupportPackage,
                    string XamarinManagedNamespaceXamarinAndroidSupport
                )
            > MapPackagesAndroidSupportToXamarinNamespace
        {
            get
            {
                return map_typed_packages_android_support_to_xamarin_namespace.ToList();
            }
        }

        public
            IEnumerable
                <
                    (
                        string AndroidSupportPackage,
                        string XamarinManagedNamespaceXamarinAndroidSupport
                    )
                >
                MapAndroidSupportPackageToXamarinManagedNamespaceXamarinAndroidSupport(IEnumerable<string[]> untyped_data)
        {
            foreach (string[] row in untyped_data)
            {
                yield return
                        (
                            AndroidSupportPackage: row[0],
                            XamarinManagedNamespaceXamarinAndroidSupport: row[2]
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------
        CharacterSeparatedValues csv_xamarin_google_play_services_and_firebase_packages;
        IEnumerable<string[]> map_xamarin_google_play_services_and_firebase_packages;
        IEnumerable
            <
                (
                    string GooglePlayServiceAndFirebaseNamespace,
                    string GooglePlayServiceAndFirebasePackage
                )
            > map_typed_xamarin_google_play_services_and_firebase_packages;
        public
            List
            <
                (
                    string GooglePlayServiceAndFirebaseNamespace,
                    string GooglePlayServiceAndFirebasePackage
                )
            > MapXamarinGooglePlayServicesAndFirebase
        {
            get
            {
                return map_typed_xamarin_google_play_services_and_firebase_packages.ToList();
            }
        }

        public
            IEnumerable
                <
                    (
                        string GooglePlayServiceAndFirebaseNamespace,
                        string GooglePlayServiceAndFirebasePackage
                    )
                >
                MapXamarinGooglePlayServiceAndFirebaseNamespaces(IEnumerable<string[]> untyped_data)
        {
            foreach (string[] row in untyped_data)
            {
                yield return
                        (
                            GooglePlayServiceAndFirebaseNamespace: row[0],
                            GooglePlayServiceAndFirebasePackage: "n/a"
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------
        CharacterSeparatedValues csv_mappings_namespaces_android_support_to_androidx;
        IEnumerable<string[]> map_namespaces_android_support_to_androidx;
        IEnumerable
            <
                (
                    string ManagedNamespaceXamarinAndroidSupport,
                    string ManagedNamespaceXamarinAndroidX
                )
            > map_typed_namespaces_android_support_to_androidx;
        public
            List
            <
                (
                    string ManagedNamespaceXamarinAndroidSupport,
                    string ManagedNamespaceXamarinAndroidX
                )
            > MapNamespacesAndroisSupportToAndroidX
        {
            get
            {
                return map_typed_namespaces_android_support_to_androidx.ToList();
            }
        }

        public
            IEnumerable
                <
                    (
                        string ManagedNamespaceXamarinAndroidSupport,
                        string ManagedNamespaceXamarinAndroidX
                    )
                >
                MapAndroidSupportNamsepcaeToManagedNamespaceXamarinAndroidX(IEnumerable<string[]> untyped_data)
        {
            foreach (string[] row in untyped_data)
            {
                yield return
                        (
                            GooglePlayServiceAndFirebaseNamespace: row[0],
                            GooglePlayServiceAndFirebasePackage: row[2]
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------


        public async Task InitializeAsync(string path_working_directory)
        {
            await MappingManager.LoadGoogleArtifactMappings(path_working_directory);
            await MappingManager.LoadGoogleClassMappings(path_working_directory);

            await MappingManager.LoadGoogleClassMappingsPrettyfied(path_working_directory);

            //map_typed_android_support_artifacts_to_androidx_artifacts =
            //        MapAndroidSupportArtifactToAndroidXArtifact(map_android_support_artifacts_to_androidx_artifacts)
            //        .ToList()
            //        ;

            //file = path_mappings + "mappings-android-support-artifacts-to-xamarin-android-support-assembly.csv";
            //csv_mappings_android_support_artifacts_to_xamarin_android_support_assembly = new CharacterSeparatedValues();
            //await csv_mappings_android_support_artifacts_to_xamarin_android_support_assembly.LoadAsync(file);
            //map_android_support_artifacts_to_xamarin_android_support_assembly =
            //                csv_mappings_android_support_artifacts_to_xamarin_android_support_assembly
            //                            .ParseTemporaryImplementation()
            //                            .ToList()
            //                            ;
            //map_typed_android_support_artifacts_to_androidx_artifacts = 
            //        MapAndroidSupportArtifactToXamarinAndroidSupportAssembly(map_android_support_artifacts_to_xamarin_android_support_assembly)
            //        .ToList()
            //        ;

            //file = path_mappings + "mappings-androidx-artifacts-with-old-packagenames.csv";
            //csv_mappings_androidx_artifacts_with_old_packagenames = new CharacterSeparatedValues();
            //await csv_mappings_androidx_artifacts_with_old_packagenames.LoadAsync(file);
            //map_androidx_artifacts_with_old_packagenames =
            //                csv_mappings_androidx_artifacts_with_old_packagenames
            //                            .ParseTemporaryImplementation()
            //                            .ToList()
            //                            ;
            //map_typed_androidx_artifacts_with_old_packagenames =
            //MapAndroidXArtifactToOldPackagename(map_androidx_artifacts_with_old_packagenames)
            //.ToList()
            //;

            //file = path_mappings + "mappings-androidx-material-packages-to-xamarin-namespaces.csv";
            //csv_mappings_androidx_material_packages_to_xamarin_namespaces = new CharacterSeparatedValues();
            //await csv_mappings_androidx_material_packages_to_xamarin_namespaces.LoadAsync(file);
            //map_androidx_material_packages_to_xamarin_namespaces =
            //                csv_mappings_androidx_material_packages_to_xamarin_namespaces
            //                            .ParseTemporaryImplementation()
            //                            .ToList()
            //                            ;
            //map_typed_androidx_material_packages_to_xamarin_namespaces =
            //        MapAndroidXMaterialPackageToXamarinNamespace(map_androidx_material_packages_to_xamarin_namespaces)
            //        .ToList()
            //        ;

            //file = path_mappings + "mappings-classes-android-support-to-androidx.csv";
            //csv_mappings_classes_android_support_to_androidx = new CharacterSeparatedValues();
            //await csv_mappings_classes_android_support_to_androidx.LoadAsync(file);
            //map_classes_android_support_to_androidx =
            //                csv_mappings_classes_android_support_to_androidx
            //                            .ParseTemporaryImplementation()
            //                            .ToList()
            //                            ;
            //map_typed_classes_android_support_to_androidx =
            //        MapClassesAndroidSupportClassToAndroidXClass(map_classes_android_support_to_androidx)
            //        .ToList()
            //        ;

            //file = path_mappings + "mappings-packages-android-support-to-androidx.csv";
            //csv_mappings_packages_android_support_to_androidx = new CharacterSeparatedValues();
            //await csv_mappings_packages_android_support_to_androidx.LoadAsync(file);
            //map_packages_android_support_to_androidx =
            //                csv_mappings_packages_android_support_to_androidx
            //                            .ParseTemporaryImplementation()
            //                            .ToList()
            //                            ;
            //map_typed_packages_android_support_to_androidx =
            //MapPackagesAndroidSupportPackageToAndroidXPackage(map_packages_android_support_to_androidx)
            //.ToList()
            //;

            //file = path_mappings + "mappings-packages-android-support-to-xamarin-namespace.csv";
            //csv_mappings_packages_android_support_to_xamarin_namespace = new CharacterSeparatedValues();
            //await csv_mappings_packages_android_support_to_xamarin_namespace.LoadAsync(file);
            //map_packages_android_support_to_xamarin_namespace =
            //                csv_mappings_packages_android_support_to_xamarin_namespace
            //                            .ParseTemporaryImplementation()
            //                            .ToList()
            //                            ;
            //map_typed_packages_android_support_to_xamarin_namespace =
            //        MapAndroidSupportPackageToXamarinManagedNamespaceXamarinAndroidSupport(map_packages_android_support_to_xamarin_namespace)
            //        .ToList()
            //        ;

            //file = path_mappings + "xamarin-google-play-services-and-firebase-packages.csv";
            //csv_xamarin_google_play_services_and_firebase_packages = new CharacterSeparatedValues();
            //await csv_xamarin_google_play_services_and_firebase_packages.LoadAsync(file);
            //map_xamarin_google_play_services_and_firebase_packages =
            //                csv_xamarin_google_play_services_and_firebase_packages
            //                            .ParseTemporaryImplementation()
            //                            .ToList()
            //                            ;
            //map_typed_xamarin_google_play_services_and_firebase_packages =
            //        MapXamarinGooglePlayServiceAndFirebaseNamespaces(map_xamarin_google_play_services_and_firebase_packages)
            //        .ToList()
            //        ;

            //file = path_mappings + "mappings-namespaces-android-support-to-androidx.csv";
            //csv_mappings_namespaces_android_support_to_androidx = new CharacterSeparatedValues();
            //await csv_mappings_namespaces_android_support_to_androidx.LoadAsync(file);
            //map_namespaces_android_support_to_androidx =
            //                csv_mappings_namespaces_android_support_to_androidx
            //                            .ParseTemporaryImplementation()
            //                            .ToList()
            //                            ;
            //map_typed_namespaces_android_support_to_androidx =
            //MapAndroidSupportNamsepcaeToManagedNamespaceXamarinAndroidX(map_namespaces_android_support_to_androidx)
            //.ToList()
            //;

            return;
        }






        private async Task InitializeMappingsNamespacesAsync(string content)
        {
            await Task.Run(() => InitializeMappingsNamespaces(content));

            return;
        }

        private void InitializeMappingsNamespaces(string content)
        {
            string[] lines = content.Split
                                        (
                                            new string[] { Environment.NewLine, @"\n" },
                                            StringSplitOptions.RemoveEmptyEntries
                                        );

            this.MappingsNamespaces = new List<(string OldAndroidSupport, string NewAndroidX)>();

            for (int i = 0; i < lines.Length; i++)
            {
                string[] columns = lines[i].Split
                                        (
                                            new char[] { ',' },
                                            StringSplitOptions.RemoveEmptyEntries
                                        );
                (string OldAndroidSupport, string NewAndroidX) t = (columns[0], columns[2]);
                MappingsNamespaces.Add(t);
            }

            // replacing long strngs 1st - less chances to replace partial (substrings)
            List<(string OldAndroidSupport, string NewAndroidX)> ns =
                (
                    from (string OldAndroidSupport, string NewAndroidX) mapping in MappingsNamespaces
                    orderby mapping.OldAndroidSupport.Length descending
                    select mapping
                ).ToList<(string OldAndroidSupport, string NewAndroidX)>();

            this.MappingsNamespaces = ns;

            return;
        }

        public async Task<string> MigrateMetadataXmlFileNamespacesAsync(string file)
        {
            string content;
            using
                (
                    System.IO.FileStream stream = System.IO.File.Open
                                                                    (
                                                                        file,
                                                                        System.IO.FileMode.Open,
                                                                        System.IO.FileAccess.Read
                                                                    )
                )
            using
                (
                    System.IO.TextReader reader = new System.IO.StreamReader(stream)
                )
            {
                content = await reader.ReadToEndAsync();
            }

            await Task.Run
                        (
                            () =>
                            {
                                for (int i = 0; i < this.MappingsNamespaces.Count; i++)
                                {
                                    string namepsace_old = this.MappingsNamespaces[i].OldAndroidSupport;
                                    string namepsace_new = this.MappingsNamespaces[i].NewAndroidX;


                                    string search = $"package[@name='{namepsace_old}']";
                                    string replace = null;

                                    AmbiguityFix
                                            (
                                                // filename for ambiguity fix - artifact based
                                                file, 
                                                //  content to be migrated
                                                ref content, 
                                                //  old namespace
                                                namepsace_old, 
                                                //  new namespace
                                                ref namepsace_new, 
                                                //  search string
                                                search, 
                                                //  replacement string
                                                ref replace
                                            );
                                }
                            }
                        );

            return content;
        }

        public async Task<string> MigrateEnumMethodsXmlFileNamespacesAsync(string file)
        {
            string content;
            using
                (
                    System.IO.FileStream stream = System.IO.File.Open
                                                                    (
                                                                        file,
                                                                        System.IO.FileMode.Open,
                                                                        System.IO.FileAccess.Read
                                                                    )
                )
            using
                (
                    System.IO.TextReader reader = new System.IO.StreamReader(stream)
                )
            {
                content = await reader.ReadToEndAsync();
            }

            await Task.Run
                        (
                            () =>
                            {
                                for (int i = 0; i < this.MappingsNamespaces.Count; i++)
                                {
                                    string namepsace_old = this.MappingsNamespaces[i].OldAndroidSupport;
                                    string namepsace_new = this.MappingsNamespaces[i].NewAndroidX;


                                    string search = $"package[@name='{namepsace_old}']";
                                    string replace = null;

                                    AmbiguityFix
                                            (
                                                // filename for ambiguity fix - artifact based
                                                file,
                                                //  content to be migrated
                                                ref content,
                                                //  old namespace
                                                namepsace_old,
                                                //  new namespace
                                                ref namepsace_new,
                                                //  search string
                                                search,
                                                //  replacement string
                                                ref replace
                                            );
                                }
                            }
                        );

            return content;
        }

        private void AmbiguityFix
                            (
                                string file, 
                                ref string content, 
                                string namepsace_old, 
                                ref string namepsace_new, 
                                string search, 
                                ref string replace
                            )
        {
            if (file.StartsWith("Metadata") && file.EndsWith(".xml"))
            {
                replace = $"package[@name='{namepsace_new}']";
            }
            else if (file.Contains("EnumMethods.xml"))
            {
                search = $"{namepsace_old}".Replace(".", "/");
            }
            else
            {
                string msg = $"Unknown file {file}";
                Console.WriteLine(msg);
            }

            if (content.Contains(search))
            {
                Console.WriteLine($"         Found         : {search}");

                if (namepsace_old == "android.support.v4.widget")
                {
                    // fixing ambigious mappings based on path to the artifact
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"         Ambiguity for        : {namepsace_old}");

                    if (file.Contains("swiperefreshlayout"))
                    {
                        namepsace_new = "androidx.swiperefreshlayout.widget";
                    }
                    else if (file.Contains("cursoradapter"))
                    {
                        namepsace_new = "androidx.cursoradapter.widget";
                    }
                    else if (file.Contains("coordinatorlayout"))
                    {
                        namepsace_new = "androidx.coordinatorlayout.widget";
                    }
                    else if (file.Contains("drawerlayout"))
                    {
                        namepsace_new = "androidx.drawerlayout.widget";
                    }
                    else if (file.Contains("customview"))
                    {
                        namepsace_new = "androidx.customview.widget";
                    }
                    else if (file.Contains("slidingpanelayout"))
                    {
                        namepsace_new = "androidx.slidingpanelayout.widget";
                    }
                    else
                    {
                        namepsace_new = "androidx.core.widget";
                    }
                    Console.WriteLine($"         Ambiguity fixed        : {namepsace_new}");
                    Console.ResetColor();
                }
                else if (namepsace_old == "android.support.v4.view")
                {
                    // fixing ambigious mappings based on path to the artifact
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"         Ambiguity for        : {namepsace_old}");

                    if (file.Contains("customview"))
                    {
                        namepsace_new = "androidx.customview.view";
                    }
                    if (file.Contains("viewpager"))
                    {
                        namepsace_new = "androidx.viewpager.widget";
                    }
                    else
                    {
                        string msg = $"Ambiguity not resolved for {namepsace_old}";
                        Console.WriteLine(msg);
                        //throw new InvalidDataException(msg);
                    }

                    Console.WriteLine($"         Ambiguity fixed        : {namepsace_new}");
                    Console.ResetColor();
                }

                if (file.StartsWith("Metadata") && file.EndsWith(".xml"))
                {
                    replace = $"package[@name='{namepsace_new}']";
                    Console.WriteLine($"         Replacing with: {replace}");
                }
                else if (file.Contains("EnumMethods.xml"))
                {
                    replace = $"{namepsace_new}".Replace(".", "/");
                    Console.WriteLine($"         Replacing with: {replace}");
                }
                else
                {
                    string msg = $"Unknown file {file}";
                    Console.WriteLine(msg);
                }

                content = content.Replace(search, replace);
            }

            return;
        }

        public string ContentApiInfoNew
        {
            get;
            set;
        }

        public string ContentApiInfoOld
        {
            get;
            set;
        }

        public string ApiInfoFileOld
        {
            get;
            set;
        }

        public string ApiInfoFileNew
        {
            get;
            set;
        }

        public ApiInfo ApiInfoDataNew
        {
            get;
            set;
        }

        public ApiInfo ApiInfoDataOld
        {
            get;
            set;
        }

        public async Task<ApiInfo> ApiInfo(string file_input)
        {
            StreamReader sr = null;
            XmlSerializer serializer = new XmlSerializer(typeof(ApiInfo));
            ApiInfo api_info;

            try
            {
                sr = new StreamReader(file_input);
                api_info = (ApiInfo)serializer.Deserialize(sr);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Exception deserializing {file_input}");
                Console.ResetColor();

                throw;
            }
            finally
            {
                serializer = null;
                sr.Close();
                sr = null;
            }

            try
            {
                sr = new StreamReader(file_input);
                ContentApiInfoNew = await sr.ReadToEndAsync();
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Exception reading {file_input}");
                Console.ResetColor();

                throw;
            }
            finally
            {
                sr.Close();
                sr = null;
            }

            return api_info;
        }



    }
}
