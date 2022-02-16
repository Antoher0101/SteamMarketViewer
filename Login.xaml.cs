using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SteamMarketViewer;

namespace SteamMarketViewer
{
	/// <summary>
	/// Логика взаимодействия для Login.xaml
	/// </summary>
	public partial class Login : Window
	{
		private SteamControl _steamControl;

		public Login()
		{
			InitializeComponent();

			_steamControl = MainWindow.SteamControl;
			this.DataContext = _steamControl;
		}

		public void CloseWindow()
		{
			this.Close();
		}
		private async void LoginBtn_Click(object sender, RoutedEventArgs e)
		{
			_steamControl.WaitingForInput = false;
			_steamControl.LoginError = false;
			if (!_steamControl.Processing)
			{
				_steamControl.AuthenticateAsync(LoginBox.Text, PasswordBox.Password);
			}
			if (_steamControl.Need2FA) SteamGuardCode.Focus();
			if (_steamControl.NeedCaptcha) CaptchaBox.Focus();
			if (_steamControl.NeedEmail) EmailCode.Focus();
		}

		private void SteamGuardCode_TextChanged(object sender, TextChangedEventArgs e)
		{
            SteamGuardCode.CaretIndex = SteamGuardCode.Text.Length;
		}
		private void EmailCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            EmailCode.CaretIndex = EmailCode.Text.Length;
        }
	}

	public class InversedBoolToVisibility : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value)
			{
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((Visibility)value == Visibility.Collapsed)
			{
				return true;
			}

			return false;
		}
	}

	public class BoolToVisibility : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value)
			{
				return Visibility.Visible;
			}

			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((Visibility)value == Visibility.Collapsed)
			{
				return false;
			}

			return true;
		}
	}

	public class InversedBoolean : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value)
				return false;
			return true;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value)
				return true;
			return false;
		}
	}
}
