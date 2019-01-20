using System;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator
{
    public partial class AndroidXMigraineatorTask : Microsoft.Build.Utilities.Task
    {
        [Microsoft.Build.Framework.Required]
        public string Options
        {
            get;
            set;
        }

        public override bool Execute()
        {

            // enforcing proper correlation between Log errors and build results (success and/or failures)
            return !Log.HasLoggedErrors;
        }


    }
}