using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
namespace NitsoAsset.ViewModels.Base
{
    public class BaseCommand : ICommand
    {
        public bool Succes
        {
            get
            {
                return this.successCommand;
            }
        }

        public BaseCommand(IViewModel vm, string message = null)
        {
            this.vmodel = vm;
            this._message = message;
            isExecuting = false;
        }

        #region ICommand implementation

#pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore 0067


        public virtual bool CanExecute(object parameter)
        {
            return !isExecuting;
        }

        public virtual void Execute(object parameter)
        {
            isExecuting = true;
        }

        public virtual async Task ExecuteAsync(object parameter)
        {
            await Task.FromResult(0);
        }

        #endregion

        protected string _message;
        protected IViewModel vmodel;
        protected bool successCommand;
        protected bool isExecuting;
    }
}