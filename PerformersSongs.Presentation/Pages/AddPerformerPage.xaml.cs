using MediatR;
using Microsoft.Maui.Storage;
using PerformersSongs.Application.Features.Performers.Commands;

namespace PerformersSongs.Presentation.Pages;

public partial class AddPerformerPage : ContentPage
{
    private readonly IMediator _mediator;
    private FileResult? _selectedPhoto;
    private bool _isSaving;

    public AddPerformerPage(IMediator mediator)
    {
        InitializeComponent();

        _mediator = mediator;
        FoundationDatePicker.Date = DateTime.Today;
    }

    private async void OnChoosePhotoClicked(
        object sender,
        EventArgs e)
    {
        if (_isSaving)
            return;

        try
        {
            var photo = await FilePicker.Default.PickAsync(
                new PickOptions
                {
                    PickerTitle = "Выберите фотографию исполнителя",
                    FileTypes = FilePickerFileType.Images
                });

            if (photo == null)
                return;

            _selectedPhoto = photo;
            PerformerImage.Source = ImageSource.FromFile(photo.FullPath);
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ошибка",
                $"Не удалось выбрать фотографию: {ex.Message}",
                "OK");
        }
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        if (_isSaving)
            return;

        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlert("Ошибка", "Введите название исполнителя.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(CountryEntry.Text))
        {
            await DisplayAlert("Ошибка", "Введите страну.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(GenreEntry.Text))
        {
            await DisplayAlert("Ошибка", "Введите жанр.", "OK");
            return;
        }

        _isSaving = true;
        SetSavingState(true);

        try
        {
            int performerId = await _mediator.Send(
                new AddPerformerCommand
                {
                    Name = NameEntry.Text.Trim(),
                    Country = CountryEntry.Text.Trim(),
                    Genre = GenreEntry.Text.Trim(),
                    FoundationDate = FoundationDatePicker.Date,
                    PhotoPath = null
                });

            string? destinationPath = null;

            if (_selectedPhoto != null)
            {
                string extension = Path.GetExtension(
                    _selectedPhoto.FileName);

                if (string.IsNullOrWhiteSpace(extension))
                    extension = ".jpg";

                string fileName = $"{performerId}{extension}";

                destinationPath = Path.Combine(
                    FileSystem.AppDataDirectory,
                    fileName);

                using var sourceStream =
                    await _selectedPhoto.OpenReadAsync();

                using var destinationStream =
                    File.Create(destinationPath);

                await sourceStream.CopyToAsync(destinationStream);

                await _mediator.Send(
                    new UpdatePerformerCommand
                    {
                        Id = performerId,
                        Name = NameEntry.Text.Trim(),
                        Country = CountryEntry.Text.Trim(),
                        Genre = GenreEntry.Text.Trim(),
                        FoundationDate = FoundationDatePicker.Date,
                        PhotoPath = destinationPath
                    });
            }

            await DisplayAlert(
                "Успешно",
                "Исполнитель добавлен.",
                "OK");

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ошибка",
                $"Не удалось добавить исполнителя: {ex.Message}",
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
        ChoosePhotoButton.IsEnabled = !isSaving;
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


