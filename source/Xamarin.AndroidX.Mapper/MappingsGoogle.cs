using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Text;

namespace Xamarin.AndroidX.Mapper
{
    public class MappingsGoogle
    {
        protected static HttpClient client = null;

        static MappingsGoogle()
        {
            client = new HttpClient();

            MappingsUrlClasses = "https://developer.android.com/topic/libraries/support-library/downloads/androidx-class-mapping.csv";
            MappingsRawClasses = Download(MappingsUrlClasses);

            MappingsUrlArtifacts = "https://developer.android.com/topic/libraries/support-library/downloads/androidx-artifact-mapping.csv";
            MappingsRawArtifacts = Download(MappingsUrlArtifacts);

            TypesAndroidSupport = new HashSet<string>();
            TypesAndroidX = new HashSet<string>();

            return;
        }

        public MappingsGoogle()
        {
        }

        public static string Download(string url)
        {
            string result = null;

            using (HttpResponseMessage response = client.GetAsync(url).Result)
            using (HttpContent content = response.Content)
            {
                // ... Read the string.
                result = content.ReadAsStringAsync().Result;
            }

            return result;
        }

        public static string MappingsUrlClasses
        {
            get;
            set;
        }

        public static string MappingsRawClasses
        {
            get;
            protected set;
        }

        public static string MappingsUrlArtifacts
        {
            get;
            set;
        }

        public static string MappingsRawArtifacts
        {
            get;
            protected set;
        }

        public static IEnumerable<string[]> DataTable
        {
            get;
            private set;
        }

        public static void Parse()
        {
            CharacterSeparatedValues csv = new CharacterSeparatedValues()
            {
                Text = MappingsRawClasses
            };

            DataTable = csv.ParseTemporaryImplementation();

            return;
        }

        public static HashSet<string> TypesAndroidSupport
        {
            get;
            private set;
        }

        public static HashSet<string> TypesAndroidX
        {
            get;
            private set;
        }

        public static void Analyze()
        {
            foreach(string[] row in DataTable)
            {
                string as_class = row[0];
                string ax_class = row[1];

                (
                    bool IsNested,
                    string Packagename,
                    string Typename
                )   as_class_parsed = AnalyzeClass(as_class);

                TypesAndroidSupport.Add(as_class_parsed.Typename);

                (
                    bool IsNested,
                    string Packagename,
                    string Typename
                )   ax_class_parsed = AnalyzeClass(ax_class);

                TypesAndroidX.Add(ax_class_parsed.Typename);

            }

            return;
        }

        public static
            IEnumerable
                <
                    (
                        string TypenameAndroidSupport,
                        string PackagenameAndroidSupport,
                        string TypenameAndroidX,
                        string PackagenameAndroidX
                    )
                >
                        Mapping
        {
            get;
            private set;
        }

        private static
                (
                    bool IsNested,
                    string Packagename,
                    string Typename
                )
                    AnalyzeClass(string class_fully_qualified)
        {
            (
                bool IsNested,
                string Packagename,
                string Typename
            )
                class_parsed;

            string type_name = null;
            string ppackage_name = null;
            bool is_nested = false;

            for(int i = 0; i < class_fully_qualified.Length; i++)
            {
                char current = class_fully_qualified[i];
                if(current == '.')
                {
                    if ( char.IsUpper(class_fully_qualified[i + 1]))
                    {
                        type_name = class_fully_qualified.Substring(i);
                        ppackage_name = class_fully_qualified.Substring(0, i);
                        if (type_name.Contains("."))
                        {
                            is_nested = true;
                        }
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            class_parsed =
                (
                    IsNested: is_nested,
                    Packagename: ppackage_name,
                    Typename: type_name
                );

            return class_parsed;
        }
    }
}
