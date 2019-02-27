using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;

using Core.Text;
using Mono.Cecil.Rocks;
using Mono.Cecil;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator
{
    public partial class AndroidXMigrator
    {
        private static ReadOnlyMemory<char> memory_android_support;
        private static ReadOnlyMemory<char> memory_androidx;

        private static Memory<Memory<char>> TypeNamesToSkip;

        static AndroidXMigrator()
        {
            Parallel.Invoke
                        (
                            () =>
                            {
                                string file = Path.Combine
                                                    (
                                                        new string[]
                                                        {
                                                            Settings.WorkingDirectory,
                                                            "mappings",
                                                            "API.Mappings.Merged.Google.with.Xamarin.Classes.csv"
                                                        }
                                                    );
                                LoadMappingsClasses(file);
                            },
                            () =>
                            {
                                Initialize();
                            }
                        );

            return;
        }

        private static void Initialize()
        {
            memory_android_support = string.Intern("Android.Support").AsMemory();
            memory_androidx = string.Intern("AndroidX").AsMemory();

            TypeNamesToSkip = 
                            new Memory<char>[]
                                    {
                                        string.Intern("<Module>").ToCharArray().AsMemory(),
                                        string.Intern("__TypeRegistrations").ToCharArray().AsMemory(),
                                    }
                                    .AsMemory();

            return;
        }

        public AndroidXMigrator(string input, string output)
        {
            this.PathAssemblyInput = input;
        }


        //-------------------------------------------------------------------------------------------------------------------
        public static
            ReadOnlyMemory
                    <
                        (
                            string ClassName,
                            string AndroidSupportClass,
                            string AndroidXClass,
                            string AndroidSupportClassFullyQualified,
                            string AndroidXClassFullyQualified,
                            // formatting space
                            string PackageAndroidSupport,
                            string PackageAndroidX,
                            string ManagedNamespaceXamarinAndroidSupport,
                            string ManagedNamespaceXamarinAndroidX
                        )
                    >
                ClassMappings
        {
            get;
            private set;
        }

        public static void LoadMappingsClasses(string file)
        {
            CharacterSeparatedValues csv = new CharacterSeparatedValues();
            string content = csv.LoadAsync(file).Result;

            IEnumerable<string[]> mapping = csv
                                            .ParseTemporaryImplementation()
                                            .ToList()
                                            ;
            IEnumerable
                    <
                        (
                            string ClassName,
                            string AndroidSupportClass,
                            string AndroidXClass,
                            string AndroidSupportClassFullyQualified,
                            string AndroidXClassFullyQualified,
                            // formatting space
                            string PackageAndroidSupport,
                            string PackageAndroidX,
                            string ManagedNamespaceXamarinAndroidSupport,
                            string ManagedNamespaceXamarinAndroidX
                        )
                    >
                    mapping_strongly_typed;

            mapping_strongly_typed = Convert_ClassMappings(mapping);

            ClassMappings = mapping_strongly_typed.ToArray().AsMemory();

            return;
        }

        public static 
            IEnumerable
                   <
                    (
                        string ClassName,
                        string AndroidSupportClass,
                        string AndroidXClass,
                        string AndroidSupportClassFullyQualified,
                        string AndroidXClassFullyQualified,
                        // formatting space
                        string PackageAndroidSupport,
                        string PackageAndroidX,
                        string ManagedNamespaceXamarinAndroidSupport,
                        string ManagedNamespaceXamarinAndroidX
                    )
                >
                Convert_ClassMappings(IEnumerable<string[]> untyped_data)
        {
            //int n = untyped_data.Count();

            //for(int i = 0; i < n; i++)
            foreach(string[] row in untyped_data)
            {
                //string[] row = untyped_data[i];

                //TODO - benchmarking
                Span<string> row_data = row.AsSpan();

                yield return
                        (
                            ClassName: row_data[0],
                            AndroidSupportClass: row_data[1],
                            AndroidXClass: row_data[2],
                            AndroidSupportClassFullyQualified: row_data[3],
                            AndroidXClassFullyQualified: row_data[4],
                                 // formatting space
                            PackageAndroidSupport: row_data[5],
                            PackageAndroidX: row_data[6],
                            ManagedNamespaceXamarinAndroidSupport: row_data[7],
                            ManagedNamespaceXamarinAndroidX: row_data[8]

                            //ClassName: row_data[0],
                            //AndroidSupportClass: row_data[1],
                            //AndroidXClass: row_data[2],
                            //AndroidSupportClassFullyQualified: row_data[3],
                            //AndroidXClassFullyQualified: row_data[4],
                            //      formatting space
                            //PackageAndroidSupport: row_data[5],
                            //PackageAndroidX: row[6],
                            //ManagedNamespaceXamarinAndroidSupport: row_data[7],
                            //ManagedNamespaceXamarinAndroidX: row_data[8]
                        );
            }
        }
        //-------------------------------------------------------------------------------------------------------------------

        public string PathAssemblyInput
        {
            get;
            set;
        }

        public string PathAssemblyOutput
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        /// https://gist.github.com/Redth/c8c3e4999ea33b80950aae3095f7839b
        public void Migrate()
        {
            Mono.Cecil.AssemblyDefinition asm_def = null;
            asm_def = Mono.Cecil.AssemblyDefinition.ReadAssembly
                                                        (
                                                            this.PathAssemblyInput,
                                                            new Mono.Cecil.ReaderParameters
                                                                {
                                                                    AssemblyResolver = CreateAssemblyResolver(),
                                                                    ReadWrite = true,
                                                                    InMemory = true 
                                                                }
                                                        );

            Mono.Collections.Generic.Collection<Mono.Cecil.TypeDefinition> types_plain = null;
            IEnumerable<Mono.Cecil.TypeDefinition> types_rocks = null;

            types_rocks = asm_def.MainModule.GetAllTypes();
            types_plain = asm_def.MainModule.Types;


            System.Diagnostics.Trace.WriteLine($"===================================================================================");
            System.Diagnostics.Trace.WriteLine($" migrating assembly                       = {this.PathAssemblyInput}");
            System.Diagnostics.Trace.WriteLine($"                 types Mono.Cecil       # = {types_plain.Count()}");
            System.Diagnostics.Trace.WriteLine($"                 types Mono.Cecil.Rocks # = {types_rocks.Count()}");

            Stopwatch timer = new Stopwatch();
            timer.Start();

            foreach (Mono.Cecil.TypeDefinition t in types_rocks)
            {
                ProcessType(t);
            }

            timer.Stop();

            long ms = timer.ElapsedMilliseconds;

            System.Diagnostics.Trace.WriteLine($"Assembly processing {ms}ms");
            System.Diagnostics.Trace.WriteLine($"===================================================================================");

            asm_def.Write(this.PathAssemblyOutput);

            return;
        }

        private void ProcessType(TypeDefinition t)
        {
            System.Diagnostics.Trace.WriteLine($"    processing Type");
            System.Diagnostics.Trace.WriteLine($"                       Name        = {t.Name}");
            System.Diagnostics.Trace.WriteLine($"                       FullName    = {t.FullName}");
            System.Diagnostics.Trace.WriteLine($"                       IsClass     = {t.IsClass}");
            System.Diagnostics.Trace.WriteLine($"                       IsInterface = {t.IsInterface}");

            ProcessAttributes(t.Attributes);
            ProcessMethods(t.GetMethods());

            return;
        }

        private void ProcessAttributes(TypeAttributes attributes)
        {
            System.Diagnostics.Trace.WriteLine($"                            Attribute = {attributes.ToString()}");

            return;
        }

        private void ProcessMethods(IEnumerable<MethodDefinition> methods)
        {
            foreach (Mono.Cecil.MethodDefinition method in methods)
            {
                System.Diagnostics.Trace.WriteLine($"                        Method = {method.Name}");

                ReadOnlyMemory<char> memory_return_type_full_name;
                string return_type_full_name = method.MethodReturnType?.ReturnType?.FullName;

                bool has_support = false;

                if (null != return_type_full_name)
                {
                    memory_return_type_full_name = return_type_full_name.AsMemory();
                    has_support = memory_return_type_full_name.Span.StartsWith(memory_android_support.Span);
                    System.Diagnostics.Trace.WriteLine($"                       returns  = {return_type_full_name}");
                    if(has_support)
                    {
                        int start = memory_android_support.Span.Length;
                        int length = memory_return_type_full_name.Span.Length - start;
                        ReadOnlySpan<char> span_androidx = memory_android_support.Span;
                        ReadOnlySpan<char> replaced = memory_return_type_full_name.Span.Slice(start, length);
                    }
                }

                bool has_support_parameters = ProcessMethodparameters(method);
                has_support = has_support || has_support_parameters;


                string registerAttrMethodName = null;
                string registerAttributeJniSig = null;
                string registerAttributeNewJniSig = null;

                bool isBindingMethod = false;

                foreach (Mono.Cecil.CustomAttribute attr in method.CustomAttributes)
                {

                    if (attr.AttributeType.FullName.Equals("Android.Runtime.RegisterAttribute"))
                    {
                        Mono.Cecil.CustomAttributeArgument jniSigArg = attr.ConstructorArguments[1];

                        registerAttrMethodName = attr.ConstructorArguments[0].Value.ToString();
                        registerAttributeJniSig = jniSigArg.Value.ToString();

                        registerAttributeNewJniSig = ReplaceJniSignature(registerAttributeJniSig);

                        attr.ConstructorArguments[1] = new Mono.Cecil.CustomAttributeArgument(jniSigArg.Type, registerAttributeNewJniSig);

                        isBindingMethod = true;

                        System.Diagnostics.Trace.WriteLine($"[Register(\"{attr.ConstructorArguments[0].Value}\", \"{registerAttributeNewJniSig}\")]");
                    }
                }

                if (!isBindingMethod)
                    return;

                if (method.HasBody)
                {
                    // Replace all the JNI Signatures inside the method body
                    foreach (var instr in method.Body.Instructions)
                    {
                        if (instr.OpCode.Name == "ldstr")
                        {
                            var jniSig = instr.Operand.ToString();

                            var indexOfDot = jniSig.IndexOf('.');

                            // New binding glue style is `methodName.(Lparamater/Type;)Lreturn/Type;`
                            if (indexOfDot >= 0)
                            {
                                var methodName = jniSig.Substring(0, indexOfDot);
                                var newJniSig = ReplaceJniSignature(jniSig.Substring(indexOfDot + 1));
                                instr.Operand = $"{methodName}.{newJniSig}";

                                System.Diagnostics.Trace.WriteLine($"{methodName} -> {newJniSig}");
                            }
                            // Old style is two strings, one with method name and then `(Lparameter/Type;)Lreturn/Type;`
                            else if (jniSig.Contains('(') && jniSig.Contains(')'))
                            {
                                var methodName = instr.Previous.Operand.ToString();
                                var newJniSig = ReplaceJniSignature(jniSig);

                                instr.Operand = newJniSig;

                                System.Diagnostics.Trace.WriteLine($"{methodName} -> {newJniSig}");
                            }
                        }
                    }
                }

            }
        }

        private static bool ProcessMethodparameters(MethodDefinition method)
        {
            bool has_support = false;

            if (method.HasParameters)
            {
                foreach (ParameterDefinition method_parameter in method.Parameters)
                {
                    ReadOnlySpan<char> span_method_parameter_namespace;
                    string method_parameter_namespace = method_parameter.ParameterType.Namespace;
                    span_method_parameter_namespace = method_parameter_namespace.AsSpan();

                    bool has_support_parameter = span_method_parameter_namespace.StartsWith(memory_android_support.Span);
                    has_support = has_support || has_support_parameter;

                    if (has_support)
                    {
                        System.Diagnostics.Trace.WriteLine($"                       parameter  = {method_parameter.ParameterType.Name}");
                        break;
                    }
                }
            }

            return has_support;
        }

        static string ReplaceJniSignature(string jniSignature)
        {
            //var sig = new global::Xamarin.Android.Tools.Bytecode.MethodTypeSignature(jniSignature);

            //var sb_newSig = new StringBuilder();

            //sb_newSig.Append("(");

            //foreach (var p in sig.Parameters)
            //    sb_newSig.Append($"L{mappings[p]};" ?? p);               

            //sb_newSig.Append(")");

            //sb_newSig.Append(sig.ReturnTypeSignature);

            string newSig = null;  // sb_newSig.ToString();

            return newSig;
        }

        private Mono.Cecil.IAssemblyResolver CreateAssemblyResolver()
        {
            string VsInstallRoot = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\";
            string TargetFrameworkVerison = "v9.0";

            Mono.Cecil.DefaultAssemblyResolver resolver = null;
            resolver = new Mono.Cecil.DefaultAssemblyResolver();
            if (!string.IsNullOrEmpty(VsInstallRoot) && Directory.Exists(VsInstallRoot))
            {
                resolver.AddSearchDirectory(Path.Combine(
                    VsInstallRoot,
                    @"Common7\IDE\ReferenceAssemblies\Microsoft\Framework\MonoAndroid\" + TargetFrameworkVerison
                    ));
            }
            else
            {
                resolver.AddSearchDirectory(Path.Combine
                    (
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    @"Reference Assemblies\Microsoft\Framework\MonoAndroid\" + TargetFrameworkVerison
                ));
            }

            return resolver;
        }
    }
}
