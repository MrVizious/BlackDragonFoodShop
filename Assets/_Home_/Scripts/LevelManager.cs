using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DesignPatterns;
using Sirenix.OdinInspector;
using UtilityMethods;
using Cysharp.Threading.Tasks;
using GameEvents;
using UnityEngine.Events;

public class LevelManager : Singleton<LevelManager>
{
    public GameObject clientPrefab;
    public GameEventString onScoreChanged;

    public int maxClientsInStore = 5;
    public int maxThiefChance = 45;

    public int stolenItems
    {
        get => failCounter.stolenItems;
        set
        {
            failCounter.stolenItems = value;
        }
    }
    public int unsatisfiedClients
    {
        get => failCounter.unsatisfiedClients;
        set
        {
            failCounter.unsatisfiedClients = value;
        }
    }
    public float trashPoints
    {
        get => failCounter.trashPoints;
        set
        {
            failCounter.trashPoints = value;
        }
    }


    public int points
    {
        get => _points;
        set
        {
            value = Mathf.Max(0, value);
            _points = value;
            onScoreChanged.Raise(" €" + points);
        }
    }
    public int rangUpClients
    {
        get => _rangUpClients;
        set
        {
            _rangUpClients = value;
            if (_rangUpClients == clientsPerLevel) LevelUp();
        }
    }

    public int currentThiefChance
    {
        get => _currentThiefChance;
        set
        {
            _currentThiefChance = Mathf.Min(value, maxThiefChance);
        }
    }

    private FailCounter _failCounter;
    private FailCounter failCounter
    {
        get
        {
            if (_failCounter == null) _failCounter = FindObjectOfType<FailCounter>();
            return _failCounter;
        }
    }
    private int clientsPerLevel;
    private int _rangUpClients;
    private int _points = 0;
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
        List<bool> initialClientsThiefList = Enumerable.Repeat(false, maxClientsInStore).ToList();
        initialClientsThiefList[Random.Range(0, initialClientsThiefList.Count)] = true;
        for (int i = 0; i < maxClientsInStore; i++)
        {
            SpawnSpecificClient(initialClientsThiefList[i]);
            await UniTask.Delay(Random.Range(1500, 3000));
        }
    }

    private void LevelUp()
    {
        maxClientsInStore++;
        clientsPerLevel += maxClientsInStore;
        currentThiefChance += 3;
        SpawnRandomClient();
    }

    [Button]
    public void SpawnRandomClient()
    {
        bool isThief = Math.ProbabilityCheck(maxThiefChance);
        SpawnSpecificClient(isThief);
    }

    [Button]
    public void SpawnClient()
    {
        SpawnSpecificClient(false);
    }

    [Button]
    public void SpawnThief()
    {
        SpawnSpecificClient(true);
    }

    private void SpawnSpecificClient(bool isThief)
    {
        Client newClient = Instantiate(clientPrefab, door.position, Quaternion.identity, transform).GetComponent<Client>();
        newClient.isThief = isThief;
    }

}
