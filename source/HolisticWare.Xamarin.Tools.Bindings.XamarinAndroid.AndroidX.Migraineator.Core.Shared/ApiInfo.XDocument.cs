using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using Core.Text;

using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core.Generated;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core
{
    public partial class ApiInfo
    {
        public class LinqXDocumentData
        {
            public LinqXDocumentData(string path)
            {
                this.file_name = path;
                fs = new FileStream(file_name, FileMode.Open, FileAccess.Read);
                xml_doc = XDocument.Load(fs);

                return;
            }

            string file_name = null;
            FileStream fs = null;
            XDocument xml_doc = null;

        }

    }
}
