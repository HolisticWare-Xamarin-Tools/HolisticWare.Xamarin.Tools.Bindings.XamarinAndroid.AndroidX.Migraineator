using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Core.Text;
using Mono.Cecil;
using Xamarin.AndroidX.Data;

namespace Xamarin.AndroidX.Mapper
{
    public class MappingsXamarin
    {
        protected static HttpClient client = null;

        static MappingsXamarin()
        {
            client = new HttpClient();

            return;
        }

        public MappingsXamarin()
        {
            client = new HttpClient();

            return;
        }

        public GoogleMappingData GoogleMappingsData
        {
            get;
            set;
        }


        string filename = null;

        public void Download(string name, string url)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidOperationException($"Argument needed {nameof(name)}");
            }

            if (string.IsNullOrEmpty(url) && string.IsNullOrEmpty(AssemblyUrl))
            {
                throw new InvalidOperationException($"Argument needed {nameof(url)} or {nameof(AssemblyUrl)}");
            }

            if(string.IsNullOrEmpty(url))
            {
                url = AssemblyUrl;
            }

            Stream result = null;

            using (HttpResponseMessage response = client.GetAsync(url).Result)
            using (HttpContent content = response.Content)
            {
                // ... Read the string.
                result = content.ReadAsStreamAsync().Result;

                filename = $"{name}.dll";
                if (File.Exists(name))
                {
                    File.Delete(name);
                }
                FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);

                result
                        .CopyToAsync(fs)
                        .ContinueWith
                                (
                                    (task) =>
                                    {
                                        fs.Flush();
                                        fs.Close();
                                        fs = null;
                                    }
                                );
            }

            return ;
        }

        public string AssemblyUrl
        {
            get;
            set;
        }

        protected
            IEnumerable
                <
                    (
                        string TypenameFullyQualifiedAndroidSupport,
                        string TypenameFullyQualifiedAndroidX
                    )
                >
                    mapping_sorted_androidx = null;

        protected IEnumerable<string> mapping_sorted_androidx_index = null;

        protected
            IEnumerable
                <
                    (
                        string TypenameFullyQualifiedAndroidSupport,
                        string TypenameFullyQualifiedAndroidX
                    )
                >
                    mapping_sorted_android_support = null;

        protected IEnumerable<string> mapping_sorted_android_support_index = null;

        public void Initialize()
        {
            if (filename.ToLowerInvariant().Contains("androidx"))
            {
                mapping_sorted_androidx = this.GoogleMappingsData
                                                    .Mapping
                                                    .OrderBy(tuple => tuple.TypenameFullyQualifiedAndroidX);

                mapping_sorted_androidx_index = mapping_sorted_androidx.Select(tuple => tuple.TypenameFullyQualifiedAndroidX);
            }
            else
            {
                mapping_sorted_android_support = this.GoogleMappingsData
                                                            .Mapping
                                                            .OrderBy(tuple => tuple.TypenameFullyQualifiedAndroidSupport);

                mapping_sorted_android_support_index = mapping_sorted_android_support.Select(tuple => tuple.TypenameFullyQualifiedAndroidSupport);
            }

            // Cecilize(); // let user call Cecilize

            //MappingsXamarinManaged = Cecilize().OrderBy(tuple => tuple.JavaType).ToArray();
            //MappingsXamarinManagedIndex = MappingsXamarinManaged.Select(tuple => tuple.JavaType).ToArray();

            return;
        }

        public void FinalizeMappings()
        {
            google_mapping_with_xamarin =
                    new List
                            <
                                (
                                    string TypenameFullyQualifiedAndroidSupport,
                                    string TypenameFullyQualifiedAndroidX,
                                    string TypenameFullyQualifiedXamarin
                                )
                            >();

            foreach
                    (
                        (
                            string TypenameFullyQualifiedAndroidSupport,
                            string TypenameFullyQualifiedAndroidX
                        )
                            mapping_google_row in this.GoogleMappingsData.Mapping
                    )
            {
                if (filename.ToLowerInvariant().Contains("androidx"))
                {
                    int idx = System.Array.BinarySearch
                                                    (
                                                        mapping_sorted_androidx.ToArray(),
                                                        mapping_google_row.TypenameFullyQualifiedAndroidX
                                                    );
                }
                else
                {
                    int idx = System.Array.BinarySearch
                                                    (
                                                        mapping_sorted_android_support.ToArray(),
                                                        mapping_google_row.TypenameFullyQualifiedAndroidSupport
                                                    );
                }


            }

            return;
        }
        public
            (
                string JavaType,
                string ManagedClass,
                string ManagedNamespace,
                string JNIPackage,
                string JNIType
            )[]
            MappingsXamarinManaged
        {
            get;
            private set;
        }

        public string[] MappingsXamarinManagedIndex
        {
            get;
            private set;
        }

        public
            (
                string JavaType,
                string ManagedClass,
                string ManagedNamespace,
                string JNIPackage,
                string JNIType
            )
                Find(string java_type)
        {
            (
                string JavaType,
                string ManagedClass,
                string ManagedNamespace,
                string JNIPackage,
                string JNIType
            )
                result;

            int index = System.Array.BinarySearch(this.MappingsXamarinManagedIndex, java_type);
            result = MappingsXamarinManaged[index];

            return result;
        }

        private List
                    <
                        (
                            string JavaTypeFullyQualified,
                            string ManagedTypeFullyQualified
                        )
                    >
                        tar;

        private List
                    <
                        (
                            string JavaTypeFullyQualified,
                            string ManagedTypeFullyQualified
                        )
                    >
                        tarig;

        private List
                    <
                        (
                            string JavaTypeFullyQualified,
                            string ManagedTypeFullyQualified
                        )
                    >
                        tarnig;

        private List
                    <
                        (
                            string JavaTypeFullyQualified,
                            string ManagedTypeFullyQualified
                        )
                    >
                        trar;
        private List
                    <
                        (
                            string JavaTypeFullyQualified,
                            string ManagedTypeFullyQualified
                        )
                    >
                        taur;
        private List
                    <
                        (
                            string JavaTypeFullyQualified,
                            string ManagedTypeFullyQualified
                        )
                    >
                        tnar;

        private List
                    <
                        (
                            string JavaTypeFullyQualified,
                            string ManagedTypeFullyQualified
                        )
                    >
                        tr;

         private List
                    <
                        (
                            string TypenameFullyQualifiedAndroidSupport,
                            string TypenameFullyQualifiedAndroidX,
                            string TypenameFullyQualifiedXamarin
                        )
                    >
                        google_mapping_with_xamarin;

        public void Cecilize()
        {
            tar = new List
                            <
                                (
                                    string JavaTypeFullyQualified,
                                    string ManagedTypeFullyQualified
                                )
                            >();
            tarig = new List
                            <
                                (
                                    string JavaTypeFullyQualified,
                                    string ManagedTypeFullyQualified
                                )
                            >();
            tarnig = new List
                            <
                                (
                                    string JavaTypeFullyQualified,
                                    string ManagedTypeFullyQualified
                                )
                            >();
            taur = new List
                            <
                                (
                                    string JavaTypeFullyQualified,
                                    string ManagedTypeFullyQualified
                                )
                            >();

            tnar = new List
                            <
                                (
                                    string JavaTypeFullyQualified,
                                    string ManagedTypeFullyQualified
                                )
                            >();

            tr = new List
                            <
                                (
                                    string JavaTypeFullyQualified,
                                    string ManagedTypeFullyQualified
                                )
                            >();

            trar = new List
                            <
                                (
                                    string JavaTypeFullyQualified,
                                    string ManagedTypeFullyQualified
                                )
                            >();


            bool has_symbols_file = false;

            ReaderParameters reader_parameters = new ReaderParameters
            {
                ReadSymbols = has_symbols_file
            };

            AssemblyDefinition assembly_definition = AssemblyDefinition.ReadAssembly(filename, reader_parameters);

            foreach (ModuleDefinition module in assembly_definition.Modules)
            {
                foreach (TypeDefinition type in module.GetTypes())
                {
                    if(type.HasCustomAttributes)
                    {
                        string managed_type = GetTypeName(type);
                        string managed_namespace = GetNamespace(type);
                        string managed_type_fq = $"{managed_namespace}.{managed_type}";

                        foreach (CustomAttribute attr in type.CustomAttributes)
                        {
                            string attribute = attr.AttributeType.FullName;

                            if (attribute.Equals("Android.Runtime.RegisterAttribute"))
                            {
                                string jni_type = attr.ConstructorArguments[0].Value.ToString();
                                string java_type = jni_type.Replace("/", ".");

                                int lastSlash = jni_type.LastIndexOf('/');

                                if (lastSlash < 0 )
                                {
                                    string type_with_nested_type = jni_type;
                                }
                                string jni_class = jni_type.Substring(lastSlash + 1).Replace('$', '.');
                                string jni_package = jni_type.Substring(0, lastSlash).Replace('/', '.');

                                 if (filename.ToLowerInvariant().Contains("androidx"))
                                {
                                    int index = System.Array.BinarySearch
                                                                (
                                                                    mapping_sorted_androidx_index.ToArray(),
                                                                    java_type
                                                                );
                                    if ( index >= 0 && index < mapping_sorted_androidx_index.Count())
                                    {
                                        // found
                                        tarig.Add
                                                (
                                                    (
                                                        JavaTypeFullyQualified: java_type,
                                                        ManagedTypeFullyQualified: managed_type_fq
                                                    )
                                                );
                                    }
                                    else
                                    {
                                        // not found
                                        tarnig.Add
                                                (
                                                    (
                                                        JavaTypeFullyQualified: java_type,
                                                        ManagedTypeFullyQualified: managed_type_fq
                                                    )
                                                );

                                    }
                                }
                                else
                                {
                                    int index = System.Array.BinarySearch
                                                                (
                                                                    mapping_sorted_android_support_index.ToArray(),
                                                                    java_type
                                                                );

                                    if ( index >=0 || index < mapping_sorted_android_support_index.Count() -1)
                                    {
                                        // found
                                        tarig.Add
                                                (
                                                    (
                                                        JavaTypeFullyQualified: java_type,
                                                        ManagedTypeFullyQualified: managed_type_fq
                                                    )
                                                );
                                    }
                                    else
                                    {
                                        // not found
                                        tarnig.Add
                                                (
                                                    (
                                                        JavaTypeFullyQualified: java_type,
                                                        ManagedTypeFullyQualified: managed_type_fq
                                                    )
                                                );

                                    }
                                }

                                tar.Add
                                        (
                                            (
                                                JavaTypeFullyQualified: java_type,
                                                ManagedTypeFullyQualified: managed_type_fq
                                                //ManagedNamespace: managed_namespace,
                                                //JNIPackage: jni_package,
                                                //JNIType: jni_type
                                            )
                                        );



                            }
                        }
                    }

                    if (type.HasNestedTypes)
                    {
                        foreach(TypeDefinition type_nested in type.NestedTypes)
                        {
                            if (type_nested.HasCustomAttributes)
                            {
                                string managed_type = GetTypeName(type);
                                string managed_namespace = GetNamespace(type);
                                string managed_type_fq = $"{managed_namespace}.{managed_type}";

                                foreach (CustomAttribute attr in type.CustomAttributes)
                                {
                                    string attribute = attr.AttributeType.FullName;

                                    if (attribute.Equals("Android.Runtime.RegisterAttribute"))
                                    {
                                        string jni_type = attr.ConstructorArguments[0].Value.ToString();
                                        string java_type = jni_type.Replace("/", ".");

                                        int lastSlash = jni_type.LastIndexOf('/');

                                        if (lastSlash < 0 )
                                        {
                                            string type_with_nested_type = jni_type;
                                        }
                                        string jni_class = jni_type.Substring(lastSlash + 1).Replace('$', '.');
                                        string jni_package = jni_type.Substring(0, lastSlash).Replace('/', '.');

                                        tnar.Add
                                                (
                                                    (
                                                        JavaTypeFullyQualified: java_type,
                                                        ManagedTypeFullyQualified: managed_type_fq
                                                        //ManagedNamespace: managed_namespace,
                                                        //JNIPackage: jni_package,
                                                        //JNIType: jni_type
                                                    )
                                                );

                                    }
                                }
                            }
                        }
                    }
                }

                foreach (TypeReference type in module.GetTypeReferences())
                {
                    string managed_type = type.FullName;
                    string java_type = "jt";

                    tr.Add
                                        (
                                            (
                                                JavaTypeFullyQualified: java_type,
                                                ManagedTypeFullyQualified: managed_type
                                            )
                                        );
                }
            }

            AnalysisData = new Dictionary<string, int>()
            {
                { "$GoogleMappings$", this.GoogleMappingsData.Mapping.Count() },
                { "TAR", tar.Count },
                { "TARIG", tarig.Count },
                { "TARNIG", tarnig.Count },
                { "TNAR", tnar.Count },
                { "TNAUR", -1 },
                { "TR", trar.Count },
             };

            return;
        }

        public Dictionary<string, int> AnalysisData
        {
            get;
            set;
        }

        private string GetNamespace(TypeDefinition type_definition)
        {
            TypeDefinition td = type_definition;
            string ns = type_definition.Namespace;

            while (string.IsNullOrEmpty(ns))
            {
                if (td.DeclaringType == null)
                {
                    break;
                }
                ns = td.DeclaringType.Namespace;
                td = td.DeclaringType;
            }

            return ns;
        }

        private string GetTypeName(TypeDefinition typeDef)
        {
            TypeDefinition td = typeDef;
            string tn = typeDef.Name;

            while (td.DeclaringType != null)
            {
                tn = td.DeclaringType.Name + "." + tn;
                td = td.DeclaringType;
            }

            return tn;
        }

        public void Dump()
        {
            Parallel.Invoke
                (
                    () =>
                    {
                        Assembly assembly = Assembly.GetExecutingAssembly();
                        string fn = assembly
                                        .GetManifestResourceNames()
                                        .Single(file => file.EndsWith("analysis-report.md", StringComparison.InvariantCulture));

                        string report = null;
                        using (Stream stream = assembly.GetManifestResourceStream(fn))
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            report = reader.ReadToEnd();
                        }

                        int n_google_mappings = GoogleMappingsData.Mapping.Count();

                        int n_tar = this.AnalysisData["TAR"];
                        int n_tarig = this.AnalysisData["TARIG"];
                        int n_tarnig = this.AnalysisData["TARNIG"];
 
                        report = report.Replace("$GoogleMappings$", n_google_mappings.ToString());

                        if (filename.ToLowerInvariant().Contains("androidx"))
                        {
                            report = report.Replace("$TARAX", n_tar.ToString());
                            report = report.Replace("TARIGAX", n_tarig.ToString());
                            report = report.Replace("TARNIGAX", n_tarnig.ToString());
                        }
                        else
                        {
                            report = report.Replace("$TARAS", n_tar.ToString());
                            report = report.Replace("TARIGAS", n_tarig.ToString());
                            report = report.Replace("TARNIGAS", n_tarnig.ToString());
                        }
                    },
                    () =>
                    {
                        string text = string.Join(Environment.NewLine, tar);
                        File.WriteAllText(Path.ChangeExtension(filename, "TAR.csv"), text);
                    },
                    () =>
                    {
                        string text = string.Join(Environment.NewLine, tarig);
                        File.WriteAllText(Path.ChangeExtension(filename, "TARIG.csv"), text);
                    },
                    () =>
                    {
                        string text = string.Join(Environment.NewLine, tarnig);
                        File.WriteAllText(Path.ChangeExtension(filename, "TARNIG.csv"), text);
                    }
                );

            return;
        }

    }
}
