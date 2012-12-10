﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace msn2.net.BarMonkey.RelayControllerService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="RelayControllerService.IRelayController")]
    public interface IRelayController {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelayController/ConnectTest", ReplyAction="http://tempuri.org/IRelayController/ConnectTestResponse")]
        void ConnectTest();
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IRelayController/ConnectTest", ReplyAction="http://tempuri.org/IRelayController/ConnectTestResponse")]
        System.IAsyncResult BeginConnectTest(System.AsyncCallback callback, object asyncState);
        
        void EndConnectTest(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelayController/SendBatch", ReplyAction="http://tempuri.org/IRelayController/SendBatchResponse")]
        void SendBatch(msn2.net.BarMonkey.RelayControllerService.BatchItem[] batch);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IRelayController/SendBatch", ReplyAction="http://tempuri.org/IRelayController/SendBatchResponse")]
        System.IAsyncResult BeginSendBatch(msn2.net.BarMonkey.RelayControllerService.BatchItem[] batch, System.AsyncCallback callback, object asyncState);
        
        void EndSendBatch(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRelayController/TurnAllOff", ReplyAction="http://tempuri.org/IRelayController/TurnAllOffResponse")]
        void TurnAllOff();
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IRelayController/TurnAllOff", ReplyAction="http://tempuri.org/IRelayController/TurnAllOffResponse")]
        System.IAsyncResult BeginTurnAllOff(System.AsyncCallback callback, object asyncState);
        
        void EndTurnAllOff(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IRelayControllerChannel : msn2.net.BarMonkey.RelayControllerService.IRelayController, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RelayControllerClient : System.ServiceModel.ClientBase<msn2.net.BarMonkey.RelayControllerService.IRelayController>, msn2.net.BarMonkey.RelayControllerService.IRelayController {
        
        private BeginOperationDelegate onBeginConnectTestDelegate;
        
        private EndOperationDelegate onEndConnectTestDelegate;
        
        private System.Threading.SendOrPostCallback onConnectTestCompletedDelegate;
        
        private BeginOperationDelegate onBeginSendBatchDelegate;
        
        private EndOperationDelegate onEndSendBatchDelegate;
        
        private System.Threading.SendOrPostCallback onSendBatchCompletedDelegate;
        
        private BeginOperationDelegate onBeginTurnAllOffDelegate;
        
        private EndOperationDelegate onEndTurnAllOffDelegate;
        
        private System.Threading.SendOrPostCallback onTurnAllOffCompletedDelegate;
        
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
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> ConnectTestCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> SendBatchCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> TurnAllOffCompleted;
        
        public void ConnectTest() {
            base.Channel.ConnectTest();
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginConnectTest(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginConnectTest(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public void EndConnectTest(System.IAsyncResult result) {
            base.Channel.EndConnectTest(result);
        }
        
        private System.IAsyncResult OnBeginConnectTest(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return this.BeginConnectTest(callback, asyncState);
        }
        
        private object[] OnEndConnectTest(System.IAsyncResult result) {
            this.EndConnectTest(result);
            return null;
        }
        
        private void OnConnectTestCompleted(object state) {
            if ((this.ConnectTestCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.ConnectTestCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void ConnectTestAsync() {
            this.ConnectTestAsync(null);
        }
        
        public void ConnectTestAsync(object userState) {
            if ((this.onBeginConnectTestDelegate == null)) {
                this.onBeginConnectTestDelegate = new BeginOperationDelegate(this.OnBeginConnectTest);
            }
            if ((this.onEndConnectTestDelegate == null)) {
                this.onEndConnectTestDelegate = new EndOperationDelegate(this.OnEndConnectTest);
            }
            if ((this.onConnectTestCompletedDelegate == null)) {
                this.onConnectTestCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnConnectTestCompleted);
            }
            base.InvokeAsync(this.onBeginConnectTestDelegate, null, this.onEndConnectTestDelegate, this.onConnectTestCompletedDelegate, userState);
        }
        
        public void SendBatch(msn2.net.BarMonkey.RelayControllerService.BatchItem[] batch) {
            base.Channel.SendBatch(batch);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginSendBatch(msn2.net.BarMonkey.RelayControllerService.BatchItem[] batch, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginSendBatch(batch, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public void EndSendBatch(System.IAsyncResult result) {
            base.Channel.EndSendBatch(result);
        }
        
        private System.IAsyncResult OnBeginSendBatch(object[] inValues, System.AsyncCallback callback, object asyncState) {
            msn2.net.BarMonkey.RelayControllerService.BatchItem[] batch = ((msn2.net.BarMonkey.RelayControllerService.BatchItem[])(inValues[0]));
            return this.BeginSendBatch(batch, callback, asyncState);
        }
        
        private object[] OnEndSendBatch(System.IAsyncResult result) {
            this.EndSendBatch(result);
            return null;
        }
        
        private void OnSendBatchCompleted(object state) {
            if ((this.SendBatchCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.SendBatchCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void SendBatchAsync(msn2.net.BarMonkey.RelayControllerService.BatchItem[] batch) {
            this.SendBatchAsync(batch, null);
        }
        
        public void SendBatchAsync(msn2.net.BarMonkey.RelayControllerService.BatchItem[] batch, object userState) {
            if ((this.onBeginSendBatchDelegate == null)) {
                this.onBeginSendBatchDelegate = new BeginOperationDelegate(this.OnBeginSendBatch);
            }
            if ((this.onEndSendBatchDelegate == null)) {
                this.onEndSendBatchDelegate = new EndOperationDelegate(this.OnEndSendBatch);
            }
            if ((this.onSendBatchCompletedDelegate == null)) {
                this.onSendBatchCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnSendBatchCompleted);
            }
            base.InvokeAsync(this.onBeginSendBatchDelegate, new object[] {
                        batch}, this.onEndSendBatchDelegate, this.onSendBatchCompletedDelegate, userState);
        }
        
        public void TurnAllOff() {
            base.Channel.TurnAllOff();
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginTurnAllOff(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginTurnAllOff(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public void EndTurnAllOff(System.IAsyncResult result) {
            base.Channel.EndTurnAllOff(result);
        }
        
        private System.IAsyncResult OnBeginTurnAllOff(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return this.BeginTurnAllOff(callback, asyncState);
        }
        
        private object[] OnEndTurnAllOff(System.IAsyncResult result) {
            this.EndTurnAllOff(result);
            return null;
        }
        
        private void OnTurnAllOffCompleted(object state) {
            if ((this.TurnAllOffCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.TurnAllOffCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void TurnAllOffAsync() {
            this.TurnAllOffAsync(null);
        }
        
        public void TurnAllOffAsync(object userState) {
            if ((this.onBeginTurnAllOffDelegate == null)) {
                this.onBeginTurnAllOffDelegate = new BeginOperationDelegate(this.OnBeginTurnAllOff);
            }
            if ((this.onEndTurnAllOffDelegate == null)) {
                this.onEndTurnAllOffDelegate = new EndOperationDelegate(this.OnEndTurnAllOff);
            }
            if ((this.onTurnAllOffCompletedDelegate == null)) {
                this.onTurnAllOffCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnTurnAllOffCompleted);
            }
            base.InvokeAsync(this.onBeginTurnAllOffDelegate, null, this.onEndTurnAllOffDelegate, this.onTurnAllOffCompletedDelegate, userState);
        }
    }
}
