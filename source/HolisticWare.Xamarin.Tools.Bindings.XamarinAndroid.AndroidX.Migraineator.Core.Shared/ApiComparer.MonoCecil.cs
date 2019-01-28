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
        public partial class MonoCecilData
        {
            public
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
                        MergeGoogleMappings
                            (
                                ReadOnlyCollection<
                                                        (
                                                            string AndroidSupportClass,
                                                            string AndroidXClass
                                                        )
                                                    > google_class_mappings,
                                IEnumerable <
                                                (
                                                    string ManagedClass,
                                                    string ManagedNamespace,
                                                    string JNIPackage,
                                                    string JNIType
                                                )
                                            > android_support,
                                IEnumerable<
                                                (
                                                    string ManagedClass,
                                                    string ManagedNamespace,
                                                    string JNIPackage,
                                                    string JNIType
                                                )
                                            > androidx
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


                    var as_c = from as_class in android_support
                               where
                                    (
                                        gm_as_cn_fq == $"{as_class.ManagedNamespace.ToLower()}.{as_class.ManagedClass}"
                                    )
                               select
                                        as_class
                               ;

                    var ax_c = from ax_class in androidx
                               where
                                    (
                                        gm_ax_cn_fq == $"{ax_class.ManagedNamespace.ToLower()}.{ax_class.ManagedClass}"
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
                        if (as_c.First().ManagedClass == ax_c.First().ManagedClass)
                        {
                            cn = as_c.First().ManagedClass;
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

                return results_found;
            }
        }
    }
}
