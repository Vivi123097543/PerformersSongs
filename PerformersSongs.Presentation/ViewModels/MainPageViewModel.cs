using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MediatR;
using PerformersSongs.Application.DTOs;
using PerformersSongs.Application.Features.Performers.Commands;
using PerformersSongs.Application.Features.Performers.Queries;
using PerformersSongs.Application.Features.Songs.Queries;

namespace PerformersSongs.Presentation.ViewModels;

public class MainPageViewModel : INotifyPropertyChanged
{
    private readonly IMediator _mediator;

    private PerformerDto? _selectedPerformer;
    private bool _isBusy;

    public ObservableCollection<PerformerDto> Performers { get; } = new();

    public ObservableCollection<SongDto> Songs { get; } = new();

    public PerformerDto? SelectedPerformer
    {
        get => _selectedPerformer;
        set
        {
            if (_selectedPerformer == value)
                return;

            _selectedPerformer = value;
            OnPropertyChanged();

            LoadSongsCommand.Execute(null);
        }
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (_isBusy == value)
                return;

            _isBusy = value;
            OnPropertyChanged();
        }
    }

    public ICommand LoadPerformersCommand { get; }

    public ICommand LoadSongsCommand { get; }

    public ICommand DeletePerformerCommand { get; }

    public MainPageViewModel(IMediator mediator)
    {
        _mediator = mediator;

        LoadPerformersCommand = new Command(
            async () => await LoadPerformersAsync());

        LoadSongsCommand = new Command(
            async () => await LoadSongsAsync());

        DeletePerformerCommand = new Command(
            async () => await DeletePerformerAsync());
    }

    private async Task LoadPerformersAsync()
    {
        try
        {
            IsBusy = true;

            int? selectedId = SelectedPerformer?.Id;

            var performers = await _mediator.Send(
                new GetAllPerformersQuery());

            Performers.Clear();

            foreach (var performer in performers)
                Performers.Add(performer);

            if (selectedId.HasValue)
            {
                SelectedPerformer = Performers.FirstOrDefault(
                    p => p.Id == selectedId.Value);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Ошибка загрузки исполнителей: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task LoadSongsAsync()
    {
        if (SelectedPerformer == null)
        {
            Songs.Clear();
            return;
        }

        try
        {
            IsBusy = true;

            var songs = await _mediator.Send(
                new GetSongsByPerformerIdQuery(
                    SelectedPerformer.Id));

            Songs.Clear();

            foreach (var song in songs)
                Songs.Add(song);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Ошибка загрузки песен: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task DeletePerformerAsync()
    {
        if (SelectedPerformer == null)
            return;

        try
        {
            IsBusy = true;

            int performerId = SelectedPerformer.Id;

            await _mediator.Send(
                new DeletePerformerCommand(performerId));

            SelectedPerformer = null;
            Songs.Clear();

            await LoadPerformersAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Ошибка удаления исполнителя: {ex.Message}");

            throw;
        }
        finally
        {
            IsBusy = false;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
