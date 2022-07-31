using System.IO;
using System.Text.Json;
using EmployeesManagement.Business.IoC;
using EmployeesManagement.Common.Configurations;
using EmployeesManagement.UI.IoC;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace EmployeesManagement.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private IServiceProvider ServiceProvider { get; set; }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var configuration = ConfigurationGetter.GetConfiguration();
            
            var serviceCollection = new ServiceCollection();
            ServiceProvider = serviceCollection
                .RegisterServices()
                .RegisterViews()
                .RegisterViewModels()
                .RegisterDbContext(configuration)
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder
                        .ClearProviders()
                        .SetMinimumLevel(LogLevel.Trace)
                        .AddNLog(configuration);
                })
                .BuildServiceProvider();

            ServiceProvider.AddDatabaseMigration();

            ShowMainForm();
        }

        private void ShowMainForm()
        {
            var logger = ServiceProvider.GetService<ILogger<App>>() 
                         ?? throw new ArgumentNullException(nameof(ILogger<App>));
            
            logger.LogInformation("Start App ...");
            
            var mainForm = ServiceProvider.GetService<MainView>();
            if (mainForm == null)
            {
                logger.LogCritical("View '{0}' not found", nameof(MainView));
                
                throw new NullReferenceException(nameof(MainView));
            }
            
            mainForm.Show();

            var result = Login(mainForm);
            if (!result.HasValue || !result.Value)
            {
                mainForm.Close();
            }
        }

        private bool? Login(Window owner)
        {
            var loginView = ServiceProvider.GetService<LoginView>();
            if (loginView == null)
            {
                throw new NullReferenceException(nameof(LoginView));
            }
            
            loginView.Owner = owner;

            return loginView.ShowDialog();
        }
        
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs ex)
        {
            if (ex.ExceptionObject is not Exception exception)
            {
                return;
            }

            var message = $"{DateTime.Now}: UnhandledException: {Environment.NewLine}";

            message += JsonSerializer.Serialize(ex) + Environment.CommandLine;
            
            File.AppendAllText("logs.log", message);
        }
    }
}