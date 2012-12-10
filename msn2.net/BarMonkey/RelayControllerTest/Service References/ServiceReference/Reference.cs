﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1318
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RelayControllerTest.ServiceReference {
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.datacontract.org/2004/07/msn2.net.BarMonkey.RelayController")]
    public partial class BatchItem : object, System.Runtime.Serialization.IExtensibleDataObject {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private int GroupField;
        
        private int RelayNumberField;
        
        private int SecondsField;
        
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
                this.GroupField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int RelayNumber {
            get {
                return this.RelayNumberField;
            }
            set {
                this.RelayNumberField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Seconds {
            get {
                return this.SecondsField;
            }
            set {
                this.SecondsField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference.IRelayController")]
    public interface IRelayController {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelayController/ConnectTest", ReplyAction="http://tempuri.org/IRelayController/ConnectTestResponse")]
        void ConnectTest();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelayController/SendBatch", ReplyAction="http://tempuri.org/IRelayController/SendBatchResponse")]
        void SendBatch(RelayControllerTest.ServiceReference.BatchItem[] batch);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface IRelayControllerChannel : RelayControllerTest.ServiceReference.IRelayController, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class RelayControllerClient : System.ServiceModel.ClientBase<RelayControllerTest.ServiceReference.IRelayController>, RelayControllerTest.ServiceReference.IRelayController {
        
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
        
        public void SendBatch(RelayControllerTest.ServiceReference.BatchItem[] batch) {
            base.Channel.SendBatch(batch);
        }
    }
}
