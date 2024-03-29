using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Core.Linq;

[assembly:System.Runtime.CompilerServices.InternalsVisibleTo("Tests.XUnit")]

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

        public static HashSet<string> AndroidSupportNotFoundInGoogle = new HashSet<string>();

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

        public static void LoadMappingsClasses(string file)
        {
            Trace.WriteLine($" reading csv = {file}");
            Core.Text.CharacterSeparatedValues csv = new Core.Text.CharacterSeparatedValues();
            string content = csv.LoadAsync(file).Result;

            Trace.WriteLine($"    parsing csv...");
            IEnumerable<string[]> mapping = csv
                                            .ParseTemporaryImplementation()
                                            .ToList()
                                            ;
            IEnumerable
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
                    >
                    mapping_strongly_typed;

            mapping_strongly_typed = Convert_ClassMappings(mapping);

            ClassMappings = mapping_strongly_typed
                                            .ToList().AsReadOnly()
                                            // hangs if used Memory
                                            //.ToArray()
                                            //.AsMemory()
                                            ;

            return;
        }

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

        //-------------------------------------------------------------------------------------------------------------------
        public static
            ReadOnlyCollection
            //ReadOnlyMemory
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
                    >
                ClassMappings
        {
            get;
            private set;
        }

        public static 
            IEnumerable
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
                >
                Convert_ClassMappings(IEnumerable<string[]> untyped_data)
        {
             Trace.WriteLine($"    converting csv to strongly typed data");
           //int n = untyped_data.Count();

            //for(int i = 0; i < n; i++)
            foreach(string[] row in untyped_data)
            {
                //string[] row = untyped_data[i];

                //TODO - benchmarking
                //Span<string> row_data = row.AsSpan();

                yield return
                        (
                            //ClassName: row_data[0],
                            //AndroidSupportClass: row_data[1],
                            //AndroidXClass: row_data[2],
                            //AndroidSupportClassFullyQualified: row_data[3],
                            //AndroidXClassFullyQualified: row_data[4],
                            //      formatting space
                            //PackageAndroidSupport: row_data[5],
                            //PackageAndroidX: row_data[6],
                            //ManagedNamespaceXamarinAndroidSupport: row_data[7],
                            //ManagedNamespaceXamarinAndroidX: row_data[8]

                            ClassName: row[0],
                            AndroidSupportClass: row[1],
                            AndroidXClass: row[2],
                            AndroidSupportClassFullyQualified: row[3],
                            AndroidXClassFullyQualified: row[4],
                                 // formatting space
                            PackageAndroidSupport: row[5],
                            PackageAndroidX: row[6],
                            ManagedNamespaceXamarinAndroidSupport: row[7],
                            ManagedNamespaceXamarinAndroidX: row[8]
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------

        private static void Initialize()
        {
            Trace.WriteLine($"    Initialize...");
            memory_android_support = string.Intern("Android.Support").AsMemory();
            memory_androidx = string.Intern("AndroidX").AsMemory();

            TypeNamesToSkip = 
                            new Memory<char>[]
                                    {
                                        string.Intern("<Module>").ToCharArray().AsMemory(),
                                        string.Intern("__TypeRegistrations").ToCharArray().AsMemory(),
                                    }
                                    .AsMemory();

            PerformanceData = new List
                                    <
                                        (
                                            string Assembly,
                                            string Algorithm, // Matthew's Shortcuts, Redth's Original Patch
                                            long Duration
                                        )
                                    >();

            return;
        }

        public AndroidXMigrator(string input, string output)
        {
            this.PathAssemblyInput = input;
            this.PathAssemblyOutput = output;

            Initialize();
            //InitializePerformance();

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

        partial void MigrateWithSpanMemory(ref long duration);

        public void Migrate(bool span_memory_implementation = false)
        {
            long duration = -1;

            if (span_memory_implementation)
            {
                MigrateWithSpanMemory(ref duration); 
            }
            else
            {
                MigrateWithWithStringsOriginalPatchByRedth(ref duration);
                PerformanceData.Add((PathAssemblyOutput, "Redth Original", duration));
                MigrateWithWithStringsMathewsShortcuts(ref duration);
                PerformanceData.Add((PathAssemblyOutput, "Matthews Shortcuts", duration));
            }

            string fp = Path.ChangeExtension(this.PathAssemblyInput, "problems.txt");
            string fasm = Path.ChangeExtension(this.PathAssemblyInput, "android-support-missing.txt");
            File.WriteAllLines(fp, this.Problems.ToList());
            File.WriteAllLines(fp, AndroidSupportNotFoundInGoogle.ToList());

            return;
        }

        public static AST.AbstractSyntaxTree AbstractSyntaxTree
        {
            get;
        }

        public static List
                            <
                                (
                                    string Assembly, 
                                    string Algorithm, // Matthew's Shortcuts, Redth's Original Patch
                                    long Duration
                                ) 
                            > PerformanceData
        {
            get;
            private set;
        }
    }
}
