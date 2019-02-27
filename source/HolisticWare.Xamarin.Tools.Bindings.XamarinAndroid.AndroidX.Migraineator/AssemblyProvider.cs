
namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator
{
    public partial class AssemblyProvider
    {
        public string Folder
        {
            get
            {
                return folder;
            }
            set
            {
                folder = value;

                Assemblies = System.IO.Directory.GetFiles
                                                    (
                                                        folder,
                                                        "*.dll",
                                                        System.IO.SearchOption.AllDirectories
                                                    );
            }

        }
        string folder = null;


        public string[] Assemblies
        {
            get;
            private set;
        }

    }
}
