using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Text;

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

                name = $"{name}.dll";
                if (File.Exists(name))
                {
                    File.Delete(name);
                }
                FileStream fs = new FileStream(name, FileMode.Create, FileAccess.Write, FileShare.None);

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


        private void Cecilize()
        {

        }

    }
}
