using PerformersSongs.Application.DTOs;
using PerformersSongs.Presentation.Pages;
using PerformersSongs.Presentation.ViewModels;

namespace PerformersSongs.Presentation;

public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel _viewModel;

    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.LoadPerformersCommand.Execute(null);
    }

    private async void OnSongSelected(
        object sender,
        SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not SongDto song)
            return;

        if (sender is CollectionView collectionView)
            collectionView.SelectedItem = null;

        await Shell.Current.GoToAsync(
            $"{nameof(SongDetailsPage)}?songId={song.Id}");
    }

    private async void OnAddSongClicked(object sender, EventArgs e)
    {
        if (_viewModel.SelectedPerformer == null)
        {
            await DisplayAlert(
                "Внимание",
                "Сначала выберите исполнителя.",
                "OK");
            return;
        }

        await Shell.Current.GoToAsync(
            $"{nameof(AddSongPage)}?performerId={_viewModel.SelectedPerformer.Id}");
    }

    private async void OnAddPerformerClicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(AddPerformerPage));
    }

    private async void OnEditPerformerClicked(
        object sender,
        EventArgs e)
    {
        if (_viewModel.SelectedPerformer == null)
        {
            await DisplayAlert(
                "Внимание",
                "Сначала выберите исполнителя.",
                "OK");
            return;
        }

        await Shell.Current.GoToAsync(
            $"{nameof(EditPerformerPage)}?performerId={_viewModel.SelectedPerformer.Id}");
    }

    private async void OnDeletePerformerClicked(
        object sender,
        EventArgs e)
    {
        if (_viewModel.SelectedPerformer == null)
        {
            await DisplayAlert(
                "Внимание",
                "Сначала выберите исполнителя.",
                "OK");
            return;
        }

        string performerName = _viewModel.SelectedPerformer.Name;

        bool confirmed = await DisplayAlert(
            "Удаление исполнителя",
            $"Вы действительно хотите удалить исполнителя «{performerName}» и все его песни?",
            "Удалить",
            "Отмена");

        if (!confirmed)
            return;

        try
        {
           await _viewModel.DeletePerformerAsync();

            await DisplayAlert(
                "Успешно",
                "Исполнитель и его песни удалены.",
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ошибка",
                $"Не удалось удалить исполнителя: {ex.Message}",
                "OK");
        }
    }
}

