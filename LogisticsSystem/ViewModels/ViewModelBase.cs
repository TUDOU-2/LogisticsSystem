using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using LogisticsSystem.Messages;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.ViewModels
{
    public class ViewModelBase : ObservableValidator
    {
        protected IMessenger Messenger { get; } = WeakReferenceMessenger.Default;

        private bool isActive;

        public bool IsActive
        {
            get => this.isActive;
            set
            {
                if (SetProperty(ref this.isActive, value, true))
                {
                    if (value)
                    {
                        OnActivated();
                    }
                    else
                    {
                        OnDeactivated();
                    }
                }
            }
        }
        protected void ShowProgress(bool isVisible)
        {
            Messenger.Send(new ProgressMessage(isVisible));
        }

        protected virtual void OnActivated()
        {
            Messenger.RegisterAll(this);
        }
        protected virtual void OnDeactivated()
        {
            Messenger.UnregisterAll(this);
        }
        protected virtual void Broadcast<T>(T oldValue, T newValue, string? propertyName)
        {
            PropertyChangedMessage<T> message = new(this, propertyName, oldValue, newValue);

            _ = Messenger.Send(message);
        }
    }
}
