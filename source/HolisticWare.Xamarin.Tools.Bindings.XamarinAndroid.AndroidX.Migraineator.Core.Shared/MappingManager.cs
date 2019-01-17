using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Core.Text;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core.Shared
{
    public partial class MappingManager
    {
        public MappingManager()
        {
        }

        Task task_load_google_artifact_mappings;
        Task task_load_google_class_mappings;

        public async Task InitializeAsync(string path)
        {
            task_load_google_artifact_mappings = LoadGoogleClassMappings();
            task_load_google_class_mappings = LoadGoogleArtifactMappings();

            return;
        }

        string path_mappings = Path.Combine("mappings");
        string file = null;

        //-------------------------------------------------------------------------------------------------------------------
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
                LoadGoogleArtifactMappings()
        {
            file = Path.Combine(path_mappings, "androidx-artifact-mapping.csv");

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
                            AndroidXArtifact: row[2]
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
                LoadGoogleClassMappings()
        {
            file = Path.Combine(path_mappings, "androidx-class-mapping.csv");

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
                        > mapping_strongly_typed = Convert_GoogleArtifactMappings(mapping);

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

    }
}
