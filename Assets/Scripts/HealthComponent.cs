using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    private int _health = 100;

    [SerializeField] private UnityEvent _onDeath; // defines a series of actions to perform whenever the object with this health component dies

    // ----------------------------------------------------------------------------------------------- //

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _health = _maxHealth;
    }

    // ----------------------------------------------------------------------------------------------- //

    public void TakeDamage(int dmg){
        _health -= dmg;

        if (_health <= 0){
            _onDeath.Invoke();
        }
    }


}
