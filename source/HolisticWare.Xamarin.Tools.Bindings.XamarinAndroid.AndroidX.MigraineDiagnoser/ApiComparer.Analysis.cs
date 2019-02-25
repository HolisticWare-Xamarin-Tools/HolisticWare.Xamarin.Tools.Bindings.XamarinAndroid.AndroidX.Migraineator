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
        public void Analyse()
        {
            Parallel.Invoke
                (
                    () =>
                    {
                        AnalyseAndroidArtifactsWithManagedAssemblies
                            (
                                MappingManager.GoogleArtifactMappings,
                                this.ApiInfoDataOld.BuildProject.AndroidArtifacts_X_ManagedAssemblies,
                                this.ApiInfoDataNew.BuildProject.AndroidArtifacts_X_ManagedAssemblies
                            );
                    }
                );

            return;
        }

        private void AnalyseAndroidArtifactsWithManagedAssemblies
                        (
                            ReadOnlyCollection
                                    <
                                        (
                                            string ArtifactAndroidSupport,
                                            string ArtifactAndroidX
                                        )
                                    >
                                    mappings_google_artifacts, 
                            List    <
                                        (
                                            string AndroidArtifact, 
                                            string ManagedAssembly
                                        )
                                    > artifacts_assemblies_old,
                            List<
                                        (
                                            string AndroidArtifact,
                                            string ManagedAssembly
                                        )
                                    > artifacts_assemblies_new
                        )
        {
            List<
                    (
                        string AtifactAndroidSupport,
                        string AtifactAndroidX,
                        string ManagedAssemblyAndroidSupport,
                        string ManagedAssemblyAndroidX
                    )
                > mapping_android_X_managed;

            mapping_android_X_managed = new List  <
                                                    (
                                                        string AtifactAndroidSupport,
                                                        string AtifactAndroidX,
                                                        string ManagedAssemblyAndroidSupport,
                                                        string ManagedAssemblyX
                                                    )
                                                >();
            foreach
                (
                    (
                        string ArtifactAndroidSupport,
                        string ArtifactAndroidX
                    ) mapping
                    in mappings_google_artifacts
                )
            {
                string artifact_as = mapping.ArtifactAndroidSupport;
                string artifact_ax = mapping.ArtifactAndroidX;
                int pos = artifact_ax.LastIndexOf(':');
                if (pos >= 0)
                {
                    artifact_ax = artifact_ax.Substring(0, pos);
                }

                var artifacts_as = from a_as in artifacts_assemblies_old
                                   where
                                            (
                                              a_as.AndroidArtifact == artifact_as   
                                            )
                                       select
                                            a_as
                                       ;
                var artifacts_ax = from a_ax in artifacts_assemblies_new
                                   where
                                            (
                                              a_ax.AndroidArtifact == artifact_ax
                                            )
                                   select
                                        a_ax
                                       ;

                string managed_assembly_as = artifacts_as.SingleOrDefault().ManagedAssembly;
                string managed_assembly_ax = artifacts_ax.SingleOrDefault().ManagedAssembly;

                mapping_android_X_managed.Add
                                            (
                                                (
                                                    AtifactAndroidSupport: artifact_as,
                                                    AtifactAndroidX: artifact_ax,
                                                    ManagedAssemblyAndroidSupport: managed_assembly_as,
                                                    ManagedAssemblyAndroidX: managed_assembly_ax
                                                )
                                            );
            }

            MappingAndroidArtifacts_X_ManagedAssemblies = mapping_android_X_managed;

            return;
        }
    }
}
