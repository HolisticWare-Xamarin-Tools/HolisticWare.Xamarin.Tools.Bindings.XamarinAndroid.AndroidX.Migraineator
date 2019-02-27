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
    public partial class ApiComparer
    {
        public partial class XmlDocumentData
        {
            public void Dump(bool prettyfied = false)
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
                string path_output = Path.Combine(path, "XmlDocument");
                if (!Directory.Exists(path_output))
                {
                    Directory.CreateDirectory(path_output);
                }

                string filename = null;

                Parallel.Invoke
                    (
                        () =>
                        {
                            filename = Path.Combine(path_output, $"API.Mappings.Merged.Google.with.Xamarin.Classes");
                            this.Dump
                                    (
                                        filename,
                                        //this.GoogleClassMappingsWithXamarin, 
                                        results_found,
                                        prettyfied
                                    );
                        },
                        () =>
                        {
                            filename = Path.Combine(path_output, $"API.not.found.at.all.in.Xamarin");
                            this.Dump
                                    (
                                        filename,
                                        //this.GoogleClassMappingsWithXamarin, 
                                        results_not_found_at_all,
                                        prettyfied
                                    );
                        },
                        () =>
                        {
                            filename = Path.Combine(path_output, $"API.not.found.in.Xamarin.Android.Support");
                            this.Dump
                                    (
                                        filename,
                                        results_not_found_in_xas,
                                        prettyfied
                                    );
                        },
                        () =>
                        {
                            filename = Path.Combine(path_output, $"API.not.found.in.Xamarin.AndroidX");
                            this.Dump
                                    (
                                        filename,
                                        results_not_found_in_xax,
                                        prettyfied
                                    );
                        }
                        //() =>
                        //{
                        //    filename = Path.Combine(path_output, $"API.not.found.in.Xamarin.AndroidX");
                        //    this.Dump
                        //            (
                        //                filename,
                        //                results_found_material,
                        //                prettyfied
                        //            );
                        //}
                    );

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
                if(null == results_found)
                {
                    return;
                }

                string fmt = "{0},{1},{2},{3},{4},{5},{6},{7},{8}";
                string fmtp = string.Intern("");
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
                if(null == results_found)
                {
                    return null;
                }

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
