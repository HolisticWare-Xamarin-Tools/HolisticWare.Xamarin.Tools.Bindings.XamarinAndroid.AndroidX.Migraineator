using System.Collections.Generic;
using System.Linq;

namespace Xamarin.AndroidX.Data
{
    public class DataOptimizedSortedOnly : Data
    {
        public DataOptimizedSortedOnly()
        {

        }

        public
            IEnumerable
                <
                    (
                        string TypenameFullyQualifiedAndroidSupport,
                        string TypenameFullyQualifiedAndroidX
                    )
                >
                        Mapping
        {
            get;
            protected set;
        }

        public
            IEnumerable
                <
                    (
                        string TypenameFullyQualifiedAndroidSupport,
                        string TypenameFullyQualifiedAndroidX
                    )
                >
                        MappingSorted
        {
            get;
            protected set;
        }

        private
            (
                string TypenameFullyQualifiedAndroidSupport,
                string TypenameFullyQualifiedAndroidX
            )[] mapping_sorted = null;


        public IEnumerable<string> MappingSortedIndex                        
        {
            get;
            protected set;
        }

        private string[] mapping_sorted_index = null;

        public override void Initialize()
        {
            this.Mapping = Cast();
            this.MappingSorted = this.Mapping.OrderBy(tuple => tuple.TypenameFullyQualifiedAndroidSupport);
            this.MappingSortedIndex = this.MappingSorted.Select(t => t.TypenameFullyQualifiedAndroidSupport);

            this.mapping_sorted = this.MappingSorted.ToArray();
            this.mapping_sorted_index = this.MappingSortedIndex.ToArray();

            return;
        }

        public
            IEnumerable
                <
                    (
                        string TypenameFullyQualifiedAndroidSupport,
                        string TypenameFullyQualifiedAndroidX
                    )
                >
                        Cast()
        {
            foreach(string[] row in DataTable)
            {
                string as_class = row[0];
                string ax_class = row[1];

                yield return
                            (
                                TypenameFullyQualifiedAndroidSupport: as_class,
                                TypenameFullyQualifiedAndroidX: ax_class
                            ); 
            }
        }

        public
            (
                string TypenameFullyQualifiedAndroidSupport,
                string TypenameFullyQualifiedAndroidX
            )
                Find(string android_support)
        {
            (
                string TypenameFullyQualifiedAndroidSupport,
                string TypenameFullyQualifiedAndroidX
            ) result;


            int index = System.Array.BinarySearch(mapping_sorted_index, android_support);

            if (index < 0 || index > mapping_sorted_index.Length - 1)
            {
                result =
                        (
                            TypenameFullyQualifiedAndroidSupport: "Not found",
                            TypenameFullyQualifiedAndroidX: "Not found"
                        );
            }
            else
            {
                result = mapping_sorted[index];
            }

            return result;
        }
    }
}
