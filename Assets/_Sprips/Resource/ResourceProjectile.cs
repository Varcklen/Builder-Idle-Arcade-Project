using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceProjectile : MonoBehaviour
{
    public ResourceSO Resource;
    private bool inMove;
    public bool IsMoveInterrupted { get; private set; }

    private const float ItemMoveSpeed = 8;
    private const float RangeToEndMovement = 0.1f;

    private Vector3 _newPosition;
    private Coroutine _coroutine;

    public void SetModel(ResourceObject resourceObject)
    {
        GameObject model = resourceObject.transform.Find("Model").gameObject;
        Instantiate(model, transform);
    }

    public void ResetResource()
    {
        transform.parent = null;
        if (inMove)
        {
            IsMoveInterrupted = true;
            StopCoroutine(_coroutine);
        }
        inMove = false;
        gameObject.SetActive(true);
    }

    public void MoveToPosition(Vector3 newPosition, Action actionAfterEnd = null)
    {
        if (inMove)
        {
            Debug.LogWarning("You can't move this bag resource. I'ts already in move.");
            return;
        }
        inMove = true;
        IsMoveInterrupted = false;
        _newPosition = newPosition;
        _coroutine = StartCoroutine(Move(actionAfterEnd));
    }

    IEnumerator Move(Action action)
    {
        Transform bagTransform = transform;
        var targetDirection = (_newPosition - bagTransform.position).normalized;
        while (inMove && Vector3.Distance(_newPosition, bagTransform.position) > RangeToEndMovement)
        {
            bagTransform.position += ItemMoveSpeed * Time.deltaTime * targetDirection;
            yield return null;
        }
        //Debug.Log("Movement ended");

        action?.Invoke();
    }
}
