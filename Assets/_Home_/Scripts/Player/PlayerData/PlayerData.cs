using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Top-Down Shooter/PlayerData", order = 0)]
public class PlayerData : ScriptableObject
{
    public float speed;
    public int moneyEarned = 0;
}