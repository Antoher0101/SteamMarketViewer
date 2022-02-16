using SteamMarketViewer.SteamAuth;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Newtonsoft.Json;
using SteamMarketViewer.Market;

namespace SteamMarketViewer
{
	public class SteamControl : INotifyPropertyChanged
	{
		private UserLogin _user;
		private bool _waitingForInput;
		private bool _needCaptcha = false;
		private bool _need2Fa = false;
		private bool _needEmail = false;
		private string _captchaLink = null;
		private string _captchaText;
		private string _guardCode;
		private string _emailCode;
		private bool _loginError;
		private string _errorMessage;
		private bool _loading;
		private bool _isLoggined;

		public ObservableCollection<Item> ItemList
		{
			get => _itemList;
			set => _itemList = value;
		}

		public ObservableCollection<Page> PageList
		{
			get => _pageList;
			set => _pageList = value;
		}

		private Game _choosedGame = Game.CSGO;
		private ObservableCollection<Item> _itemList = new ObservableCollection<Item>();
		private ObservableCollection<Page> _pageList = new ObservableCollection<Page>();
		private CookieContainer _loginCookies;
		private bool _processing = false;
		private int _pageSize;
		private int _pageCount;
		private int _currentPage;
		private int _searchResultsStart;
		private int _searchResultsEnd;
		private int _totalResults;

		public Game ChoosedGame
		{
			get => _choosedGame;
			set
			{
				_choosedGame = value;
				NotifyPropertyChanged(nameof(ChoosedGame));
			}
		}

		public UserLogin User
		{
			get => _user;
			private set => _user = value;
		}

		public async void AuthenticateAsync(string login, string pass)
		{
			User = new UserLogin(login, pass);
			LoginResult response = LoginResult.BadCredentials;
			await Task.Run(() =>
			{
				while (response != LoginResult.LoginOkay)
				{
					Processing = true;
					Loading = true;
					response = User.DoLogin();
					Loading = false;
					Console.WriteLine(response);
					switch (response)
					{
						case LoginResult.NeedEmail:
							NeedEmail = true;
							WaitingForInput = true;
							Console.WriteLine("Please enter email code: ");
							while (WaitingForInput)
							{
							}

							User.EmailCode = EmailCode;
							break;

						case LoginResult.NeedCaptcha:
							NeedCaptcha = true;
							WaitingForInput = true;
							Console.WriteLine("Please enter Captcha: ");
							while (WaitingForInput)
							{

							}

							User.CaptchaText = CaptchaText;
							break;

						case LoginResult.Need2FA:
							Need2FA = true;
							WaitingForInput = true;
							while (WaitingForInput)
							{
							}

							User.TwoFactorCode = GuardCode;
							break;
						case LoginResult.TooManyFailedLogins:
							ErrorMessage =
								"За последнее время в вашей сети произошло слишком много безуспешных попыток входа. Пожалуйста, подождите и повторите попытку позже.";
							LoginError = true;
							return;
							break;
						case LoginResult.BadCredentials:
							ErrorMessage = "Неверное имя аккаунта или пароль.";
							LoginError = true;
							return;
							break;
						case LoginResult.GeneralFailure:
							ErrorMessage = "Неверное имя аккаунта или пароль.";
							LoginError = true;
							return;
							break;
						case LoginResult.BadRSA:
							ErrorMessage = "Неверное имя аккаунта или пароль.";
							LoginError = true;
							return;
							break;
					}

					Processing = false;
				}

				if (response == LoginResult.LoginOkay)
				{
					IsLoggined = true;
				}
				else IsLoggined = false;
			});
            CheckLoggined();
        }

		public void CheckLoggined()
		{
			LoginCookies = Util.ReadCookiesFromDisk("cookies.txt");
			if (LoginCookies.Count > 0)
			{
				var response = SteamWeb.Request("https://steamcommunity.com/market/mylistings", "GET", dataString: null,
					LoginCookies);
				if (response != null)
				{
					var json = JsonConvert.DeserializeObject<SuccessClass>(response);
					IsLoggined = json.Success;
				}
			}
			else
			{
				IsLoggined = false;
			}
		}

		public void GeneratePageList()
		{
			PageList.Clear();
			if (PageCount > 8)
			{
				if (CurrentPage == 1)
				{
					PageList.Add(new Page()
					{
						NotCurrent = false,
						Number = "1"
					});
				}
				else
				{
					PageList.Add(new Page()
					{
						NotCurrent = true,
						Number = "1"
					});
				}
				if (CurrentPage + 4 >= PageCount)
				{
					PageList.Add(new Page()
					{
						NotCurrent = false,
						Number = "..."
					});
					for (int i = PageCount - 5; i <= PageCount; i++)
					{
						PageList.Add(new Page()
						{
							NotCurrent = i != CurrentPage,
							Number = i.ToString()
						});
					}
				}
				else if (CurrentPage > 4)
				{
					PageList.Add(new Page()
					{
						NotCurrent = false,
						Number = "..."
					});
					for (int i = CurrentPage - 2; i < CurrentPage + 3; i++)
					{
						if (i == CurrentPage)
						{
							PageList.Add(new Page()
							{
								NotCurrent = false,
								Number = i.ToString()
							});
							continue;
						}
						PageList.Add(new Page()
						{
							NotCurrent = true,
							Number = i.ToString()
						});
					}
					PageList.Add(new Page()
					{
						NotCurrent = false,
						Number = "..."
					});
					PageList.Add(new Page()
					{
						NotCurrent = true,
						Number = PageCount.ToString()
					});
				}
				else
				{
					for (int i = 2; i < 7; i++)
					{
						if (i == 1)
						{
							continue;
						}
						if (i == CurrentPage)
						{
							PageList.Add(new Page()
							{
								NotCurrent = false,
								Number = i.ToString()
							});
							continue;
						}
						PageList.Add(new Page()
						{
							NotCurrent = true,
							Number = i.ToString()
						});
					}
					PageList.Add(new Page()
					{
						NotCurrent = false,
						Number = "..."
					});
					PageList.Add(new Page()
					{
						NotCurrent = true,
						Number = PageCount.ToString()
					});
				}
			}
			else
			{
				for (int i = 1; i <= PageCount; i++)
				{
					PageList.Add(new Page()
					{
						NotCurrent = i != CurrentPage,
						Number = i.ToString()
					});
				}
			}
		}
		public CookieContainer LoginCookies
		{
			get => _loginCookies;
			set => _loginCookies = value;
		}

		public bool Processing
		{
			get => _processing;
			set => _processing = value;
		}

		public bool IsLoggined
		{
			get => _isLoggined;
			set
			{
				_isLoggined = value;
				NotifyPropertyChanged(nameof(IsLoggined));
			}
		}

		public bool Loading
		{
			get => _loading;
			set
			{
				_loading = value;
				NotifyPropertyChanged(nameof(Loading));
			}
		}

		public string ErrorMessage
		{
			get => _errorMessage;
			set
			{
				_errorMessage = value;
				NotifyPropertyChanged(nameof(ErrorMessage));
			}
		}

		public bool LoginError
		{
			get => _loginError;
			set
			{
				_loginError = value;
				NotifyPropertyChanged(nameof(LoginError));
			}
		}

		public bool WaitingForInput
		{
			get => _waitingForInput;
			set
			{
				_waitingForInput = value;
				NotifyPropertyChanged(nameof(WaitingForInput));
			}
		}

		public bool NeedCaptcha
		{
			get => _needCaptcha;
			set
			{
				_needCaptcha = value;
				NotifyPropertyChanged(nameof(NeedCaptcha));
			}
		}

		public bool Need2FA
		{
			get => _need2Fa;
			set
			{
				_need2Fa = value;
				NotifyPropertyChanged(nameof(Need2FA));
			}
		}

		public string GuardCode
		{
			get => _guardCode;
			set
			{
				_guardCode = value;
				NotifyPropertyChanged(nameof(GuardCode));
			}
		}

		public string EmailCode
		{
			get => _emailCode;
			set
			{
				_emailCode = value;
				NotifyPropertyChanged(nameof(EmailCode));
			}
		}

		public bool NeedEmail
		{
			get => _needEmail;
			set
			{
				_needEmail = value;
				NotifyPropertyChanged(nameof(NeedEmail));
			}
		}



		public string CaptchaLink
		{
			get => _captchaLink;
			set => _captchaLink = value;
		}

		public string CaptchaText
		{
			get => _captchaText;
			set => _captchaText = value;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		class SuccessClass
		{
			private bool _success = false;

			[JsonProperty("success")]
			public bool Success
			{
				get => _success;
				set => _success = value;
			}
		}

		public System.Windows.Input.ICommand RemoveItem
		{
			get => new RelayCommand(p => RemoveItemCommand((Item)p));
		}
		private void RemoveItemCommand(Item item)
		{
			ItemList.Remove(item);
		}
		public System.Windows.Input.ICommand AnalyzeItem
		{
			get => new RelayCommand(p => AnalyzeItemCommand((Item)p));
		}
		private void AnalyzeItemCommand(Item item)
		{
			Clipboard.SetText(item.Link);
			Analytics.Analyze(item);
		}
        public int PageSize
		{
			get => _pageSize;
			set
			{
				_pageSize = value;
				NotifyPropertyChanged(nameof(PageSize));
			}
		}
		public int PageCount
		{
			get => _pageCount;
			set
			{
				_pageCount = value;
				NotifyPropertyChanged(nameof(PageCount));
			}
		}
		public int CurrentPage
		{
			get => _currentPage;
			set
			{
				_currentPage = value;
				NotifyPropertyChanged(nameof(CurrentPage));
			}
		}
		public int SearchResultsStart
		{
			get => _searchResultsStart;
			set
			{
				_searchResultsStart = value;
				NotifyPropertyChanged(nameof(SearchResultsStart));
			}
		}
		public int SearchResultsEnd
		{
			get => _searchResultsEnd;
			set
			{
				_searchResultsEnd = value;
				NotifyPropertyChanged(nameof(SearchResultsEnd));
			}
		}

		private bool _canPrevPage;
		private bool _canNextPage;
		public bool CanPrevPage
		{
			get => _canPrevPage;
			set
			{
				_canPrevPage = value;
				NotifyPropertyChanged(nameof(CanPrevPage));
			}
		}
		public bool CanNextPage
		{
			get => _canNextPage;
			set
			{
				_canNextPage = value;
				NotifyPropertyChanged(nameof(CanNextPage));
			}
		}
		public int TotalResults
		{
			get => _totalResults;
			set
			{
				_totalResults = value;
				NotifyPropertyChanged(nameof(TotalResults));
			}
		}
	}

	public class Page
	{
		public string Number { get; set; }
		public bool NotCurrent { get; set; }

	}

	public class EnumBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.Equals(parameter);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((bool)value) ? parameter : Binding.DoNothing;
		}
	}
	public class RelayCommand : ICommand
	{
		private Action<object> execute;
		private Func<object, bool> canExecute;

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
		{
			this.execute = execute;
			this.canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return this.canExecute == null || this.canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			this.execute(parameter);
		}
	}
}