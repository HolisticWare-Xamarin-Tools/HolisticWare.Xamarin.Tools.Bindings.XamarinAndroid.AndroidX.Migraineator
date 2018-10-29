using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core
{
    public partial class FileLoader
    {
        protected List<(string Old, string New)> MappingsArtifacts
        {
            get;
            set;
        }

        protected List<(string Old, string New)> MappingsNamespaces
        {
            get;
            set;
        }

        protected List<(string Old, string New)> MappingsClasses
        {
            get;
            set;
        }

        private string content_mappings_artifacts;
        private string content_mappings_namespaces;
        private string content_mappings_classes;
        private string path = null;

        public FileLoader()
        {
            path = "./mappings/android-support-to-androidx-mappings-artifacts.tsv";
            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                content_mappings_artifacts = sr.ReadToEnd();
            }
            path = "./mappings/android-support-to-androidx-mappings-namespaces.tsv";
            using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                content_mappings_namespaces = streamReader.ReadToEnd();
            }
            path = "./mappings/android-support-to-androidx-mappings-classes.tsv";
            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                content_mappings_classes = sr.ReadToEnd();
            }


            return;
        }

    }
}
