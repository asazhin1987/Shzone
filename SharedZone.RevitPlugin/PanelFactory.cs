using System;
using System.IO;
using Autodesk.Revit.UI;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace SharedZone.RevitPlugin
{

	public class PanelFactory
	{
		internal string RevitVersion { get; }
		internal ToggleButton TurnOnOffButton { get;}

		public static PanelFactory Instance { get; private set; }

		internal SharedZoneButton SettingButton { get; }

		/// <summary>
		/// Создание панели Revit
		/// </summary>
		/// <param name="app"></param>
		public PanelFactory(UIControlledApplication app)
		{
			try
			{
				//TaskDialog.Show("SharedZone", "BEER 2");
				RevitVersion = app.ControlledApplication.VersionNumber;
				Instance = this;
				string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;


				RibbonPanel ribbonPanel = CreateRibbonPanel(title: "BIM Manager");

				string location = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"{assemblyName}.dll");

				//SettingsButton
				BitmapImage imgsetting = new BitmapImage(new Uri($"pack://application:,,,/{assemblyName};component/Content/settings-16.png"));
				var settingBtn = ribbonPanel.AddItem(new PushButtonData("SharedZoneSettingBtn", "BIM Manager", location, $"{assemblyName}.Commands.ShowSettingCommand")
				{
					Image = imgsetting,
					ToolTip = "",
					LargeImage = imgsetting,
					AvailabilityClassName = $"{assemblyName}.{nameof(Availability)}"
				});
				SettingButton = new SharedZoneButton(settingBtn);

				//TurnOnButton
				BitmapImage imgTurnOn = new BitmapImage(new Uri($"pack://application:,,,/{assemblyName};component/Content/high-connection-16.png"));
				var buttonTon = ribbonPanel.AddItem(new PushButtonData("SharedZoneTurnOnBtn", "On", location, $"{assemblyName}.Commands.TurnOnCommand")
				{
					Image = imgTurnOn,
					ToolTip = "",
					LargeImage = imgTurnOn,
					AvailabilityClassName = $"{assemblyName}.{nameof(Availability)}"
				});

				//TurnOffButton
				BitmapImage imgTurnOff = new BitmapImage(new Uri($"pack://application:,,,/{assemblyName};component/Content/no-connection-16.png"));
				var buttonToff = ribbonPanel.AddItem(new PushButtonData("SharedZoneTurnOffBtn", "Off", location, $"{assemblyName}.Commands.TurnOffCommand")
				{
					Image = imgTurnOff,
					ToolTip = "",
					LargeImage = imgTurnOff,
					AvailabilityClassName = $"{assemblyName}.{nameof(Availability)}"
				});

				TurnOnOffButton = new ToggleButton(new SharedZoneButton(buttonTon), new SharedZoneButton(buttonToff));
				TurnOnOffButton.OnToggleChanged += ToggleButtonInstance_OnToggleChanged;
				TurnOnOffButton.ToggleButton_Off();
			}
			catch (Exception ex)
			{
				throw ex;
			}

			RibbonPanel CreateRibbonPanel(string tabName = "BIMACAD", string title = "")
			{
				try
				{
					app.CreateRibbonTab(tabName);
				}
				catch { }

				return app.CreateRibbonPanel(tabName, title);
			}
		}

		private void ToggleButtonInstance_OnToggleChanged(object sender, EventArgs e)
		{
			if (sender is ToggleButton toggle)
				SettingButton.SedEnable(toggle.ToggleButtonTurnedOn);
		}

		/// <summary>
		/// Класс управления кнопкой меню
		/// </summary>
		internal class SharedZoneButton
		{
			private RibbonItem Button { get; }

			internal SharedZoneButton(RibbonItem item)
			{
				Button = item;
			}

			internal void Hide()
			{
				Button.Visible = false;
			}

			internal void Show()
			{
				Button.Visible = true;
			}

			internal void Disable()
			{
				Button.Enabled = false;
			}

			internal void Enable()
			{
				Button.Enabled = true;
			}

			internal void SetVisible(bool visible)
			{
				if (visible)
					Show();
				else
					Hide();
			}

			internal void SedEnable(bool enable)
			{
				if (enable)
					Enable();
				else
					Disable();
			}
		}

		/// <summary>
		/// Класс управления кнопками вкл/выкл
		/// </summary>
		internal class ToggleButton
		{
			private SharedZoneButton TurnOnButton { get; }
			private SharedZoneButton TurnOffButton { get; }

			internal event EventHandler<EventArgs> OnToggleChanged;

			private bool toggleButtonTurnedOn;
			internal bool ToggleButtonTurnedOn
			{
				get { return toggleButtonTurnedOn; }
				set
				{
					toggleButtonTurnedOn = value;
					ToggleChanged();
				}
			}

			internal ToggleButton(SharedZoneButton turnOnButton, SharedZoneButton turnOffButton, bool turnedOn = false)
			{
				TurnOnButton = turnOnButton;
				TurnOffButton = turnOffButton;
				ToggleButtonTurnedOn = turnedOn;
			}

			private void ToggleChanged()
			{
				TurnOnButton.SetVisible(!ToggleButtonTurnedOn);
				TurnOffButton.SetVisible(ToggleButtonTurnedOn);
				OnToggleChanged?.Invoke(this, new EventArgs());
			}

			internal void Toggle()
			{
				ToggleButtonTurnedOn = !ToggleButtonTurnedOn;
			}

			internal void ToggleButton_On()
			{
				ToggleButtonTurnedOn = true;
			}

			internal void ToggleButton_Off()
			{
				ToggleButtonTurnedOn = false;
			}
		}

	}

	
}
