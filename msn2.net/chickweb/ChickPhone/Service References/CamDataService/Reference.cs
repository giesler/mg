﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.Phone.ServiceReference, version 3.7.0.0
// 
namespace ChickPhone.CamDataService {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LogItem", Namespace="http://schemas.datacontract.org/2004/07/")]
    public partial class LogItem : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string IdField;
        
        private System.DateTime TimestampField;
        
        private string UrlField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Id {
            get {
                return this.IdField;
            }
            set {
                if ((object.ReferenceEquals(this.IdField, value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Timestamp {
            get {
                return this.TimestampField;
            }
            set {
                if ((this.TimestampField.Equals(value) != true)) {
                    this.TimestampField = value;
                    this.RaisePropertyChanged("Timestamp");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Url {
            get {
                return this.UrlField;
            }
            set {
                if ((object.ReferenceEquals(this.UrlField, value) != true)) {
                    this.UrlField = value;
                    this.RaisePropertyChanged("Url");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="VideoItem", Namespace="http://schemas.datacontract.org/2004/07/")]
    public partial class VideoItem : object, System.ComponentModel.INotifyPropertyChanged {
        
        private double DurationField;
        
        private string IdField;
        
        private double MotionPercentageField;
        
        private string NameField;
        
        private System.DateTime TimestampField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Duration {
            get {
                return this.DurationField;
            }
            set {
                if ((this.DurationField.Equals(value) != true)) {
                    this.DurationField = value;
                    this.RaisePropertyChanged("Duration");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Id {
            get {
                return this.IdField;
            }
            set {
                if ((object.ReferenceEquals(this.IdField, value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double MotionPercentage {
            get {
                return this.MotionPercentageField;
            }
            set {
                if ((this.MotionPercentageField.Equals(value) != true)) {
                    this.MotionPercentageField = value;
                    this.RaisePropertyChanged("MotionPercentage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Timestamp {
            get {
                return this.TimestampField;
            }
            set {
                if ((this.TimestampField.Equals(value) != true)) {
                    this.TimestampField = value;
                    this.RaisePropertyChanged("Timestamp");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CamDataService.ICameraData")]
    public interface ICameraData {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ICameraData/GetItems", ReplyAction="http://tempuri.org/ICameraData/GetItemsResponse")]
        System.IAsyncResult BeginGetItems(System.DateTime date, System.AsyncCallback callback, object asyncState);
        
        System.Collections.ObjectModel.ObservableCollection<ChickPhone.CamDataService.LogItem> EndGetItems(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/ICameraData/GetVideos", ReplyAction="http://tempuri.org/ICameraData/GetVideosResponse")]
        System.IAsyncResult BeginGetVideos(System.DateTime startTime, System.DateTime endTime, System.AsyncCallback callback, object asyncState);
        
        System.Collections.ObjectModel.ObservableCollection<ChickPhone.CamDataService.VideoItem> EndGetVideos(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICameraDataChannel : ChickPhone.CamDataService.ICameraData, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetItemsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetItemsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.ObjectModel.ObservableCollection<ChickPhone.CamDataService.LogItem> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.ObjectModel.ObservableCollection<ChickPhone.CamDataService.LogItem>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetVideosCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetVideosCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.ObjectModel.ObservableCollection<ChickPhone.CamDataService.VideoItem> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.ObjectModel.ObservableCollection<ChickPhone.CamDataService.VideoItem>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CameraDataClient : System.ServiceModel.ClientBase<ChickPhone.CamDataService.ICameraData>, ChickPhone.CamDataService.ICameraData {
        
        private BeginOperationDelegate onBeginGetItemsDelegate;
        
        private EndOperationDelegate onEndGetItemsDelegate;
        
        private System.Threading.SendOrPostCallback onGetItemsCompletedDelegate;
        
        private BeginOperationDelegate onBeginGetVideosDelegate;
        
        private EndOperationDelegate onEndGetVideosDelegate;
        
        private System.Threading.SendOrPostCallback onGetVideosCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public CameraDataClient() {
        }
        
        public CameraDataClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CameraDataClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CameraDataClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CameraDataClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<GetItemsCompletedEventArgs> GetItemsCompleted;
        
        public event System.EventHandler<GetVideosCompletedEventArgs> GetVideosCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult ChickPhone.CamDataService.ICameraData.BeginGetItems(System.DateTime date, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetItems(date, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.ObjectModel.ObservableCollection<ChickPhone.CamDataService.LogItem> ChickPhone.CamDataService.ICameraData.EndGetItems(System.IAsyncResult result) {
            return base.Channel.EndGetItems(result);
        }
        
        private System.IAsyncResult OnBeginGetItems(object[] inValues, System.AsyncCallback callback, object asyncState) {
            System.DateTime date = ((System.DateTime)(inValues[0]));
            return ((ChickPhone.CamDataService.ICameraData)(this)).BeginGetItems(date, callback, asyncState);
        }
        
        private object[] OnEndGetItems(System.IAsyncResult result) {
            System.Collections.ObjectModel.ObservableCollection<ChickPhone.CamDataService.LogItem> retVal = ((ChickPhone.CamDataService.ICameraData)(this)).EndGetItems(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetItemsCompleted(object state) {
            if ((this.GetItemsCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetItemsCompleted(this, new GetItemsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetItemsAsync(System.DateTime date) {
            this.GetItemsAsync(date, null);
        }
        
        public void GetItemsAsync(System.DateTime date, object userState) {
            if ((this.onBeginGetItemsDelegate == null)) {
                this.onBeginGetItemsDelegate = new BeginOperationDelegate(this.OnBeginGetItems);
            }
            if ((this.onEndGetItemsDelegate == null)) {
                this.onEndGetItemsDelegate = new EndOperationDelegate(this.OnEndGetItems);
            }
            if ((this.onGetItemsCompletedDelegate == null)) {
                this.onGetItemsCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetItemsCompleted);
            }
            base.InvokeAsync(this.onBeginGetItemsDelegate, new object[] {
                        date}, this.onEndGetItemsDelegate, this.onGetItemsCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult ChickPhone.CamDataService.ICameraData.BeginGetVideos(System.DateTime startTime, System.DateTime endTime, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetVideos(startTime, endTime, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.ObjectModel.ObservableCollection<ChickPhone.CamDataService.VideoItem> ChickPhone.CamDataService.ICameraData.EndGetVideos(System.IAsyncResult result) {
            return base.Channel.EndGetVideos(result);
        }
        
        private System.IAsyncResult OnBeginGetVideos(object[] inValues, System.AsyncCallback callback, object asyncState) {
            System.DateTime startTime = ((System.DateTime)(inValues[0]));
            System.DateTime endTime = ((System.DateTime)(inValues[1]));
            return ((ChickPhone.CamDataService.ICameraData)(this)).BeginGetVideos(startTime, endTime, callback, asyncState);
        }
        
        private object[] OnEndGetVideos(System.IAsyncResult result) {
            System.Collections.ObjectModel.ObservableCollection<ChickPhone.CamDataService.VideoItem> retVal = ((ChickPhone.CamDataService.ICameraData)(this)).EndGetVideos(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetVideosCompleted(object state) {
            if ((this.GetVideosCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetVideosCompleted(this, new GetVideosCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetVideosAsync(System.DateTime startTime, System.DateTime endTime) {
            this.GetVideosAsync(startTime, endTime, null);
        }
        
        public void GetVideosAsync(System.DateTime startTime, System.DateTime endTime, object userState) {
            if ((this.onBeginGetVideosDelegate == null)) {
                this.onBeginGetVideosDelegate = new BeginOperationDelegate(this.OnBeginGetVideos);
            }
            if ((this.onEndGetVideosDelegate == null)) {
                this.onEndGetVideosDelegate = new EndOperationDelegate(this.OnEndGetVideos);
            }
            if ((this.onGetVideosCompletedDelegate == null)) {
                this.onGetVideosCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetVideosCompleted);
            }
            base.InvokeAsync(this.onBeginGetVideosDelegate, new object[] {
                        startTime,
                        endTime}, this.onEndGetVideosDelegate, this.onGetVideosCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override ChickPhone.CamDataService.ICameraData CreateChannel() {
            return new CameraDataClientChannel(this);
        }
        
        private class CameraDataClientChannel : ChannelBase<ChickPhone.CamDataService.ICameraData>, ChickPhone.CamDataService.ICameraData {
            
            public CameraDataClientChannel(System.ServiceModel.ClientBase<ChickPhone.CamDataService.ICameraData> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetItems(System.DateTime date, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = date;
                System.IAsyncResult _result = base.BeginInvoke("GetItems", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.ObjectModel.ObservableCollection<ChickPhone.CamDataService.LogItem> EndGetItems(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.ObjectModel.ObservableCollection<ChickPhone.CamDataService.LogItem> _result = ((System.Collections.ObjectModel.ObservableCollection<ChickPhone.CamDataService.LogItem>)(base.EndInvoke("GetItems", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginGetVideos(System.DateTime startTime, System.DateTime endTime, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[2];
                _args[0] = startTime;
                _args[1] = endTime;
                System.IAsyncResult _result = base.BeginInvoke("GetVideos", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.ObjectModel.ObservableCollection<ChickPhone.CamDataService.VideoItem> EndGetVideos(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.ObjectModel.ObservableCollection<ChickPhone.CamDataService.VideoItem> _result = ((System.Collections.ObjectModel.ObservableCollection<ChickPhone.CamDataService.VideoItem>)(base.EndInvoke("GetVideos", _args, result)));
                return _result;
            }
        }
    }
}
