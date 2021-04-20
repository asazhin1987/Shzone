using SharedZone.Client.Applications;
using System;
using System.Windows;


namespace SharedZone.Client.Xaml
{
	/// <summary>
	/// Логика взаимодействия для SettingsForm.xaml
	/// </summary>
	public partial class SettingsForm : Window
	{
		public SettingsForm(SettingApplication model)
		{
			InitializeComponent();
			DataContext = model;
		}
	}
}
