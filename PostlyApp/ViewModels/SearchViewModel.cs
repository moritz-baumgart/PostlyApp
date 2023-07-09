
using System.ComponentModel;
using System.Windows.Input;

namespace PostlyApp.ViewModels
{
    class SearchViewModel : INotifyPropertyChanged
    {

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get { return _searchCommand; }
            set
            {
                _searchCommand = value;
                OnPropertyChanged(nameof(SearchCommand));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
