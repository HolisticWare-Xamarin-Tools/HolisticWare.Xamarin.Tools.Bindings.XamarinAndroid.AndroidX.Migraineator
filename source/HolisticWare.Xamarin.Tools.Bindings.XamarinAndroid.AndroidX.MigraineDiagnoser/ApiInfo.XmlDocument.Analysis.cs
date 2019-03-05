using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Core.Text;
using HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Generated;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator
{
    public partial class ApiInfo
    {
        public partial class XmlDocumentData
        {
            public void Analyse()
            {
                Parallel.Invoke
                (
                    () =>           
                    {
                        this.Namespaces = this.GetNamespaces();
                    },
                    () =>           
                    {
                        this.Classes = this.GetClasses();
                    },
                    () =>
                    {
                        this.ClassesInner = this.GetClassesInner();
                    },
                    () =>
                    {
                        this.Interfaces = this.GetInterfaces();
                    },
                    () =>
                    {
                        this.InterfacesFromClasses = this.GetInterfacesFromClasses();
                    }
                );

                return;
            }

        }
    }
}
