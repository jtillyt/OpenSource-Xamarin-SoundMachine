using ReactiveUI;
using System.Reactive.Disposables;

namespace SoundMachine.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        protected CompositeDisposable Disposables { get; } = new CompositeDisposable();
    }
}
