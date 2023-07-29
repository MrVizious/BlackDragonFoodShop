using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;
using Sirenix.OdinInspector;

public class FailCounter : MonoBehaviour
{
    public int maxFailPoints = 10;
    public FailPointImage stolenItemsImage, unsatisfiedClientsImage, trashPointsImage;
    [SerializeField]
    [Range(0, 10)]
    [OnValueChanged("UpdateGraphics")]
    private int _stolenItems = 0;
    public int stolenItems
    {
        get => _stolenItems;
        set
        {
            value = Mathf.Max(0, value);
            _stolenItems = value;
            UpdateGraphics();
        }
    }
    [SerializeField]
    [Range(0, 10)]
    [OnValueChanged("UpdateGraphics")]
    private int _unsatisfiedClients = 0;
    public int unsatisfiedClients
    {
        get => _unsatisfiedClients;
        set
        {
            value = Mathf.Max(0, value);
            _unsatisfiedClients = value;
            UpdateGraphics();
        }
    }
    [SerializeField]
    [Range(0, 10)]
    [OnValueChanged("UpdateGraphics")]
    private float _trashPoints = 0;
    public float trashPoints
    {
        get => _trashPoints;
        set
        {
            value = Mathf.Max(0, value);
            _trashPoints = value;
            UpdateGraphics();
        }
    }

    [Button]
    public void UpdateGraphics()
    {
        // Stolen items
        stolenItemsImage.SetPositionAndScale(0f, stolenItems);

        // Unsatisfied clients
        unsatisfiedClientsImage.SetPositionAndScale(stolenItems, unsatisfiedClients);

        // Trash points
        trashPointsImage.SetPositionAndScale(stolenItems + unsatisfiedClients, trashPoints);
    }

    private void Start()
    {
        stolenItems = 0;
        unsatisfiedClients = 0;
        trashPoints = 0f;
        UpdateGraphics();
    }
}
