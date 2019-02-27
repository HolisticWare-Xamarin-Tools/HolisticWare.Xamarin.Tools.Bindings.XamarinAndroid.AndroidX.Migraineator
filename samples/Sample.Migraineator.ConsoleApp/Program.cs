using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core;

namespace Sample.Migraineator.ConsoleApp
{
    class Program
    {
        static bool overwrite_files = true;
        static int verbosity;
        //static AndroidXMigrator androidx_migrator = null;

        public static void Main(string[] args)
        {
            //androidx_migrator = new AndroidXMigrator();

            bool show_help = false;
            List<string> names = new List<string>();
            string folder_input = null;
            string folder_output = null;


            List<string> extra;
            try
            {
                extra = Options.OptionSet.Parse(args);
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
                //ShowHelp(option_set);
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

            if (string.IsNullOrWhiteSpace(folder_output))
            {
                folder_output = "../../../dumps/";
            }
            if (string.IsNullOrWhiteSpace(folder_input))
            {
                folder_input =
                    @"../../../../X/AndroidSupportComponents-AndroidX-binderate/source/"
                    //@"../../../../X/AndroidSupportComponents-AndroidX-binderate/source/androidx.core/"
                    //@"../../../../X/AndroidSupportComponents-AndroidX-binderate/source/androidx.core/"
                    ;
            }

            Console.WriteLine($"Migrating Android.Support in:");
            Console.WriteLine($"    {folder_input}");
            Console.WriteLine($"  to  AndroidX in:");
            Console.WriteLine($"    {folder_output}");

            Task t = ProcessMetadataFilesAsync(folder_input, folder_output);

            Task.WaitAll(t);

            return;
        }

        private static async Task ProcessMetadataFilesAsync(string folder_input, string folder_output)
        {
            //#if DEBUG && NETCOREAPP
            //await androidx_migrator.InitializeAsync("./bin/Debug/netcoreapp3.0/mappings/");
            //#elif RELEASE && NETCOREAPP
            //await androidx_migrator.InitializeAsync("./bin/Debug/netcoreapp2.1/mappings/");
            //#else
            //androidx_migrator.Initialize("./mappings/");
            //#endif

            Console.WriteLine($"Migrating files: ");


            Console.WriteLine($"    Metadata.xml: ");
            string[] files_metadata_xml = System.IO.Directory.GetFiles
                                                        (
                                                            folder_input,
                                                            "Metadata*.xml",
                                                            System.IO.SearchOption.AllDirectories
                                                        );

            await MigrateFilesMedtadataXmlSequentialAsync(files_metadata_xml);
            //MigrateFilesParallel(files);

            Console.WriteLine($"    EnumMethods.xml: ");
            string[] files_enummethods_xml = System.IO.Directory.GetFiles
                                                        (
                                                            folder_input,
                                                            "EnumMethods.xml",
                                                            System.IO.SearchOption.AllDirectories
                                                        );
            await MigrateFilesEnumMethodsXmlSequentialAsync(files_enummethods_xml);

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
                    string content_migrated =
                        //await androidx_migrator.MigrateMetadataXmlFileNamespacesAsync(file)
                        null
                        ;

                    if(overwrite_files)
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
                    string content_migrated =
                        //await androidx_migrator.MigrateEnumMethodsXmlFileNamespacesAsync(file)
                        null
                        ;

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
                                string content_migrated =
                                    //await androidx_migrator.MigrateMetadataXmlFileNamespacesAsync(content)
                                    null
                                    ;
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
