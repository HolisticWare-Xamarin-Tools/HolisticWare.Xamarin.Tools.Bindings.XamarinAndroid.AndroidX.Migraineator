﻿using System.Linq;
using Xamarin.AndroidX.Data;


using Xamarin.AndroidX.Mapper;

namespace App.Xamarin.AndroidX.Mapper
{
    using System;
    using System.Collections.Generic;
    using Core;

    class Program
    {

        static string url_base = "https://github.com/moljac/HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Data/raw/master/data";

        static void Main(string[] args)
        {

            MappingsXamarin xamarin_android_support = new MappingsXamarin();
            xamarin_android_support.Download
                                    (
                                        "Android.Support.merged",
                                        $"{url_base}/Android.Support/AndroidSupport.Merged.dll"
                                    );

            MappingsXamarin xamarin_androidx = new MappingsXamarin();
            xamarin_androidx.Download
                                    (
                                        "AndroidX.merged",
                                        $"{url_base}/AndroidX/AndroidSupport.Merged.dll"
                                    );

            // various Data objects will prepare Collections for faster search

            GoogleMappingDataOptimizedSortedOnly google_data_optimized_sorted = null;
            google_data_optimized_sorted = new GoogleMappingDataOptimizedSortedOnly();
            // parse CSV - google's mappings
            google_data_optimized_sorted.DataTable = MappingsGoogle.Parse();

            // initialize all objects for search
            google_data_optimized_sorted.Initialize();

            // Filter to reduce size (removing packages not used in Xamarin
            google_data_optimized_sorted.FilterForSizeRedcution();


            GoogleMappingDataOptimizedSortedSharded google_data_optimized_sorted_sharded = null;
            google_data_optimized_sorted_sharded = new GoogleMappingDataOptimizedSortedSharded();
            // parse CSV - google's mappings
            google_data_optimized_sorted_sharded.DataTable = MappingsGoogle.Parse();
            // initialize all objects for search
            google_data_optimized_sorted_sharded.Initialize();
            google_data_optimized_sorted_sharded.Analyze();


            xamarin_android_support.GoogleMappingsData = google_data_optimized_sorted;
            xamarin_android_support.Initialize();
            xamarin_android_support.Cecilize();
            //xamarin_android_support.FinalizeMappings();
            xamarin_android_support.Dump();

            xamarin_androidx.GoogleMappingsData = google_data_optimized_sorted;
            xamarin_androidx.Initialize();
            xamarin_androidx.Cecilize();
            //xamarin_androidx.FinalizeMappings();
            xamarin_androidx.Dump();


            MappingsMergedJoined mappings_merged = new MappingsMergedJoined()
            {
                MappingsAndroidSupport = xamarin_android_support,
                MappingsAndroidX = xamarin_androidx
            };
            mappings_merged.MergeJoin();


            ReportGenerator report_generator = new ReportGenerator();
            report_generator.MappingsGoogleWithXamarin.Add(xamarin_android_support);
            report_generator.MappingsGoogleWithXamarin.Add(xamarin_androidx);


            return;
        }


    }
}
