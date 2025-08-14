using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    private int _health = 100;

    [SerializeField] private UnityEvent _onDeath; // defines a series of actions to perform whenever the object with this health component dies
    [SerializeField] private Image _healthBarImg; // ! use material rather than shared material on this to only affect the instance

    // ----------------------------------------------------------------------------------------------- //

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _health = _maxHealth;

        _healthBarImg.material = new Material(_healthBarImg.material); // need to make a new INSTANCE of the material so that when setting
        // properties its not setting the props of the shared material


        _healthBarImg.material.SetFloat("_MaxValue", _maxHealth);
        _healthBarImg.material.SetFloat("_CurrentValue", _maxHealth);
    }

    // ----------------------------------------------------------------------------------------------- //

    public void TakeDamage(int dmg){
        _health -= dmg;
        _healthBarImg.material.SetFloat("_CurrentValue", _health);

        if (_health <= 0){
            _onDeath.Invoke();
        }
    }


}
