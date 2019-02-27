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
        public void DumpToFiles(ApiInfo api_info, string version)
        {
            (
                List<string> namespaces,
                List<string> namespaces_new_suspicious,
                List<string> namespaces_old_suspicious,
                List< (string ClassName, string ClassNameFullyQualified)> classes
            ) = this.Analyse(api_info);


            Parallel.Invoke
                (
                    () =>
                    {
                        System.IO.File.WriteAllLines
                                            (
                                                $"namespaces_{version}.txt", 
                                                namespaces
                                            );
                    },
                    () =>
                    {
                        System.IO.File.WriteAllLines
                                            (
                                                $"namespaces_new_suspicious_{version}.txt", 
                                                namespaces_new_suspicious
                                            );
                    },
                    () =>
                    {
                        System.IO.File.WriteAllLines
                                            (
                                                $"namespaces_old_suspicious_{version}.txt", 
                                                namespaces_old_suspicious
                                            );
                    },
                    () =>
                    {
                        //System.IO.File.WriteAllLines
                                            //(
                                            //    $"classes_{version}.txt", 
                                            //    classes
                                            //);
                    }
                );

            return;

        }

        public void DumpMappingAndroidArtifacts_X_ManagedAssemblies(bool prettyfied = false)
        {
            string fmt = "{0},{1},{2},{3}";
            string fmtp = string.Intern("");
            if (prettyfied == true)
            {
                fmtp = GetDumpFormat(MappingAndroidArtifacts_X_ManagedAssemblies);
            }

            StringBuilder sb = new StringBuilder();
            StringBuilder sbp = new StringBuilder();

            foreach
            (
                (
                    string AtifactAndroidSupport,
                    string AtifactAndroidX,
                    string ManagedAssemblyAndroidSupport,
                    string ManagedAssemblyAndroidX
                ) 
                mapping 
                in MappingAndroidArtifacts_X_ManagedAssemblies
            )
            {
                string a_as = mapping.AtifactAndroidSupport;
                string a_ax = mapping.AtifactAndroidX;
                string ma_as = mapping.ManagedAssemblyAndroidSupport;
                string ma_ax = mapping.ManagedAssemblyAndroidX;

                sb.AppendLine
                    (
                        string.Format
                                (
                                    fmt,
                                    a_as, a_ax, ma_as, ma_ax 
                                )
                    );
                sbp.AppendLine
                    (
                        string.Format
                                (
                                    fmtp,
                                    a_as, a_ax, ma_as, ma_ax
                                )
                    );
            }
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
            string path_output = Path.Combine(path, "analysis");
            if (!Directory.Exists(path_output))
            {
                Directory.CreateDirectory(path_output);
            }

            string filename = null;

            filename = Path.Combine(path_output, "MappingAndroidArtifacts_X_ManagedAssemblies");

            if (prettyfied == true)
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
                            string AtifactAndroidSupport,
                            string AtifactAndroidX,
                            string ManagedAssemblyAndroidSupport,
                            string ManagedAssemblyAndroidX
                        )
                    >
                    results_found,
                int padding = 3
            )
        {
            int length_aas = 0;
            int length_aas_max = 0;

            int length_aax = 0;
            int length_aax_max = 0;

            int length_maas = 0;
            int length_maas_max = 0;

            int length_maax = 0;
            int length_maax_max = 0;

            foreach
                (
                    (
                        string AtifactAndroidSupport,
                        string AtifactAndroidX,
                        string ManagedAssemblyAndroidSupport,
                        string ManagedAssemblyAndroidX
                    ) mapping
                    in MappingAndroidArtifacts_X_ManagedAssemblies
                )
            {
                if (null == mapping.AtifactAndroidSupport)
                {
                    length_aas = 0;
                }
                else
                {
                    length_aas = mapping.AtifactAndroidSupport.Length;
                }
                if (length_aas > length_aas_max)
                {
                    length_aas_max = length_aas;
                }

                if (null == mapping.AtifactAndroidX)
                {
                    length_aax = 0;
                }
                else
                {
                    length_aax = mapping.AtifactAndroidSupport.Length;
                }
                if (length_aax > length_aax_max)
                {
                    length_aax_max = length_aax;
                }

                if (null == mapping.ManagedAssemblyAndroidSupport)
                {
                    length_maas = 0;
                }
                else
                {
                    length_maas = mapping.ManagedAssemblyAndroidSupport.Length;
                }
                if (length_maas > length_maas_max)
                {
                    length_maas_max = length_maas;
                }

                if (null == mapping.ManagedAssemblyAndroidX)
                {
                    length_maax = 0;
                }
                else
                {
                    length_maax = mapping.ManagedAssemblyAndroidX.Length;
                }
                if (length_maax > length_maax_max)
                {
                    length_maax_max = length_maax;
                }

            }
            string fmt =
                    "{0,-" + (length_aas_max + padding) + "}"
                    +
                    ",{1,-" + (length_aax_max + padding) + "}"
                    +
                    ",{2,-" + (length_maas_max + padding) + "}"
                    +
                    ",{3}"
                        ;

            return fmt;
        }
    }
}
