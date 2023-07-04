using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Класи, якi наслiдуються вiд ResourceMover можуть використовувати перемiщення
/// </summary>
public class ResourceMover : MonoBehaviour
{
    public ResourceSO Resource;
    private bool inMove;
    public bool IsMoveInterrupted { get; private set; }

    private const float ItemMoveSpeed = 8;
    private const float RangeToEndMovement = 0.1f;

    private Vector3 _newPosition;
    private Coroutine _coroutine;

    public void ResetMovement()
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

    public void MoveToPosition(Vector3 newPosition, bool destroyAtEnd = false, Action actionAfterMove = null)
    {
        if (inMove)
        {
            Debug.LogWarning("You can't move this bag resource. It's already in move.");
            return;
        }
        inMove = true;
        IsMoveInterrupted = false;
        _newPosition = newPosition;
        _coroutine = StartCoroutine(Move(destroyAtEnd, actionAfterMove));
    }

    IEnumerator Move(bool destroyAtEnd, Action action)
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
        if (destroyAtEnd)
        {
            Destroy(gameObject);
        }
    }
}
