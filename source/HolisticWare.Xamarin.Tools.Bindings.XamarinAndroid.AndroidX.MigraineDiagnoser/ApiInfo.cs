using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Core.Text;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Generated;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator
{
    public partial class ApiInfo
    {
        public ApiInfo(BuildProject build_project)
        {
            this.BuildProject = build_project;

            string path_api_info_xml = $"{build_project.ApiInfoFile}";
            string path_assembly = $"{build_project.FolderOutput}/AndroidSupport.Merged.dll";

            api_info_path = path_api_info_xml;

            this.XmlDocumentAPI = new XmlDocumentData(path_api_info_xml);
            this.MonoCecilAPI = new MonoCecilData(path_assembly);
            this.XmlSerializerAPI = new XmlSerializerData(path_api_info_xml);
            this.LinqXDocumentAPI = new LinqXDocumentData(path_api_info_xml);

            return;
        }

        BuildProject build_project;

        public BuildProject BuildProject
        {
            get
            {
                return build_project;
            }
            protected set
            {
                build_project = value;
            }
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
