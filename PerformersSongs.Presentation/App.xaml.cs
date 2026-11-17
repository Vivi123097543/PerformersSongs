using PerformersSongs.Infrastructure.Data;

namespace PerformersSongs.Presentation
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        private readonly DatabaseInitializer _databaseInitializer;

        public App(DatabaseInitializer databaseInitializer)
        {
            InitializeComponent();
            _databaseInitializer = databaseInitializer;
        }

        protected override Window CreateWindow(
            IActivationState? activationState)
        {
            Task.Run(async () =>
            {
                await _databaseInitializer.InitializeAsync();
            }).GetAwaiter().GetResult();

            return new Window(new AppShell());
        }
    }
}
