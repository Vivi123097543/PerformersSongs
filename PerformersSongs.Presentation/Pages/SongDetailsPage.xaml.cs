
using MediatR;
using PerformersSongs.Application.DTOs;
using PerformersSongs.Application.Features.Songs.Commands;
using PerformersSongs.Application.Features.Songs.Queries;

namespace PerformersSongs.Presentation.Pages;

public partial class SongDetailsPage : ContentPage, IQueryAttributable
{
    private readonly IMediator _mediator;

    private int _songId;
    private bool _isLoading;
    private bool _isDeleting;

    public SongDetailsPage(IMediator mediator)
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
        if (_songId <= 0)
        {
            await DisplayAlert(
                "Ошибка",
                "Не удалось определить песню.",
                "OK");

            await Shell.Current.GoToAsync("..");
            return;
        }

        try
        {
            _isLoading = true;
            SetBusyState(true);

            SongDto? song = await _mediator.Send(
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

            TitleLabel.Text = song.Title;
            ChartPositionLabel.Text = song.ChartPosition.ToString();
            ReleaseYearLabel.Text = song.ReleaseYear.ToString();
            DurationLabel.Text = song.Duration.ToString(@"hh\:mm\:ss");
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

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (_isLoading || _isDeleting)
            return;

        await Shell.Current.GoToAsync(
            $"{nameof(EditSongPage)}?songId={_songId}");
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (_isLoading || _isDeleting)
            return;

        bool confirmed = await DisplayAlert(
            "Удаление песни",
            "Вы действительно хотите удалить эту песню?",
            "Удалить",
            "Отмена");

        if (!confirmed)
            return;

        _isDeleting = true;
        SetBusyState(true);

        try
        {
            await _mediator.Send(new DeleteSongCommand(_songId));

            await DisplayAlert(
                "Успешно",
                "Песня удалена.",
                "OK");

            await Shell.Current.GoToAsync("//MainPage");
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ошибка",
                $"Не удалось удалить песню: {ex.Message}",
                "OK");
        }
        finally
        {
            _isDeleting = false;
            SetBusyState(false);
        }
    }

    private void SetBusyState(bool isBusy)
    {
        EditButton.IsEnabled = !isBusy;
        DeleteButton.IsEnabled = !isBusy;

        LoadingIndicator.IsVisible = isBusy;
        LoadingIndicator.IsRunning = isBusy;
    }
}


