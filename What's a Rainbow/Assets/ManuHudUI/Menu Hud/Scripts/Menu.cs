using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using JetBrains.Annotations;

[System.Serializable]
public class ClickChange : UnityEvent<ClickEvent> { }

[System.Serializable]
public class BoolChange : UnityEvent<bool> { }

[System.Serializable]
public class VolumeSlider
{
    [SerializeField] string _sliderName = "";
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] string _volumeName = "";
    [SerializeField] ClickChange _clickEvent;
    private Slider _slider;

    public void Activate(UIDocument document)
    {
        if (_slider == null)
        {
            _slider = document.rootVisualElement.Q<Slider>(_sliderName);
        }

        float startVol = 0;
        _audioMixer.GetFloat(_volumeName, out startVol);
        _slider.value = startVol + 80;
        _slider.RegisterCallback<ChangeEvent<float>>(evt => _audioMixer.SetFloat(_volumeName, evt.newValue - 80));
        _slider.RegisterCallback<ClickEvent>(_clickEvent.Invoke);
    }

    public void Inactivate(UIDocument document)
    {
        _slider.UnregisterCallback<ChangeEvent<float>>(evt => _audioMixer.SetFloat(_volumeName, evt.newValue - 80));
    }
}

[System.Serializable]
public class ToggleEvent
{
    [SerializeField] string _toggleName = "";
    [SerializeField] BoolChange _boolEvent;
    private Toggle _toggle;

    public void Activate(UIDocument document)
    {
        if (_toggle == null)
        {
            _toggle = document.rootVisualElement.Q<Toggle>(_toggleName);
        }

        _toggle.RegisterCallback<ChangeEvent<bool>>(evt => _boolEvent.Invoke(evt.newValue));
    }

    public void Inactivate(UIDocument document)
    {
        _toggle.UnregisterCallback<ChangeEvent<bool>>(evt => _boolEvent.Invoke(evt.newValue));
    }
}

[System.Serializable]
public class ButtonEvent
{
    [SerializeField] string _buttonName = "";
    [SerializeField] UnityEvent _unityEvent;
    [SerializeField] UnityEvent _hoverEnterEvent;
    [SerializeField] UnityEvent _hoverExitEvent;
    [SerializeField] Texture2D _hoverCursor;
    private Button _button;

    public void Activate(UIDocument document)
    {
        if (_button == null)
        {
            _button = document.rootVisualElement.Q<Button>(_buttonName);
        }

        _button.clicked += _unityEvent.Invoke;
        _button.RegisterCallback<PointerEnterEvent>(evt =>
        {
            _hoverEnterEvent.Invoke();
            UnityEngine.Cursor.SetCursor(_hoverCursor, Vector2.zero, CursorMode.Auto);
        });

        _button.RegisterCallback<PointerLeaveEvent>(evt =>
        {
            _hoverExitEvent.Invoke();
            UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        });
    }

    public void Inactivate(UIDocument document)
    {
        _button.clicked -= _unityEvent.Invoke;
    }
}

public class Menu : MonoBehaviour
{
    [SerializeField] UIDocument _document;
    [SerializeField] List<ButtonEvent> _buttonEvents;
    [SerializeField] List<VolumeSlider> _volumeSliders;
    [SerializeField] List<ToggleEvent> _toggleEvents;
    [SerializeField] Texture2D _defaultCursor;
    private VisualElement _curMenu = null;

    private void Start()
    {
        UnityEngine.Cursor.SetCursor(_defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SwitchMenu(string menuName)
    {
        if (_curMenu != null)
        {
            _curMenu.style.display = DisplayStyle.None;
        }
        _curMenu = _document.rootVisualElement.Q<VisualElement>(menuName);
        _curMenu.style.display = DisplayStyle.Flex;
    }

    public void LoadScene(int buildIndex)
    {
        UnityEngine.Cursor.SetCursor(_defaultCursor, Vector2.zero, CursorMode.Auto);
        SceneManager.LoadScene(buildIndex);
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnEnable()
    {
        _buttonEvents.ForEach(button => button.Activate(_document));
        _volumeSliders.ForEach(slider => slider.Activate(_document));
        _toggleEvents.ForEach(toggle => toggle.Activate(_document));
    }

    private void OnDisable()
    {
        _buttonEvents.ForEach(button => button.Inactivate(_document));
        _volumeSliders.ForEach(slider => slider.Inactivate(_document));
        _toggleEvents.ForEach(toggle => toggle.Inactivate(_document));
    }
}

public class ApplyCustomFont : MonoBehaviour
{
    [SerializeField] private UIDocument _document;
    private Label _textLabel;

    private void Start()
    {
        var root = _document.rootVisualElement;
        _textLabel = root.Q<Label>("Pause");

        Font customFont = Resources.Load<Font>("Fonts/MyCustomFont");
        if (customFont != null)
        {
            _textLabel.style.unityFont = customFont;
            _textLabel.style.fontSize = 24;
        }
        else
        {
            Debug.LogError("Font not found in Resources!");
        }
    }
}
