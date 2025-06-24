using System;
using System.Collections.Generic;
using NLog;
using SnakeGame.Core.Dialogs;

namespace SnakeGame.Core.Systems;

public class DialogManager
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly Dictionary<string, Dialog> _dialogs = [];
    private readonly InputManager _inputs;
    private readonly List<Dialog> _openDialogs = [];

    public DialogManager(InputManager inputs)
    {
        _inputs = inputs;
    }
    
    public void AddDialog(Dialog dialog)
    {
        _dialogs[dialog.GetType().Name] = dialog;
    }

    public void Show<T>(params object[] args) where T : Dialog
    {
        _logger.Info($"Showing dialog {typeof(T).Name}");
        
        var dialog = GetDialogOrThrow<T>();
        dialog.IsVisible = true;
        dialog.OnShown(args);
        dialog.OnHideRequest += OnDialogHideRequest;
        _inputs.Remove();
        _inputs.ApplyTo(dialog);
        _openDialogs.Add(dialog);
    }

    public void Hide<T>() where T : Dialog
    {
        var dialog = GetDialogOrThrow<T>();
        Hide(dialog);
    }

    public void HideCurrent()
    {
        if (_openDialogs.Count > 0)
        {
            var dialog = _openDialogs[^1];
            Hide(dialog);
        }
    }

    private void Hide(Dialog dialog)
    {
        _logger.Info($"Hiding dialog {dialog.GetType().Name}");
        
        dialog.IsVisible = false;
        dialog.OnHideRequest -= OnDialogHideRequest;
        dialog.OnHide();
        
        _inputs.RemoveFrom(dialog);
        
        _openDialogs.Remove(dialog);
        
        if (_openDialogs.Count > 0)
            _inputs.ApplyTo(_openDialogs[^1]);
        else
            _inputs.Apply();
    }

    private void OnDialogHideRequest(Dialog dialog)
    {
        Hide(dialog);
    }

    private T GetDialogOrThrow<T>() where T : Dialog
    {
        var typeName = typeof(T).Name;
        
        if (!_dialogs.TryGetValue(typeName, out var dialog))
            throw new Exception($"Dialog of type {typeName} not found");

        return (T)dialog;
    }
}