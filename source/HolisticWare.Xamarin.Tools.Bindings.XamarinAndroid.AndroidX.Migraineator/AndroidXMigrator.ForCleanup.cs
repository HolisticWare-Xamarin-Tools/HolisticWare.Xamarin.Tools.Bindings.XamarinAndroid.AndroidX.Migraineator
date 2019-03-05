using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator
{
    public partial class AndroidXMigrator
    {
        public List <
                        (
                            string ArtifactAndroidSupport, 
                            string ArtifactAndroidX
                        )
                    > MappingsArtifacts
        {
            get;
            protected set;
        }

        public List<
                        (
                            string NamespaceAndroidSupport,
                            string NamespaceAndroidX
                        )
                    > MappingsNamespaces
        {
            get;
            protected set;
        }

        public List <
                        (
                            string ClassAndroidSupport,
                            string ClassArtifactAndroidX
                        )
                    > MappingsClasses
        {
            get;
            protected set;
        }

        private string content_mappings_artifacts;
        private string content_mappings_namespaces;
        private string content_mappings_classes;

        string path_mappings = null;

        public async Task InitializeAsync(string path)
        {
            path_mappings = path;

            string file = null;

            file = path_mappings + "android-support-to-androidx-mappings-artifacts.csv";
            content_mappings_artifacts = File.ReadAllText(file);
            using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
            {
                content_mappings_artifacts = await sr.ReadToEndAsync();
            }
            InitializeMappingsArtifacts(content_mappings_artifacts);

            file = path_mappings + "android-support-to-androidx-mappings-namespaces.csv";
            content_mappings_namespaces = File.ReadAllText(file);
            using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
            {
                content_mappings_namespaces = await sr.ReadToEndAsync();
            }
            InitializeMappingsNamespaces(content_mappings_namespaces);

            file = path_mappings + "android-support-to-androidx-mappings-classes.csv";
            content_mappings_namespaces = File.ReadAllText(file);
            using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
            {
                content_mappings_classes = await sr.ReadToEndAsync();
            }

            return;
        }

        private void InitializeMappingsArtifacts(string content)
        {
            string[] lines = content.Split
                                        (
                                            new string[] { Environment.NewLine, @"\n" },
                                            StringSplitOptions.RemoveEmptyEntries
                                        );

            this.MappingsArtifacts = new List<(string OldAndroidSupport, string NewAndroidX)>();

            for (int i = 0; i < lines.Length; i++)
            {
                string[] columns = lines[i].Split
                                        (
                                            new char[] { ',' },
                                            StringSplitOptions.RemoveEmptyEntries
                                        );
                (string OldAndroidSupport, string NewAndroidX) t = (columns[0], columns[2]);
                MappingsArtifacts.Add(t);
            }

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
                                    string namepsace_old = this.MappingsNamespaces[i].NamespaceAndroidSupport;
                                    string namepsace_new = this.MappingsNamespaces[i].NamespaceAndroidX;


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
                                    string namepsace_old = this.MappingsNamespaces[i].NamespaceAndroidSupport;
                                    string namepsace_new = this.MappingsNamespaces[i].NamespaceAndroidX;


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

    }
}
