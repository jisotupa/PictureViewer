using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace PictureViewer
{
    public partial class MainWindow : Window
    {
        const string SettingsFilename = "settings.json";

        private readonly PicturesViewModel mPictures;
        private readonly DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            var files = GetPictureFileNames();

            if (!files.Any())
            {
                Close();
            }

            mPictures = new PicturesViewModel(files.RandomOrder());
        }

        private IEnumerable<string> GetPictureFileNames()
        {
            // TODO get from settings
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (!Directory.Exists(path))
            {
                return Enumerable.Empty<string>();
            }

            var files = Directory
                .EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                .Select(x => new FileInfo(x))
                .Where(x => IsSupported(x.Extension))
                .Select(x => x.FullName)
                .ToArray();

            return files;
        }

        private static bool IsSupported(string extension)
        {
            var lowered = extension.ToLower();

            return lowered == ".jpeg" || lowered == ".jpg" || lowered == ".png" || lowered == ".gif";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSettings();

            sp.DataContext = mPictures;

            timer.Tick += (s, evt) => Next();
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Start();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.Left:
                    Previous();
                    break;
                case Key.Right:
                    Next();
                    break;
                case Key.Up:
                    RotateClockwise();
                    break;
                case Key.Down:
                    RotateCounterClockwise();
                    break;
            }
        }

        private void LoadSettings()
        {
            if (File.Exists(GetSettingsFile()))
            {
                var json = File.ReadAllText(GetSettingsFile());

                mPictures.ImportJson(json);
            }
        }

        private void Next()
        {
            RestartTimer();
            mPictures.Next();
        }

        private void RotateClockwise()
        {
            RestartTimer();

            mPictures.Current.RotateClockwise();

            SaveSettings();
        }

        private void RotateCounterClockwise()
        {
            RestartTimer();

            mPictures.Current.RotateCounterClockwise();

            SaveSettings();
        }

        private void Previous()
        {
            RestartTimer();
            mPictures.Previous();
        }

        private static string GetFolder()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Sammaltupa", "PictureViewer");
        }

        private static string GetSettingsFile()
        {
            return Path.Combine(GetFolder(), SettingsFilename);
        }

        private static void EnsureFolderExists()
        {
            string folder = GetFolder();

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        private void RestartTimer()
        {
            timer.Stop();
            timer.Start();
        }

        private void SaveSettings()
        {
            EnsureFolderExists();

            File.WriteAllText(GetSettingsFile(), mPictures.SerializeToJson());
        }
    }
}