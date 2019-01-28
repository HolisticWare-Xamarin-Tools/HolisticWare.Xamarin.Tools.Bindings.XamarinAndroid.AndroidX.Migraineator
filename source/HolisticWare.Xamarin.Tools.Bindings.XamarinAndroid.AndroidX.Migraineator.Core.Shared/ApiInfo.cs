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
        public ApiInfo(string path_api_info_xml, string path_assembly)
        {
            api_info_path = path_api_info_xml;

            this.XmlDocumentAPI = new XmlDocumentData(path_api_info_xml);
            this.MonoCecilAPI = new MonoCecilData(path_assembly);
            this.XmlSerializerAPI = new XmlSerializerData(path_api_info_xml);
            this.LinqXDocumentAPI = new LinqXDocumentData(path_api_info_xml);

            return;
        }

        string api_info_path;

        public string ApiInfoPath
        {
            get
            {
                return api_info_path;
            }
            protected set
            {
            }
        }

        public string ApiInfoContent
        {
            get
            {
                return api_info_content;
            }
        }

        string api_info_content = null;

        StreamReader sr = null;

        public async Task<string> LoadAsync()
        {
            sr = new StreamReader(api_info_path);
            api_info_content = await sr.ReadToEndAsync();

            return api_info_content;
        }

        public MonoCecilData MonoCecilAPI
        {
            get;
            private set;
        }

        public XmlDocumentData XmlDocumentAPI
        {
            get;
            private set;
        }

        public XmlSerializerData XmlSerializerAPI
        {
            get;
            private set;
        }

        public LinqXDocumentData LinqXDocumentAPI
        {
            get;
            private set;
        }
    }
}
