using UnityEngine;

/// <summary>
/// ������� ������ ��� ������������ Canvas � �������
/// </summary>
[RequireComponent(typeof(Canvas))]
public class CanvasCameraSetter : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
