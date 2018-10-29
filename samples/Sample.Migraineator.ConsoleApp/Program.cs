using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Migraineator.ConsoleApp
{
    class Program
    {
        static int verbosity;

        public static void Main(string[] args)
        {
            bool show_help = false;
            List<string> names = new List<string>();
            string folder_input = null;
            string folder_output = null;

            Mono.Options.OptionSet option_set = new Mono.Options.OptionSet() 
            {
                { 
                    "i|input=", 
                    "input folder with Android Support Xamarin.Android Metadata files (Metadata.xml, Metadata*.xml)",
                    (string v) =>
                    {
                        folder_input = v;
                        return;
                    }
                },
                { 
                    "o|output=",
                    "output folder with AndroidX Xamarin.Android Metadata files (Metadata.xml, Metadata*.xml)",
                    (string v) => 
                    {
                        folder_output = v;
                        return;
                    }
                },
                { 
                    "v|verbosity", 
                    "increase debug message verbosity",
                    v => 
                    { 
                        if (v != null) ++verbosity; 
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

            if (string.IsNullOrWhiteSpace(folder_input))
            {
                folder_input = 
                    @"../../../../X/AndroidSupportComponents-AndroidX-binderate/source/androidx.core/"
                    //@"../../../../X/AndroidSupportComponents-AndroidX-binderate/source/androidx.core/"
                    ;
            }
            if (string.IsNullOrWhiteSpace(folder_output))
            {
                folder_output = "../../../dumps/";
            }

            Console.WriteLine($"Migrating Android.Support in:");
            Console.WriteLine($"    {folder_input}");
            Console.WriteLine($"  to  AndroidX in:");
            Console.WriteLine($"    {folder_output}");

            Task t = await ProcessMetadataFilesAsync(folder_input, folder_output);

            return;
        }

        private static async Task ProcessMetadataFilesAsync(string folder_input, string folder_output)
        {
            string[] files = System.IO.Directory.GetFiles
                                                        (
                                                            folder_input, 
                                                            "Metadata*.xml", 
                                                            System.IO.SearchOption.AllDirectories
                                                        );

            Console.WriteLine($"Migrating files: ");
            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine($"    {files[i]}");

                using (var stream = new System.IO.FileStream(@"C:\temp\test.txt", System.IO.FileMode.Open))
                using (var reader = new System.IO.StreamReader(stream))
                {
                    string content =  await reader.ReadToEndAsync();
                }
            }


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
