using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

using Core.Text;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core.Generated;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core
{
    public partial class ApiComparer
    {
        public partial class XmlDocumentData
        {
            public
                List<
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
                        MergeGoogleMappings
                            (
                                ReadOnlyCollection<
                                                        (
                                                            string AndroidSupportClass,
                                                            string AndroidXClass
                                                        )
                                                    > google_class_mappings,
                                ReadOnlyCollection<
                                                        (
                                                            string Action,
                                                            string AndroidSupportPackage,
                                                            string AndroidXPackage
                                                        )
                                                    > blacklisted_packages,
                                IEnumerable<
                                                (
                                                    string ManagedClass,
                                                    string ManagedNamespace,
                                                    string JNIPackage,
                                                    string JNIType
                                                )
                                            > android_support,
                                IEnumerable<
                                                (
                                                    string ManagedClass,
                                                    string ManagedNamespace,
                                                    string JNIPackage,
                                                    string JNIType
                                                )
                                            > androidx
                            )
            {
                //--------------------------------------------------------------------------------------------
                // Initialization
                results_found =
                    new
                        List<
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
                            >();

                results_missing_in_xamarin_android_support =
                    new
                        List<
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
                            >();

                results_missing_in_xamarin_androidx =
                    new
                        List<
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
                            >();

                results_missing_completely =
                    new
                        List<
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
                            >();

                results_complete =
                    new
                        List<
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
                            >();

                packages_namespaces =
                    new
                        List<
                                (
                                    string PackageAndroidSupport,
                                    string PackageAndroidX,
                                    string ManagedNamespaceXamarinAndroidSupport,
                                    string ManagedNamespaceXamarinAndroidX
                                )
                            >();

                results_not_found_at_all =
                    new
                        List<
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
                            >();
                results_not_found_in_xas =
                    new
                        List<
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
                            >();
                results_not_found_in_xax =
                    new
                        List<
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
                            >();
                //--------------------------------------------------------------------------------------------


                return results_found;
            }


            //--------------------------------------------------------------------------------------------
            // Data structures (tuples) for analysis
            List<
                    (
                        string PackageAndroidSupport,
                        string PackageAndroidX,
                        string ManagedNamespaceXamarinAndroidSupport,
                        string ManagedNamespaceXamarinAndroidX
                    )
                >
                    packages_namespaces;

            List<
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
                    results_found;

            List<
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
                    results_missing_in_xamarin_androidx;

            List<
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
                    results_missing_in_xamarin_android_support;

            List<
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
                    results_missing_completely;

            List<
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
                    results_complete;

            List<
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
                    results_not_found_at_all;

            List<
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
                    results_not_found_in_xas;

            List<
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
                    results_not_found_in_xax;
            //--------------------------------------------------------------------------------------------
        }
    }
}
