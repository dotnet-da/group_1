using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StreamKing.MainApplication
{
    public class DelegateCommand : ICommand
    {
        readonly Action<object> execute;
        readonly Predicate<object> canExecute;

        public DelegateCommand(Predicate<object> canExecute, Action<object> execute)
            => (this.canExecute, this.execute) = (canExecute, execute);

        public DelegateCommand(Action<object> execute) : this(null, execute) { }
        // Binding Engine evaluates if event can be executed or not
        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        // is command ready for usage? if returns
        // false, the Button is deactivated
        public bool CanExecute(object? parameter) => this.canExecute?.Invoke(parameter) ?? true;

        // when button gets clicked, this method will be invoked
        public void Execute(object? parameter) => this.execute?.Invoke(parameter);
    }
      
}
