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
                IEnumerable <
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
                GoogleClassMappingsWithXamarin
            {
                get;
                protected set;
            }


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
                        results_complete;

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

                results_complete =
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
                    bool is_blacklisted_as = blacklisted_as.Contains(gm_as_pn);
                    bool is_blacklisted_ax = blacklisted_ax.Contains(gm_ax_pn);

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



                    // material fixes
                    if 
                        (
                            //gm_ax_cn_fq.Contains("com.google.android.material")
                            gm_ax_pn == "com.google.android.material"

                        )
                    {
                        ax_c = from ax_class in androidx
                                   where
                                        (
                                            gm_ax_cn_fq == $"{ax_class.JNIPackage}.{ax_class.ManagedClass}"
                                        )
                                   select
                                        ax_class
                                        ;
                        as_c = from as_class in android_support
                                    where
                                         (
                                             gm_as_cn_fq == $"{as_class.JNIPackage}.{as_class.ManagedClass}"
                                         )
                                    select
                                        as_class
                                        ;

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        try
                        {
                            var as_found = as_c.FirstOrDefault();
                            var ax_found = ax_c.FirstOrDefault();

                            if (as_found != (null, null, null, null))
                            {
                                class_name_fq_xamarin_android_support = $"{as_found.ManagedNamespace}.{as_found.ManagedClass}";
                                ns_xamarin_android_support = $"{as_found.ManagedNamespace}";
                            }
                            if (ax_found != (null, null, null, null))
                            {
                                class_name_fq_xamarin_androidx = $"{ax_found.ManagedNamespace}.{ax_found.ManagedClass}";
                                ns_xamarin_androidx = $"{ax_found.ManagedNamespace}";
                            }
                        }
                        catch(Exception exc)
                        {
                            string msg = exc.Message;
                        }
                        finally
                        {

                        }

                        Console.WriteLine($"MATERIAL found: {class_name_fq_xamarin_android_support}");
                        Console.WriteLine($"MATERIAL found: {class_name_fq_xamarin_androidx}");
                    }


                    if 
                    (
                        gm_as_cn_fq.Contains("android.support.transition.")
                        &&
                        gm_ax_cn_fq.Contains("androidx.transition.")
                    )
                    {
                        ax_c = from ax_class in androidx
                               where
                                    (
                                            gm_ax_cn_fq == $"{ax_class.JNIPackage}.{ax_class.ManagedClass}"
                                    )
                               select
                                    ax_class
                                        ;
                        as_c = from as_class in android_support
                               where
                                    (
                                             gm_as_cn_fq == $"{as_class.JNIPackage}.{as_class.ManagedClass}"
                                    )
                               select
                                   as_class
                                        ;

                        Console.ForegroundColor = ConsoleColor.Blue;
                        try
                        {
                            var as_found = as_c.FirstOrDefault();
                            var ax_found = ax_c.FirstOrDefault();

                            if (as_found != (null, null, null, null) )
                            {
                                class_name_fq_xamarin_android_support = $"{as_found.ManagedNamespace}.{as_found.ManagedClass}";
                                ns_xamarin_android_support = $"{as_found.ManagedNamespace}";
                            }
                            if (ax_found != (null, null, null, null))
                            {
                                class_name_fq_xamarin_androidx = $"{ax_found.ManagedNamespace}.{ax_found.ManagedClass}";
                                ns_xamarin_androidx = $"{ax_found.ManagedNamespace}";
                            }
                        }
                        catch (Exception exc)
                        {
                            string msg = exc.Message;
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"Transitons exception: {msg}");
                        }
                        finally
                        {

                        }
                        Console.WriteLine($"Transitons found: {class_name_fq_xamarin_android_support}");
                        Console.WriteLine($"Transitons found: {class_name_fq_xamarin_androidx}");
                    }

                    if
                    (
                        gm_as_cn_fq.StartsWith("android.databinding.")
                        &&
                        gm_ax_cn_fq.StartsWith("androidx.databinding.")
                    )
                    {
                        continue;
                    }

                    if
                    (
                        gm_as_cn_fq.EndsWith(".R")
                        &&
                        gm_ax_cn_fq.EndsWith(".R")
                    )
                    {
                        continue;
                    }

                    if
                    (
                        gm_as_cn_fq.StartsWith("android.support.test.")
                        &&
                        gm_ax_cn_fq.StartsWith("androidx.test.")
                    )
                    {
                        continue;
                    }


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
                            cn = as_c_found.ManagedClass;
                            ns_xamarin_android_support = $"{as_c_found.ManagedNamespace}";
                            ns_xamarin_androidx = $"{ax_c_found.ManagedNamespace}";
                            class_name_fq_xamarin_android_support = $"{ns_xamarin_android_support}.{cn}";
                            class_name_fq_xamarin_androidx = $"{ns_xamarin_androidx}.{cn}";

                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine($"merge class/type found: {cn} in BOTH Xamarin Android.Support and AndroidX ");
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

                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine($"merge class/type NOT found: {cn} in Xamarin Android.Support ");
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

                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine($"merge class/type NOT found: {cn} in Xamarin AndroidX ");
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

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"merge class/type NOT found: {cn} in either Xamarin Android.Support or AndroidX");
                        }
                    }
                    else if (n_results_as_c > 1 || n_results_ax_c > 1)
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
                    }
                    else
                    {
                        throw new InvalidProgramException("mc++ nothing covered");
                    }
                    Console.ResetColor();

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

                    if(class_name_fq_xamarin_android_support != null && class_name_fq_xamarin_androidx == null)
                    {
                        results_missing_in_xamarin_androidx.Add(result_found);
                    }
                    else if (class_name_fq_xamarin_android_support == null && class_name_fq_xamarin_androidx != null)
                    {
                        results_missing_in_xamarin_android_support.Add(result_found);
                    }
                    else if (class_name_fq_xamarin_android_support == null && class_name_fq_xamarin_androidx == null)
                    {
                        results_missing_completely.Add(result_found);
                    }
                    else 
                    {
                        results_complete.Add(result_found);
                    }
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

            public void DumpAPI(bool prettyfied = false)
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

                filename = Path.Combine(path_output, $"API.Mappings.Google.with.Xamarin.Classes");

                this.Dump(filename, this.GoogleClassMappingsWithXamarin, prettyfied);

                return;
            }

            private void Dump
                (
                    string filename,
                    IEnumerable
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
                        results_found,
                    bool prettyified = false
                )
            {
                string fmt = "{0},{1},{2},{3},{4},{5},{6},{7},{8}";
                string fmtp = "";
                if (prettyified == true)
                {
                    fmtp = GetDumpFormat(results_found);
                }

                StringBuilder sb = new StringBuilder();
                StringBuilder sbp = new StringBuilder();
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

                    sb.AppendLine
                        (
                            string.Format
                                    (
                                        fmt, 
                                        cn, as_cn, ax_cn, xas_cn, xax_cn, pn_as, pn_ax, ns_xas, ns_xax
                                    )
                        );
                    sbp.AppendLine
                        (
                            string.Format
                                    (
                                        fmtp,
                                        cn, as_cn, ax_cn, xas_cn, xax_cn, pn_as, pn_ax, ns_xas, ns_xax
                                    )
                        );
                }

                if (prettyified == true)
                {
                    System.IO.File.WriteAllText($"{filename}.prettyfied.csv", sbp.ToString());
                }
                System.IO.File.WriteAllText($"{filename}.csv", sb.ToString());

                return;
            }

            private string GetDumpFormat
                (
                    IEnumerable
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
                        results_found,
                    int padding = 3
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
                return fmt;
            }
        }
    }
}
