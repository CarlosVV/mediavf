using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Runtime.Serialization;

using Shared = MediaVF.Common.Communication.Invocation;
using MediaVF.UI.Core;

namespace MediaVF.Web.BandedTogether.UI
{
    public interface IInvokableServiceChannel : IInvocableService, IClientChannel
    {
    }

    public partial class InvokeCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        public InvokeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState)
            : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public Shared.InvokeResponse Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return ((Shared.InvokeResponse)(this.results[0]));
            }
        }
    }

    public partial class BandedTogetherServiceClient : ClientBase<IInvocableService>, IInvocableService
    {
        private BeginOperationDelegate onBeginInvokeDelegate;

        private EndOperationDelegate onEndInvokeDelegate;

        private System.Threading.SendOrPostCallback onInvokeCompletedDelegate;

        private BeginOperationDelegate onBeginOpenDelegate;

        private EndOperationDelegate onEndOpenDelegate;

        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;

        private BeginOperationDelegate onBeginCloseDelegate;

        private EndOperationDelegate onEndCloseDelegate;

        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;

        public BandedTogetherServiceClient()
        {
        }

        public System.Net.CookieContainer CookieContainer
        {
            get
            {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null))
                {
                    return httpCookieContainerManager.CookieContainer;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null))
                {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else
                {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }

        public event EventHandler<InvokeCompletedEventArgs> InvokeCompleted;

        public event EventHandler<AsyncCompletedEventArgs> OpenCompleted;

        public event EventHandler<AsyncCompletedEventArgs> CloseCompleted;

        [EditorBrowsableAttribute(EditorBrowsableState.Advanced)]
        IAsyncResult IInvocableService.BeginInvoke(Shared.InvokeRequest request, AsyncCallback callback, object asyncState)
        {
            return base.Channel.BeginInvoke(request, callback, asyncState);
        }

        [EditorBrowsableAttribute(EditorBrowsableState.Advanced)]
        Shared.InvokeResponse IInvocableService.EndInvoke(IAsyncResult result)
        {
            return base.Channel.EndInvoke(result);
        }

        private System.IAsyncResult OnBeginInvoke(object[] inValues, System.AsyncCallback callback, object asyncState)
        {
            Shared.InvokeRequest request = ((Shared.InvokeRequest)(inValues[0]));
            return ((IInvocableService)(this)).BeginInvoke(request, callback, asyncState);
        }

        private object[] OnEndInvoke(System.IAsyncResult result)
        {
            Shared.InvokeResponse retVal = ((IInvocableService)(this)).EndInvoke(result);
            return new object[] {
                    retVal};
        }

        private void OnInvokeCompleted(object state)
        {
            if ((this.InvokeCompleted != null))
            {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.InvokeCompleted(this, new InvokeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }

        public void InvokeAsync(Shared.InvokeRequest request)
        {
            this.InvokeAsync(request, null);
        }

        public void InvokeAsync(Shared.InvokeRequest request, object userState)
        {
            if ((this.onBeginInvokeDelegate == null))
            {
                this.onBeginInvokeDelegate = new BeginOperationDelegate(this.OnBeginInvoke);
            }
            if ((this.onEndInvokeDelegate == null))
            {
                this.onEndInvokeDelegate = new EndOperationDelegate(this.OnEndInvoke);
            }
            if ((this.onInvokeCompletedDelegate == null))
            {
                this.onInvokeCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnInvokeCompleted);
            }
            base.InvokeAsync(this.onBeginInvokeDelegate, new object[] {
                        request}, this.onEndInvokeDelegate, this.onInvokeCompletedDelegate, userState);
        }

        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState)
        {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }

        private object[] OnEndOpen(System.IAsyncResult result)
        {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }

        private void OnOpenCompleted(object state)
        {
            if ((this.OpenCompleted != null))
            {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }

        public void OpenAsync()
        {
            this.OpenAsync(null);
        }

        public void OpenAsync(object userState)
        {
            if ((this.onBeginOpenDelegate == null))
            {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null))
            {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null))
            {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }

        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState)
        {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }

        private object[] OnEndClose(System.IAsyncResult result)
        {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }

        private void OnCloseCompleted(object state)
        {
            if ((this.CloseCompleted != null))
            {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }

        public void CloseAsync()
        {
            this.CloseAsync(null);
        }

        public void CloseAsync(object userState)
        {
            if ((this.onBeginCloseDelegate == null))
            {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null))
            {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null))
            {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }

        protected override IInvocableService CreateChannel()
        {
            return new InvokableServiceClientChannel(this);
        }

        private class InvokableServiceClientChannel : ChannelBase<IInvocableService>, IInvocableService
        {

            public InvokableServiceClientChannel(System.ServiceModel.ClientBase<IInvocableService> client) :
                base(client)
            {
            }

            public IAsyncResult BeginInvoke(Shared.InvokeRequest request, System.AsyncCallback callback, object asyncState)
            {
                object[] _args = new object[1];
                _args[0] = request;
                System.IAsyncResult _result = base.BeginInvoke("Invoke", _args, callback, asyncState);
                return _result;
            }

            public Shared.InvokeResponse EndInvoke(System.IAsyncResult result)
            {
                object[] _args = new object[0];
                Shared.InvokeResponse _result = ((Shared.InvokeResponse)(base.EndInvoke("Invoke", _args, result)));
                return _result;
            }
        }
    }
}
