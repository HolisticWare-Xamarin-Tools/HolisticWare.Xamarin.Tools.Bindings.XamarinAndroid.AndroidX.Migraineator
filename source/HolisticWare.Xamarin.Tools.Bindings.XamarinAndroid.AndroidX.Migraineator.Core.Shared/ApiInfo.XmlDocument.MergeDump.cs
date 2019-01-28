using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Text;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core.Generated;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core
{
    public partial class ApiInfo
    {
        public partial class XmlDocumentData
        {
            public void Dump()
            {
                if (null == comparison_results)
                {
                    //this.comparison_results = Merge();
                }

                //this.Dump(comparison_results);
                //this.Dump(results_found);
                //this.Dump(packages_namespaces);
                //this.Dump(results_not_found_at_all, "NotFoundAtAll");
                //this.Dump(results_not_found_in_xas, "NotFoundInXamarinAndroidSupport");
                //this.Dump(results_not_found_in_xax, "NotFoundInXamarinAndroidX");

                return;
            }

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
                > comparison_results = null;



            private void Dump
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
                    bool prettyified = false
                )
            {
                string fmt = "";
                if (prettyified == true)
                {
                    fmt = GetDumpFormat(results_found);
                }

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

                    sb.AppendLine(string.Format(fmt, cn, as_cn, ax_cn, xas_cn, xax_cn, pn_as, pn_ax, ns_xas, ns_xax));
                }

                if (prettyified == true)
                {
                    System.IO.File.WriteAllText($"API.Classes.prettyfied.csv", sb.ToString());
                }
                else
                {
                    System.IO.File.WriteAllText($"API.Classes.csv", sb.ToString());
                }

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
                        results_found,
                    bool prettyified = false
                )
            {

                string fmt = "";
                if (prettyified == true)
                {
                    fmt = GetDumpFormat(results_found);
                }

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

                if (prettyified == true)
                {
                    System.IO.File.WriteAllText($"API.PackagesNamespaces.prettyfied.csv", sb_final.ToString());
                }
                else
                {
                    System.IO.File.WriteAllText($"API.Classes.csv", sb.ToString());
                }

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


            private string GetDumpFormat
                (
                    IEnumerable
                        <
                            (
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

                return fmt;
            }


        }
    }
}
