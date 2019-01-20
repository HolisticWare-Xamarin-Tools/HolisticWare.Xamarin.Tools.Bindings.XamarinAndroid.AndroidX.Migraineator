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
        public
            IEnumerable<
                            (
                                string AndroidSupportArtifact,
                                string AndroidXArtifact,
                                string ManagedAssemblyAndroidSupport
                            )
                        > 
                        Merge_Old_AndroidSupport
                            (
                                ReadOnlyCollection  <
                                                        (
                                                            string AndroidSupportArtifact, 
                                                            string AndroidXArtifact
                                                        )
                                                    > googleArtifactMappings, 
                                ApiInfo apiInfoDataOld
                            )
        {
            int number = googleArtifactMappings.Count;

            List<string> assemblies = new List<string>();

            foreach (Namespace n in apiInfoDataOld.XmlSerializerAPI.ApiInfo.Assembly.Namespaces.Namespace)
            {
                string namespace_name = n.Name;

                string assembly_name = namespace_name;

                assemblies.Add(assembly_name);

                yield return
                            (
                                AndroidSupportArtifact: "aaa",
                                AndroidXArtifact: "bbb",
                                ManagedAssemblyAndroidSupport: assembly_name
                            );
            }

        }

        public async Task Merge_New_AndroidSupport
                            (
                                ReadOnlyCollection  <
                                                        (
                                                            string AndroidSupportArtifact, 
                                                            string AndroidXArtifact
                                                        )
                                                    > googleArtifactMappings,
                                ApiInfo apiInfoNew
                            )
        {
            return;
        }
    }

}
