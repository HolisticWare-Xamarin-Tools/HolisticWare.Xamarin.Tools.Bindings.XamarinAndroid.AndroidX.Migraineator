using System;
namespace Xamarin.AndroidX.Mapper
{
    public class MappingsMergedJoined
    {
        public MappingsMergedJoined()
        {
        }

        public MappingsXamarin MappingsAndroidSupport
        {
            get;
            set;
        }

        public MappingsXamarin MappingsAndroidX
        {
            get;
            set;
        }

        public void MergeJoin()
        {
            foreach((string JavaType, string ManagedClass, string ManagedNamespace, string JNIPackage, string JNIType) row in MappingsAndroidSupport.MappingsXamarinManaged)
            {

            }

            return;
        }
    }
}
