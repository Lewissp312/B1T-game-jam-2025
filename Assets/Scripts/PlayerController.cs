using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction _moveAction;
    private Rigidbody _body;
    private readonly float _speed = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 moveValue = _moveAction.ReadValue<Vector2>();
        moveValue = Vector3.Normalize(moveValue);
        _body.MovePosition((Vector2)transform.position + (_speed * Time.deltaTime * moveValue));
        // transform.Translate(_speed * Time.deltaTime * moveValue);
    }

    void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.CompareTag("Flower bed"))
        {
            print("Collided with flower bed");
        }
    }
}
