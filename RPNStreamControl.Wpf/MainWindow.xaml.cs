using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace RPNStreamControl.Wpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		readonly string FILES_DIRECTORY = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory), "StreamFiles");
		readonly string[] FILENAMES = new string[4] { "datetime.txt", "title.txt", "subtitle.txt", "scroll.txt" };
		public MainWindow()
		{
			InitializeComponent();

			Loaded += MainWindow_Loaded;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			// Create .txt files to be read by XSplit/OBS
			CreateTxtFilesAsync();

			InitTimer();
		}

		private void InitTimer()
		{
			string path = System.IO.Path.Combine(FILES_DIRECTORY, FILENAMES[0]);
			if (!File.Exists(path)) return;

			// Initialize Timer
			var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
			dispatcherTimer.Tick += new EventHandler((s, e) => WriteToFile(path, DateTime.Now.ToString("MMM d, yyy hh:mm tt").ToUpperInvariant()));
			dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
			dispatcherTimer.Start();
		}

		private void WriteToFile(string path, string content)
		{
			using (FileStream fs = File.Create(path))
			{
				byte[] info = new UTF8Encoding(true).GetBytes(content);
				fs.Write(info, 0, info.Length);
			}
		}

		/// <summary>
		/// Create datetime.txt, title.txt, subtitle.txt, scroll.txt
		/// </summary>
		/// <returns></returns>
		private void CreateTxtFilesAsync()
		{
			if (!Directory.Exists(FILES_DIRECTORY))
			{
				Directory.CreateDirectory(FILES_DIRECTORY);
			}

			foreach (var filename in FILENAMES)
			{
				string path = System.IO.Path.Combine(FILES_DIRECTORY, filename);
				if (!File.Exists(path))
				{
					File.Create(path);
				}
			}
		}

		private void ScrollTextBox_Loaded(object sender, RoutedEventArgs e)
		{
			string path = System.IO.Path.Combine(FILES_DIRECTORY, FILENAMES[3]);
			
			LoadText(path, tbxScroll);
		}

		private void LoadText(string path, TextBox textbox)
		{
			if (!File.Exists(path)) return;

			// Read and Load saved text
			textbox.Text = File.ReadAllText(path);
		}

		private void UpdateFiles_Click(object sender, RoutedEventArgs e)
		{
			for (int i = 1; i < FILENAMES.Length; i++)
			{
				string path = System.IO.Path.Combine(FILES_DIRECTORY, FILENAMES[i]);

				if (i == 1)
				{
					WriteToFile(path, tbxTitle.Text);
				}

				if (i == 2)
				{
					WriteToFile(path, tbxSubTitle.Text);
				}

				if (i == 3)
				{
					WriteToFile(path, tbxScroll.Text);
				}
			}
		}

		private void TitleTextBox_Loaded(object sender, RoutedEventArgs e)
		{
			string path = System.IO.Path.Combine(FILES_DIRECTORY, FILENAMES[1]);

			LoadText(path, tbxTitle);
		}

		private void SubtitleTextBox_Loaded(object sender, RoutedEventArgs e)
		{
			string path = System.IO.Path.Combine(FILES_DIRECTORY, FILENAMES[2]);

			LoadText(path, tbxSubTitle);
		}
	}
}
