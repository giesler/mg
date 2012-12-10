﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17379
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace msn2.net.BarMonkey.RelayControllerService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BatchItem", Namespace="http://schemas.datacontract.org/2004/07/msn2.net.BarMonkey.RelayController")]
    [System.SerializableAttribute()]
    public partial class BatchItem : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int GroupField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int RelayNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double SecondsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Group {
            get {
                return this.GroupField;
            }
            set {
                if ((this.GroupField.Equals(value) != true)) {
                    this.GroupField = value;
                    this.RaisePropertyChanged("Group");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int RelayNumber {
            get {
                return this.RelayNumberField;
            }
            set {
                if ((this.RelayNumberField.Equals(value) != true)) {
                    this.RelayNumberField = value;
                    this.RaisePropertyChanged("RelayNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Seconds {
            get {
                return this.SecondsField;
            }
            set {
                if ((this.SecondsField.Equals(value) != true)) {
                    this.SecondsField = value;
                    this.RaisePropertyChanged("Seconds");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="RelayControllerService.IRelayController")]
    public interface IRelayController {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelayController/ConnectTest", ReplyAction="http://tempuri.org/IRelayController/ConnectTestResponse")]
        void ConnectTest();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelayController/SendBatch", ReplyAction="http://tempuri.org/IRelayController/SendBatchResponse")]
        void SendBatch(msn2.net.BarMonkey.RelayControllerService.BatchItem[] batch);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelayController/TurnAllOff", ReplyAction="http://tempuri.org/IRelayController/TurnAllOffResponse")]
        void TurnAllOff();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IRelayControllerChannel : msn2.net.BarMonkey.RelayControllerService.IRelayController, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RelayControllerClient : System.ServiceModel.ClientBase<msn2.net.BarMonkey.RelayControllerService.IRelayController>, msn2.net.BarMonkey.RelayControllerService.IRelayController {
        
        public RelayControllerClient() {
        }
        
        public RelayControllerClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public RelayControllerClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RelayControllerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RelayControllerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void ConnectTest() {
            base.Channel.ConnectTest();
        }
        
        public void SendBatch(msn2.net.BarMonkey.RelayControllerService.BatchItem[] batch) {
            base.Channel.SendBatch(batch);
        }
        
        public void TurnAllOff() {
            base.Channel.TurnAllOff();
        }
    }
}
