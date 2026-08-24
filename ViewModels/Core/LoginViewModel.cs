using System.Linq;
using System.Linq; // Cần thiết để dùng .OfType<Window>()
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QL_quanCafe.Data;
using QL_quanCafe.ViewModels.Common;
using QL_quanCafe.Views.Core; // để nhận diện LoginWindow


namespace QL_quanCafe.ViewModels.Core
{
    public class LoginViewModel : BaseViewModel
    {
        private string _userName = string.Empty;
        public string UserName { get => _userName; set { _userName = value; OnPropertyChanged(); } }

        public ICommand LoginCommand { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<PasswordBox>((p) =>
            {
                if (p == null) return;

                string password = p.Password;

                using (var db = new CoffeeDbContext())
                {
                    var account = db.TaiKhoans.FirstOrDefault(t => t.UserName == UserName && t.Password == password);

                    if (account != null)
                    {
                        UserSession.CurrentUser = account;   // THÊM DÒNG NÀY

                        MainWindow main = new MainWindow(account.Role);
                        main.Show();

                        Application.Current.Windows
                            .OfType<Window>()
                            .FirstOrDefault(w => w is LoginWindow)?
                            .Close();
                    }
                    else
                    {
                        MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            },
            (p) => { return true; });
        }
    }
}