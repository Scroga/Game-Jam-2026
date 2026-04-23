using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
public class SlotController : MonoBehaviour
{
    [SerializeField]
    private float _initialPosition;

    private float _positionStep = 4.85f;

    private const float _startPosition = 18.2f;
    private const float _endPosition = -20.5f;

    private const float _timeInterval = 0.01f; // time between shifts of row positions
    private const float _rotationSpeed = 1.2f;

    private float rotatingDuration = 955.0f;
    private float timeInterval;

    public bool rowStopped { get; private set; }
    public SlotType stoppedSlot { get; private set; }
    void Start()
    {
        rowStopped = true;
        _initialPosition = UnityEngine.Random.Range(_endPosition, _startPosition);
        SetYPos(_initialPosition);
    }

    private void SetYPos(float value)
    {
        var pos = transform.localPosition;
        pos.y = value;
        transform.localPosition = pos;
    }

    public void StartRotating()
    {
        if (rowStopped)
        {
            StartCoroutine(Rotate());
            //StartCoroutine(RotateDebug());
            rowStopped = false;
        }
    }

    public void SetInitialPosition()
    {
        SetYPos(_initialPosition);
    }

    private IEnumerator Rotate()
    {
        int randomSpeed = UnityEngine.Random.Range(6, 15);
        int randomSpeedFactor = UnityEngine.Random.Range(5, 9);

        float rotatinsSpeed = _rotationSpeed * (randomSpeed / 10.0f);
        while (rotatinsSpeed > 0.05f)
        {
            if (transform.localPosition.y <= _endPosition)
            {
                SetYPos(_startPosition);
            }
            SetYPos(transform.localPosition.y - rotatinsSpeed);
            rotatinsSpeed *= 1 - (randomSpeedFactor / rotatingDuration);

            yield return new WaitForSecondsRealtime(_timeInterval);
        }
        SetCurrentSlot();
        rowStopped = true;
    }

    private IEnumerator RotateDebug()
    {
        float timeInterval = 0.4f;

        while (true)
        {
            if (transform.localPosition.y <= _endPosition)
            {
                SetYPos(_startPosition);
            }
            SetYPos(transform.localPosition.y - _positionStep);
            Debug.Log(transform.localPosition.y);
            yield return new WaitForSecondsRealtime(timeInterval);
        }
        SetCurrentSlot();
        rowStopped = true;
    }

    private bool IsInRange(float rangeCenter, float value)
    {
        float range = _positionStep / 2.0f;
        return (rangeCenter + range) >= value && (rangeCenter - range) < value;
    }

    private void SetCurrentSlot()
    {
        float heartPosition = -22.0f;

        if (IsInRange(heartPosition + (_positionStep * 0), transform.localPosition.y))
        {
            stoppedSlot = SlotType.HEART;
        }
        else if (IsInRange(heartPosition + (_positionStep * 1), transform.localPosition.y))
        {
            stoppedSlot = SlotType.SPADE;
        }
        else if (IsInRange(heartPosition + (_positionStep * 2), transform.localPosition.y))
        {
            stoppedSlot = SlotType.BAR;
        }
        else if (IsInRange(heartPosition + (_positionStep * 3), transform.localPosition.y))
        {
            stoppedSlot = SlotType.LEMON;
        }
        else if (IsInRange(heartPosition + (_positionStep * 4), transform.localPosition.y))
        {
            stoppedSlot = SlotType.HEART;
        }
        else if (IsInRange(heartPosition + (_positionStep * 5), transform.localPosition.y))
        {
            stoppedSlot = SlotType.DOLLAR;
        }
        else if (IsInRange(heartPosition + (_positionStep * 6), transform.localPosition.y))
        {
            stoppedSlot = SlotType.SEVEN;
        }
        else if (IsInRange(heartPosition + (_positionStep * 7), transform.localPosition.y))
        {
            stoppedSlot = SlotType.LEMON;
        }
        else if (IsInRange(heartPosition + (_positionStep * 8), transform.localPosition.y))
        {
            stoppedSlot = SlotType.HEART;
        }
        else
        {
            stoppedSlot = SlotType.UNKNOWN;
        }
    }
}
