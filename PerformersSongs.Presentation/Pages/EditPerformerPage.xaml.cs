
using MediatR;
using Microsoft.Maui.Storage;
using PerformersSongs.Application.Features.Performers.Commands;
using PerformersSongs.Application.Features.Performers.Queries;

namespace PerformersSongs.Presentation.Pages;

public partial class EditPerformerPage : ContentPage, IQueryAttributable
{
    private readonly IMediator _mediator;

    private int _performerId;
    private string? _photoPath;
    private FileResult? _selectedPhoto;
    private bool _isSaving;
    private bool _isLoading;

    public EditPerformerPage(IMediator mediator)
    {
        InitializeComponent();
        _mediator = mediator;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("performerId", out var value))
            _performerId = Convert.ToInt32(value);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_isLoading)
            await LoadPerformerAsync();
    }

    private async Task LoadPerformerAsync()
    {
        try
        {
            _isLoading = true;
            SetSavingState(true);

            var performer = await _mediator.Send(
                new GetPerformerByIdQuery(_performerId));

            if (performer == null)
            {
                await DisplayAlert(
                    "Ошибка",
                    "Исполнитель не найден.",
                    "OK");

                await Shell.Current.GoToAsync("..");
                return;
            }

            NameEntry.Text = performer.Name;
            CountryEntry.Text = performer.Country;
            GenreEntry.Text = performer.Genre;
            FoundationDatePicker.Date = performer.FoundationDate;

            _photoPath = performer.PhotoPath;

            if (!string.IsNullOrWhiteSpace(_photoPath)
                && File.Exists(_photoPath))
            {
                PerformerImage.Source = ImageSource.FromFile(_photoPath);
            }
            else
            {
                PerformerImage.Source = null;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Ошибка",
                $"Не удалось загрузить исполнителя: {ex.Message}",
                "OK");
        }
        finally
        {
            _isLoading = false;
            SetSavingState(false);
        }
    }

    private async void OnChoosePhotoClicked(
        object sender,
        EventArgs e)
    {
        if (_isSaving || _isLoading)
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

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (_isSaving || _isLoading)
            return;

        if (string.IsNullOrWhiteSpace(NameEntry.Text)
            || string.IsNullOrWhiteSpace(CountryEntry.Text)
            || string.IsNullOrWhiteSpace(GenreEntry.Text))
        {
            await DisplayAlert(
                "Ошибка",
                "Заполните все текстовые поля.",
                "OK");

            return;
        }

        _isSaving = true;
        SetSavingState(true);

        string? oldPhotoPath = _photoPath;

        try
        {
            if (_selectedPhoto != null)
            {
                string extension = Path.GetExtension(
                    _selectedPhoto.FileName);

                if (string.IsNullOrWhiteSpace(extension))
                    extension = ".jpg";

                string destinationPath = Path.Combine(
                    FileSystem.AppDataDirectory,
                    $"{_performerId}{extension}");

                using var sourceStream =
                    await _selectedPhoto.OpenReadAsync();

                using var destinationStream =
                    File.Create(destinationPath);

                await sourceStream.CopyToAsync(destinationStream);

                _photoPath = destinationPath;
            }

            await _mediator.Send(
                new UpdatePerformerCommand
                {
                    Id = _performerId,
                    Name = NameEntry.Text.Trim(),
                    Country = CountryEntry.Text.Trim(),
                    Genre = GenreEntry.Text.Trim(),
                    FoundationDate = FoundationDatePicker.Date,
                    PhotoPath = _photoPath
                });

            if (_selectedPhoto != null
                && !string.IsNullOrWhiteSpace(oldPhotoPath)
                && oldPhotoPath != _photoPath
                && File.Exists(oldPhotoPath))
            {
                File.Delete(oldPhotoPath);
            }

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
            SetSavingState(false);
        }
    }

    private void SetSavingState(bool isBusy)
    {
        SaveButton.IsEnabled = !isBusy;
        ChoosePhotoButton.IsEnabled = !isBusy;
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
