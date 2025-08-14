using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour
{
    private InputAction _moveAction;
    private InputAction _interactAction;
    private InputAction _dropAction;
    private Rigidbody _body;
    private enum Items { FLOWER, STICK, PESTICIDE, NONE };
    private Items _heldItemType;
    private GameObject _currentFlowerBed;
    private GameObject _currentFlower;
    private GameObject _heldFlower;
    [SerializeField] private GameObject _stick;
    [SerializeField] private GameObject _pesticide;
    private bool _isOnPesticide;
    private bool _isOnStick;
    private bool _isOnFlower;
    private bool _isOnFlowerBed;
    private bool _isOnDog;
    private bool _isOnPest;

    private const float _speed = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _interactAction = InputSystem.actions.FindAction("Interact");
        _dropAction = InputSystem.actions.FindAction("Drop");
        _body = GetComponent<Rigidbody>();
        _heldItemType = Items.NONE;
        _currentFlowerBed = gameObject;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 moveValue = _moveAction.ReadValue<Vector2>();
        moveValue = Vector3.Normalize(moveValue);
        _body.MovePosition((Vector2)transform.position + (_speed * Time.deltaTime * moveValue));
        // transform.Translate(_speed * Time.deltaTime * moveValue);
    }

    void Update()
    {
        if (_interactAction.WasPressedThisFrame())
        {
            switch (_heldItemType)
            {
                case Items.FLOWER:
                    //TODO: make the flower unable to be moved if it is being attacked
                    if (_isOnFlowerBed && _currentFlowerBed.transform.Find("Flower") == null)
                    {
                        _heldFlower.transform.parent = _currentFlowerBed.transform;
                        _heldFlower.transform.localPosition = new Vector3(0.0f, 0.48f, 0);
                        _heldFlower = gameObject;
                        _heldItemType = Items.NONE;
                    }
                    break;
                case Items.STICK:
                    //TODO: stick stuff
                    break;
                case Items.PESTICIDE:
                    //TODO: pesticide stuff
                    break;
                case Items.NONE:
                    if (_isOnStick)
                    {
                        print("Picking up the stick");
                        _stick.transform.parent = gameObject.transform;
                        _stick.transform.localPosition = new Vector3(0.14f, 0.5f, 0);
                        _heldItemType = Items.STICK;
                        //pick up the stick, set item to stick
                    }
                    else if (_isOnPesticide)
                    {
                        print("Picking up the pesticide");
                        _pesticide.transform.parent = gameObject.transform;
                        _pesticide.transform.localPosition = new Vector3(0.2f, 0.63f, 0);
                        _heldItemType = Items.PESTICIDE;
                        //pick up the pesticide
                    }
                    else if (_isOnFlower)
                    { 
                        _currentFlower.transform.parent = gameObject.transform;
                        _currentFlower.transform.localPosition = new Vector3(0, 0.4f, 0);
                        _heldFlower = _currentFlower;
                        _heldItemType = Items.FLOWER;
                    }
                    else if (_isOnFlowerBed && _currentFlowerBed.transform.Find("Flower") != null)
                    {
                        print("Picking up flower");
                        _heldFlower = _currentFlowerBed.transform.Find("Flower").gameObject;
                        _heldFlower.transform.parent = gameObject.transform;
                        _heldFlower.transform.localPosition = new Vector3(0, 0.4f, 0);
                        _heldItemType = Items.FLOWER;
                    }
                    break;
            }
        }
        else if (_dropAction.WasPressedThisFrame() && _heldItemType != Items.NONE)
        {
            switch (_heldItemType)
            {
                case Items.FLOWER:
                    GameObject _itemToDrop = transform.Find("Flower").gameObject;
                    _itemToDrop.transform.parent = null;
                    break;
                case Items.STICK:
                    _itemToDrop = transform.Find("Stick").gameObject;
                    _itemToDrop.transform.parent = null;
                    break;
                case Items.PESTICIDE:
                    _itemToDrop = transform.Find("Pesticide").gameObject;
                    _itemToDrop.transform.parent = null;
                    break;
            }
            _heldItemType = Items.NONE;
        }
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Flower bed"))
    //     {
    //         print("Collided with flower bed");
    //     }
    // }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Flower bed"))
        {
            print("Collided with flower bed");
            _isOnFlowerBed = true;
            _currentFlowerBed = other.gameObject.transform.parent.gameObject;
        }
        if (other.gameObject.CompareTag("Flower"))
        {
            print("Collided with flower");
            _isOnFlower = true;
            _currentFlower = other.gameObject;
            // _flower = other.gameObject;
        }
        if (other.gameObject.CompareTag("Pesticide"))
        {
            print("Collided with pesticide");
            _isOnPesticide = true;
        }
        if (other.gameObject.CompareTag("Stick"))
        {
            _isOnStick = true;
            print("Collided with stick");
        }
        if (other.gameObject.CompareTag("Dog"))
        {
            _isOnDog = true;
            print("Collided with dog");
        }
        if (other.gameObject.CompareTag("Pest"))
        {
            _isOnPest = true;
            print("Collided with pest");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Flower bed"))
        {
            print("Left flower bed");
            _isOnFlowerBed = false;
            _currentFlowerBed = gameObject;
        }
        if (other.gameObject.CompareTag("Flower"))
        {
            print("Left flower");
            _isOnFlower = false;
            _currentFlower = gameObject;
            // _flower = gameObject;
        }
        if (other.gameObject.CompareTag("Pesticide"))
        {
            print("Left pesticide");
            _isOnPesticide = false;
        }
        if (other.gameObject.CompareTag("Stick"))
        {
            _isOnStick = false;
            print("Left stick");
        }
        if (other.gameObject.CompareTag("Dog"))
        {
            _isOnDog = false;
            print("Left dog");
        }
        if (other.gameObject.CompareTag("Pest"))
        {
            _isOnPest = false;
            print("Left pest");
        }
    }
}
