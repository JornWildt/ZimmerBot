﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Denne kode blev oprettet ved hjælp af et værktøj.
//     Runtime-version:4.0.30319.42000
//
//     Ændringer af denne fil kan resultere i ukorrekt funktion, og ændringerne mistes, hvis
//     koden oprettes igen.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.6.81.0.
// 
namespace Rejseplanen.ZimmerBot.AddOn.Schemas {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class DepartureBoard {
        
        private Departure[] departureField;
        
        private string errorField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Departure")]
        public Departure[] Departure {
            get {
                return this.departureField;
            }
            set {
                this.departureField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string error {
            get {
                return this.errorField;
            }
            set {
                this.errorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class Departure {
        
        private JourneyDetailRef journeyDetailRefField;
        
        private string nameField;
        
        private DepartureType typeField;
        
        private string stopField;
        
        private string timeField;
        
        private string dateField;
        
        private string trackField;
        
        private string rtTimeField;
        
        private string rtDateField;
        
        private string rtTrackField;
        
        private string directionField;
        
        private bool cancelledField;
        
        private string messagesField;
        
        private string finalStopField;
        
        private string stateField;
        
        public Departure() {
            this.cancelledField = false;
        }
        
        /// <remarks/>
        public JourneyDetailRef JourneyDetailRef {
            get {
                return this.journeyDetailRefField;
            }
            set {
                this.journeyDetailRefField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DepartureType type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string stop {
            get {
                return this.stopField;
            }
            set {
                this.stopField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string time {
            get {
                return this.timeField;
            }
            set {
                this.timeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string date {
            get {
                return this.dateField;
            }
            set {
                this.dateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string track {
            get {
                return this.trackField;
            }
            set {
                this.trackField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string rtTime {
            get {
                return this.rtTimeField;
            }
            set {
                this.rtTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string rtDate {
            get {
                return this.rtDateField;
            }
            set {
                this.rtDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string rtTrack {
            get {
                return this.rtTrackField;
            }
            set {
                this.rtTrackField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string direction {
            get {
                return this.directionField;
            }
            set {
                this.directionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool cancelled {
            get {
                return this.cancelledField;
            }
            set {
                this.cancelledField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
        public string messages {
            get {
                return this.messagesField;
            }
            set {
                this.messagesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string finalStop {
            get {
                return this.finalStopField;
            }
            set {
                this.finalStopField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string state {
            get {
                return this.stateField;
            }
            set {
                this.stateField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class JourneyDetailRef {
        
        private string refField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @ref {
            get {
                return this.refField;
            }
            set {
                this.refField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.81.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public enum DepartureType {
        
        /// <remarks/>
        IC,
        
        /// <remarks/>
        LYN,
        
        /// <remarks/>
        REG,
        
        /// <remarks/>
        S,
        
        /// <remarks/>
        TOG,
        
        /// <remarks/>
        BUS,
        
        /// <remarks/>
        EXB,
        
        /// <remarks/>
        NB,
        
        /// <remarks/>
        TB,
        
        /// <remarks/>
        F,
        
        /// <remarks/>
        M,
    }
}
