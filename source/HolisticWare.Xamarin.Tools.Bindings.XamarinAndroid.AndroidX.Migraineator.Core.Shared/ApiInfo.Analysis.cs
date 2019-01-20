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
    public partial class ApiInfo
    {
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
                return mapping_manager.GoogleArtifactMappings;
            }
        }

        protected static MappingManager mapping_manager = new MappingManager();

        public static int? GoogleArtifactMappingsCount
        {
            get
            {
                return GoogleArtifactMappings?.Count();
            }
        }


    }
}
