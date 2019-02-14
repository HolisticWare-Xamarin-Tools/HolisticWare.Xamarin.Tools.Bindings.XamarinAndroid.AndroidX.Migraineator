using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Text;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core.Generated;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core
{
    public partial class BuildProject
    {
        public BuildProject()
        {
            return;
        }

        public BuildProject(string root_folder)
        {
            this.FolderRoot = root_folder;

            return;
        }

        public string FolderRoot
        {
            get;
            set;
        }

        public string ApiInfoFile
        {
            get;
            set;
        }

        public string FolderOutput
        {
            get
            {
                string path = Path.Combine(FolderRoot, "output");

                return path;
            }
        }

        public string FolderWithGenerated
        {
            get
            {
                string path = Path.Combine(FolderRoot, "generated");

                return path;
            }
        }

        public List <
                        (
                            string AndroidArtifact,
                            string ManagedAssembly
                        )
                    > 
                    AndroidArtifacts_X_ManagedAssemblies
        {
            get;
            set;
        }



        string xml_namespace_name = "csproj";
        string xml_namespace = "http://schemas.microsoft.com/developer/msbuild/2003";
        string[] files = null;


        public void ProjectFiles(string path, string name)
        {
            AndroidArtifacts_X_ManagedAssemblies = new List <
                                                                (
                                                                    string AndroidArtifact, 
                                                                    string ManagedAssembly
                                                                )
                                                            >();

            files = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
                xmldoc.Load(file);

                System.Xml.XmlNamespaceManager ns = new System.Xml.XmlNamespaceManager(xmldoc.NameTable);
                //  null namespace, is the namespace with no URI. I.e. the namespace that this element is in:
                //  <a xmlns=""></a>
                ns.AddNamespace(xml_namespace_name, xml_namespace);

                string assembly_name = null;
                System.Xml.XmlNodeList nl1 = xmldoc.SelectNodes("//AssemblyName", ns);
                foreach (System.Xml.XmlNode n1 in nl1)
                {
                    assembly_name = n1.InnerText;
                }

                string artifact_name = null;

                if (name == "AndroidX")
                {
                    artifact_name = ArtifactNameForAndroidX(xmldoc, ns);
                }
                else if (name == "Android.Support")
                {
                    artifact_name = ArtifactNameForAndroidSupport(xmldoc, ns);
                }

                AndroidArtifacts_X_ManagedAssemblies.Add
                                                        (
                                                            (
                                                                AndroidArtifact: artifact_name,
                                                                ManagedAssembly: assembly_name
                                                            )
                                                        );

            }

            return;
        }

        private string ArtifactNameForAndroidX
                                        (
                                            System.Xml.XmlDocument xmldoc, 
                                            System.Xml.XmlNamespaceManager ns
                                        )
        {
            string artifact_name = null;

            System.Xml.XmlNodeList nl2 = xmldoc.SelectNodes("//InputJar/@Include", ns);
            foreach (System.Xml.XmlNode n2 in nl2)
            {
                string txt = n2.InnerText;
                if (txt.Contains($@"externals\"))
                {
                    int pos = n2.InnerText.IndexOf(@"externals\");
                    artifact_name = n2.InnerText.Substring(pos + @"externals\".Length);
                    artifact_name = artifact_name.Replace(@"\", ":");

                    if (txt.Contains("classes.jar"))
                    {
                        artifact_name = artifact_name.Replace(@"classes.jar", "");
                    }
                    else
                    {
                        artifact_name = artifact_name.Replace(@"classes.jar", "");
                    }

                    break;
                }
            }

            System.Xml.XmlNodeList nl3 = xmldoc.SelectNodes("//EmbeddedJar/@Include", ns);
            foreach (System.Xml.XmlNode n3 in nl3)
            {
                string txt = n3.InnerText;
                if (txt.Contains($@"externals\"))
                {
                    int pos = n3.InnerText.IndexOf(@"externals\");
                    artifact_name = n3.InnerText.Substring(pos + @"externals\".Length);
                    int pos2 = artifact_name.LastIndexOf('\\');
                    char[] chars = artifact_name.ToCharArray();
                    if (pos2 >= 0)
                    {
                        chars[pos2] = ':';
                    }
                    artifact_name = new string(chars);
                    artifact_name = artifact_name.Replace(@"\", ":");

                    if (txt.Contains("classes.jar"))
                    {
                        artifact_name = artifact_name.Replace(@"classes.jar", "");
                    }
                    else
                    {
                        artifact_name = artifact_name.Replace(@".jar", "");
                    }

                    break;
                }
            }

            if (artifact_name.EndsWith(":"))
            {
                artifact_name = artifact_name.TrimEnd(new char[] { ':' });
            }

            return artifact_name;
        }

        private string ArtifactNameForAndroidSupport
                                        (
                                            System.Xml.XmlDocument xmldoc,
                                            System.Xml.XmlNamespaceManager ns
                                        )
        {
            string artifact_name = null;

            System.Xml.XmlNodeList nl2 = xmldoc.SelectNodes("//InputJar/@Include", ns);
            foreach (System.Xml.XmlNode n2 in nl2)
            {
                string txt = n2.InnerText;
                if (txt.Contains($@"externals\"))
                {
                    int pos = n2.InnerText.IndexOf(@"externals\");
                    artifact_name = n2.InnerText.Substring(pos + @"externals\".Length);
                    artifact_name = artifact_name.Replace(@"\", ":");

                    if (txt.Contains("classes.jar"))
                    {
                        artifact_name = artifact_name.Replace(@"classes.jar", "");
                    }
                    else
                    {
                        artifact_name = artifact_name.Replace(@"classes.jar", "");
                    }

                    break;
                }
            }

            System.Xml.XmlNodeList nl3 = xmldoc.SelectNodes("//EmbeddedJar/@Include", ns);
            foreach (System.Xml.XmlNode n3 in nl3)
            {
                string txt = n3.InnerText;
                if (txt.Contains($@"externals\"))
                {
                    int pos = n3.InnerText.IndexOf(@"externals\");
                    artifact_name = n3.InnerText.Substring(pos + @"externals\".Length);
                    int pos2 = artifact_name.LastIndexOf('\\');
                    char[] chars = artifact_name.ToCharArray();
                    if (pos2 >= 0)
                    {
                        chars[pos2] = ':';
                    }
                    artifact_name = new string(chars);
                    artifact_name = artifact_name.Replace(@"\", ":");

                    if (txt.Contains("classes.jar"))
                    {
                        artifact_name = artifact_name.Replace(@"classes.jar", "");
                    }
                    else
                    {
                        artifact_name = artifact_name.Replace(@".jar", "");
                    }

                    break;
                }
            }

            if (artifact_name.EndsWith(":"))
            {
                artifact_name = artifact_name.TrimEnd(new char[] { ':' });
            }

            int pos3 = artifact_name.LastIndexOf('.');
            char[] chars3 = artifact_name.ToCharArray();
            if (pos3 >= 0)
            {
                chars3[pos3] = ':';
            }
            artifact_name = new string(chars3);

            return artifact_name;
        }




        public void Dump(string name, bool prettyified = false)
        {
            string path = Path.Combine
                (
                    new string[]
                    {
                            Environment.CurrentDirectory,
                            "..",
                            "output"
                    }
                );
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string path_output = Path.Combine(path, "XmlDocument-ProjectData");
            if (!Directory.Exists(path_output))
            {
                Directory.CreateDirectory(path_output);
            }

            string filename = $"{name}.AndroidArtifacts_X_ManagedAssemblies";

            filename = Path.Combine(path_output, filename);

            string fmt = "{0},{1}";
            string fmtp = "";
            if (prettyified == true)
            {
                fmtp = GetDumpFormat(this.AndroidArtifacts_X_ManagedAssemblies);
            }

            StringBuilder sb = new StringBuilder();
            StringBuilder sbp = new StringBuilder();
            foreach
                (
                    (
                        string AndroidArtifact,
                        string ManagedAssembly
                    ) mapping
                    in this.AndroidArtifacts_X_ManagedAssemblies
                )
            {
                string android_artifact = mapping.AndroidArtifact;
                string managed_assembly = mapping.ManagedAssembly;

                sb.AppendLine
                    (
                        string.Format
                                (
                                    fmt,
                                    android_artifact, managed_assembly
                                )
                    );
                sbp.AppendLine
                    (
                        string.Format
                                (
                                    fmtp,
                                    android_artifact, managed_assembly
                                )
                    );
            }

            if (prettyified == true)
            {
                System.IO.File.WriteAllText($"{filename}.prettyfied.csv", sbp.ToString());
            }
            System.IO.File.WriteAllText($"{filename}.csv", sb.ToString());

            return;
        }

        private string GetDumpFormat
            (
                IEnumerable
                    <
                        (
                            string AndroidArtifact,
                            string ManagedAssembly
                        )
                    >
                    results_found,
                int padding = 3
            )
        {
            int? length_aa = 0;
            int length_aa_max = 0;

            int? length_ma = 0;
            int length_ma_max = 0;

            foreach
                (
                    (
                        string AndroidArtifact,
                        string ManagedAssembly
                    ) c
                    in results_found
                )
            {
                length_aa = c.AndroidArtifact?.Length;
                if (length_aa > length_aa_max)
                {
                    length_aa_max = length_aa.Value;
                }

                length_ma = c.ManagedAssembly?.Length;
                if (length_ma > length_ma_max)
                {
                    length_ma_max = length_ma.Value;
                }

            }
            string fmt =
                    "{0,-" + (length_aa_max + padding) + "}"
                    +
                    ",{1}"
                        ;

            return fmt;
        }

    }
}
