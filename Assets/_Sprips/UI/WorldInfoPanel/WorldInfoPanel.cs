using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI-елемент, який відображає кiлькiсть ресурсiв у вибраному мiсцi
/// </summary>
public class WorldInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject _child;
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private Image _image;

    private void Start()
    {
        _child.SetActive(false);
    }

    public void SetValue(int newValue)
    {
        _valueText.text = newValue.ToString();
    }

    public void SetImage(Sprite sprite)
    {
        _image.sprite = sprite;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Player" || _child.activeSelf) return;
        _child.SetActive(true);
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag != "Player" || _child.activeSelf == false) return;
        _child.SetActive(false);
    }
}
