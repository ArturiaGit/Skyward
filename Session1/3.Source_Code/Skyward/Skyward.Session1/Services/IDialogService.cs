using System.Threading.Tasks;
using Avalonia.Controls;

namespace Skyward.Session1.Services;

public interface IDialogService
{
    /// <summary>
    /// 显示对话框
    /// </summary>
    /// <param name="message">显示的信息</param>
    /// <param name="ownerWindow">对话框的父窗体</param>
    Task ShowDialogAsync(string message,Window ownerWindow);
}