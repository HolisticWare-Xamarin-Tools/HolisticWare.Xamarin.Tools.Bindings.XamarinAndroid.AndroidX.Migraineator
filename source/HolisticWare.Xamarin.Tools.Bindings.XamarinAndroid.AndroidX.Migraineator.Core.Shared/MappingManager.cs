using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Core.Text;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core.Generated;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core
{
    public partial class MappingManager
    {
        public MappingManager()
        {
        }

        Task task_load_google_artifact_mappings;
        Task task_load_google_class_mappings;

        public async Task InitializeAsync(string path_working_directory)
        {
            task_load_google_artifact_mappings = LoadGoogleClassMappings(path_working_directory);
            task_load_google_class_mappings = LoadGoogleArtifactMappings(path_working_directory);

            return;
        }

        string path_mappings = Path.Combine("mappings");
        string file = null;

        //-------------------------------------------------------------------------------------------------------------------
        // Google provided mappings for Artifacts
        public
            ReadOnlyCollection<
                                    (
                                        string AndroidSupportArtifact,
                                        string AndroidXArtifact
                                    )
                                >
                GoogleArtifactMappings
        {
            get;
            private set;
        }

        public async
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

        private
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
        public
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

        public async
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
                        > mapping_strongly_typed = Convert_GoogleClassMappings(mapping);

            GoogleClassMappings = mapping_strongly_typed.ToList().AsReadOnly();

            return;
        }

        private
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
        public
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

        public async
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

        private
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
    }
}
