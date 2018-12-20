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
        public void DumpToFiles(ApiInfo api_info, string version)
        {
            (
                List<string> namespaces,
                List<string> namespaces_new_suspicious,
                List<string> namespaces_old_suspicious,
                List<string> classes
            ) = this.Analyse(api_info);

            System.IO.File.WriteAllLines($"namespaces_{version}.txt", namespaces);
            System.IO.File.WriteAllLines($"namespaces_new_suspicious_{version}.txt", namespaces_new_suspicious);
            System.IO.File.WriteAllLines($"namespaces_old_suspicious_{version}.txt", namespaces_old_suspicious);
            System.IO.File.WriteAllLines($"classes_{version}.txt", classes);

            return;

        }


    }
}
