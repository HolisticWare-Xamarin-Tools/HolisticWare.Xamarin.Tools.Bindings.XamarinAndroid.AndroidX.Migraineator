using System.Collections.Generic;
using System.IO;
using System.Linq;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.AST;

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

            AndroidXMigrator migrator = null;
            List<string> dlls = new List<string>(
                                                    Directory.EnumerateFiles
                                                                (
                                                                    "../../samples.test.targets",
                                                                    "*.dll",
                                                                    SearchOption.AllDirectories
                                                                )
                                                )
                                                .Where(x => ! x.Contains("linksrc"))
                                                .Where(x => ! x.Contains("android/assets"))
                                                .Where(x => ! x.Contains(".app/"))
                                                .Where(x => ! x.Contains(".resources.dll"))
                                                .ToList();
                                                ;
                       
            foreach (string dll in dlls)
            {
                if(dll.Contains("linksrc"))
                {
                    continue;
                }
                migrator = new AndroidXMigrator(dll, dll.Replace(".dll", ".migrated.dll"));
                migrator.Migrate();
            }

            File.WriteAllText
                (
                    $"AndroidSupportNotFoundInGoogle.json",
                    Newtonsoft.Json.JsonConvert.SerializeObject
                                                    (
                                                        AndroidXMigrator.AndroidSupportNotFoundInGoogle,
                                                        Newtonsoft.Json.Formatting.Indented
                                                    )

                );

            List<string> files = new List<string>(Directory.EnumerateFiles(".", "AbstractSyntaxTree.*.json"));
           
            foreach (string f in files)
            {
                File.Delete(f);
            }

            string input = null;
            string output = null;

            input = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.Android.dll";
            output = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.Android.migrated.dll";
            migrator = new AndroidXMigrator
                                        (
                                            Path.Combine(Path.Combine(path_parts), input),
                                            Path.Combine(Path.Combine(path_parts), output)
                                        );
            migrator.Migrate();

            input = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.Messenger.Android.dll";
            output = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.Messenger.Android.migrated.dll";
            migrator = new AndroidXMigrator
                                        (
                                            Path.Combine(Path.Combine(path_parts), input),
                                            Path.Combine(Path.Combine(path_parts), output)
                                        );
            migrator.Migrate();

            input = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.AppLinks.Android.dll";
            output = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.AppLinks.Android.migrated.dll";
            migrator = new AndroidXMigrator
                                        (
                                            Path.Combine(Path.Combine(path_parts), input),
                                            Path.Combine(Path.Combine(path_parts), output)
                                        );
            migrator.Migrate();


            input = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.Common.Android.dll";
            output = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.Common.Android.migrated.dll";
            migrator = new AndroidXMigrator
                                        (
                                            Path.Combine(Path.Combine(path_parts), input),
                                            Path.Combine(Path.Combine(path_parts), output)
                                        );
            migrator.Migrate();


            input = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.AppLinks.Android.dll";
            output = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.AppLinks.Android.migrated.dll";
            migrator = new AndroidXMigrator
                                        (
                                            Path.Combine(Path.Combine(path_parts), input),
                                            Path.Combine(Path.Combine(path_parts), output)
                                        );
            migrator.Migrate();


            input = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.Core.Android.dll";
            output = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.Core.Android.migrated.dll";
            migrator = new AndroidXMigrator
                                        (
                                            Path.Combine(Path.Combine(path_parts), input),
                                            Path.Combine(Path.Combine(path_parts), output)
                                        );
            migrator.Migrate();


            input = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.Login.Android.dll";
            output = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.Login.Android.migrated.dll";
            migrator = new AndroidXMigrator
                                        (
                                            Path.Combine(Path.Combine(path_parts), input),
                                            Path.Combine(Path.Combine(path_parts), output)
                                        );
            migrator.Migrate();

            input = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.Places.Android.dll";
            output = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.Places.Android.migrated.dll";
            migrator = new AndroidXMigrator
                                        (
                                            Path.Combine(Path.Combine(path_parts), input),
                                            Path.Combine(Path.Combine(path_parts), output)
                                        );
            migrator.Migrate();


            input = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.Share.Android.dll";
            output = "Traditional.Standard/HelloFacebookSample/bin/Debug/Xamarin.Facebook.Places.Android.migrated.dll";
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
