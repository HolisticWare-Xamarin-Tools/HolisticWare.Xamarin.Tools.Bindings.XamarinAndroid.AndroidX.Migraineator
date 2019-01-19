# Android.Support <-> AndroidX

Set of mappings from/to Android.Support to/from AndroidX, both on Android native side and Managed (.NET) side.

Google migration guide: 

https://developer.android.com/jetpack/androidx/migrate

mentiones 2 mapping files (CSV)

*   https://developer.android.com/topic/libraries/support-library/downloads/androidx-class-mapping.csv

*   https://developer.android.com/topic/libraries/support-library/downloads/androidx-artifact-mapping.csv

are considered baseline (readonly) and can be found here:

```
mappings/google-readonly-1-baseline/
mappings/google-readonly-1-baseline/androidx-artifact-mapping.csv
mappings/google-readonly-1-baseline/androidx-class-mapping.csv
```

[androidx-artifact-mapping.csv](mappings/google-readonly-1-baseline/androidx-artifact-mapping.csv)

[androidx-class-mapping.csv](mappings/google-readonly-1-baseline/androidx-class-mapping.csv)

Number of mappings/rows:

*   `google-readonly-2-baseline-prettyfied/androidx-artifact-mapping.csv`

    N = 105

    structure line included (1st line, header)

*   'google-readonly-2-baseline-prettyfied/androidx-class-mapping.csv'

    N = 1807

    structure line included (1st line, header)

Edited (prettyfied) version for easier reading in textual format can be found here:

```
google-readonly-2-baseline-prettyfied/
google-readonly-2-baseline-prettyfied/androidx-artifact-mapping.csv
google-readonly-2-baseline-prettyfied/androidx-class-mapping.csv
```

[androidx-artifact-mapping.csv](mappings/google-readonly-2-baseline-prettyfied/androidx-artifact-mapping.csv)

[androidx-class-mapping.csv](mappings/google-readonly-2-baseline-prettyfied/androidx-class-mapping.csv)


Check (number of mappings/rows):

*   `google-readonly-2-baseline-prettyfied/androidx-artifact-mapping.csv`

    N = 104

    NO structure line included (1st line, header)

*   'google-readonly-2-baseline-prettyfied/androidx-class-mapping.csv'

    N = 1807

    structure line included (1st line, header)

## Docs

*   https://paper.dropbox.com/doc/AndroidX-Migration-Plan-h3CswlPGJitJp0rx6gcI1

*   https://paper.dropbox.com/doc/AndroidX-JetPacks-Planning-xMixAnoc0cahtQ8F0NsMh

*   https://paper.dropbox.com/doc/AndroidX-Binding-Discussion--AVzw~JQhBD1XoGWnkZbazyUcAg-1gs1DHcPsFtDyKC2lIeJ5



## Analysis

Analysis of google mappings pointed out 2 issues:

*   new Material mappings were not included

    `xamarin-managed/material-android-support-to-androidx.csv`

*   some packagenames were not mapped (migrated)

    `google-readonly-3-analyzed/androidx-artifacts-with-old-packagenames.csv`

Those files can be found in following mappings:



## Mapping Generation

NOTE: Planned (work in progress) and subject to discussion.

Class mapping CSV structure (columns):

*   Class

*   Android.Support.PackageName

*   Android.Support.FullyQualifiedClass

*   Android.Support.Artifact

*   AndroidX.PackageName

*   AndroidX.FullyQualifiedClass

*   AndroidX.Artifact

*   Managed.Xamarin.Android.Support.Namespace

*   Managed.Xamarin.Android.Support.FullyQualifiedClass

*   Managed.Xamarin.Android.Support.Assembly

*   Managed.Xamarin.AndroidX.Namespace

*   Managed.Xamarin.AndroidX.FullyQualifiedClass

*   Managed.Xamarin.AndroidX.Assembly


### Bugfixes 

Due to the fact that might be erroneous due to the various reasons (missing data in baseline or incomplete 
Xamarin.Android bindings), fixes will be provided in for of deltas (diff or csv).

```
Filename.csv
Filename.TimeStamp.csv
```


## References / Links

*   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/extern-alias

*   https://paper.dropbox.com/doc/AndroidX-Binding-Discussion--AVzw~JQhBD1XoGWnkZbazyUcAg-1gs1DHcPsFtDyKC2lIeJ5

*   https://android-developers.googleblog.com/2018/05/hello-world-androidx.html

