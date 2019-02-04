using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

using Mono.Cecil;
using Mono.Cecil.Rocks;

using Core.Text;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core.Generated;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core
{
    public partial class ApiInfo
    {
        public partial class MonoCecilData
        {
            public void Dump(string filename_base)
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
                string path_output = Path.Combine(path, "MonoCecil");
                if (!Directory.Exists(path_output))
                {
                    Directory.CreateDirectory(path_output);
                }

                string filename = null;

                Parallel.Invoke
                    (
                        () =>
                        {
                            filename = Path.Combine(path_output, $"API.{filename_base}.TypesManaged.csv");
                            this.DumpTypesManaged(filename);
                        },
                        () =>
                        {
                            filename = Path.Combine(path_output, $"API.{filename_base}.TypesAndroidRegistered.csv");
                            this.DumpTypesAndroidRegistered(filename);
                        }
                    );

                return;
            }

            private void DumpTypesManaged(string filename)
            {
                List<string> dump = new List<string>();
                StringBuilder sb = new StringBuilder();

                foreach
                (
                    (
                        string ManagedClass,
                        string ManagedNamespace,
                        string JNIPackage,
                        string JNIType
                    ) typ in this.TypesManaged
                )
                {
                    sb.AppendLine($"{typ.ManagedClass},{typ.ManagedNamespace},{typ.JNIPackage},{typ.JNIType}");
                }

                File.WriteAllText($@"{filename}", sb.ToString());

                return;
            }

            private void DumpTypesAndroidRegistered(string filename)
            {
                List<string> dump = new List<string>();
                StringBuilder sb = new StringBuilder();

                foreach
                (
                    (
                        string ManagedClass,
                        string ManagedNamespace,
                        string JNIPackage,
                        string JNIType
                    ) typ in this.TypesAndroidRegistered
                )
                {
                    sb.AppendLine($"{typ.ManagedClass},{typ.ManagedNamespace},{typ.JNIPackage},{typ.JNIType}");
                }

                File.WriteAllText($@"{filename}", sb.ToString());

                return;
            }
        }
    }
}
