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

    [Button]
    public void SetPosition(float newPosition)
    {
        if (backgroundImage.rectTransform.anchoredPosition.x == newPosition) return;
        if (positionPlayer == null)
        {
            Debug.Log("No position player", this);
            return;
        }
        backgroundImage.rectTransform.anchoredPosition = new Vector2(newPosition, 0f);
        MMF_Position positionFeedback = positionPlayer.GetFeedbackOfType<MMF_Position>();
        positionFeedback.InitialPosition = new Vector3(backgroundImage.rectTransform.anchoredPosition.x, 0f, 0f);
        positionFeedback.DestinationPosition = new Vector3(newPosition, 0f, 0f);

        scalePlayer.PlayFeedbacks();
    }

    [Button]
    public void SetScale(float newScale)
    {
        if (backgroundImage.rectTransform.localScale.x == newScale) return;
        if (scalePlayer == null)
        {
            Debug.Log("No scale player", this);
            return;
        }
        MMF_Scale scaleFeedback = scalePlayer.GetFeedbackOfType<MMF_Scale>();
        scaleFeedback.RemapCurveZero = backgroundImage.rectTransform.localScale.x;
        scaleFeedback.RemapCurveOne = newScale;

        scalePlayer.PlayFeedbacks();
    }

    public void SetPositionAndScale(float newPosition, float newScale)
    {
        SetPosition(newPosition);
        SetScale(newScale);
    }

    [Button]
    public void AddToScale(float addition = 1f)
    {
        SetScale(backgroundImage.rectTransform.localScale.x + addition);
    }
    [Button]
    public void AddToPosition(float addition = 1f)
    {
        SetPosition(backgroundImage.rectTransform.anchoredPosition.x + addition);
    }
}
