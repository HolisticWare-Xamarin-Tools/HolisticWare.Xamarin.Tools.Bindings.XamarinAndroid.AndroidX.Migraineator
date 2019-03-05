using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;

using Core.Text;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Generated;
using System.Collections.ObjectModel;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator
{
    public partial class ApiComparer
    {
        public partial class MonoCecilData
        {
            public
                IEnumerable <
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
                GoogleClassMappingsWithXamarin
            {
                get;
                protected set;
            }


        }
    }
}
