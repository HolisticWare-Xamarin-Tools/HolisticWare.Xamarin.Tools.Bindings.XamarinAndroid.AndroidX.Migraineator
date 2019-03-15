using System;
using System.Collections.Generic;

using Xamarin.AndroidX.Data;
using Xamarin.AndroidX.Mapper;

namespace App.Xamarin.AndroidX.Mapper
{
    class Program
    {
        static string url_base = "https://github.com/moljac/HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Data/raw/master/data/";

        static void Main(string[] args)
        {

            MappingsXamarin xamarin_android_support = new MappingsXamarin();
            xamarin_android_support.Download
                                    (
                                        "Android.Support.merged",
                                        $"{url_base}Android.Support/AndroidSupport.Merged.dll"
                                    );

            MappingsXamarin xamarin_androidx = new MappingsXamarin();
            xamarin_androidx.Download
                                    (
                                        "AndroidX.merged",
                                        $"{url_base}AndroidX/AndroidSupport.Merged.dll"
                                    );

            // various Data objects will prepare Collections for faster search

            DataOptimizedSortedOnly data_optimized_sorted = new DataOptimizedSortedOnly();
            // parse CSV - google's mappings
            data_optimized_sorted.DataTable = MappingsGoogle.Parse();
            // initialize all objects for search
            data_optimized_sorted.Initialize();


            DataOptimizedSortedSharded data_optimized_sorted_sharded = new DataOptimizedSortedSharded();
            // parse CSV - google's mappings
            data_optimized_sorted_sharded.DataTable = MappingsGoogle.Parse();
            // initialize all objects for search
            data_optimized_sorted_sharded.Initialize();
            data_optimized_sorted_sharded.Analyze();




            return;
        }
    }
}
