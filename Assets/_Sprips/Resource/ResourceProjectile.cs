using UnityEngine;

/// <summary>
/// "Dummy" ������, ���� ��������������� ��� �i��������� �i���������� (���������, � i�������i, ��� � ��������i �� ������i)
/// </summary>
public class ResourceProjectile : ResourceMover
{
    public void SetModel(ResourceObject resourceObject)
    {
        GameObject model = resourceObject.transform.Find("Model").gameObject;
        Instantiate(model, transform);
    }
}
