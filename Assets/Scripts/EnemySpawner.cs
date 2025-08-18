using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _northSpawnLocations = new();
    [SerializeField] private List<Transform> _eastSpawnLocations = new();
    [SerializeField] private List<Transform> _southSpawnLocations = new();
    [SerializeField] private List<Transform> _westSpawnLocations = new();
    private int _dirIndex = 0;
    [SerializeField] private GameObject _dogPrefab;
    [SerializeField] private GameObject _pestPrefab;

    [SerializeField] private float _spawnInterval = 4f;
    [SerializeField] private float _spawnQuickenAmount = 0.01f;
    [SerializeField] private float _spawnQuickenInterval = 1f;
    private Unity.Mathematics.Random _random;

    

    // ----------------------------------------------------------------------------------------------- //

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        System.Random helperRandom = new();
        _random = new((uint)helperRandom.Next(1, int.MaxValue-1));

        _dirIndex = 0;
        StartCoroutine(SpawnEnemies());
        StartCoroutine(QuickenSpawnTime());
    }

    // ----------------------------------------------------------------------------------------------- //

    private IEnumerator SpawnEnemies(){
        int numSpawns = 0; // switch between spawning dogs every even, and pests every odd
        while (true){
            // wait delay, then spawn enemy repeatedly
            yield return new WaitForSeconds(_spawnInterval);

            Vector3 randomSpawnPosition;
            if (_dirIndex == 0){
                // north
                randomSpawnPosition = _northSpawnLocations[_random.NextInt(0, _northSpawnLocations.Count)].position;
            }
            else if (_dirIndex == 1){
                // east
                randomSpawnPosition = _eastSpawnLocations[_random.NextInt(0, _eastSpawnLocations.Count)].position;
            }
            else if (_dirIndex == 2){
                // south
                randomSpawnPosition = _southSpawnLocations[_random.NextInt(0, _southSpawnLocations.Count)].position;
            }
            else{
                // west
                randomSpawnPosition = _westSpawnLocations[_random.NextInt(0, _westSpawnLocations.Count)].position;
            }
            _dirIndex = (_dirIndex + 1) % 4;

            GameObject newEnemy = Instantiate(numSpawns % 2 == 0 ? _dogPrefab : _pestPrefab);
            newEnemy.transform.position = randomSpawnPosition;
            numSpawns++;
        }
    }

    private IEnumerator QuickenSpawnTime(){
        while (true){
            yield return new WaitForSeconds(_spawnQuickenInterval);
            _spawnInterval = Mathf.Max(0, _spawnInterval - _spawnQuickenAmount);
        }
    }

}
