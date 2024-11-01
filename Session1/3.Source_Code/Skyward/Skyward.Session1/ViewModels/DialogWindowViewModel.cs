using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Skyward.Session1.ViewModels;

public partial class DialogWindowViewModel : ObservableObject
{
    [ObservableProperty] private string? _message;
    [ObservableProperty] private string? _title;
    [ObservableProperty] private WindowIcon? _icon;
}