using System;
namespace Sample.Migraineator.ConsoleApp
{
    public class Options
    {
        public Options()
        {
        }

        static string folder_input = null;
        static string folder_output = null;
        static int verbosity;
        static bool show_help;

        public static Mono.Options.OptionSet OptionSet
        {
            get
            {
                return option_set;
            }

        }

        private static Mono.Options.OptionSet option_set = new Mono.Options.OptionSet() 
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
                    if (v != null)
                    {
                        ++verbosity;
                    }
                } 
            },
            { 
                "h|help",  
                "show this message and exit",
                v => show_help = v != null 
            },
        };
    }
}
