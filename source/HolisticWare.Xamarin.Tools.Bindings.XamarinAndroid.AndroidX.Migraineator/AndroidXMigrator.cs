using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;

using Core.Text;
using Core.Linq;
using Mono.Cecil.Rocks;
using Mono.Cecil;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator
{
    public partial class AndroidXMigrator
    {
        private static ReadOnlyMemory<char> memory_android_support;
        private static ReadOnlyMemory<char> memory_androidx;

        private static Memory<Memory<char>> TypeNamesToSkip;

        static AndroidXMigrator()
        {
            AbstractSyntaxTree = new AST.AbstractSyntaxTree();

            Parallel.Invoke
                        (
                            () =>
                            {
                                string file = Path.Combine
                                                    (
                                                        new string[]
                                                        {
                                                            Settings.WorkingDirectory,
                                                            "mappings",
                                                            "API.Mappings.Merged.Google.with.Xamarin.Classes.csv"
                                                        }
                                                    );
                                LoadMappingsClasses(file);
                                InitializePerformance();
                            }
                            //() =>
                            //{
                            //    Initialize();
                            //}
                        );

            return;
        }

        // Android Support for searching
        // sorted for BinarySearch
        private static Memory<string> ClassMappingsSortedProjected;

        private static Memory
                <
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
                > ClassMappingsSorted;

        private static void InitializePerformance()
        {
            ClassMappingsSorted = ClassMappings
                                            .Where(i => !string.IsNullOrEmpty(i.AndroidSupportClassFullyQualified))
                                            .OrderBy(i => i.AndroidSupportClassFullyQualified)
                                            .ToArray()
                                            .AsMemory()
                                            ;

            int n = ClassMappingsSorted.Length;

            ClassMappingsSortedProjected = ClassMappingsSorted
                                                    .Select(i => i.AndroidSupportClassFullyQualified)
                                                    ;

            // Test
            string classname = "Android.Support.CustomTabs.CustomTabsServiceConnection";
            int idx = ClassMappingsSortedProjected.Span.BinarySearch(classname);
            if( idx != 42 )
            {
                string msg =
                    "Android.Support sorted classnames changed"
                    + Environment.NewLine +
                    "Could be change in mappings or bindings!"
                    + Environment.NewLine +
                    "CHECK!!!!"
                    ;

                throw new InvalidDataException(msg);
            }

            return;
        }

        public static HashSet<string> AndroidSupportNotFoundInGoogle = new HashSet<string>();

        private static void Initialize()
        {
            System.Diagnostics.Trace.WriteLine($"    Initialize...");
            memory_android_support = string.Intern("Android.Support").AsMemory();
            memory_androidx = string.Intern("AndroidX").AsMemory();

            TypeNamesToSkip = 
                            new Memory<char>[]
                                    {
                                        string.Intern("<Module>").ToCharArray().AsMemory(),
                                        string.Intern("__TypeRegistrations").ToCharArray().AsMemory(),
                                    }
                                    .AsMemory();

            return;
        }

        public AndroidXMigrator(string input, string output)
        {
            this.PathAssemblyInput = input;
            this.PathAssemblyOutput = output;

            return;
        }

        public string PathAssemblyInput
        {
            get;
            set;
        }

        public string PathAssemblyOutput
        {
            get;
            set;
        }

        public void Migrate(bool span_memory_implementation = false)
        {
            if (span_memory_implementation)
            {
                MigrateWithSpanMemory(); 
            }
            else
            {
                MigrateWithWithStringsOriginalPatchByRedth();
            }

            return;
        }

        public static AST.AbstractSyntaxTree AbstractSyntaxTree
        {
            get;
        }
    }
}
