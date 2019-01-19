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
        // https://aloiskraus.wordpress.com/2017/04/23/the-definitive-serialization-performance-guide/
        // https://www.codeproject.com/Articles/526956/%2FArticles%2F526956%2FAll-about-XmlSerializer-Performance-and-Sgen
        // https://kalapos.net/Blog/ShowPost/how-the-evil-system-xml-serialization-xmlserializer-class-can-bring-a-server-with-32gb-ram-down
        // https://medium.com/@neuecc/zeroformatter-fastest-c-serializer-and-infinitely-fast-deserializer-for-net-88e752803fe9
        // https://hackernoon.com/comparing-the-performance-of-various-serializers-8cc459a24c21
        // https://maxondev.com/serialization-performance-comparison-c-net-formats-frameworks-xmldatacontractserializer-xmlserializer-binaryformatter-json-newtonsoft-servicestack-text/

        public class XmlSerializerData
        {
            public XmlSerializerData(string path)
            {
                this.file_name = path;
                sr = new StreamReader(file_name);
                serializer = new System.Xml.Serialization.XmlSerializer(typeof(Generated.ApiInfo));

                return;
            }

            string file_name = null;
            StreamReader sr = null;
            System.Xml.Serialization.XmlSerializer serializer = null;

            public Generated.ApiInfo ApiInfo
            {
                get
                {
                    task_deserialize?.Wait();
                    return api_info;
                }
            }

            protected Task task_deserialize = null;
            protected Generated.ApiInfo api_info = null;

            public async Task Deserialize()
            {
                try
                {
                    task_deserialize = Task.Run
                            (
                                () =>
                                {
                                    api_info = (Generated.ApiInfo)serializer.Deserialize(sr);
                                }
                            );
                }
                catch(Exception exc)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Exception reading: {file_name}");
                    Console.WriteLine($"          message: {exc.Message}");
                    Console.ResetColor();

                    throw;
                }
                finally
                {
                    sr.Close();
                    sr = null;
                    serializer = null;
                }

                return;
            }
        }

    }
}
