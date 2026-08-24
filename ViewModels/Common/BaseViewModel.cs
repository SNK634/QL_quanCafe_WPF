using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QL_quanCafe.ViewModels.Common
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        // Đã thêm dấu ?
        public event PropertyChangedEventHandler? PropertyChanged;

        // Đã thêm dấu ?
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
