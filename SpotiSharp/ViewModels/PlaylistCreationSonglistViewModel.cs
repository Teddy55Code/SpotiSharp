﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using SpotiSharp.Models;

namespace SpotiSharp.ViewModels;

public delegate void PlaylistIsFiltered();

public class PlaylistCreationSonglistViewModel : BaseViewModel
{
    public static event PlaylistIsFiltered OnPlalistIsFiltered;

    private ObservableCollection<object> _selectedItems = new ObservableCollection<object>();
    
    public ObservableCollection<object> SelectedItems
    {
        get { return _selectedItems; }
        set { SetProperty(ref _selectedItems, value); }
    } 
    
    
    private List<SongEditable> _songs = new List<SongEditable>();

    public List<SongEditable> Songs
    {
        get { return _songs; }
        set { SetProperty(ref _songs, value); }
    }

    public PlaylistCreationSonglistViewModel()
    {
        RemoveSongs = new Command(RemoveSongsHandler);
        
        PlaylistCreatorPageModel.OnSongListChange += RefreshSongs;
    }

    private async void RefreshSongs()
    {
        Songs = (await PlaylistCreatorPageModel.GetFilteredSongs()).Select((fullTrack, index) =>
        {
            return new SongEditable(index, fullTrack);
        }).ToList();
        OnPlalistIsFiltered?.Invoke();
    }
    
    private void RemoveSongsHandler()
    {
        
        PlaylistCreatorPageModel.RemoveSongsByIndex(SelectedItems.Select(si =>
        {
            if (si is SongEditable songEditable) return songEditable.Index;
            return 0;
        }).ToList());
    }

    public ICommand RemoveSongs { private set; get; }
}