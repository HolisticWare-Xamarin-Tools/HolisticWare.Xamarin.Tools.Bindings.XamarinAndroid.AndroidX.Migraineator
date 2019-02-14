using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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

            BuildProject bp_androidx = null;
            BuildProject bp_android_support = null;

            bp_androidx = new BuildProject()
            {
                FolderRoot = @"../../../../X/AndroidSupportComponents-AndroidX-binderate/",
                ApiInfoFile = @"../../../../X/AndroidSupportComponents-AndroidX-binderate/output/AndroidSupport.api-info.xml"
            };

            bp_android_support = new BuildProject()
            {
                FolderRoot = @"../../../../X/AndroidSupportComponents-master/",
                ApiInfoFile = @"../../../../X/AndroidSupportComponents-AndroidX-binderate/output/AndroidSupport.api-info.previous.xml"
            };




            Task t = ProcessApiInfoFilesAsync
                            (
                                bp_androidx, 
                                bp_android_support,
                                file_output
                            );


            Task.WaitAll(t);

           
            return;
        }

        private static async Task ProcessApiInfoFilesAsync
                                                (
                                                    BuildProject build_project_androidx,
                                                    BuildProject build_project_android_support_28_0_0,
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


            ApiInfo api_info_old_android_support = new ApiInfo
                                                        (
                                                            build_project_android_support_28_0_0
                                                        );
            await api_info_old_android_support.LoadAsync();

            ApiInfo api_info_new_androidx = new ApiInfo
                                                        (
                                                            build_project_androidx
                                                        );
            await api_info_new_androidx.LoadAsync();


            api_info_new_androidx.BuildProject.ProjectFiles
                                                        (
                                                            api_info_new_androidx.BuildProject.FolderWithGenerated,
                                                            "AndroidX"
                                                        );
            api_info_new_androidx.BuildProject.Dump("AndroidX", prettyified: true);

            api_info_old_android_support.BuildProject.ProjectFiles
                                                        (
                                                            api_info_old_android_support.BuildProject.FolderWithGenerated,
                                                            "Android.Support"
                                                        );
            api_info_old_android_support.BuildProject.Dump("Android.Support", prettyified: true);



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

            api_comparer.ApiInfoDataOld = api_info_old_android_support;
            api_comparer.ApiInfoDataNew = api_info_new_androidx;

            api_comparer.Analyse();
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

            //api_comparer.XmlDocumentAPI.MergeGoogleMappings
            //(
            //    ApiComparer.GoogleClassMappings,
            //    api_info_old_android_support.XmlDocumentAPI.Classes,
            //    api_info_new_androidx.XmlDocumentAPI.Classes
            //);

            Task.WaitAll();


            api_comparer.ApiInfoFileNew = build_project_androidx.ApiInfoFile;
            api_comparer.ApiInfoFileOld = build_project_android_support_28_0_0.ApiInfoFile;

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
