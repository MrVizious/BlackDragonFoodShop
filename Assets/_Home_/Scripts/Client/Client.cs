using System.Collections;
using System.Collections.Generic;
using DesignPatterns;
using UnityEngine;
using Sirenix.OdinInspector;
using ExtensionMethods;
using UnityEngine.U2D.Animation;
using Pathfinding;
using RuntimeSet;

public class Client : StateMachine<ClientState>
{
    public bool isSeen = false;
    public bool isThief = false;
    public ClientSpritesCollection spritesCollection;
    public RuntimeSetPointOfInterest activePointsOfInterest;

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
    public EventQueue eventQueue
    {
        get
        {
            if (_eventQueue == null)
                _eventQueue = gameObject.GetOrAddComponent<EventQueue>();
            return _eventQueue;
        }
    }

    private void Start()
    {
        ChangeAppearance();
        ChooseNextEvent();
        eventQueue.onQueueEmpty.AddListener(ChooseNextEvent);
        eventQueue.ExecuteNextEvent();
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
        if (activePointsOfInterest.Items.Count <= 0) return;
        PointOfInterest chosenPointOfInterest = activePointsOfInterest.GetRandom();

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.ToLower().Equals("unmask"))
        {
            isSeen = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isSeen) return;
        if (other.tag.ToLower().Equals("unmask"))
        {
            isSeen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.ToLower().Equals("unmask"))
        {
            isSeen = false;
        }
    }

    [Button]
    public void ChangeAppearance()
    {
        GetComponentInChildren<SpriteResolver>().spriteLibrary.spriteLibraryAsset = spritesCollection.getRandom();
    }
}