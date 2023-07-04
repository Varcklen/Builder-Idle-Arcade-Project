using UnityEngine;

/// <summary>
/// Простий скрипт для налагодження Canvas з камерою
/// </summary>
[RequireComponent(typeof(Canvas))]
public class CanvasCameraSetter : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
