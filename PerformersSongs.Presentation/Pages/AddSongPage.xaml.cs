
using MediatR;
using PerformersSongs.Application.Features.Songs.Commands;

namespace PerformersSongs.Presentation.Pages;

public partial class AddSongPage : ContentPage, IQueryAttributable
{
    private readonly IMediator _mediator;
    private int _performerId;
    private bool _isSaving;

    public AddSongPage(IMediator mediator)
    {
        InitializeComponent();
        _mediator = mediator;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("performerId", out var value))
        {
            _performerId = Convert.ToInt32(value);
        }
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        if (_isSaving)
            return;

        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            await DisplayAlert("Ошибка", "Введите название песни.", "OK");
            return;
        }

        if (!int.TryParse(ChartPositionEntry.Text, out int chartPosition)
            || chartPosition <= 0)
        {
            await DisplayAlert("Ошибка", "Введите корректное место в чарте.", "OK");
            return;
        }

        if (!int.TryParse(ReleaseYearEntry.Text, out int releaseYear)
            || releaseYear < 1900
            || releaseYear > DateTime.Today.Year)
        {
            await DisplayAlert(
                "Ошибка",
                $"Введите год выпуска от 1900 до {DateTime.Today.Year}.",
                "OK");
            return;
        }

        if (!TimeSpan.TryParse(DurationEntry.Text, out TimeSpan duration)
            || duration <= TimeSpan.Zero)
        {
            await DisplayAlert(
                "Ошибка",
                "Введите корректную продолжительность в формате чч:мм:сс.",
                "OK");
            return;
        }

        if (_performerId <= 0)
        {
            await DisplayAlert(
                "Ошибка",
                "Не удалось определить исполнителя.",
                "OK");
            return;
        }

        _isSaving = true;
        SetSavingState(true);

        try
        {
            await _mediator.Send(new AddSongCommand
            {
                Title = TitleEntry.Text.Trim(),
                ChartPosition = chartPosition,
                ReleaseYear = releaseYear,
                Duration = duration,
                PerformerId = _performerId
            });

            await DisplayAlert(
                "Успешно",
                "Песня добавлена.",
                "OK");

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ошибка",
                $"Не удалось добавить песню: {ex.Message}",
                "OK");
        }
        finally
        {
            _isSaving = false;
            SetSavingState(false);
        }
    }

    private void SetSavingState(bool isSaving)
    {
        AddButton.IsEnabled = !isSaving;
        CancelButton.IsEnabled = !isSaving;

        LoadingIndicator.IsVisible = isSaving;
        LoadingIndicator.IsRunning = isSaving;
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        if (_isSaving)
            return;

        await Shell.Current.GoToAsync("..");
    }
}
