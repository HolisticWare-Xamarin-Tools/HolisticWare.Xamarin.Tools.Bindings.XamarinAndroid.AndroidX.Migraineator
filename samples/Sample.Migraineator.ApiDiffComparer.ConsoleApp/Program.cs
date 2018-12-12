using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core.Generated;

namespace Sample.Migraineator.ConsoleApp
{
    class Program
    {
        //------------------------------------------------------------------------------------------------------------------
        /*
        _ _  ____ _______ ______                                              
         | \ | |/ __ \__ __|  ____|_                                            
         |  \| | |  | | | |  | |__(_)
         | . ` | |  | | | |  |  __|                                               
         | |\  | |__| | | |  | |____ _                                            
         |_|_\_|\____/  |_|  |______(_)                       _ _   _             
         |  ____(_) |                                        (_) | (_)            
         | |__ _| | ___ _____   _____ _ ____ ___ __ _| |_ _ _ __   __ _ 
         |  __| | | |/ _ \  / _ \ \ / / _ \ '__\ \ /\ / / '__| | __| | '_ \ / _` |
         | |    | | |  __/ | (_) \ V /  __/ |   \ V V /| |  | | |_| | | | | (_| |
         |_|    |_|_|\___|  \___/ \_/ \___|_|    \_/\_/ |_|  |_|\__|_|_| |_|\__, |
                                                                             __/ |
                                                                            |___/ 
        */
        static bool overwrite_files = true;
        //------------------------------------------------------------------------------------------------------------------
        static int verbosity;
        static AndroidXDiffComparer androidx_diff_comparer = null;

        public static void Main(string[] args)
        {
            androidx_diff_comparer = new AndroidXDiffComparer();

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
                    @"../../../../../../../X/AndroidSupportComponents-AndroidX-binderate/output/AndroidX.api-diff.xml"
                    ;
            }
            if ( file_input_androidx == null || string.IsNullOrWhiteSpace(file_input_androidx) )
            {
                file_input_androidx =
                    @"../../../../X/AndroidSupportComponents-AndroidX-binderate/output/AndroidSupport.api-diff.xml"
                    //@"../../../../../../../X/AndroidSupportComponents-AndroidX-binderate/output/AndroidSupport.api-diff.xml"
                    ;
            }
            if (file_input_android_support_28_0_0 == null || string.IsNullOrWhiteSpace(file_input_android_support_28_0_0))
            {
                file_input_android_support_28_0_0 =
                    @"../../../../X/AndroidSupportComponents-28.0.0-binderate/output/AndroidSupport.api-diff.xml"
                    ;
            }

            Task t = ProcessMetadataFilesAsync
                            (
                                file_input_androidx, 
                                file_input_android_support_28_0_0,
                                file_output
                            );

            Task.WaitAll(t);

            return;
        }

        private static async Task ProcessMetadataFilesAsync
                                                (
                                                    string file_input_androidx,
                                                    string file_input_android_support_28_0_0,
                                                    string file_output
                                                )
        {
            #if DEBUG && NETCOREAPP
            await androidx_diff_comparer.InitializeAsync("./bin/Debug/netcoreapp2.1/mappings/");
            #elif RELEASE && NETCOREAPP
            await androidx_diff_comparer.InitializeAsync("./bin/Debug/netcoreapp2.1/mappings/");
            #else
            androidx_diff_comparer.Initialize("./mappings/");
            #endif

            ApiDiff api_diff_androidx = androidx_diff_comparer.ApiDiff(file_input_androidx);
            ApiDiff api_diff_previous_androidsupport_28_0_0 = androidx_diff_comparer.ApiDiff(file_input_android_support_28_0_0); ;


            (
                List<string> namespaces_x,
                List<string> namespaces_x_new_suspicious,
                List<string> namespaces_x_old_suspicious,
                List<string> classes_x
            ) = androidx_diff_comparer.Analyse(api_diff_androidx);

            androidx_diff_comparer.DumpToFiles(api_diff_androidx, "AndroidX");


            (
                List<string> namespaces_28,
                List<string> namespaces_28_new_suspicious,
                List<string> namespaces_28_old_suspicious,
                List<string> classes_28
            ) = androidx_diff_comparer.Analyse(api_diff_previous_androidsupport_28_0_0);

            androidx_diff_comparer.DumpToFiles(api_diff_previous_androidsupport_28_0_0, "AndroidSupport_28_0_0");

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
    }
}
