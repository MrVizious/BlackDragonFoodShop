using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailCounter : MonoBehaviour
{
    public int maxFailPoints = 10;
    public Image stolenItemsImage, unsatisfiedClientsImage, trashPointsImage;
    private int _stolenItems;
    private int stolenItems
    {
        get => _stolenItems;
        set
        {
            value = Mathf.Max(0, value);
            _stolenItems = value;
        }
    }
    private int _unsatisfiedClients;
    private int unsatisfiedClients
    {
        get => _unsatisfiedClients;
        set
        {
            value = Mathf.Max(0, value);
            _unsatisfiedClients = value;
        }
    }
    private int _trashPoints;
    private int trashPoints
    {
        get => _trashPoints;
        set
        {
            value = Mathf.Max(0, value);
            _trashPoints = value;
        }
    }

    public void AddStolenItem()
    {

    }
    public void AddUnsatisfiedClient()
    {

    }
    public void AddTrashPoint()
    {

    }
}
