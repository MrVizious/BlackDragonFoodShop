using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using Sirenix.OdinInspector;
using UtilityMethods;
using Cysharp.Threading.Tasks;

public class LevelManager : Singleton<LevelManager>
{
    public GameObject clientPrefab;
    public int points = 0;
    public int stolenItems = 0;
    public int unsatisfiedClients = 0;
    public int rangUpClients
    {
        get => _rangUpClients;
        set
        {
            if (rangUpClients == clientsPerLevel) LevelUp();
            if (_rangUpClients < value) SpawnClient();
            _rangUpClients = value;
        }
    }
    public int maxClientsInStore = 3;
    public int currentThiefChance
    {
        get => _currentThiefChance;
        set
        {
            _currentThiefChance = Mathf.Min(value, maxThiefChance);
        }
    }
    public int maxThiefChance = 45;

    private int clientsPerLevel;
    private int _rangUpClients;
    public int _currentThiefChance = 20;
    protected override bool dontDestroyOnLoad
    {
        get { return false; }
    }

    private Transform _door = null;
    private Transform door
    {
        get
        {
            if (_door == null) _door = GameObject.Find("Door").transform;
            return _door;
        }
    }

    private async void Start()
    {
        clientsPerLevel = maxClientsInStore;
        for (int i = 0; i < maxClientsInStore; i++)
        {
            SpawnClient();
            await UniTask.Delay(800);
        }
    }

    private void LevelUp()
    {
        maxClientsInStore++;
        clientsPerLevel += maxClientsInStore;
        currentThiefChance++;
        SpawnClient();
    }

    [Button]
    private void SpawnClient()
    {
        Client newClient = Instantiate(clientPrefab, door.position, Quaternion.identity, transform).GetComponent<Client>();
        bool isThief = Math.ProbabilityCheck(maxThiefChance);
        newClient.isThief = isThief;
    }

}
