using System.Linq;
using System.Reactive.Disposables;
using ReactiveUI;

namespace SoundMachine.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        protected CompositeDisposable Disposables { get; } = new CompositeDisposable();
    }
}