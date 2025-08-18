using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction _moveAction;
    private InputAction _interactAction;
    private InputAction _dropAction;
    private Rigidbody2D _body;
    private enum Items { FLOWER, STICK, PESTICIDE, NONE };
    private Items _heldItemType;
    private GameObject _currentFlowerBed;
    private GameObject _currentFlower;
    private GameObject _heldFlower;
    private Dictionary<int, GameObject> _currentPests;
    private Dictionary<int, GameObject> _currentDogs;
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
        //WASD or left stick on a controller
        _moveAction = InputSystem.actions.FindAction("Move");
        //The E key or north gamepad button (e.g Y, Triangle)
        _interactAction = InputSystem.actions.FindAction("Interact");
        //The Q key or east gamepad button (e.g B, Circle)
        _dropAction = InputSystem.actions.FindAction("Drop");
        _currentPests = new();
        _currentDogs = new();
        _body = GetComponent<Rigidbody2D>();
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
        MovementRestrictions();
        if (_interactAction.WasPressedThisFrame())
        {
            switch (_heldItemType)
            {
                case Items.FLOWER:
                    //TODO: make the flower unable to be moved if it is being attacked
                    if (_isOnFlowerBed && _currentFlowerBed.transform.Find("Plant(Clone)") == null)
                    {
                        // ! called when dropping a flower onto the flower bed
                        _heldFlower.transform.SetParent(_currentFlowerBed.transform);
                        _heldFlower.transform.localPosition = new Vector3(0, 0f, -0.25f);
                        _heldFlower.GetComponent<BoxCollider2D>().enabled = true;
                        _heldFlower.GetComponentInChildren<PlantGrower>().IsPlacedDown();
                        // _heldFlower = gameObject;
                        _heldItemType = Items.NONE;
                    }
                    break;
                case Items.STICK:
                    if (_isOnDog)
                    {
                        DamageEnemies(_currentDogs);
                    }
                    break;
                case Items.PESTICIDE:
                    if (_isOnPest)
                    {
                        DamageEnemies(_currentPests);
                    }
                    break;
                case Items.NONE:
                    if (_isOnStick)
                    {
                        _stick.transform.Find("Interact collider").gameObject.SetActive(false);
                        _stick.transform.SetParent(transform);
                        _stick.transform.localPosition = new Vector3(0.21f, 0.23f, 1);
                        _heldItemType = Items.STICK;
                    }
                    else if (_isOnPesticide)
                    {
                        _pesticide.transform.Find("Interact collider").gameObject.SetActive(false);
                        _pesticide.transform.SetParent(transform);
                        _pesticide.transform.localPosition = new Vector3(0.225f, 0.37f, 0);
                        _heldItemType = Items.PESTICIDE;
                    }
                    else if (_isOnFlower)
                    {
                        _currentFlower.GetComponent<BoxCollider2D>().enabled = false;
                        _currentFlower.transform.SetParent(transform);
                        _currentFlower.transform.localPosition = new Vector3(0.22f, 0.37f, 0);
                        _heldFlower = _currentFlower;
                        _heldItemType = Items.FLOWER;
                    }
                    else if (_isOnFlowerBed && _currentFlowerBed.transform.Find("Plant(Clone)") != null)
                    {
                        _heldFlower = _currentFlowerBed.transform.Find("Plant(Clone)").gameObject;
                        _heldFlower.GetComponent<BoxCollider2D>().enabled = false;
                        _heldFlower.transform.SetParent(transform);
                        _heldFlower.transform.localPosition = new Vector3(0.22f, 0.37f, 0);
                        _heldItemType = Items.FLOWER;
                        _heldFlower.GetComponentInChildren<PlantGrower>().IsPickedUp();
                    }
                    break;
            }
        }
        else if (_dropAction.WasPressedThisFrame() && _heldItemType != Items.NONE)
        {
            switch (_heldItemType)
            {
                case Items.FLOWER:
                    print("flower dropped");
                    GameObject _itemToDrop = transform.Find("Plant(Clone)").gameObject;
                    _itemToDrop.transform.SetParent(null);
                    _itemToDrop.GetComponent<BoxCollider2D>().enabled = true;
                    break;
                case Items.STICK:
                    _stick.transform.SetParent(null);
                    _stick.transform.Find("Interact collider").gameObject.SetActive(true);
                    break;
                case Items.PESTICIDE:
                    _pesticide.transform.SetParent(null);
                    _pesticide.transform.Find("Interact collider").gameObject.SetActive(true);
                    break;
            }
            _heldItemType = Items.NONE;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Flower bed interact"))
        {
            print("Collided with Flower bed interact");
            _isOnFlowerBed = true;
            _currentFlowerBed = collision.transform.parent.gameObject;
        }
        if (collision.gameObject.CompareTag("Flower"))
        {
            print("Collided with flower");
            _isOnFlower = true;
            _currentFlower = collision.gameObject;
        }
        if (collision.gameObject.CompareTag("Pesticide"))
        {
            print("Collided with pesticide");
            _isOnPesticide = true;
        }
        if (collision.gameObject.CompareTag("Stick"))
        {
            _isOnStick = true;
            print("Collided with stick");
        }
        if (collision.gameObject.CompareTag("Dog"))
        {
            _isOnDog = true;
            print("Collided with dog");
        }
        if (collision.gameObject.CompareTag("Pest interact"))
        {
            GameObject enemy = collision.transform.parent.gameObject;
            print(enemy);
            print(enemy.GetComponent<EnemyBehaviour>().GetID());
            _currentPests.Add(enemy.GetComponent<EnemyBehaviour>().GetID(), enemy);
            _isOnPest = true;
            print("Collided with pest");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Flower bed interact"))
        {
            print("Left Flower bed interact");
            _isOnFlowerBed = false;
            // _currentFlowerBed = gameObject;
        }
        if (collision.gameObject.CompareTag("Flower"))
        {
            print("Left flower");
            _isOnFlower = false;
            // _currentFlower = gameObject;
        }
        if (collision.gameObject.CompareTag("Pesticide"))
        {
            print("Left pesticide");
            _isOnPesticide = false;
        }
        if (collision.gameObject.CompareTag("Stick"))
        {
            _isOnStick = false;
            print("Left stick");
        }
        if (collision.gameObject.CompareTag("Dog interact"))
        {
            // This part only handles removing enemies from the dictionary when they move outside the player's range.
            // It is also triggered whenever an enemy is set to inactive while in the player's range, 
            // but the removing is prevented here as it would interfer with the looping through the _currentPests dictionary
            // when the pesticide is used. Removing in this case is handled just after said loop 
            GameObject enemy = collision.transform.parent.gameObject;
            if (enemy.activeInHierarchy)
            {
                _currentDogs.Remove(enemy.GetComponent<EnemyBehaviour>().GetID());
            }
            if (_currentDogs.Count == 0) _isOnDog = false;
            print("Left dog");
        }
        if (collision.gameObject.CompareTag("Pest interact"))
        {
            GameObject enemy = collision.transform.parent.gameObject;
            if (enemy.activeInHierarchy)
            {
                _currentPests.Remove(enemy.GetComponent<EnemyBehaviour>().GetID());
            }
            if (_currentPests.Count == 0) _isOnPest = false;
        }
    }

    private void DamageEnemies(Dictionary<int, GameObject> enemyDict)
    {
        List<int> enemiesToRemove = new();
        foreach (KeyValuePair<int, GameObject> enemy in enemyDict)
        {
            enemy.Value.GetComponent<HealthComponent>().TakeDamage(5);
            if (!enemy.Value.activeInHierarchy)
            {
                enemiesToRemove.Add(enemy.Key);
            }
        }
        if (enemiesToRemove.Count != 0)
        {
            foreach (int i in enemiesToRemove)
            {
                enemyDict.Remove(i);
            }
        }
    }
    
    private void MovementRestrictions()
    { 
        if (transform.position.y > 14)
        {
            transform.position = new Vector3(transform.position.x, 14, -2);
        }
        else if (transform.position.y < -14)
        {
            transform.position = new Vector3(transform.position.x, -14, -2);
        }
        if (transform.position.x > 24)
        {
            transform.position = new Vector3(24, transform.position.y, -2);
        }
        else if (transform.position.x < -5)
        {
            transform.position = new Vector3(-5, transform.position.y, -2);
        }
    }
}
