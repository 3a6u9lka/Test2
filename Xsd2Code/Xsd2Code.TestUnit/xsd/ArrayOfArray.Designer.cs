// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code. Version 3.4.0.14769 Microsoft Reciprocal License (Ms-RL) 
//    <NameSpace>Xsd2Code.TestUnit</NameSpace><Collection>Array</Collection><codeType>CSharp</codeType><EnableDataBinding>False</EnableDataBinding><EnableLazyLoading>False</EnableLazyLoading><TrackingChangesEnable>False</TrackingChangesEnable><GenTrackingClasses>False</GenTrackingClasses><HidePrivateFieldInIDE>False</HidePrivateFieldInIDE><EnableSummaryComment>True</EnableSummaryComment><VirtualProp>False</VirtualProp><IncludeSerializeMethod>False</IncludeSerializeMethod><UseBaseClass>False</UseBaseClass><GenBaseClass>False</GenBaseClass><GenerateCloneMethod>False</GenerateCloneMethod><GenerateDataContracts>False</GenerateDataContracts><CodeBaseTag>Net20</CodeBaseTag><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><GenerateXMLAttributes>False</GenerateXMLAttributes><OrderXMLAttrib>False</OrderXMLAttrib><EnableEncoding>False</EnableEncoding><AutomaticProperties>False</AutomaticProperties><GenerateShouldSerialize>False</GenerateShouldSerialize><DisableDebug>False</DisableDebug><PropNameSpecified>Default</PropNameSpecified><Encoder>ASCII</Encoder><CustomUsings></CustomUsings><ExcludeIncludedTypes>False</ExcludeIncludedTypes><EnableInitializeFields>True</EnableInitializeFields>
//  </auto-generated>
// ------------------------------------------------------------------------------
namespace Xsd2Code.TestUnit
{
    using System;
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System.Collections;
    using System.Xml.Schema;
    using System.ComponentModel;


    public partial class root
    {

        private string[] elementField;

        [System.Xml.Serialization.XmlArrayItemAttribute("FirstName", typeof(string), IsNullable = false)]
        public string[] Element
        {
            get
            {
                return this.elementField;
            }
            set
            {
                this.elementField = value;
            }
        }
    }

    public partial class element
    {

        private string[] firstNameField;

        public string[] FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }
    }
}
