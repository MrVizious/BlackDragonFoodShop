using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;
using Sirenix.OdinInspector;

public class FailCounter : MonoBehaviour
{
    public int maxFailPoints = 10;
    public Image stolenItemsImage, unsatisfiedClientsImage, trashPointsImage;
    [SerializeField]
    [OnValueChanged("UpdateGraphics")]
    private int _stolenItems = 0;
    public int stolenItems
    {
        get => _stolenItems;
        set
        {
            value = Mathf.Max(0, value);
            _stolenItems = value;
        }
    }
    [SerializeField]
    [OnValueChanged("UpdateGraphics")]
    private int _unsatisfiedClients = 0;
    public int unsatisfiedClients
    {
        get => _unsatisfiedClients;
        set
        {
            value = Mathf.Max(0, value);
            _unsatisfiedClients = value;
        }
    }
    [SerializeField]
    [OnValueChanged("UpdateGraphics")]
    private float _trashPoints = 0;
    public float trashPoints
    {
        get => _trashPoints;
        set
        {
            value = Mathf.Max(0, value);
            _trashPoints = value;
        }
    }

    [Button]
    public void UpdateGraphics()
    {
        // Stolen items
        stolenItemsImage.rectTransform.localScale = new Vector2(stolenItems, 1f);

        // Unsatisfied clients
        unsatisfiedClientsImage.rectTransform.anchoredPosition = new Vector2(stolenItems + 0.5f, 0f);
        unsatisfiedClientsImage.rectTransform.localScale = new Vector2(unsatisfiedClients, 1f);

        // Trash points
        trashPointsImage.rectTransform.anchoredPosition = new Vector2(stolenItems + unsatisfiedClients + 0.5f, 0f);
        trashPointsImage.rectTransform.localScale = new Vector2(trashPoints, 1f);
    }

    private void Start()
    {
        UpdateGraphics();
    }
}
