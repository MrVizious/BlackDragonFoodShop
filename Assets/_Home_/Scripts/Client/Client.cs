using System.Collections;
using System.Collections.Generic;
using DesignPatterns;
using UnityEngine;
using Sirenix.OdinInspector;
using ExtensionMethods;
using UnityEngine.U2D.Animation;
using Pathfinding;
using RuntimeSet;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

public class Client : StateMachine<ClientState>
{
    public bool isSeen = false;
    public bool isThief = false;
    public Trash trashPrefab;
    public UnityEvent onClientExit = new UnityEvent();
    public UnityEvent onThiefExitWithItems = new UnityEvent();
    public UnityEvent onClientUnsatisfied = new UnityEvent();
    public UnityEvent onSpawn = new UnityEvent();
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

    private AIPath _aiPath;
    private AIPath aiPath
    {
        get
        {
            if (_aiPath == null) _aiPath = GetComponent<AIPath>();
            return _aiPath;
        }
    }
    private EventQueue _eventQueue;

    [SerializeField]
    private int noEventCounter = 0;

    private void Start()
    {
        ChangeAppearance();
        ChooseNextEvent();
        eventQueue.onQueueEmpty.AddListener(ChooseNextEvent);
        eventQueue.ExecuteNextEvent();
        onSpawn.Invoke();
    }

    private void Update()
    {
        UpdateAnimation();
        if (Time.frameCount % 15 != 0) return;
        if (eventQueue.currentEvent == null && eventQueue.nextEvents.Count == 0)
        {
            Debug.Log("No event");
            noEventCounter++;
            if (noEventCounter >= 3)
            {
                if (isThief || currentNumberOfItems <= 0) LeaveStore();
                else if (!isThief && currentNumberOfItems > 0) GoToCashier();
            }
            return;
        }
        noEventCounter = 0;

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

    private async void GoToCashier()
    {
        eventQueue.Clear();
        await UniTask.Delay(200).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());

        // Add walk to interest point event
        GoToCashierEvent goToCashierEvent = gameObject.AddComponent<GoToCashierEvent>();
        goToCashierEvent.client = this;
        eventQueue.AddEvent(goToCashierEvent);
        eventQueue.ExecuteNextEvent();
    }

    public async void LeaveStore()
    {
        eventQueue.Clear();
        await UniTask.Delay(200).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());

        // Add walk to interest point event
        LeaveEvent leaveEvent = gameObject.AddComponent<LeaveEvent>();
        leaveEvent.client = this;
        eventQueue.AddEvent(leaveEvent);
        eventQueue.ExecuteNextEvent();
        eventQueue.currentEvent.onEnded.AddListener(OnClientExit);
    }

    private void OnClientExit()
    {
        if (isThief && currentNumberOfItems > 0) onThiefExitWithItems.Invoke();
        else onClientExit.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.ToLower().Equals("unmask"))
        {
            isSeen = true;
        }
        else if (other.tag.ToLower().Equals("player"))
        {
            if (seenStealing)
            {
                InteractionController interactionController = other.GetComponent<InteractionController>();
                interactionController.AddInteraction(this, CatchStealing);
            }
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
            if (seenStealing)
            {
                InteractionController interactionController = other.GetComponent<InteractionController>();
                interactionController.AddInteraction(this, CatchStealing);
            }
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
            if (seenStealing)
            {
                InteractionController interactionController = other.GetComponent<InteractionController>();
                interactionController.RemoveInteraction(this);
            }
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
        DropItems();

        GetComponentInChildren<SpriteRenderer>().color = Color.gray;

        seenStealing = false;
        LeaveStore();
    }

    public void DropItems()
    {
        if (currentNumberOfItems > 0)
        {
            Instantiate(trashPrefab, transform.position, Quaternion.identity);
        }
        currentNumberOfItems = 0;
    }

    public bool HasArrivedToDestination()
    {
        bool hasArrived = aiPath.reachedDestination;
        return hasArrived;
    }
}