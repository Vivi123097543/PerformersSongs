using PerformersSongs.Presentation.Pages;

namespace PerformersSongs.Presentation
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(
                nameof(SongDetailsPage),
                typeof(SongDetailsPage));

            Routing.RegisterRoute(
                nameof(EditSongPage),
                typeof(EditSongPage));

            Routing.RegisterRoute(
                 nameof(AddSongPage),
                 typeof(AddSongPage));

            Routing.RegisterRoute(
                 nameof(AddPerformerPage),
                 typeof(AddPerformerPage));

            Routing.RegisterRoute(
                 nameof(EditPerformerPage),
                 typeof(EditPerformerPage));
        }
    }
}
