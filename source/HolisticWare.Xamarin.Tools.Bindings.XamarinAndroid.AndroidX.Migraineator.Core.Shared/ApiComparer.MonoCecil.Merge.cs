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
                                ReadOnlyCollection<
                                                        (
                                                            string Action,
                                                            string AndroidSupportPackage,
                                                            string AndroidXPackage
                                                        )
                                                    > blacklisted_packages,
                                IEnumerable<
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

                results_missing_in_xamarin_android_support =
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

                results_missing_in_xamarin_androidx =
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

                results_missing_completely =
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
                results_not_found_in_xas =
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
                results_not_found_in_xax =
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

                results_missing_in_xamarin_android_support_cn = new List<string>();

                results_missing_in_xamarin_androidx_cn = new List<string>();

                results_found_material =
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
                //--------------------------------------------------------------------------------------------


                //--------------------------------------------------------------------------------------------
                // preparing blacklisted packagenames (Bindings not done yet or will never be done)
                var blacklisted_ax = from blp in blacklisted_packages
                                         //where
                                         //(
                                         //    blp.Action == "skip-continue"
                                         //)
                                     select
                                              blp.AndroidXPackage
                                           ;
                var blacklisted_as = from blp in blacklisted_packages
                                         //where
                                         //(
                                         //    blp.Action == "skip-continue"
                                         //)
                                     select
                                              blp.AndroidSupportPackage
                                           ;
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
                        string AndroidXClass,
                        string AndroidSupportClassFullyQualified,
                        string AndroidXClassFullyQualified,
                        // formatting space
                        string PackageAndroidSupport,
                        string PackageAndroidX,
                        string ManagedNamespaceXamarinAndroidSupport,
                        string ManagedNamespaceXamarinAndroidX
                    )
                        result_not_found_in_xas;
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
                        result_not_found_in_xax;
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
                        result_not_found_at_all;



                    string cn = null;
                    string class_name_fq_xamarin_android_support = null;
                    string class_name_fq_xamarin_androidx = null;
                    string ns_xamarin_android_support = null;
                    string ns_xamarin_androidx = null;
                    string gm_as_cn = null;
                    string gm_ax_cn = null;
                    string gm_as_pn = null;
                    string gm_ax_pn = null;


                    int gm_as_cn_packagename_end = gm_as_cn_fq.LastIndexOf('.');
                    int gm_ax_cn_packagename_end = gm_ax_cn_fq.LastIndexOf('.');

                    // packagenames
                    gm_as_pn = gm.AndroidSupportClass.Substring(0, gm_as_cn_packagename_end);
                    gm_ax_pn = gm.AndroidXClass.Substring(0, gm_ax_cn_packagename_end);

                    // classnames
                    gm_as_cn = gm.AndroidSupportClass.Replace($"{gm_as_pn}.", "");
                    gm_ax_cn = gm.AndroidXClass.Replace($"{gm_ax_pn}.", "");


                    //---------------------------------------------------------------------------------------
                    // skip blacklisted
                    bool is_blacklisted_as = blacklisted_as.Any(pn => gm_as_pn.StartsWith(pn));
                    bool is_blacklisted_ax = blacklisted_ax.Any(pn => gm_ax_pn.StartsWith(pn));

                    if (is_blacklisted_as && is_blacklisted_ax)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"skipping:");
                        Console.WriteLine($"          Android.Support: {gm_as_pn}");
                        Console.WriteLine($"          AndroidX       : {gm_ax_pn}");
                        Console.ResetColor();

                        continue;
                    }
                    //---------------------------------------------------------------------------------------

                    var as_c = from as_class in android_support
                               where
                                    (
                                        gm_as_cn_fq == $"{as_class.JNIPackage}.{as_class.ManagedClass}"
                                    )
                               select
                                        as_class
                               ;

                    var ax_c = from ax_class in androidx
                               where
                                    (
                                        gm_ax_cn_fq == $"{ax_class.JNIPackage}.{ax_class.ManagedClass}"
                                    )
                               select
                                        ax_class
                               ;


                    int n_results_as_c = as_c.Count();
                    int n_results_ax_c = ax_c.Count();


                    if (gm_as_cn != gm_ax_cn)
                    {
                        // Google changed class name too
                        cn = "ClassName differs by Google!";
                    }
                    else
                    {
                        cn = gm_ax_cn;
                    }

                    var as_c_found = as_c.FirstOrDefault();
                    var ax_c_found = ax_c.FirstOrDefault();


                    if (n_results_as_c == 1 && n_results_ax_c == 1)
                    {
                        // found exact match
                        if (as_c_found.ManagedClass == ax_c_found.ManagedClass)
                        {
                            cn = ax_c_found.ManagedClass; // cn = gm_as_cn; cn = gm_ax_cn;
                            ns_xamarin_android_support = $"{as_c_found.ManagedNamespace}";
                            ns_xamarin_androidx = $"{ax_c_found.ManagedNamespace}";
                            class_name_fq_xamarin_android_support = $"{ns_xamarin_android_support}.{cn}";
                            class_name_fq_xamarin_androidx = $"{ns_xamarin_androidx}.{cn}";

                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine($"merge class/type found: {cn} in BOTH Xamarin Android.Support and AndroidX ");
                            Console.WriteLine($"            Android.Support: {ns_xamarin_android_support}.{cn}");
                            Console.WriteLine($"            Android.Support: {gm_as_pn}.{cn}");
                            Console.WriteLine($"            AndroidX       : {ns_xamarin_androidx}.{cn}");
                            Console.WriteLine($"            AndroidX       : {gm_ax_pn}.{cn}");
                            if (gm_ax_pn.Contains("material"))
                            {
                                Console.WriteLine($"            MATERIAL");
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
                            results_found.Add(result_found);
                        }
                    }
                    else if (n_results_as_c == 0 && n_results_ax_c == 1)
                    {
                        cn = ax_c_found.ManagedClass; // cn = gm_as_cn; cn = gm_ax_cn;
                        ns_xamarin_android_support = null;
                        ns_xamarin_androidx = $"{ax_c_found.ManagedNamespace}";
                        class_name_fq_xamarin_android_support = null;
                        class_name_fq_xamarin_androidx = $"{ns_xamarin_androidx}.{cn}";

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
                        result_not_found_in_xas = result_found;

                        results_not_found_in_xas.Add(result_not_found_in_xas);
                        results_missing_in_xamarin_android_support_cn.Add(cn);

                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"merge class/type NOT found: {cn} in Xamarin Android.Support ");
                        Console.WriteLine($"            Android.Support: {ns_xamarin_android_support}.{cn}");
                        Console.WriteLine($"            Android.Support: {gm_as_pn}.{cn}");
                        Console.WriteLine($"            AndroidX       : {ns_xamarin_androidx}.{cn}");
                        Console.WriteLine($"            AndroidX       : {gm_ax_pn}.{cn}");
                        if (gm_ax_pn.Contains("material"))
                        {
                            Console.WriteLine($"            MATERIAL");
                        }

                        results_found.Add(result_found);
                    }
                    else if (n_results_as_c == 1 && n_results_ax_c == 0)
                    {
                        cn = as_c_found.ManagedClass; // cn = gm_as_cn; cn = gm_ax_cn;
                        ns_xamarin_android_support = $"{as_c_found.ManagedNamespace}";
                        ns_xamarin_androidx = null;
                        class_name_fq_xamarin_android_support = $"{ns_xamarin_android_support}.{cn}";
                        class_name_fq_xamarin_androidx = null;

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
                        result_not_found_in_xax = result_found;

                        results_not_found_in_xax.Add(result_not_found_in_xax);
                        results_missing_in_xamarin_androidx_cn.Add(cn);

                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"merge class/type NOT found: {cn} in Xamarin AndroidX ");
                        Console.WriteLine($"            Android.Support: {ns_xamarin_android_support}.{cn}");
                        Console.WriteLine($"            Android.Support: {gm_as_pn}.{cn}");
                        Console.WriteLine($"            AndroidX       : {ns_xamarin_androidx}.{cn}");
                        Console.WriteLine($"            AndroidX       : {gm_ax_pn}.{cn}");
                        if (gm_ax_pn.Contains("material"))
                        {
                            Console.WriteLine($"            MATERIAL");
                        }

                        results_found.Add(result_found);
                    }
                    else if (n_results_as_c == 0 && n_results_ax_c == 0)
                    {
                        cn = as_c_found.ManagedClass;
                        ns_xamarin_android_support = null;
                        ns_xamarin_androidx = null;
                        class_name_fq_xamarin_android_support = null;
                        class_name_fq_xamarin_androidx = null;

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
                        result_not_found_at_all = result_found;

                        results_not_found_at_all.Add(result_not_found_at_all);
                        results_missing_in_xamarin_androidx_cn.Add(cn);
                        results_missing_in_xamarin_android_support_cn.Add(cn);

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"merge class/type NOT found: {cn} in either Xamarin Android.Support or AndroidX");
                        Console.WriteLine($"            Android.Support: {ns_xamarin_android_support}.{cn}");
                        Console.WriteLine($"            Android.Support: {gm_as_pn}.{cn}");
                        Console.WriteLine($"            AndroidX       : {ns_xamarin_androidx}.{cn}");
                        Console.WriteLine($"            AndroidX       : {gm_ax_pn}.{cn}");

                        results_found.Add(result_found);
                    }
                    else 
                    {
                        // found ambiguous (more results)

                        string msg = "mc++ ambiugity";
                        //throw new InvalidProgramException(msg);
                        class_name_fq_xamarin_android_support = msg;
                        class_name_fq_xamarin_androidx = msg;
                        ns_xamarin_android_support = msg;
                        ns_xamarin_androidx = msg;

                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine($"merge class/type AMBIGUITY found: {cn}");

                        throw new InvalidProgramException("mc++ nothing covered");
                    }

                    Console.ResetColor();

                    package_namespace =
                                (
                                    PackageAndroidSupport: gm_as_pn,
                                    PackageAndroidX: gm_ax_pn,
                                    ManagedNamespaceXamarinAndroidSupport: ns_xamarin_android_support,
                                    ManagedNamespaceXamarinAndroidX: ns_xamarin_androidx
                                );


                    //yield return result_found;

                    packages_namespaces.Add(package_namespace);

                    //yield return
                    //(
                    //    AndroidSupportClass: gm_as_cn_fq,
                    //    AndroidXClass: ax_cn_fq
                    //);
                }

                var packages_namespaces_normalized = packages_namespaces.Distinct();

                GoogleClassMappingsWithXamarin = results_found;

                return results_found;
            }

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
                    results_missing_in_xamarin_androidx;

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
                    results_missing_in_xamarin_android_support;

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
                    results_missing_completely;
                
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
                    results_not_found_at_all;

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
                    results_not_found_in_xas;

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
                    results_not_found_in_xax;



            List<string> results_missing_in_xamarin_androidx_cn;

            List<string> results_missing_in_xamarin_android_support_cn;

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
                    results_found_material;

            //--------------------------------------------------------------------------------------------

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
                        MergeGoogleMappingsMaterial
                            (
                                ReadOnlyCollection<
                                                        (
                                                            string AndroidSupportClass,
                                                            string AndroidXClass
                                                        )
                                                    > google_class_mappings,
                                ReadOnlyCollection<
                                                        (
                                                            string Action,
                                                            string AndroidSupportPackage,
                                                            string AndroidXPackage
                                                        )
                                                    > blacklisted_packages,
                                IEnumerable<
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

                return results_found_material;
            }
        }
    }
}
