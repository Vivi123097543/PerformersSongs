
using MediatR;
using PerformersSongs.Application.Features.Songs.Commands;
using PerformersSongs.Application.Features.Songs.Queries;

namespace PerformersSongs.Presentation.Pages;

public partial class EditSongPage : ContentPage, IQueryAttributable
{
    private readonly IMediator _mediator;

    private int _songId;
    private int _performerId;
    private bool _isSaving;
    private bool _isLoading;

    public EditSongPage(IMediator mediator)
    {
        InitializeComponent();
        _mediator = mediator;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("songId", out var value))
        {
            _songId = Convert.ToInt32(value);
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_isLoading)
            await LoadSongAsync();
    }

    private async Task LoadSongAsync()
    {
        try
        {
            _isLoading = true;
            SetBusyState(true);

            var song = await _mediator.Send(
                new GetSongByIdQuery(_songId));

            if (song == null)
            {
                await DisplayAlert(
                    "Ошибка",
                    "Песня не найдена.",
                    "OK");

                await Shell.Current.GoToAsync("..");
                return;
            }

            _performerId = song.PerformerId;

            TitleEntry.Text = song.Title;
            ChartPositionEntry.Text = song.ChartPosition.ToString();
            ReleaseYearEntry.Text = song.ReleaseYear.ToString();
            DurationEntry.Text = song.Duration.ToString(@"hh\:mm\:ss");
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ошибка",
                $"Не удалось загрузить песню: {ex.Message}",
                "OK");
        }
        finally
        {
            _isLoading = false;
            SetBusyState(false);
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (_isSaving || _isLoading)
            return;

        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            await DisplayAlert(
                "Ошибка",
                "Введите название песни.",
                "OK");
            return;
        }

        if (!int.TryParse(ChartPositionEntry.Text, out int chartPosition)
            || chartPosition <= 0)
        {
            await DisplayAlert(
                "Ошибка",
                "Введите корректное место в чарте.",
                "OK");
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

        if (_songId <= 0 || _performerId <= 0)
        {
            await DisplayAlert(
                "Ошибка",
                "Не удалось определить песню или исполнителя.",
                "OK");
            return;
        }

        _isSaving = true;
        SetBusyState(true);

        try
        {
            await _mediator.Send(new UpdateSongCommand
            {
                Id = _songId,
                Title = TitleEntry.Text.Trim(),
                ChartPosition = chartPosition,
                ReleaseYear = releaseYear,
                Duration = duration,
                PerformerId = _performerId
            });

            await DisplayAlert(
                "Успешно",
                "Изменения сохранены.",
                "OK");

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ошибка",
                $"Не удалось сохранить изменения: {ex.Message}",
                "OK");
        }
        finally
        {
            _isSaving = false;
            SetBusyState(false);
        }
    }

    private void SetBusyState(bool isBusy)
    {
        SaveButton.IsEnabled = !isBusy;
        CancelButton.IsEnabled = !isBusy;

        LoadingIndicator.IsVisible = isBusy;
        LoadingIndicator.IsRunning = isBusy;
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        if (_isSaving || _isLoading)
            return;

        await Shell.Current.GoToAsync("..");
    }
}
