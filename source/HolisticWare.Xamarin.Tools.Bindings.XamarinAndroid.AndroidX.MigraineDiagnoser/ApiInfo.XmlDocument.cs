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
        public partial class XmlDocumentData
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

                Console.WriteLine("XmlDocument initialized for API scraping");
                return;
            }

            string file_name = null;
            System.Xml.XmlDocument xmldoc = null;
            System.Xml.XmlNamespaceManager ns = null;
            string xml_namespace_name = "apixml";
            string xml_namespace = string.Intern("");

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
                                    string ClassName,
                                    string ManagedNamespace
                                )
                            >
                    ClassesInner
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
                List <
                        (
                            string ClassName,
                            string ManagedNamespace
                        )
                    >
                                GetClasses()
            {
                List<
                                (
                                    string ClassName,
                                    string ManagedNamespace
                                )
                            > result;
                result = new List<(string ClassName, string ManagedNamespace)>();

                System.Xml.XmlNodeList node_list = null;
               
                 string nodes_to_find =
                    $@"/apixml:assemblies/apixml:assembly/apixml:namespaces/apixml:namespace"
                    +
                    $@"/apixml:classes/apixml:class"
                    ;

                node_list = xmldoc.SelectNodes(nodes_to_find, ns);

                foreach(System.Xml.XmlNode node in node_list)
                {
                    string class_name = node.Attributes["name"].Value;

                    System.Xml.XmlNode node_parent = node.ParentNode.ParentNode;
                    string namespace_name = node_parent.Attributes["name"].Value;

                    System.Xml.XmlNodeList node_list_inner = null;
                    node_list_inner = node.SelectNodes
                                                    (
                                                        $@"/apixml:classes/apixml:class",
                                                        ns
                                                    );
                    if (node_list_inner.Count > 0)
                    {
                        string class_name_inner = string.Intern("");
                    }

                    //yield return
                    //(
                    //    ClassName: class_name,
                    //    ManagedNamespace: namespace_name
                    //);
                    result.Add  (
                                    (
                                        ClassName: class_name,
                                        ManagedNamespace: namespace_name
                                    )
                                );
                }

                return result;
            }

            protected
                IEnumerable<
                                (
                                    string ClassName,
                                    string ManagedNamespace
                                )
                            >
                                GetClassesInner()
            {
                System.Xml.XmlNodeList node_list = null;

                string nodes_to_find =
                    $@"/apixml:assemblies/apixml:assembly/apixml:namespaces/apixml:namespace"
                    +
                    $@"/apixml:classes/apixml:class"
                    +
                    $@"/apixml:classes/apixml:class"
                    ;

                node_list = xmldoc.SelectNodes(nodes_to_find, ns);

                foreach (System.Xml.XmlNode node in node_list)
                {
                    string class_name = node.Attributes["name"].Value;

                    if(class_name.EndsWith("EventArgs"))
                    {
                        continue;
                    }

                    System.Xml.XmlNode node_parent = node.ParentNode.ParentNode;
                    string namespace_name = node_parent.Attributes["name"].Value;

                    System.Xml.XmlNodeList node_list_inner = null;
                    node_list_inner = node.SelectNodes
                                                    (
                                                        $@"/apixml:classes/apixml:class",
                                                        ns
                                                    );
                    if (node_list_inner.Count > 0)
                    {
                        string class_name_inner = string.Intern("");
                    }

                    yield return
                                (
                                    ClassName: class_name,
                                    ManagedNamespace: namespace_name
                                );
                }

                node_list = xmldoc.SelectNodes(nodes_to_find, ns);

                foreach (System.Xml.XmlNode node in node_list)
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

                    if ( ! interface_name.StartsWith("Android.Support."))
                    {
                        continue;
                    }
                    int postion_interface_name = interface_name.LastIndexOf('.');
                    string namespace_name = interface_name.Substring(0, postion_interface_name);
                    interface_name = interface_name.Replace($"{namespace_name}.", string.Intern(""));
                    yield return
                                (
                                    InterfaceName: interface_name,
                                    ManagedNamespace: namespace_name
                                );
                }
            }

            private void DumpAPINamespaces(string filename)
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

                System.IO.File.WriteAllText(filename, sb.ToString());

                return;
            }


            private void DumpAPIClasses(string filename)
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
                string fmt = 
                        "{0,-" + (length_class + padding) + "}"
                        +
                        ",{1}"  //,-" + (length_namepsace + padding) + "}"
                        //+
                        //",{2}"
                        ;

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
                    string nn = c.ManagedNamespace;
                    
                    sb.AppendLine(string.Format(fmt, cn, nn));
                }

                System.IO.File.WriteAllText(filename, sb.ToString());

                return;
            }

            private void DumpAPIClassesInner(string filename)
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
                string fmt =
                        "{0,-" + (length_class + padding) + "}"
                        +
                        ",{1}"  //,-" + (length_namepsace + padding) + "}"
                                //+
                                //",{2}"
                        ;

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
                    string nn = c.ManagedNamespace;

                    sb.AppendLine(string.Format(fmt, cn, nn));
                }

                System.IO.File.WriteAllText(filename, sb.ToString());

                return;
            }

            private void DumpAPIInterfaces(string filename)
            {
                int n = this.Interfaces.Count();

                int length_interface = 0;
                int length_namepsace = 0;

                foreach
                    (
                        (
                            string InterfaceName,
                            string ManagedNamespace
                        ) i
                        in this.Classes
                    )
                {
                    int lci = i.InterfaceName.Length;
                    if (i.InterfaceName.Length > length_interface)
                    {
                        length_interface = lci;
                    }
                    int lnoi = i.ManagedNamespace.Length;
                    if (i.ManagedNamespace.Length > length_namepsace)
                    {
                        length_namepsace = lnoi;
                    }
                }

                int padding = 3;
                string fmt =
                        "{0,-" + (length_interface + padding) + "}"
                        +
                        ",{1}" //,-" + (length_namepsace + padding) + "}"
                        //+
                        //",{2}"
                        ;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                foreach
                    (
                        (
                            string InterfaceName,
                            string ManagedNamespace
                        ) i
                        in this.Interfaces
                    )
                {
                    string ifn = i.InterfaceName;
                    string nn = i.ManagedNamespace;

                    sb.AppendLine(string.Format(fmt, ifn, nn));
                }

                System.IO.File.WriteAllText(filename, sb.ToString());

                return;
            }

            private void DumpAPIInterfacesFromClasses(string filename)
            {
                int length_interface = 0;
                int length_namepsace = 0;

                foreach
                    (
                        (
                            string InterfaceName,
                            string ManagedNamespace
                        ) i
                        in this.InterfacesFromClasses
                    )
                {
                    int lci = i.InterfaceName.Length;
                    if (i.InterfaceName.Length > length_interface)
                    {
                        length_interface = lci;
                    }
                    int lnoi = i.ManagedNamespace.Length;
                    if (i.ManagedNamespace.Length > length_namepsace)
                    {
                        length_namepsace = lnoi;
                    }
                }

                int padding = 3;
                string fmt =
                        "{0,-" + (length_interface + padding) + "}"
                        +
                        ",{1}"  //,-" + (length_namepsace + padding) + "}"
                        //+
                        //",{2}"
                        ;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                foreach
                    (
                        (
                            string InterfaceName,
                            string ManagedNamespace
                        ) i
                        in this.InterfacesFromClasses
                    )
                {
                    string ifn = i.InterfaceName;
                    string nn = i.ManagedNamespace;

                    sb.AppendLine(string.Format(fmt, ifn, nn));
                }

                System.IO.File.WriteAllText(filename, sb.ToString());

                return;
            }

        }
}
}
