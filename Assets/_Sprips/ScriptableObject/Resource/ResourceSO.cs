using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Custom/Resource")]
public class ResourceSO : ScriptableObject
{
    [SerializeField] private string _name;
    public string Name => _name;
    [SerializeField] private Sprite _sprite;
    public Sprite Sprite => _sprite;
}
