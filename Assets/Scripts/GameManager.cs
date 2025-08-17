using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _enemyIDCount;
    private int _plantIDCount = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _plantIDCount = 0;
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

    public int GetPlantID(){
        _plantIDCount++;
        return _plantIDCount; // 1-4
    }
}
