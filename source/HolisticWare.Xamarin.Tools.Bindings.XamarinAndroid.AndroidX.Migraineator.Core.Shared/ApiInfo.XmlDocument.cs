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
    public partial class ApiInfo
    {
        public class XmlDocumentData
        {
            public XmlDocumentData(string path)
            {
                file_name = path;
                xmldoc = new System.Xml.XmlDocument();
                xmldoc.Load(file_name);

                ns = new System.Xml.XmlNamespaceManager(xmldoc.NameTable);
                //  null namespace, is the namespace with no URI. I.e. the namespace that this element is in:
                //  <a xmlns=""></a>
                ns.AddNamespace(xml_namespace_name, xml_namespace);

                return;
            }

            string file_name = null;
            System.Xml.XmlDocument xmldoc = null;
            System.Xml.XmlNamespaceManager ns = null;
            string xml_namespace_name = "apixml";
            string xml_namespace = "";

            public 
                IEnumerable<string> 
                    Namespaces
            {
                get;
                protected set;
            }

            public
                IEnumerable<
                                (
                                    string ClassName,
                                    string ManagedNamespace
                                )
                            >
                    Classes
            {
                get;
                protected set;
            }

            public
                IEnumerable<
                                (
                                    string InterfaceName,
                                    string ManagedNamespace
                                )
                            >
                    Interfaces
            {
                get;
                protected set;
            }

            public
                IEnumerable<
                                (
                                    string InterfaceName,
                                    string ManagedNamespace
                                )
                            >
                    InterfacesFromClasses
            {
                get;
                protected set;
            }

            public void AnayseAPI()
            {
                this.Namespaces = this.GetNamespaces();

                this.Classes = this.GetClasses();

                this.Interfaces = this.GetInterfaces();

                this.InterfacesFromClasses = this.GetInterfacesFromClasses();

            }

            public void DumpAPI(string filename_base)
            {
                this.DumpNamespaces(filename_base);
                this.DumpClasses(filename_base);
                this.DumpInterfaces(filename_base);
                this.DumpInterfacesFromClasses(filename_base);

                return;
            }

            protected
                IEnumerable<string>
                                GetNamespaces()
            {
                string nodes_to_find =
                    $@"/apixml:assemblies/apixml:assembly/apixml:namespaces/apixml:namespace"
                    ;
                System.Xml.XmlNodeList node_list = xmldoc.SelectNodes(nodes_to_find, ns);

                foreach (System.Xml.XmlNode node in node_list)
                {
                    string namespace_name = node.Attributes["name"].Value;

                    yield return namespace_name;
                }

            }

            protected
                IEnumerable <
                                (
                                    string ClassName,
                                    string ManagedNamespace
                                )
                            >
                                GetClasses()
            {
                string nodes_to_find =
                    $@"/apixml:assemblies/apixml:assembly/apixml:namespaces/apixml:namespace"
                    +
                    $@"/apixml:classes/apixml:class"
                    ;
                System.Xml.XmlNodeList node_list = xmldoc.SelectNodes(nodes_to_find, ns);

                foreach(System.Xml.XmlNode node in node_list)
                {
                    string class_name = node.Attributes["name"].Value;
                    System.Xml.XmlNode node_parent = node.ParentNode.ParentNode;
                    string namespace_name = node_parent.Attributes["name"].Value;

                    yield return
                                (
                                    ClassName: class_name,
                                    ManagedNamespace: namespace_name
                                );
                }
            }

            protected
                IEnumerable<
                                (
                                    string InterfaceName,
                                    string ManagedNamespace
                                )
                            >
                                GetInterfaces()
            {
                string nodes_to_find =
                    $@"/apixml:assemblies/apixml:assembly/apixml:namespaces/apixml:namespace"
                    +
                    $@"/apixml:interfaces/apixml:interfaces"
                    ;
                System.Xml.XmlNodeList node_list = xmldoc.SelectNodes(nodes_to_find, ns);

                foreach (System.Xml.XmlNode node in node_list)
                {
                    string interface_name = node.Attributes["name"].Value;
                    System.Xml.XmlNode node_parent = node.ParentNode.ParentNode;
                    string namespace_name = node_parent.Attributes["name"].Value;

                    yield return
                                (
                                    InterfaceName: interface_name,
                                    ManagedNamespace: namespace_name
                                );
                }
            }

            protected
                IEnumerable<
                                (
                                    string InterfaceName,
                                    string ManagedNamespace
                                )
                            >
                                GetInterfacesFromClasses()
            {
                string nodes_to_find =
                    $@"/apixml:assemblies/apixml:assembly/apixml:namespaces/apixml:namespace"
                    +
                    $@"/apixml:classes/apixml:class"
                    +
                    $@"/apixml:interfaces/apixml:interface"
                    ;
                System.Xml.XmlNodeList node_list = xmldoc.SelectNodes(nodes_to_find, ns);

                foreach (System.Xml.XmlNode node in node_list)
                {
                    string interface_name = node.Attributes["name"].Value;
                    int postion_interface_name = interface_name.LastIndexOf('.');
                    string namespace_name = interface_name.Substring(0, postion_interface_name);
                    interface_name = interface_name.Replace($"{namespace_name}.", "");
                    yield return
                                (
                                    InterfaceName: interface_name,
                                    ManagedNamespace: namespace_name
                                );
                }
            }

            private void DumpNamespaces(string filename_base)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach
                    (
                        string namespace_name
                        in this.Namespaces
                    )
                {
                    sb.AppendLine(namespace_name);
                }

                return;
            }


            private void DumpClasses(string filename_base)
            {
                int n = this.Classes.Count();

                int length_class = 0;
                int length_namepsace = 0;

                foreach
                    (
                        (
                            string ClassName,
                            string ManagedNamespace
                        ) c
                        in this.Classes
                    )
                {
                    int lci = c.ClassName.Length;
                    if (c.ClassName.Length > length_class)
                    {
                        length_class = lci;
                    }
                    int lnoi = c.ManagedNamespace.Length;
                    if (c.ManagedNamespace.Length > length_namepsace)
                    {
                        length_namepsace = lnoi;
                    }
                }

                int padding = 3;
                string fmt0 = "{0,-" + (length_class + padding) + "}";
                string fmt1 = ",{1,-" + (length_namepsace + padding) + "}";
                string fmt2 = ",{2}";

                string fmt = fmt0 + fmt1 + fmt2;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                foreach
                    (
                        (
                            string ClassName,
                            string ManagedNamespace
                        ) c
                        in this.Classes
                    )
                {
                    string cn = c.ClassName;
                }

                System.IO.File.WriteAllText("mapping-xamarin-android-support-to-androidx.csv", sb.ToString());

                return;
            }

            private void DumpInterfaces(string filename_base)
            {
            }

            private void DumpInterfacesFromClasses(string filename_base)
            {

            }

    }
}
}
