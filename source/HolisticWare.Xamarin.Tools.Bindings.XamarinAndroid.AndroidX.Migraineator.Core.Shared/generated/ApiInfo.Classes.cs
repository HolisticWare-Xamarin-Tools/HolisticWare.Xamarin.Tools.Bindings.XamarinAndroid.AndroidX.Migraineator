/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace HolisticWare.Xamarin.Tools.Bindings.XamarinAndroid.AndroidX.Migraineator.Core.Generated
{
    [XmlRoot(ElementName = "attribute")]
    public class Attribute
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "presence")]
        public string Presence { get; set; }
        [XmlElement(ElementName = "warnings")]
        public Warnings Warnings { get; set; }
        [XmlAttribute(AttributeName = "error")]
        public string Error { get; set; }
    }

    [XmlRoot(ElementName = "warning")]
    public class Warning
    {
        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "warnings")]
    public class Warnings
    {
        [XmlElement(ElementName = "warning")]
        public List<Warning> Warning { get; set; }
    }

    [XmlRoot(ElementName = "attributes")]
    public class Attributes
    {
        [XmlElement(ElementName = "attribute")]
        public List<Attribute> Attribute { get; set; }
    }

    [XmlRoot(ElementName = "interface")]
    public class Interface
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "presence")]
        public string Presence { get; set; }
    }

    [XmlRoot(ElementName = "interfaces")]
    public class Interfaces
    {
        [XmlElement(ElementName = "interface")]
        public List<Interface> Interface { get; set; }
    }

    [XmlRoot(ElementName = "constructor")]
    public class Constructor
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "presence")]
        public string Presence { get; set; }
        [XmlElement(ElementName = "parameters")]
        public Parameters Parameters { get; set; }
        [XmlAttribute(AttributeName = "present")]
        public string Present { get; set; }
        [XmlAttribute(AttributeName = "ok")]
        public string Ok { get; set; }
        [XmlAttribute(AttributeName = "complete")]
        public string Complete { get; set; }
        [XmlAttribute(AttributeName = "present_total")]
        public string Present_total { get; set; }
        [XmlAttribute(AttributeName = "ok_total")]
        public string Ok_total { get; set; }
        [XmlAttribute(AttributeName = "complete_total")]
        public string Complete_total { get; set; }
        [XmlElement(ElementName = "attributes")]
        public Attributes Attributes { get; set; }
    }

    [XmlRoot(ElementName = "constructors")]
    public class Constructors
    {
        [XmlElement(ElementName = "constructor")]
        public List<Constructor> Constructor { get; set; }
    }

    [XmlRoot(ElementName = "property")]
    public class Property
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "presence")]
        public string Presence { get; set; }
        [XmlElement(ElementName = "methods")]
        public Methods Methods { get; set; }
        [XmlAttribute(AttributeName = "present")]
        public string Present { get; set; }
        [XmlAttribute(AttributeName = "ok")]
        public string Ok { get; set; }
        [XmlAttribute(AttributeName = "complete")]
        public string Complete { get; set; }
        [XmlAttribute(AttributeName = "present_total")]
        public string Present_total { get; set; }
        [XmlAttribute(AttributeName = "ok_total")]
        public string Ok_total { get; set; }
        [XmlAttribute(AttributeName = "complete_total")]
        public string Complete_total { get; set; }
        [XmlElement(ElementName = "attributes")]
        public Attributes Attributes { get; set; }
        [XmlAttribute(AttributeName = "warning")]
        public string Warning { get; set; }
        [XmlAttribute(AttributeName = "warning_total")]
        public string Warning_total { get; set; }
        [XmlAttribute(AttributeName = "extra")]
        public string Extra { get; set; }
        [XmlAttribute(AttributeName = "extra_total")]
        public string Extra_total { get; set; }
    }

    [XmlRoot(ElementName = "properties")]
    public class Properties
    {
        [XmlElement(ElementName = "property")]
        public List<Property> Property { get; set; }
    }

    [XmlRoot(ElementName = "method")]
    public class Method
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "presence")]
        public string Presence { get; set; }
        [XmlElement(ElementName = "attributes")]
        public Attributes Attributes { get; set; }
        [XmlAttribute(AttributeName = "present")]
        public string Present { get; set; }
        [XmlAttribute(AttributeName = "ok")]
        public string Ok { get; set; }
        [XmlAttribute(AttributeName = "complete")]
        public string Complete { get; set; }
        [XmlAttribute(AttributeName = "present_total")]
        public string Present_total { get; set; }
        [XmlAttribute(AttributeName = "ok_total")]
        public string Ok_total { get; set; }
        [XmlAttribute(AttributeName = "complete_total")]
        public string Complete_total { get; set; }
        [XmlElement(ElementName = "parameters")]
        public Parameters Parameters { get; set; }
        [XmlAttribute(AttributeName = "warning")]
        public string Warning { get; set; }
        [XmlAttribute(AttributeName = "warning_total")]
        public string Warning_total { get; set; }
    }

    [XmlRoot(ElementName = "methods")]
    public class Methods
    {
        [XmlElement(ElementName = "method")]
        public List<Method> Method { get; set; }
    }

    [XmlRoot(ElementName = "class")]
    public class Class
    {
        [XmlElement(ElementName = "attributes")]
        public Attributes Attributes { get; set; }
        [XmlElement(ElementName = "warnings")]
        public Warnings Warnings { get; set; }
        [XmlElement(ElementName = "interfaces")]
        public Interfaces Interfaces { get; set; }
        [XmlElement(ElementName = "constructors")]
        public Constructors Constructors { get; set; }
        [XmlElement(ElementName = "properties")]
        public Properties Properties { get; set; }
        [XmlElement(ElementName = "methods")]
        public Methods Methods { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "error")]
        public string Error { get; set; }
        [XmlAttribute(AttributeName = "missing")]
        public string Missing { get; set; }
        [XmlAttribute(AttributeName = "complete")]
        public string Complete { get; set; }
        [XmlAttribute(AttributeName = "warning")]
        public string Warning { get; set; }
        [XmlAttribute(AttributeName = "missing_total")]
        public string Missing_total { get; set; }
        [XmlAttribute(AttributeName = "complete_total")]
        public string Complete_total { get; set; }
        [XmlElement(ElementName = "classes")]
        public Classes Classes { get; set; }
        [XmlElement(ElementName = "fields")]
        public Fields Fields { get; set; }
        [XmlElement(ElementName = "events")]
        public Events Events { get; set; }
        [XmlAttribute(AttributeName = "present")]
        public string Present { get; set; }
        [XmlAttribute(AttributeName = "extra")]
        public string Extra { get; set; }
        [XmlAttribute(AttributeName = "ok")]
        public string Ok { get; set; }
        [XmlAttribute(AttributeName = "present_total")]
        public string Present_total { get; set; }
        [XmlAttribute(AttributeName = "extra_total")]
        public string Extra_total { get; set; }
        [XmlAttribute(AttributeName = "ok_total")]
        public string Ok_total { get; set; }
        [XmlAttribute(AttributeName = "warning_total")]
        public string Warning_total { get; set; }
        [XmlAttribute(AttributeName = "presence")]
        public string Presence { get; set; }
    }

    [XmlRoot(ElementName = "classes")]
    public class Classes
    {
        [XmlElement(ElementName = "class")]
        public List<Class> Class { get; set; }
    }

    [XmlRoot(ElementName = "namespace")]
    public class Namespace
    {
        [XmlElement(ElementName = "classes")]
        public Classes Classes { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "missing")]
        public string Missing { get; set; }
        [XmlAttribute(AttributeName = "complete")]
        public string Complete { get; set; }
        [XmlAttribute(AttributeName = "warning")]
        public string Warning { get; set; }
        [XmlAttribute(AttributeName = "missing_total")]
        public string Missing_total { get; set; }
        [XmlAttribute(AttributeName = "complete_total")]
        public string Complete_total { get; set; }
        [XmlAttribute(AttributeName = "warning_total")]
        public string Warning_total { get; set; }
        [XmlAttribute(AttributeName = "presence")]
        public string Presence { get; set; }
        [XmlAttribute(AttributeName = "ok")]
        public string Ok { get; set; }
        [XmlAttribute(AttributeName = "ok_total")]
        public string Ok_total { get; set; }
        [XmlAttribute(AttributeName = "extra")]
        public string Extra { get; set; }
        [XmlAttribute(AttributeName = "extra_total")]
        public string Extra_total { get; set; }
        [XmlAttribute(AttributeName = "present")]
        public string Present { get; set; }
        [XmlAttribute(AttributeName = "present_total")]
        public string Present_total { get; set; }
    }

    [XmlRoot(ElementName = "field")]
    public class Field
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "presence")]
        public string Presence { get; set; }
        [XmlElement(ElementName = "attributes")]
        public Attributes Attributes { get; set; }
    }

    [XmlRoot(ElementName = "fields")]
    public class Fields
    {
        [XmlElement(ElementName = "field")]
        public List<Field> Field { get; set; }
    }

    [XmlRoot(ElementName = "event")]
    public class Event
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "presence")]
        public string Presence { get; set; }
    }

    [XmlRoot(ElementName = "events")]
    public class Events
    {
        [XmlElement(ElementName = "event")]
        public List<Event> Event { get; set; }
    }

    [XmlRoot(ElementName = "parameter")]
    public class Parameter
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "attributes")]
        public Attributes Attributes { get; set; }
        [XmlElement(ElementName = "warnings")]
        public Warnings Warnings { get; set; }
        [XmlAttribute(AttributeName = "error")]
        public string Error { get; set; }
    }

    [XmlRoot(ElementName = "parameters")]
    public class Parameters
    {
        [XmlElement(ElementName = "parameter")]
        public List<Parameter> Parameter { get; set; }
    }

    [XmlRoot(ElementName = "namespaces")]
    public class Namespaces
    {
        [XmlElement(ElementName = "namespace")]
        public List<Namespace> Namespace { get; set; }
    }

    [XmlRoot(ElementName = "assembly")]
    public class Assembly
    {
        [XmlElement(ElementName = "attributes")]
        public Attributes Attributes { get; set; }
        [XmlElement(ElementName = "namespaces")]
        public Namespaces Namespaces { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
        [XmlAttribute(AttributeName = "missing")]
        public string Missing { get; set; }
        [XmlAttribute(AttributeName = "present")]
        public string Present { get; set; }
        [XmlAttribute(AttributeName = "extra")]
        public string Extra { get; set; }
        [XmlAttribute(AttributeName = "ok")]
        public string Ok { get; set; }
        [XmlAttribute(AttributeName = "complete")]
        public string Complete { get; set; }
        [XmlAttribute(AttributeName = "warning")]
        public string Warning { get; set; }
        [XmlAttribute(AttributeName = "missing_total")]
        public string Missing_total { get; set; }
        [XmlAttribute(AttributeName = "present_total")]
        public string Present_total { get; set; }
        [XmlAttribute(AttributeName = "extra_total")]
        public string Extra_total { get; set; }
        [XmlAttribute(AttributeName = "ok_total")]
        public string Ok_total { get; set; }
        [XmlAttribute(AttributeName = "complete_total")]
        public string Complete_total { get; set; }
        [XmlAttribute(AttributeName = "warning_total")]
        public string Warning_total { get; set; }
    }

    [XmlRoot(ElementName = "assemblies")]
    public class ApiInfo
    {
        [XmlElement(ElementName = "assembly")]
        public Assembly Assembly { get; set; }
    }

}
