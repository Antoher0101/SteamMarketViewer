using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SteamMarketViewer
{
	public partial class Calculator : Window
	{
		private bool setting = false;

		private double comission = 1.15;
		string CalcBuyPrice(string str)
		{
			double sPrice;
			if (double.TryParse(str, out sPrice))
			{
				double p = Math.Round((sPrice / comission) + 0.005, 2);
				return p.ToString();
			}
			return this.BuyPrice.Text;
		}
		string CalcSellPrice(string str)
		{
			double sPrice;
			if (double.TryParse(str, out sPrice))
			{
				double p = Math.Round((sPrice * comission) + 0.005, 2);
				return p.ToString();
			}
			return this.SellPrice.Text;
		}
		public Calculator()
		{
			InitializeComponent();
			this.DataContext = this;
		}

		private void SellPrice_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (SellPrice.IsFocused && !setting)
			{
				this.BuyPrice.Text = CalcBuyPrice(this.SellPrice.Text);
			}

		}
		private void BuyPrice_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (BuyPrice.IsFocused && !setting)
			{
				this.SellPrice.Text = CalcSellPrice(this.BuyPrice.Text);
			}
		}

		private void SetСoefficient_Click(object sender, RoutedEventArgs e)
		{
			if (!setting)
			{
				setting = true;
				SetСoefficient.Content = "Готово";
			}
			else
			{
				double b;
				double s;
				double.TryParse(BuyPrice.Text, out b);
				double.TryParse(SellPrice.Text, out s);
				comission = s / b;
				SetСoefficient.Content = "Установить коэффициент";
				setting = false;
			}
		}
	}
}
