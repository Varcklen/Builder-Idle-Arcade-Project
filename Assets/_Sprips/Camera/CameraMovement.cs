using UnityEngine;

/// <summary>
/// ���������� ���i�� ��� �����i����� ������ �� �������
/// </summary>
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private Vector3 _difference;

    private void Start()
    {
        _difference = transform.position - _target.position;
    }

    private void Update()
    {
        transform.position = _target.position + _difference;
    }
}
