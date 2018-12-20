using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;

using Core.Text;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core.Generated;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core
{
    public partial class AndroidXDiffComparer
    {
        public List<(string OldAndroidSupport, string NewAndroidX)> MappingsArtifacts
        {
            get;
            protected set;
        }

        public List<(string OldAndroidSupport, string NewAndroidX)> MappingsNamespaces
        {
            get;
            protected set;
        }

        public List<(string OldAndroidSupport, string NewAndroidX)> MappingsClasses
        {
            get;
            protected set;
        }

        private string content_mappings_android_support_artifacts_to_androidx_artifacts;

        private string content_mappings_namespaces;
        private string content_mappings_classes;

        public AndroidXDiffComparer()
        {
            return;
        }

        string path_mappings = null;

        //-------------------------------------------------------------------------------------------------------------------
        CharacterSeparatedValues csv_mappings_android_support_artifacts_to_androidx_artifacts;
        IEnumerable<string[]> map_android_support_artifacts_to_androidx_artifacts;
        IEnumerable
            <
                (
                    string AndroidSupportArtifact,
                    string AndroidXArtifact
                )
            > map_typed_android_support_artifacts_to_androidx_artifacts;
        public
            List
            <
                (
                    string AndroidSupportArtifact,
                    string AndroidXArtifact
                )
            > MapAndroidSupportArtifactsToAndroidXArtifacts
        {
            get 
            {
                return map_typed_android_support_artifacts_to_androidx_artifacts.ToList();
            }
        }

        public
            IEnumerable
                <
                    (
                        string AndroidSupportArtifact,
                        string AndroidXArtifact
                    )
                >
                MapAndroidSupportArtifactToAndroidXArtifact(IEnumerable<string[]> untyped_data)
        {
            foreach (string[] row in untyped_data)
            {
                yield return
                        (
                            AndroidSupportArtifact: row[0],
                            AndroidXArtifact: row[2]
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------------
        CharacterSeparatedValues csv_mappings_android_support_artifacts_to_xamarin_android_support_assembly;
        IEnumerable<string[]> map_android_support_artifacts_to_xamarin_android_support_assembly;
        IEnumerable
            <
                (
                    string AndroidSupportArtifact,
                    string XamarinAndroidSupportAssembly
                )
            > map_typed_android_support_artifacts_to_xamarin_android_support_assembly;
        public
            List
            <
                (
                    string AndroidSupportArtifact,
                    string XamarinAndroidSupportAssembly
                )
            > MapAndroidSupportArtifactsToXamarinAndroidSupportAssembly
        {
            get
            {
                return map_typed_android_support_artifacts_to_xamarin_android_support_assembly.ToList();
            }
        }

        public
            IEnumerable
                <
                    (
                        string AndroidSupportArtifact,
                        string XamarinAndroidSupportAssembly
                    )
                >
                MapAndroidSupportArtifactToXamarinAndroidSupportAssembly(IEnumerable<string[]> untyped_data)
        {
            foreach (string[] row in untyped_data)
            {
                yield return
                        (
                            AndroidSupportArtifact: row[0],
                            XamarinAndroidSupportAssembly: row[2]
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------
        CharacterSeparatedValues csv_mappings_androidx_artifacts_with_old_packagenames;
        IEnumerable<string[]> map_androidx_artifacts_with_old_packagenames;
        IEnumerable
            <
                (
                    string AndroidXArtifact,
                    string OldPackageName
                )
            > map_typed_androidx_artifacts_with_old_packagenames;
        public
            List
            <
                (
                    string AndroidXArtifact,
                    string OldPackageName
                )
            > MapAndroidXArtifactToOldPackageName
        {
            get
            {
                return map_typed_androidx_artifacts_with_old_packagenames.ToList();
            }
        }

        public
            IEnumerable
                <
                    (
                        string AndroidXArtifact,
                        string OldPackageName
                    )
                >
                MapAndroidXArtifactToOldPackagename(IEnumerable<string[]> untyped_data)
        {
            foreach (string[] row in untyped_data)
            {
                yield return
                        (
                            AndroidXArtifact: row[0],
                            OldPackageName: row[2]
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------
        CharacterSeparatedValues csv_mappings_androidx_material_packages_to_xamarin_namespaces;
        IEnumerable<string[]> map_androidx_material_packages_to_xamarin_namespaces;
        IEnumerable
            <
                (
                    string AndroidXMaterialPackage,
                    string XamarinNamespace
                )
            > map_typed_androidx_material_packages_to_xamarin_namespaces;
        public
            List
            <
                (
                    string AndroidXMaterialPackage,
                    string XamarinNamespace
                )
            > MapAndroidXMaterialPackagesToXamarinNamespaces
        {
            get
            {
                return map_typed_androidx_material_packages_to_xamarin_namespaces.ToList();
            }
        }

        public
            IEnumerable
                <
                    (
                        string AndroidXMaterialPackage,
                        string XamarinNamespace
                    )
                >
                MapAndroidXMaterialPackageToXamarinNamespace(IEnumerable<string[]> untyped_data)
        {
            foreach (string[] row in untyped_data)
            {
                yield return
                        (
                            AndroidXMaterialPackage: row[0],
                            XamarinNamespace: row[2]
                        );
            }
        }

        public void ModifyApiInfo
                                (
                                    string content_previous,
                                    ApiInfo api_info_androidx
                                )
        {
            foreach (Namespace n in api_info_androidx.Assembly.Namespaces.Namespace)
            {
                string namespace_name = n.Name; 

                if (namespace_name.StartsWith("Android.Support."))
                {
                    var shiet =
                        from map in MapNamespacesAndroisSupportToAndroidX
                            where map.AndroidSupportNamespace == namespace_name
                            select map.AndroidXNamespace
                            ;

                    List<string> replacements = shiet.ToList();

                    foreach(string replacement in  replacements)
                    {
                        Console.WriteLine($"Replacing:");
                        Console.WriteLine($"         {namespace_name}");
                        Console.WriteLine($"   with");
                        Console.WriteLine($"         {replacement}");
                        content_previous = content_previous.Replace(n.Name, replacement);
                    }
                }

                /*
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
                */
            }

            string file = "../../../../X/AndroidSupportComponents-AndroidX-binderate/output/AndroidSupport.AndroidX.api-info.previous.xml";
            System.IO.File.WriteAllText(file, content_previous);

            return;
        }

        //-------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------
        CharacterSeparatedValues csv_mappings_classes_android_support_to_androidx;
        IEnumerable<string[]> map_classes_android_support_to_androidx;
        IEnumerable
            <
                (
                    string AndroidSupportClass,
                    string AndroidXClass
                )
            > map_typed_classes_android_support_to_androidx;
        public
            List
            <
                (
                    string AndroidSupportClass,
                    string AndroidXClass
                )
            > MapClassesAndroidSupportToAndroidX
        {
            get
            {
                return map_typed_classes_android_support_to_androidx.ToList();
            }
        }

        public
            IEnumerable
                <
                    (
                        string AndroidSupportClass,
                        string AndroidXClass
                    )
                >
                MapClassesAndroidSupportClassToAndroidXClass(IEnumerable<string[]> untyped_data)
        {
            foreach (string[] row in untyped_data)
            {
                yield return
                        (
                            AndroidSupportClass: row[0],
                            AndroidXClass: row[2]
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------
        CharacterSeparatedValues csv_mappings_packages_android_support_to_androidx;
        IEnumerable<string[]> map_packages_android_support_to_androidx;
        IEnumerable
            <
                (
                    string AndroidSupportPackage,
                    string AndroidXPackage
                )
            > map_typed_packages_android_support_to_androidx;
        public
            List
            <
                (
                    string AndroidSupportPackage,
                    string AndroidXPackage
                )
            > MapPackagesAndroidSupportToAndroidX
        {
            get
            {
                return map_typed_packages_android_support_to_androidx.ToList();
            }
        }

        public
            IEnumerable
                <
                    (
                        string AndroidSupportPackage,
                        string AndroidXPackage
                    )
                >
                MapPackagesAndroidSupportPackageToAndroidXPackage(IEnumerable<string[]> untyped_data)
        {
            foreach (string[] row in untyped_data)
            {
                yield return
                        (
                            AndroidSupportPackage: row[0],
                            AndroidXPackage: row[2]
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------
        CharacterSeparatedValues csv_mappings_packages_android_support_to_xamarin_namespace;
        IEnumerable<string[]> map_packages_android_support_to_xamarin_namespace;
        IEnumerable
            <
                (
                    string AndroidSupportPackage,
                    string XamarinAndroidSupportNamespace
                )
            > map_typed_packages_android_support_to_xamarin_namespace;
        public
            List
            <
                (
                    string AndroidSupportPackage,
                    string XamarinAndroidSupportNamespace
                )
            > MapPackagesAndroidSupportToXamarinNamespace
        {
            get
            {
                return map_typed_packages_android_support_to_xamarin_namespace.ToList();
            }
        }

        public
            IEnumerable
                <
                    (
                        string AndroidSupportPackage,
                        string XamarinAndroidSupportNamespace
                    )
                >
                MapAndroidSupportPackageToXamarinAndroidSupportNamespace(IEnumerable<string[]> untyped_data)
        {
            foreach (string[] row in untyped_data)
            {
                yield return
                        (
                            AndroidSupportPackage: row[0],
                            XamarinAndroidSupportNamespace: row[2]
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------
        CharacterSeparatedValues csv_xamarin_google_play_services_and_firebase_packages;
        IEnumerable<string[]> map_xamarin_google_play_services_and_firebase_packages;
        IEnumerable
            <
                (
                    string GooglePlayServiceAndFirebaseNamespace,
                    string GooglePlayServiceAndFirebasePackage
                )
            > map_typed_xamarin_google_play_services_and_firebase_packages;
        public
            List
            <
                (
                    string GooglePlayServiceAndFirebaseNamespace,
                    string GooglePlayServiceAndFirebasePackage
                )
            > MapXamarinGooglePlayServicesAndFirebase
        {
            get
            {
                return map_typed_xamarin_google_play_services_and_firebase_packages.ToList();
            }
        }

        public
            IEnumerable
                <
                    (
                        string GooglePlayServiceAndFirebaseNamespace,
                        string GooglePlayServiceAndFirebasePackage
                    )
                >
                MapXamarinGooglePlayServiceAndFirebaseNamespaces(IEnumerable<string[]> untyped_data)
        {
            foreach (string[] row in untyped_data)
            {
                yield return
                        (
                            GooglePlayServiceAndFirebaseNamespace: row[0],
                            GooglePlayServiceAndFirebasePackage: "n/a"
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------
        CharacterSeparatedValues csv_mappings_namespaces_android_support_to_androidx;
        IEnumerable<string[]> map_namespaces_android_support_to_androidx;
        IEnumerable
            <
                (
                    string AndroidSupportNamespace,
                    string AndroidXNamespace
                )
            > map_typed_namespaces_android_support_to_androidx;
        public
            List
            <
                (
                    string AndroidSupportNamespace,
                    string AndroidXNamespace
                )
            > MapNamespacesAndroisSupportToAndroidX
        {
            get
            {
                return map_typed_namespaces_android_support_to_androidx.ToList();
            }
        }

        public
            IEnumerable
                <
                    (
                        string AndroidSupportNamespace,
                        string AndroidXNamespace
                    )
                >
                MapAndroidSupportNamsepcaeToAndroidXNamespace(IEnumerable<string[]> untyped_data)
        {
            foreach (string[] row in untyped_data)
            {
                yield return
                        (
                            GooglePlayServiceAndFirebaseNamespace: row[0],
                            GooglePlayServiceAndFirebasePackage: row[2]
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------


        public async Task InitializeAsync(string path)
        {
            path_mappings = path;

            string file = null;

            file = path_mappings + "mappings-android-support-artifacts-to-androidx-artifacts.csv";
            csv_mappings_android_support_artifacts_to_androidx_artifacts = new CharacterSeparatedValues();
            await csv_mappings_android_support_artifacts_to_androidx_artifacts.LoadAsync(file);
            map_android_support_artifacts_to_androidx_artifacts =
                            csv_mappings_android_support_artifacts_to_androidx_artifacts
                                        .ParseTemporaryImplementation()
                                        .ToList()
                                        ;
            map_typed_android_support_artifacts_to_androidx_artifacts =
                    MapAndroidSupportArtifactToAndroidXArtifact(map_android_support_artifacts_to_androidx_artifacts)
                    .ToList()
                    ;

            file = path_mappings + "mappings-android-support-artifacts-to-xamarin-android-support-assembly.csv";
            csv_mappings_android_support_artifacts_to_xamarin_android_support_assembly = new CharacterSeparatedValues();
            await csv_mappings_android_support_artifacts_to_xamarin_android_support_assembly.LoadAsync(file);
            map_android_support_artifacts_to_xamarin_android_support_assembly =
                            csv_mappings_android_support_artifacts_to_xamarin_android_support_assembly
                                        .ParseTemporaryImplementation()
                                        .ToList()
                                        ;
            map_typed_android_support_artifacts_to_androidx_artifacts = 
                    MapAndroidSupportArtifactToXamarinAndroidSupportAssembly(map_android_support_artifacts_to_xamarin_android_support_assembly)
                    .ToList()
                    ;

            file = path_mappings + "mappings-androidx-artifacts-with-old-packagenames.csv";
            csv_mappings_androidx_artifacts_with_old_packagenames = new CharacterSeparatedValues();
            await csv_mappings_androidx_artifacts_with_old_packagenames.LoadAsync(file);
            map_androidx_artifacts_with_old_packagenames =
                            csv_mappings_androidx_artifacts_with_old_packagenames
                                        .ParseTemporaryImplementation()
                                        .ToList()
                                        ;
            map_typed_androidx_artifacts_with_old_packagenames =
                    MapAndroidXArtifactToOldPackagename(map_androidx_artifacts_with_old_packagenames)
                    .ToList()
                    ;

            file = path_mappings + "mappings-androidx-material-packages-to-xamarin-namespaces.csv";
            csv_mappings_androidx_material_packages_to_xamarin_namespaces = new CharacterSeparatedValues();
            await csv_mappings_androidx_material_packages_to_xamarin_namespaces.LoadAsync(file);
            map_androidx_material_packages_to_xamarin_namespaces =
                            csv_mappings_androidx_material_packages_to_xamarin_namespaces
                                        .ParseTemporaryImplementation()
                                        .ToList()
                                        ;
            map_typed_androidx_material_packages_to_xamarin_namespaces =
                    MapAndroidXMaterialPackageToXamarinNamespace(map_androidx_material_packages_to_xamarin_namespaces)
                    .ToList()
                    ;

            file = path_mappings + "mappings-classes-android-support-to-androidx.csv";
            csv_mappings_classes_android_support_to_androidx = new CharacterSeparatedValues();
            await csv_mappings_classes_android_support_to_androidx.LoadAsync(file);
            map_classes_android_support_to_androidx =
                            csv_mappings_classes_android_support_to_androidx
                                        .ParseTemporaryImplementation()
                                        .ToList()
                                        ;
            map_typed_classes_android_support_to_androidx =
                    MapClassesAndroidSupportClassToAndroidXClass(map_classes_android_support_to_androidx)
                    .ToList()
                    ;

            file = path_mappings + "mappings-packages-android-support-to-androidx.csv";
            csv_mappings_packages_android_support_to_androidx = new CharacterSeparatedValues();
            await csv_mappings_packages_android_support_to_androidx.LoadAsync(file);
            map_packages_android_support_to_androidx =
                            csv_mappings_packages_android_support_to_androidx
                                        .ParseTemporaryImplementation()
                                        .ToList()
                                        ;
            map_typed_packages_android_support_to_androidx =
                    MapPackagesAndroidSupportPackageToAndroidXPackage(map_packages_android_support_to_androidx)
                    .ToList()
                    ;

            file = path_mappings + "mappings-packages-android-support-to-xamarin-namespace.csv";
            csv_mappings_packages_android_support_to_xamarin_namespace = new CharacterSeparatedValues();
            await csv_mappings_packages_android_support_to_xamarin_namespace.LoadAsync(file);
            map_packages_android_support_to_xamarin_namespace =
                            csv_mappings_packages_android_support_to_xamarin_namespace
                                        .ParseTemporaryImplementation()
                                        .ToList()
                                        ;
            map_typed_packages_android_support_to_xamarin_namespace =
                    MapAndroidSupportPackageToXamarinAndroidSupportNamespace(map_packages_android_support_to_xamarin_namespace)
                    .ToList()
                    ;

            file = path_mappings + "xamarin-google-play-services-and-firebase-packages.csv";
            csv_xamarin_google_play_services_and_firebase_packages = new CharacterSeparatedValues();
            await csv_xamarin_google_play_services_and_firebase_packages.LoadAsync(file);
            map_xamarin_google_play_services_and_firebase_packages =
                            csv_xamarin_google_play_services_and_firebase_packages
                                        .ParseTemporaryImplementation()
                                        .ToList()
                                        ;
            map_typed_xamarin_google_play_services_and_firebase_packages =
                    MapXamarinGooglePlayServiceAndFirebaseNamespaces(map_xamarin_google_play_services_and_firebase_packages)
                    .ToList()
                    ;

            file = path_mappings + "mappings-namespaces-android-support-to-androidx.csv";
            csv_mappings_namespaces_android_support_to_androidx = new CharacterSeparatedValues();
            await csv_mappings_namespaces_android_support_to_androidx.LoadAsync(file);
            map_namespaces_android_support_to_androidx =
                            csv_mappings_namespaces_android_support_to_androidx
                                        .ParseTemporaryImplementation()
                                        .ToList()
                                        ;
            map_typed_namespaces_android_support_to_androidx =
                    MapAndroidSupportNamsepcaeToAndroidXNamespace(map_namespaces_android_support_to_androidx)
                    .ToList()
                    ;

            return;
        }






        private async Task InitializeMappingsNamespacesAsync(string content)
        {
            await Task.Run(() => InitializeMappingsNamespaces(content));

            return;
        }

        private void InitializeMappingsNamespaces(string content)
        {
            string[] lines = content.Split
                                        (
                                            new string[] { Environment.NewLine, @"\n" },
                                            StringSplitOptions.RemoveEmptyEntries
                                        );

            this.MappingsNamespaces = new List<(string OldAndroidSupport, string NewAndroidX)>();

            for (int i = 0; i < lines.Length; i++)
            {
                string[] columns = lines[i].Split
                                        (
                                            new char[] { ',' },
                                            StringSplitOptions.RemoveEmptyEntries
                                        );
                (string OldAndroidSupport, string NewAndroidX) t = (columns[0], columns[2]);
                MappingsNamespaces.Add(t);
            }

            // replacing long strngs 1st - less chances to replace partial (substrings)
            List<(string OldAndroidSupport, string NewAndroidX)> ns =
                (
                    from (string OldAndroidSupport, string NewAndroidX) mapping in MappingsNamespaces
                    orderby mapping.OldAndroidSupport.Length descending
                    select mapping
                ).ToList<(string OldAndroidSupport, string NewAndroidX)>();

            this.MappingsNamespaces = ns;

            return;
        }

        public async Task<string> MigrateMetadataXmlFileNamespacesAsync(string file)
        {
            string content;
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

            await Task.Run
                        (
                            () =>
                            {
                                for (int i = 0; i < this.MappingsNamespaces.Count; i++)
                                {
                                    string namepsace_old = this.MappingsNamespaces[i].OldAndroidSupport;
                                    string namepsace_new = this.MappingsNamespaces[i].NewAndroidX;


                                    string search = $"package[@name='{namepsace_old}']";
                                    string replace = null;

                                    AmbiguityFix
                                            (
                                                // filename for ambiguity fix - artifact based
                                                file, 
                                                //  content to be migrated
                                                ref content, 
                                                //  old namespace
                                                namepsace_old, 
                                                //  new namespace
                                                ref namepsace_new, 
                                                //  search string
                                                search, 
                                                //  replacement string
                                                ref replace
                                            );
                                }
                            }
                        );

            return content;
        }

        public async Task<string> MigrateEnumMethodsXmlFileNamespacesAsync(string file)
        {
            string content;
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

            await Task.Run
                        (
                            () =>
                            {
                                for (int i = 0; i < this.MappingsNamespaces.Count; i++)
                                {
                                    string namepsace_old = this.MappingsNamespaces[i].OldAndroidSupport;
                                    string namepsace_new = this.MappingsNamespaces[i].NewAndroidX;


                                    string search = $"package[@name='{namepsace_old}']";
                                    string replace = null;

                                    AmbiguityFix
                                            (
                                                // filename for ambiguity fix - artifact based
                                                file,
                                                //  content to be migrated
                                                ref content,
                                                //  old namespace
                                                namepsace_old,
                                                //  new namespace
                                                ref namepsace_new,
                                                //  search string
                                                search,
                                                //  replacement string
                                                ref replace
                                            );
                                }
                            }
                        );

            return content;
        }

        private void AmbiguityFix
                            (
                                string file, 
                                ref string content, 
                                string namepsace_old, 
                                ref string namepsace_new, 
                                string search, 
                                ref string replace
                            )
        {
            if (file.StartsWith("Metadata") && file.EndsWith(".xml"))
            {
                replace = $"package[@name='{namepsace_new}']";
            }
            else if (file.Contains("EnumMethods.xml"))
            {
                search = $"{namepsace_old}".Replace(".", "/");
            }
            else
            {
                string msg = $"Unknown file {file}";
                Console.WriteLine(msg);
            }

            if (content.Contains(search))
            {
                Console.WriteLine($"         Found         : {search}");

                if (namepsace_old == "android.support.v4.widget")
                {
                    // fixing ambigious mappings based on path to the artifact
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"         Ambiguity for        : {namepsace_old}");

                    if (file.Contains("swiperefreshlayout"))
                    {
                        namepsace_new = "androidx.swiperefreshlayout.widget";
                    }
                    else if (file.Contains("cursoradapter"))
                    {
                        namepsace_new = "androidx.cursoradapter.widget";
                    }
                    else if (file.Contains("coordinatorlayout"))
                    {
                        namepsace_new = "androidx.coordinatorlayout.widget";
                    }
                    else if (file.Contains("drawerlayout"))
                    {
                        namepsace_new = "androidx.drawerlayout.widget";
                    }
                    else if (file.Contains("customview"))
                    {
                        namepsace_new = "androidx.customview.widget";
                    }
                    else if (file.Contains("slidingpanelayout"))
                    {
                        namepsace_new = "androidx.slidingpanelayout.widget";
                    }
                    else
                    {
                        namepsace_new = "androidx.core.widget";
                    }
                    Console.WriteLine($"         Ambiguity fixed        : {namepsace_new}");
                    Console.ResetColor();
                }
                else if (namepsace_old == "android.support.v4.view")
                {
                    // fixing ambigious mappings based on path to the artifact
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"         Ambiguity for        : {namepsace_old}");

                    if (file.Contains("customview"))
                    {
                        namepsace_new = "androidx.customview.view";
                    }
                    if (file.Contains("viewpager"))
                    {
                        namepsace_new = "androidx.viewpager.widget";
                    }
                    else
                    {
                        string msg = $"Ambiguity not resolved for {namepsace_old}";
                        Console.WriteLine(msg);
                        //throw new InvalidDataException(msg);
                    }

                    Console.WriteLine($"         Ambiguity fixed        : {namepsace_new}");
                    Console.ResetColor();
                }

                if (file.StartsWith("Metadata") && file.EndsWith(".xml"))
                {
                    replace = $"package[@name='{namepsace_new}']";
                    Console.WriteLine($"         Replacing with: {replace}");
                }
                else if (file.Contains("EnumMethods.xml"))
                {
                    replace = $"{namepsace_new}".Replace(".", "/");
                    Console.WriteLine($"         Replacing with: {replace}");
                }
                else
                {
                    string msg = $"Unknown file {file}";
                    Console.WriteLine(msg);
                }

                content = content.Replace(search, replace);
            }

            return;
        }

        public string ContentApiInfoNew
        {
            get;
            set;
        }

        public string ContentApiInfoOld
        {
            get;
            set;
        }

        public string ApiInfoFileOld
        {
            get;
            set;
        }

        public string ApiInfoFileNew
        {
            get;
            set;
        }

        public ApiInfo ApiInfoDataNew
        {
            get;
            set;
        }

        public ApiInfo ApiInfoDataOld
        {
            get;
            set;
        }

        public async Task<ApiInfo> ApiInfo(string file_input)
        {
            StreamReader sr = null;
            XmlSerializer serializer = new XmlSerializer(typeof(ApiInfo));
            ApiInfo api_info;

            try
            {
                sr = new StreamReader(file_input);
                api_info = (ApiInfo)serializer.Deserialize(sr);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Exception deserializing {file_input}");
                Console.ResetColor();

                throw;
            }
            finally
            {
                serializer = null;
                sr.Close();
                sr = null;
            }

            try
            {
                sr = new StreamReader(file_input);
                ContentApiInfoNew = await sr.ReadToEndAsync();
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Exception reading {file_input}");
                Console.ResetColor();

                throw;
            }
            finally
            {
                sr.Close();
                sr = null;
            }

            return api_info;
        }



    }
}
