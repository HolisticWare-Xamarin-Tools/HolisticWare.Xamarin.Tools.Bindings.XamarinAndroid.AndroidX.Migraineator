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
        public MappingManager()
        {
        }

        static Task task_load_google_artifact_mappings;
        static Task task_load_google_class_mappings;
        static Task task_process_google_package_mappings;
        static Task task_load_android_packages_blacklisted;

        public static async Task InitializeAsync(string path_working_directory)
        {
            task_load_google_class_mappings = LoadGoogleArtifactMappings(path_working_directory);
            task_load_google_artifact_mappings = LoadGoogleClassMappings(path_working_directory);
            task_load_android_packages_blacklisted= LoadAndroidPackagesBlackList(path_working_directory);
            task_process_google_package_mappings = ProcessGooglePackageMappings();

            return;
        }

        static string path_mappings = Path.Combine("mappings");
        static string file = null;

        //-------------------------------------------------------------------------------------------------------------------
        // Google provided mappings for Artifacts
        public static
            ReadOnlyCollection<
                                    (
                                        string ArtifactAndroidSupport,
                                        string ArtifactAndroidX
                                    )
                                >
                GoogleArtifactMappings
        {
            get;
            private set;
        }

        public static async
            Task
                LoadGoogleArtifactMappings(string path_working_directory)
        {
            string[] path = new string[]
            {
                path_working_directory,
                path_mappings,
                "google-readonly-1-baseline",
                "androidx-artifact-mapping.csv",
            };
            file = Path.Combine(path);

            CharacterSeparatedValues csv = new CharacterSeparatedValues();
            string content = await csv.LoadAsync(file);

            IEnumerable<string[]> mapping = csv
                                            .ParseTemporaryImplementation()
                                            .ToList()
                                            ;
            IEnumerable<
                            (
                                string AndroidSupportArtifact,
                                string AndroidXArtifact
                            )
                        > mapping_strongly_typed = Convert_GoogleArtifactMappings(mapping);

            GoogleArtifactMappings = mapping_strongly_typed.ToList().AsReadOnly();

            return;
        }

        private static
            IEnumerable<
                            (
                                string AndroidSupportArtifact,
                                string AndroidXArtifact
                            )
                        >
                Convert_GoogleArtifactMappings(IEnumerable<string[]> untyped_data)
        {
            foreach (string[] row in untyped_data)
            {
                yield return
                        (
                            AndroidSupportArtifact: row[0],
                            AndroidXArtifact: row[1]
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------------
        public static
            ReadOnlyCollection<
                                    (
                                        string AndroidSupportClass,
                                        string AndroidXClass
                                    )
                                >
                GoogleClassMappings
        {
            get;
            private set;
        }

        public static async
            Task
                LoadGoogleClassMappings(string path_working_directory)
        {
            string[] path = new string[]
            {
                path_working_directory,
                path_mappings,
                "google-readonly-1-baseline",
                "androidx-class-mapping.csv",
            };
            file = Path.Combine(path);

            CharacterSeparatedValues csv = new CharacterSeparatedValues();
            string content = csv.LoadAsync(file).Result; // cannot await need this ASAP

            IEnumerable<string[]> mapping = csv
                                            .ParseTemporaryImplementation()
                                            .ToList()
                                            ;
            IEnumerable<
                            (
                                string AndroidSupportClass,
                                string AndroidXClass
                            )
                        > mapping_strongly_typed = Convert_GoogleClassMappings(mapping);

            GoogleClassMappings = mapping_strongly_typed.ToList().AsReadOnly();

            return;
        }

        private static
            IEnumerable<
                            (
                                string AndroidSupportClass,
                                string AndroidXClass
                            )
                        >
                Convert_GoogleClassMappings(IEnumerable<string[]> untyped_data)
        {
            foreach (string[] row in untyped_data)
            {
                yield return
                        (
                            AndroidSupportClass: row[0],
                            AndroidXClass: row[1]
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------------
        public static
            ReadOnlyCollection<
                                    (
                                        string AndroidSupportClass,
                                        string AndroidXClass
                                    )
                                >
                GoogleClassMappingsPrettyfied
        {
            get;
            private set;
        }

        public static async
            Task
                LoadGoogleClassMappingsPrettyfied(string path_working_directory)
        {
            string[] path = new string[]
            {
                path_working_directory,
                path_mappings,
                "google-readonly-2-baseline-prettyfied",
                "androidx-class-mapping.csv",
            };
            file = Path.Combine(path);

            CharacterSeparatedValues csv = new CharacterSeparatedValues();
            string content = await csv.LoadAsync(file);

            IEnumerable<string[]> mapping = csv
                                            .ParseTemporaryImplementation()
                                            .ToList()
                                            ;
            IEnumerable<
                            (
                                string AndroidSupportClass,
                                string AndroidXClass
                            )
                        > mapping_strongly_typed = Convert_GoogleClassMappingsPrettyfied(mapping);

            GoogleClassMappingsPrettyfied = mapping_strongly_typed.ToList().AsReadOnly();

            return;
        }

        private static
            IEnumerable<
                            (
                                string AndroidSupportClass,
                                string AndroidXClass
                            )
                        >
                Convert_GoogleClassMappingsPrettyfied(IEnumerable<string[]> untyped_data)
        {
            foreach (string[] row in untyped_data)
            {
                yield return
                        (
                            AndroidSupportClass: row[0],
                            // skip column 1 - emptyt one!!
                            AndroidXClass: row[2]
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------
        public static
            ReadOnlyCollection<
                                    (
                                        string Action,
                                        string AndroidSupportPackage,
                                        string AndroidXPackage
                                    )
                                >
                AndroidPackagesBlackList
        {
            get;
            private set;
        }

        public static async
            Task
                LoadAndroidPackagesBlackList(string path_working_directory)
        {
            string[] path = new string[]
            {
                path_working_directory,
                path_mappings,
                "xamarin-business",
                "packagenames-for-exclusion.csv",
            };
            file = Path.Combine(path);

            CharacterSeparatedValues csv = new CharacterSeparatedValues();
            string content = csv.LoadAsync(file).Result; // cannot await need this ASAP

            IEnumerable<string[]> mapping = csv
                                            .ParseTemporaryImplementation()
                                            .ToList()
                                            ;
            IEnumerable<
                            (
                                string Action,
                                string AndroidSupportPackage,
                                string AndroidXPackage
                            )
                        > mapping_strongly_typed = Convert_AndroidPackagesBlackList(mapping);

            AndroidPackagesBlackList = mapping_strongly_typed
                                                .Skip(1)
                                                .ToList()
                                                .AsReadOnly()
                                                ;

            return;
        }

        private static
            IEnumerable<
                            (
                                string Action,
                                string AndroidSupportPackage,
                                string AndroidXPackage
                            )
                        >
                Convert_AndroidPackagesBlackList(IEnumerable<string[]> untyped_data)
        {
            foreach (string[] row in untyped_data)
            {
                yield return
                        (
                            Action: "skip",
                            AndroidSupportPackage: row[0],
                            AndroidXPackage: row[2]
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
    }
}
