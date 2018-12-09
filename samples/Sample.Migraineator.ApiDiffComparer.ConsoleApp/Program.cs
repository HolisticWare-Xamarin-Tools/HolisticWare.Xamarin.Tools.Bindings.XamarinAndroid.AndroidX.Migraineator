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
        static AndroidXMigrator androidx_diff_comparer = null;

        public static void Main(string[] args)
        {
            androidx_diff_comparer = new AndroidXMigrator();

            bool show_help = false;
            List<string> names = new List<string>();
            string file_input = null;
            string file_output = null;

            Mono.Options.OptionSet option_set = new Mono.Options.OptionSet()
            {
                // ../../../../../../../X/AndroidSupportComponents-AndroidX-binderate/output/AndroidSupport.api-diff.xml
                {
                    "i|input=",
                    "input folder with Android Support Diff (Metadata.xml, Metadata*.xml)",
                    (string v) =>
                    {
                        file_input = v;
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
            if ( file_input == null || string.IsNullOrWhiteSpace(file_input) )
            {
                file_input =
                    @"../../../../X/AndroidSupportComponents-AndroidX-binderate/output/AndroidSupport.api-diff.xml"
                    //@"../../../../../../../X/AndroidSupportComponents-AndroidX-binderate/output/AndroidSupport.api-diff.xml"
                    ;
            }

            Console.WriteLine($"Migrating Android.Support in:");
            Console.WriteLine($"    {file_input}");
            Console.WriteLine($"  to  AndroidX in:");
            Console.WriteLine($"    {file_output}");

            Task t = ProcessMetadataFilesAsync(file_input, file_output);

            Task.WaitAll(t);

            return;
        }

        private static async Task ProcessMetadataFilesAsync(string file_input, string file_output)
        {
            #if DEBUG && NETCOREAPP
            await androidx_diff_comparer.InitializeAsync("./bin/Debug/netcoreapp2.1/mappings/");
            #elif RELEASE && NETCOREAPP
            await androidx_diff_comparer.InitializeAsync("./bin/Debug/netcoreapp2.1/mappings/");
            #else
            androidx_diff_comparer.Initialize("./mappings/");
            #endif

            XmlSerializer serializer = new XmlSerializer(typeof(ApiDiff));

            StreamReader sr = null;
            ApiDiff api_diff = null;
            ApiDiff api_diff_previous = null;

            try
            {
                sr = new StreamReader(file_input);
                api_diff = (ApiDiff)serializer.Deserialize(sr);
            }
            catch
            {
                throw;
            }
            finally
            {
                sr.Close();
            }


            try
            {
                sr = new StreamReader(file_input.Replace("AndroidX.api-diff.xml", "AndroidX.api-diff.previous.xml"));
                api_diff_previous = (ApiDiff)serializer.Deserialize(sr);
            }
            catch
            {
                throw;
            }
            finally
            {
                sr.Close();
            }

            List<string> namespaces = new List<string>();
            List<string> namespaces_new_suspicious = new List<string>();
            List<string> namespaces_old_suspicious = new List<string>();

            List<string> classes = new List<string>();

            foreach (Namespace n in api_diff.Assembly.Namespaces.Namespace)
            {
                string namespace_name = n.Name;
                namespaces.Add(namespace_name);

                if (namespace_name.StartsWith("Androidx."))
                {
                    namespaces_new_suspicious.Add(namespace_name);
                }
                else if (namespace_name.StartsWith("Android."))
                {
                    namespaces_old_suspicious.Add(namespace_name);
                }

                try
                {
                    if (n.Classes != null)
                    {
                        foreach (Class c in n?.Classes.Class)
                        {
                            string class_name = c?.Name;
                            string class_name_fq = $"{n.Name}.{class_name}";
                            classes.Add($"{class_name_fq},                                 ,{class_name}");
                        }
                    }
                }
                catch
                {
                    throw; 
                }

            }

            System.IO.File.WriteAllLines("api_diff_namespaces.txt", namespaces);
            System.IO.File.WriteAllLines("api_diff_namespaces_new_suspicious.txt", namespaces_new_suspicious);
            System.IO.File.WriteAllLines("api_diff_namespaces_old_suspicious.txt", namespaces_old_suspicious);


            List<string> namespaces_previous = new List<string>();

            List<string> classes_previous = new List<string>();

            foreach (Namespace n in api_diff_previous.Assembly.Namespaces.Namespace)
            {
                string namespace_name = n.Name;
                namespaces_previous.Add(namespace_name);

                try
                {
                    if (n.Classes != null)
                    {
                        foreach (Class c in n?.Classes.Class)
                        {
                            string class_name = c?.Name;
                            classes.Add(class_name);
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }

            System.IO.File.WriteAllLines("api_diff_previous_namespaces.txt", namespaces_previous);

            return;
        }

        private static async Task MigrateFilesMedtadataXmlSequentialAsync(string[] files)
        {
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                Console.WriteLine($"    {file}");

                try
                {
                    string content_migrated = await androidx_diff_comparer.MigrateMetadataXmlFileNamespacesAsync(file);

                    if (overwrite_files)
                    {
                        System.IO.File.WriteAllText($"{file}", content_migrated);
                    }
                }
                catch (AggregateException exc)
                {
                    Console.WriteLine($"AndroidXMigrator Exception {exc.Message}");

                    throw;
                }
            }

            return;
        }

        private static async Task MigrateFilesEnumMethodsXmlSequentialAsync(string[] files)
        {
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                Console.WriteLine($"    {file}");

                try
                {
                    string content_migrated = await androidx_diff_comparer.MigrateEnumMethodsXmlFileNamespacesAsync(file);

                    if (overwrite_files)
                    {
                        System.IO.File.WriteAllText($"{file}", content_migrated);
                    }
                }
                catch (AggregateException exc)
                {
                    Console.WriteLine($"AndroidXMigrator Exception {exc.Message}");

                    throw;
                }
            }

            return;
        }

        private static void MigrateFilesParallel(string[] files)
        {
            Parallel.For
                    (
                        0,
                        files.Length,
                        async i =>
                        {
                            string file = files[i];
                            Console.WriteLine($"    {file}");

                            try
                            {
                                string content = null;
                                using
                                    (
                                        System.IO.FileStream stream = System.IO.File.Open
                                                                                        (
                                                                                            file,
                                                                                            System.IO.FileMode.Open,
                                                                                            System.IO.FileAccess.Read
                                                                                        )
                                    )
                                using
                                    (
                                        System.IO.TextReader reader = new System.IO.StreamReader(stream)
                                    )
                                {
                                    content = await reader.ReadToEndAsync();
                                }
                                //string content = System.IO.File.ReadAllText(file);
                                string content_migrated = await androidx_diff_comparer.MigrateMetadataXmlFileNamespacesAsync(content);
                                //System.IO.File.WriteAllText(file, content_migrated);
                            }
                            catch (AggregateException exc)
                            {
                                Console.WriteLine($"AndroidXMigrator Exception {exc.Message}");

                                throw;
                            }
                        }
                    );
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
