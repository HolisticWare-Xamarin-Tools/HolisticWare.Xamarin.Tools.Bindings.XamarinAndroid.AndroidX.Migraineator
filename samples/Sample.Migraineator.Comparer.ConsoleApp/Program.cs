using System;

namespace Sample.Migraineator.Comparer.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
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
    }
}
