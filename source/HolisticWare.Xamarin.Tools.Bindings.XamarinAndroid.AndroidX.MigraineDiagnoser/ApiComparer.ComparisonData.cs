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

        protected static MappingManager mapping_manager = new MappingManager();

        public static MappingManager MappingManager
        {
            get
            {
                return mapping_manager;
            }
        }

        public static 
            ReadOnlyCollection<
                                    (
                                        string AndroidSupportArtifact,
                                        string AndroidXArtifact
                                    )
                                >
                GoogleArtifactMappings
        {
            get
            {
                return MappingManager.GoogleArtifactMappings;
            }
        }

        public static int? GoogleArtifactMappingsCount
        {
            get
            {
                return GoogleArtifactMappings?.Count();
            }
        }


        public static
            ReadOnlyCollection<
                                    (
                                        string AndroidSupportClass,
                                        string AndroidXClass
                                    )
                                >
                GoogleClassMappings
        {
            get
            {
                return MappingManager.GoogleClassMappings;
            }
        }

        public static int? GoogleClassMappingsCount
        {
            get
            {
                return GoogleClassMappings?.Count();
            }
        }


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
            get
            {
                return MappingManager.AndroidPackagesBlackList;
            }
        }

    }
}
