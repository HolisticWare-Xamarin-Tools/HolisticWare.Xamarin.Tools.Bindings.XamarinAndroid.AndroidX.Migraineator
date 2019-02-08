using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Core.Text;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core.Generated;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core
{
    public partial class BuildProject
    {
        public BuildProject(string root_folder)
        {
            this.FolderRoot = root_folder;

            return;
        }

        public string FolderRoot
        {
            get;
            protected set;
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

        string xml_namespace_name = "csproj";
        string xml_namespace = "http://schemas.microsoft.com/developer/msbuild/2003";

        public IEnumerable   <
                                    (
                                        string AssemblyName,
                                        string  
                                    )
                                > ProjectFiles(string path)
        {
            var files = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
                xmldoc.Load(file);

                System.Xml.XmlNamespaceManager ns = new System.Xml.XmlNamespaceManager(xmldoc.NameTable);
                //  null namespace, is the namespace with no URI. I.e. the namespace that this element is in:
                //  <a xmlns=""></a>
                ns.AddNamespace(xml_namespace_name, xml_namespace);

                System.Xml.XmlNodeList nl1 = xmldoc.SelectNodes("//AssemblyName");
                System.Xml.XmlNodeList nl2 = xmldoc.SelectNodes("//InputJar");
            }

            return null;
        }

    }
}
