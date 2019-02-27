using System.Collections.Generic;
using System.IO;

using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator;

namespace Sample.Migrainator
{
    class Program
    {

        static int verbosity;
        static AndroidXMigrator androidx_migrator = null;
        static List<AssemblyProvider> assemblies = null;

        static string[] path_parts = new string[]
                                            {
                                                Settings.WorkingDirectory,
                                                string.Intern(".."),
                                                string.Intern(".."),
                                                string.Intern(".."),
                                                string.Intern(".."),
                                                string.Intern(".."),
                                                "samples.test.targets",
                                            };

        static void Main(string[] args)
        {
            System.Diagnostics.Trace.Listeners.Add
                                                (
                                                new System.Diagnostics.TextWriterTraceListener
                                                                            (
                                                                                "AndroidX.Migraineator.log",
                                                                                "AndroidX.Migraineator"
                                                                            )
                                                );
            System.Diagnostics.Trace.Listeners.Add
                                                (
                                                    new System.Diagnostics.ConsoleTraceListener()
                                                );
            string input = null;
            string output = null;

            input = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.Android.dll";
            output = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.Android.migrated.dll";
            AndroidXMigrator migrator = null;
            migrator = new AndroidXMigrator
                                        (
                                            Path.Combine(Path.Combine(path_parts), input),
                                            Path.Combine(Path.Combine(path_parts), output)
                                        );

            input = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.Messenger.Android.dll";
            output = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.Messenger.Android.migrated.dll";
            migrator = new AndroidXMigrator
                                        (
                                            Path.Combine(Path.Combine(path_parts), input),
                                            Path.Combine(Path.Combine(path_parts), output)
                                        );
            migrator.Migrate();




            //BatchMigrate();

            System.Diagnostics.Trace.Flush();

            return;
        }

        private static void BatchMigrate()
        {
            assemblies = new List<AssemblyProvider>
            {
                new AssemblyProvider
                {
                    Folder = Path.Combine
                                    (
                                        new string[]
                                        {
                                            Path.Combine(path_parts),
                                           "Traditional.Standard",
                                            "HelloFacebookSample",
                                        }
                                    )
                },
                new AssemblyProvider
                {
                    Folder = Path.Combine
                                    (
                                        new string[]
                                        {
                                            Path.Combine(path_parts),
                                           "Traditional.Standard",
                                            "Sample.AndroidX.XamarinAndroid",
                                        }
                                    )
                },
                new AssemblyProvider
                {
                    Folder = Path.Combine
                                    (
                                        new string[]
                                        {
                                            Path.Combine(path_parts),
                                           "Traditional.Standard",
                                            "XamPercentLib",
                                            "PercentLib"
                                        }
                                    )
                },
            };

            foreach (AssemblyProvider assembly_provider in assemblies)
            {
                foreach (string asm in assembly_provider.Assemblies)
                {
                    string input = asm;
                    string output = $"{asm}.androidx.dll";

                    AndroidXMigrator migrator = new AndroidXMigrator(input, output);
                    migrator.Migrate();

                }
            }
        }
    }
}
