using System;
using System.Diagnostics;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace NitsoAsset_Maui.ViewModels.Base
{
    public class RelayCommandWithArgs<T> : BaseCommand
    {
        public override void Execute(object parameter)
        {
            try
            {
                if (!CanExecute(parameter))
                {
                    return;
                }

                base.Execute(parameter);

                actionExecute.Invoke((T)parameter);
                successCommand = true;
            }
            catch (Exception ex)
            {
                if (vmodel != null)
                {
                    Debug.WriteLine(ex, _message);
                }

                successCommand = false;
            }
            finally
            {
                isExecuting = false;
            }
        }

        public RelayCommandWithArgs(Action<T> action, ViewModel vm, string message = null)
            : base(vm, message)
        {
            actionExecute = action;
        }

        private Action<T> actionExecute;
    }
}