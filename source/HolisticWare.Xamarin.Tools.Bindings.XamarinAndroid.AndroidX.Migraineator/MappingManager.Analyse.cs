using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Core.Text;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Generated;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator
{
    public partial class MappingManager
    {
        //-------------------------------------------------------------------------------------------------------------------
        // Google provided mappings for Artifacts
        public static
            ReadOnlyCollection<
                                    (
                                string PackageAndroidSupport,
                                string PackageAndroidX
                                    )
                                >
                GoogleDerivedPackageMappings
        {
            get;
            private set;
        }

        public static async
            Task
               ProcessGooglePackageMappings()
        {
            IEnumerable<
                            (
                                string AndroidSupportArtifact,
                                string AndroidXArtifact
                            )
                        > mapping_strongly_typed = ProcessPackageNames();

            GoogleDerivedPackageMappings = mapping_strongly_typed.ToList().AsReadOnly();

            return;
        }


        protected static
            IEnumerable<
                            (
                                string PackageAndroidSupport,
                                string PackageAndroidX
                            )
                        > 
                        ProcessPackageNames()
        {
            task_load_google_class_mappings.Wait();

            foreach
            (
                (
                    string AndroidSupportClass,
                    string AndroidXClass
                ) gm
                in GoogleClassMappings
            )
            {
                string gm_as_cn_fq = gm.AndroidSupportClass;
                string gm_ax_cn_fq = gm.AndroidXClass;

                if (gm_as_cn_fq == "Support Library class" && gm_ax_cn_fq == "Android X class")
                {
                    // header ignore
                    continue;
                }

                int gm_as_cn_packagename_end = gm_as_cn_fq.LastIndexOf('.');
                int gm_ax_cn_packagename_end = gm_ax_cn_fq.LastIndexOf('.');

                // packagenames
                string gm_as_pn = gm.AndroidSupportClass.Substring(0, gm_as_cn_packagename_end);
                string gm_ax_pn = gm.AndroidXClass.Substring(0, gm_ax_cn_packagename_end);

                yield return
                            (
                                PackageAndroidSupport: gm_as_pn,
                                PackageAndroidX: gm_ax_pn
                            );
            }

        }
        //-------------------------------------------------------------------------------------------------------------------

        public async static Task DumpPackageNamesAsync()
        {
            int length_pn_as_max = int.MinValue;
            int length_pn_ax_max = int.MinValue;

            task_process_google_package_mappings.Wait();

            foreach
                (
                    (
                        string PackageAndroidSupport,
                        string PackageAndroidX
                    ) package_mapping
                    in GoogleDerivedPackageMappings
                )
            {
                int lpnas = package_mapping.PackageAndroidSupport.Length;
                if (lpnas > length_pn_as_max)
                {
                    length_pn_as_max = lpnas;
                }
                int lpnax = package_mapping.PackageAndroidX.Length;
                if (lpnax > length_pn_ax_max)
                {
                    length_pn_ax_max = lpnax;
                }
            }

            int padding = 3;
            string fmt =
                    "{0,-" + (length_pn_as_max + padding) + "}"
                    +
                    ",{1}" // ,-" + (length_pn_ax_max + padding) + "}"
                    //+
                    //",{2}"
                    ;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach
                (
                    (
                        string PackageAndroidSupport,
                        string PackageAndroidX
                    ) pn
                    in GoogleDerivedPackageMappings
                )
            {
                string pnas = pn.PackageAndroidSupport;
                string pnax = pn.PackageAndroidX;

                sb.AppendLine(string.Format(fmt, pnas, pnax));
            }

            string[] lines = sb.ToString().Split
                                            (
                                                new string[] { Environment.NewLine },
                                                StringSplitOptions.RemoveEmptyEntries
                                            );

            IEnumerable<string> lines_normalized = lines.Distinct();

            //.............................................................................
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
            path_output = Path.Combine(path_output, "androidx-packagename-mapping.csv");
            System.IO.File.WriteAllText(path_output, sb.ToString());
            //.............................................................................

            return;
        }
    }

}
