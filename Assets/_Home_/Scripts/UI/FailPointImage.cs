using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;

public class FailPointImage : MonoBehaviour
{
    public Image logoImage;
    public Image backgroundImage;
    public Sprite logoSprite;
    public MMF_Player scalePlayer, positionPlayer;
    private float _currentScale = 0f, _currentPosition = 0f;
    private float currentScale
    {
        get => _currentScale;
        set
        {
            value = Mathf.Max(0f, value);
            if (value == _currentScale) return;
            _currentScale = value;
            SetScale(currentScale);
        }
    }
    private float currentPosition
    {
        get => _currentPosition;
        set
        {
            value = Mathf.Max(0f, value);
            if (value == _currentPosition) return;
            Debug.Log("Value: " + value + ", currentPosition: " + currentPosition);
            SetPosition(value, currentPosition);
            _currentPosition = value;
        }
    }

    [Button]
    public void SetPosition(float newPosition, float oldPosition = 0f)
    {
        MMF_Position positionFeedback = positionPlayer.GetFeedbackOfType<MMF_Position>();
        positionFeedback.Stop(Vector3.zero);

        positionFeedback.RemapCurveZero = oldPosition;
        positionFeedback.RemapCurveOne = newPosition;

        positionPlayer.PlayFeedbacks();
    }

    [Button]
    public void SetScale(float newScale)
    {
        if (scalePlayer == null)
        {
            backgroundImage.rectTransform.localScale = new Vector2(newScale, 1f);
            return;
        }
        MMF_Scale scaleFeedback = scalePlayer.GetFeedbackOfType<MMF_Scale>();
        scaleFeedback.Stop(Vector3.zero);

        scaleFeedback.DestinationScale = new Vector2(newScale, 1f);

        scalePlayer.PlayFeedbacks();
    }

    public void SetPositionAndScale(float newPosition, float newScale)
    {
        currentPosition = newPosition;
        currentScale = newScale;
    }

    [Button]
    public void AddToScale(float addition = 1f)
    {
        currentScale++;
    }
    [Button]
    public void AddToPosition(float addition = 1f)
    {
        currentPosition++;
    }
}
