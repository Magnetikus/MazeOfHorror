using UnityEngine;
using UnityEngine.UI;

public class SliderRowCol : MonoBehaviour
{
    public enum SliderType
    {
        onlyForSize,
        normal
    }
    public SliderType _type;

    public Slider _slider;
    public Text _text;

    [HideInInspector]
    public int _size;

    void Start()
    {
        switch (_type)
        {
            case SliderType.onlyForSize:
                _slider.value = 15;
                break;
            case SliderType.normal:
                _slider.value = 1;
                break;
        }

    }

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(SliderValueChanged);
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(SliderValueChanged);
    }

    private void SliderValueChanged(float value)
    {
        switch (_type)
        {
            case SliderType.onlyForSize:
                int a = (int)value;
                if (a % 2 == 0) a++;
                _size = a;
                break;
            case SliderType.normal:
                _size = (int)value;
                break;
        }
        _text.text = $"{_size}";

    }

}
