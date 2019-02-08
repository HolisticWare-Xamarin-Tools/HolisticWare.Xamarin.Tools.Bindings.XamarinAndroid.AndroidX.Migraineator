using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace Sample.Migraineator.ConsoleApp
{
    class Program
    {
        static int verbosity;
        static ApiComparer api_comparer = null;

        public static void Main(string[] args)
        {
            api_comparer = new ApiComparer();

            bool show_help = false;
            List<string> names = new List<string>();
            string file_input_androidx = null;
            string file_input_android_support_28_0_0 = null;
            string file_output = null;

            Mono.Options.OptionSet option_set = new Mono.Options.OptionSet()
            {
                // ../../../../../../../X/AndroidSupportComponents-AndroidX-binderate/output/AndroidSupport.api-diff.xml
                {
                    "i|input=",
                    "input folder with Android Support Diff (Metadata.xml, Metadata*.xml)",
                    (string v) =>
                    {
                        file_input_androidx = v;
                        return;
                    }
                },
                {
                    "o|output=",
                    "output folder with AndroidX Xamarin.Android Metadata files (Metadata.xml, Metadata*.xml)",
                    (string v) =>
                    {
                        file_output = v;
                        return;
                    }
                },
                {
                    "h|help",
                    "show this message and exit",
                    v => show_help = v != null
                },
            };

            List<string> extra;
            try
            {
                extra = option_set.Parse(args);
            }
            catch (Mono.Options.OptionException e)
            {
                Console.Write("AndroidX.Migraineator: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `AndroidX.Migraineator --help' for more information.");
                return;
            }

            if (show_help)
            {
                ShowHelp(option_set);
                return;
            }

            string message = "AndroidX.Migraineator";
            if (extra.Count > 0)
            {
                message = string.Join(" ", extra.ToArray());
                Debug($"Using new message: {message}");
            }
            else
            {
                Debug($"Using default message: {message}");
            }

            if (string.IsNullOrWhiteSpace(file_output))
            {
                file_output =
                    @"../../../../../../../X/AndroidSupportComponents-AndroidX-binderate/output/AndroidX.api-info.xml"
                    ;
            }
            if ( file_input_androidx == null || string.IsNullOrWhiteSpace(file_input_androidx) )
            {
                file_input_androidx =
                    @"../../../../X/AndroidSupportComponents-AndroidX-binderate/output/AndroidSupport.api-info.xml"
                    //@"../../../../../../../X/AndroidSupportComponents-AndroidX-binderate/output/AndroidSupport.api-diff.xml"
                    ;
            }
            if (file_input_android_support_28_0_0 == null || string.IsNullOrWhiteSpace(file_input_android_support_28_0_0))
            {
                file_input_android_support_28_0_0 =
                    //@"../../../../X/AndroidSupportComponents-28.0.0-binderate/output/AndroidSupport.api-info.xml"
                    @"../../../../X/AndroidSupportComponents-AndroidX-binderate/output/AndroidSupport.api-info.previous.xml"
                    ;
            }
            BuildProject bp1 = new BuildProject(@"../../../../X/AndroidSupportComponents-AndroidX-binderate/");
            var a = bp1.ProjectFiles(bp1.FolderWithGenerated);

            Task t = ProcessApiInfoFilesAsync
                            (
                                file_input_androidx, 
                                file_input_android_support_28_0_0,
                                file_output
                            );

            Task.WaitAll(t);

           
            return;
        }

        private static async Task ProcessApiInfoFilesAsync
                                                (
                                                    string file_input_androidx,
                                                    string file_input_android_support_28_0_0,
                                                    string file_output
                                                )
        {

            string working_dir = null;

            #if NETCOREAPP && NETCOREAPP2_1
            #if DEBUG
            working_dir = "./bin/Debug/netcoreapp2.1/";
            #elif RELEASE
            working_dir = "./bin/Release/netcoreapp2.1/";
            #endif
            #else
            working_dir = "./mappings/";
            #endif



            // Load Mappings (Google Android.Support <-> AndroidX
            await MappingManager.InitializeAsync(working_dir);
            // Dump packagename mappings (not provided by Google) 
            // this is data for our checks
            await MappingManager.DumpPackageNamesAsync();

            // Load OLD Android.Support api-info.xml 
            string assembly_android_support = "../../../../X/AndroidSupportComponents-master/output/AndroidSupport.Merged.dll";
            ApiInfo api_info_old_android_support = new ApiInfo
                                                        (
                                                            file_input_android_support_28_0_0,
                                                            assembly_android_support
                                                        );
            await api_info_old_android_support.LoadAsync();

            // Load NEW AndroidX api-info.xml 
            string assembly_androidx = "../../../../X/AndroidSupportComponents-AndroidX-binderate/output/AndroidSupport.Merged.dll";
            ApiInfo api_info_new_androidx = new ApiInfo
                                                        (
                                                            file_input_androidx,
                                                            assembly_androidx
                                                        );
            await api_info_new_androidx.LoadAsync();


            api_info_old_android_support.MonoCecilAPI.Analyse();
            api_info_old_android_support.MonoCecilAPI.Dump("Android.Support");

            api_info_new_androidx.MonoCecilAPI.Analyse();
            api_info_new_androidx.MonoCecilAPI.Dump("AndroidX");



            api_info_old_android_support.XmlDocumentAPI.Analyse();
            api_info_old_android_support.XmlDocumentAPI.Dump("Android.Support");

            api_info_new_androidx.XmlDocumentAPI.Analyse();
            api_info_new_androidx.XmlDocumentAPI.Dump("AndroidX");



            await api_info_old_android_support.XmlSerializerAPI.Deserialize();
            await api_info_new_androidx.XmlSerializerAPI.Deserialize();


            api_comparer.MonoCecilAPI.MergeGoogleMappings
                                                (
                                                    ApiComparer.GoogleClassMappings,
                                                    ApiComparer.AndroidPackagesBlackList,
                                                    api_info_old_android_support.MonoCecilAPI.TypesAndroidRegistered,
                                                    api_info_new_androidx.MonoCecilAPI.TypesAndroidRegistered
                                                );
                                                    
            //api_comparer.XmlDocumentAPI.MergeGoogleMappings
                                                //(
                                                //    ApiComparer.GoogleClassMappings,
                                                //    ApiComparer.AndroidPackagesBlackList,
                                                //    api_info_old_android_support.XmlDocumentAPI.TypesAndroidRegistered,
                                                //    api_info_new_androidx.XmlDocumentAPI.TypesAndroidRegistered
                                                //);

            api_comparer.MonoCecilAPI.Dump(prettyfied: true);
            api_comparer.XmlDocumentAPI.Dump(prettyfied: true);

            await AnalyseAsync("AndroidSupport", assembly_android_support);
            await AnalyseAsync("AndroidX", assembly_android_support);

            //api_comparer.XmlDocumentAPI.MergeGoogleMappings
            //(
            //    ApiComparer.GoogleClassMappings,
            //    api_info_old_android_support.XmlDocumentAPI.Classes,
            //    api_info_new_androidx.XmlDocumentAPI.Classes
            //);

            Task.WaitAll();













            api_comparer.ApiInfoFileNew = file_input_androidx;
            api_comparer.ApiInfoFileOld = file_input_android_support_28_0_0;

            (
                List<string> namespaces,
                List<string> namespaces_new_suspicious,
                List<string> namespaces_old_suspicious,
                List<(string ClassName, string ClassNameFullyQualified)> classes
            ) analysis_data_old;

            //var merge1 =
            //api_info_comparer.MappingManager.Merge_Old_AndroidSupport
            //                        (
            //                            api_info_comparer.MappingManager.GoogleArtifactMappings,
            //                            api_info_comparer.ApiInfoDataOld
            //                        );
            //await api_info_comparer.MappingManager.Merge_New_AndroidSupport
            //                        (
            //                            api_info_comparer.MappingManager.GoogleArtifactMappings,
            //                            api_info_comparer.ApiInfoDataNew
            //                        );


            //analysis_data_old = api_info_comparer.Analyse(api_info_comparer.ApiInfoDataOld);

            //api_info_comparer.MappingApiInfoMatertial();






            //api_info_comparer.ModifyApiInfo
                                        //(
                                        //    api_info_comparer.ContentApiInfoNew,
                                        //    api_info_comparer.ApiInfoDataOld
                                        //);

            return;
        }

        static void ShowHelp(Mono.Options.OptionSet p)
        {
            Console.WriteLine("Usage: greet [OPTIONS]+ message");
            Console.WriteLine("Greet a list of individuals with an optional message.");
            Console.WriteLine("If no message is specified, a generic greeting is used.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);

            return;
        }

        static async Task AnalyseAsync()
        {
            /*
            (
                List<string> namespaces_28,
                List<string> namespaces_28_new_suspicious,
                List<string> namespaces_28_old_suspicious,
                List<string> classes_28
            ) = androidx_diff_comparer.Analyse(api_info_previous_androidsupport_28_0_0.api_info_previous);

            androidx_diff_comparer.DumpToFiles
            (
                api_info_previous_androidsupport_28_0_0.api_info_previous, 
                "AndroidSupport_28_0_0"
            );

            (
                string content_new,
                ApiInfo api_info_new
            )
                api_info_androidx = await androidx_diff_comparer.ApiInfo(file_input_androidx);

            (
                List<string> namespaces_x,
                List<string> namespaces_x_new_suspicious,
                List<string> namespaces_x_old_suspicious,
                List<string> classes_x
            ) = androidx_diff_comparer.Analyse(api_info_androidx.api_info_new);

            androidx_diff_comparer.DumpToFiles
            (
                api_info_androidx.api_info_new, 
                "AndroidX"
            );
            */

        }


        static void Debug(string format, params object[] args)
        {
            if (verbosity > 0)
            {
                Console.Write("# ");
                Console.WriteLine(format, args);
            }

            return;
        }


        public static async Task AnalyseAsync(string name, string file_path)
        {
            AssemblyDefinition asm = AssemblyDefinition.ReadAssembly
                (
                    file_path,
                    new ReaderParameters 
                    { 
                        AssemblyResolver = ApiInfo.MonoCecilData.CreateAssemblyResolver() 
                    }
                );

            IEnumerable<TypeDefinition> allTypes = asm.MainModule.GetAllTypes();

            List<string> info = new List<string>();

            foreach (TypeDefinition t in allTypes)
            {
                foreach (CustomAttribute attr in t.CustomAttributes)
                {
                    if (attr.AttributeType.FullName.Equals("Android.Runtime.RegisterAttribute"))
                    {
                        var jniType = attr.ConstructorArguments[0].Value.ToString();

                        var lastSlash = jniType.LastIndexOf('/');

                        var jniClass = jniType.Substring(lastSlash + 1).Replace('$', '.');
                        var jniPkg = jniType.Substring(0, lastSlash).Replace('/', '.');

                        //var mngdClass = GetTypeName(t);
                        //var mngdNs = GetNamespace(t);

                        //info.Add($"{jniPkg}, {jniClass}, {mngdNs}, {mngdClass}");
                    }
                }
            }

            //.............................................................................
            string path = Path.Combine
                (
                    new string[]
                    {
                            Environment.CurrentDirectory,
                            "..",
                            "output"
                    }
                );
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string path_output = Path.Combine(path, "analysis");
            if (!Directory.Exists(path_output))
            {
                Directory.CreateDirectory(path_output);
            }

            string path_file = null;
            path_file = Path.Combine(path_output, $"{name}.mono-cecil-dump.csv");
            await System.IO.File.WriteAllLinesAsync(path_file, info);
            //.............................................................................

        }
    }
}
