using System.Collections;
using System.Collections.Generic;
using DesignPatterns;
using UnityEngine;
using Sirenix.OdinInspector;
using ExtensionMethods;
using UnityEngine.U2D.Animation;
using Pathfinding;
using RuntimeSet;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

public class Client : StateMachine<ClientState>
{
    public bool isSeen = false;
    public bool isThief = false;
    public bool seenStealing
    {
        get => _seenStealing;
        set
        {
            if (value)
            {
                this.GetOrAddComponent<ThiefFoundSpriteAnimation>();
            }
            else
            {
                ThiefFoundSpriteAnimation animation = GetComponent<ThiefFoundSpriteAnimation>();
                Destroy(animation);
            }
            _seenStealing = value;
        }
    }
    public ClientSpritesCollection spritesCollection;
    public RuntimeSetPointOfInterest activePointsOfInterest;
    public int currentNumberOfItems = 0;
    public EventQueue eventQueue
    {
        get
        {
            if (_eventQueue == null)
                _eventQueue = gameObject.GetOrAddComponent<EventQueue>();
            return _eventQueue;
        }
    }


    private bool _seenStealing = false;
    private PointOfInterest chosenPointOfInterest;
    private bool isTouchingPlayer = false;
    private AIPath _seeker;
    private AIPath seeker
    {
        get
        {
            if (_seeker == null) _seeker = GetComponentInChildren<AIPath>();
            return _seeker;
        }
    }

    private Animator _animator;
    private Animator animator
    {
        get
        {
            if (_animator == null) _animator = GetComponentInChildren<Animator>();
            return _animator;
        }
    }

    private EventQueue _eventQueue;
    private PlayerInput _input;
    private PlayerInput input
    {
        get
        {
            if (_input == null) _input = FindObjectOfType<PlayerInput>();
            return _input;
        }
    }

    private void Start()
    {
        ChangeAppearance();
        ChooseNextEvent();
        eventQueue.onQueueEmpty.AddListener(ChooseNextEvent);
        eventQueue.ExecuteNextEvent();
        input.actions["Interact"].performed += ctx => CatchStealing();
    }

    private void Update()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        Vector3 desiredVelocity = seeker.desiredVelocity;
        if (desiredVelocity.magnitude > 0f)
        {
            animator.SetBool("Walking", true);
            if (Mathf.Abs(desiredVelocity.x) >= Mathf.Abs(desiredVelocity.y))
            {
                if (desiredVelocity.x > 0) animator.SetInteger("Direction", 3);
                else animator.SetInteger("Direction", 2);
            }
            else
            {
                if (desiredVelocity.y > 0) animator.SetInteger("Direction", 0);
                else animator.SetInteger("Direction", 1);
            }
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    [Button]
    public void ChooseNextEvent()
    {
        if (DoesStopRoaming())
        {
            if (!isThief) GoToCashier();
            else LeaveStore();
            return;
        }
        if (activePointsOfInterest.Items.Count <= 0) return;
        chosenPointOfInterest = activePointsOfInterest.GetRandomExluding(chosenPointOfInterest);

        // Add walk to interest point event
        WalkEvent walkEvent = gameObject.AddComponent<WalkEvent>();
        walkEvent.client = this;
        walkEvent.pointOfInterest = chosenPointOfInterest;
        eventQueue.AddEvent(walkEvent);

        // Add event
        ClientEvent mainEvent = (ClientEvent)gameObject.AddComponent(chosenPointOfInterest.GetEvent(isThief));
        mainEvent.client = this;
        mainEvent.pointOfInterest = chosenPointOfInterest;
        eventQueue.AddEvent(mainEvent);
    }

    /// <summary>
    /// Calculate whether the client goes to pay for the items or not
    /// </summary>
    /// <returns></returns>
    private bool DoesStopRoaming()
    {
        if (currentNumberOfItems <= 0) return false;

        int randomNumber = Random.Range(0, 101);

        if (currentNumberOfItems == 1)
        {
            if (randomNumber <= 50) return true;
        }
        else if (currentNumberOfItems == 2)
        {
            if (randomNumber <= 75) return true;
        }
        else if (currentNumberOfItems == 3)
        {
            if (randomNumber <= 90) return true;
        }
        else if (currentNumberOfItems == 4)
        {
            if (randomNumber <= 99) return true;
        }
        else
        {
            return true;
        }

        return false;
    }

    private void GoToCashier()
    {
        Transform newTarget = FindObjectOfType<Cashier>().GetFreeSpot(this);
        if (newTarget == null)
        {
            LevelManager.Instance.unsatisfiedClients++;
            // TODO: Drop their items
            return;
        }
        GetComponent<AIDestinationSetter>().target = newTarget;
    }

    public void LeaveStore()
    {
        // Add walk to interest point event
        LeaveEvent leaveEvent = gameObject.AddComponent<LeaveEvent>();
        leaveEvent.client = this;
        eventQueue.AddEvent(leaveEvent);
        eventQueue.ExecuteNextEvent();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.ToLower().Equals("unmask"))
        {
            isSeen = true;
        }
        else if (other.tag.ToLower().Equals("player"))
        {
            isTouchingPlayer = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.ToLower().Equals("unmask"))
        {
            isSeen = true;
        }
        else if (other.tag.ToLower().Equals("player"))
        {
            isTouchingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.ToLower().Equals("unmask"))
        {
            isSeen = false;
        }
        if (other.tag.ToLower().Equals("player"))
        {
            isTouchingPlayer = false;
        }
    }

    [Button]
    public void ChangeAppearance()
    {
        GetComponentInChildren<SpriteResolver>().spriteLibrary.spriteLibraryAsset = spritesCollection.getRandom();
    }

    public async void CatchStealing()
    {
        if (!seenStealing) return;

        // TODO: Leave items on the floor
        currentNumberOfItems = 0;

        seenStealing = false;
        eventQueue.Clear();
        await UniTask.Delay(100);
        LeaveStore();
    }
}