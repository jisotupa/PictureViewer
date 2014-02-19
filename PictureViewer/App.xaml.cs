using System;
using System.Reflection;
using System.Windows;

namespace PictureViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (e.Args.Length < 1)
            {
                Console.Error.WriteLine("Incorrect arguments, start with /s to show the screensaver.");
            }

            switch (e.Args[0].ToLower())
            {
                case "/s":
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    break;
                case "/c":
                case "/p":
                // nop

                default:
                    throw new NotSupportedException();
            }
        }

        private static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            var resourceName = String.Format("PictureViewer.{0}.dll", new AssemblyName(args.Name).Name);

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                Byte[] assemblyData = new Byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }
    }
}