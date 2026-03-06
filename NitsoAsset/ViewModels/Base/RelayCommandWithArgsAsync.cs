using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace NitsoAsset.ViewModels.Base
{
    public class RelayCommandWithArgsAsync<T> : BaseCommand
    {
        public RelayCommandWithArgsAsync(Func<T, Task> action, ViewModel vm, string message = null)
          : base(vm, message)
        {
            this.actionExecute = action;
        }

        public override async void Execute(object parameter)
        {

            try
            {
                if (!CanExecute(parameter))
                {
                    return;
                }

                base.Execute(parameter);

                await actionExecute.Invoke((T)parameter);
                successCommand = true;
            }
            catch (Exception ex)
            {
                if (vmodel != null)
                    Debug.Write(ex, _message);


                successCommand = false;
            }
            finally
            {
                isExecuting = false;
            }
        }

        private Func<T, Task> actionExecute;
    }
}