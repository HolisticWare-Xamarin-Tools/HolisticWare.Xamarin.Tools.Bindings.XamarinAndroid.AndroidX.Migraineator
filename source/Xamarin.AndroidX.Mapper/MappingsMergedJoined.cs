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
            (
                string TypenameFullyQualifiedAndroidSupport,
                string TypenameFullyQualifiedAndroidX,
                string C
            )
                mapping_migration_tuple;

            foreach
                (
                    (
                        string TypenameFullyQualifiedAndroidSupport,
                        string TypenameFullyQualifiedAndroidX
                    )
                        row in MappingsAndroidSupport.GoogleMappingsData.Mapping
                )
            {
                
            }

            return;
        }
    }
}
