using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _enemyIDCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetID()
    {
        _enemyIDCount++;
        return _enemyIDCount;
    }
}
