using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

// used to spawn in and despawn sunshines on different beds for intervals of time
public class SunshineSpawner : MonoBehaviour
{
    // keep track of all the flower beds to know where to spawn in the sunshines
    [SerializeField] private Transform[] _flowerBedTransforms;
    private List<Transform> _currentBedsShinedOn = new(); // to keep track of which beds already have sunshine on them so dont put
    // sunshine on them again
    private List<Transform> _availableBeds{
        get => _flowerBedTransforms.Except(_currentBedsShinedOn).ToList();
    }
    [SerializeField] private GameObject _sunshinePrefab;


    [SerializeField] private int _numConcurrentSunshines = 3; // how many sunshines will be active at the same time for the entire game
    [SerializeField] private float _minPossibleSunshineDuration = 4f; // each sunshine will last for a random duration between these 2 amounts
    [SerializeField] private float _maxPossibleSunshineDuration = 6f;
    [SerializeField] private float _sunshineSpawnStaggerTime = 1f;


    private Unity.Mathematics.Random _random;







    // ----------------------------------------------------------------------------------------------- //


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        System.Random helperRandom = new(); // used to randomise the random im using, then can be disposed of
        _random = new((uint)helperRandom.Next(1, int.MaxValue-1)); // a uint that is > 0 must be passed in for it to work
        // when game is started then spawn in the total concurrent number of sunshines, with a small delay between them to cause staggered
        // despawning and spawning

        for (int i = 0; i < _numConcurrentSunshines; i++){
            StartCoroutine(SpawnSunshine(_sunshineSpawnStaggerTime * (i+1)));
        }
    }

    // ----------------------------------------------------------------------------------------------- //

    // a recursive coroutine that when a sunshine is done then it is moved to a new position repeatedly until the game ends
    private IEnumerator SpawnSunshine(float initialDelaySeconds){
        yield return new WaitForSeconds(initialDelaySeconds);


        GameObject sunshine = Instantiate(_sunshinePrefab);

        while (true){
            sunshine.SetActive(false);
            // move the sunshine to an empty bed
            Transform bedToOccupy = _availableBeds[_random.NextInt(0, _availableBeds.Count)];
            _currentBedsShinedOn.Add(bedToOccupy);

            sunshine.transform.position = bedToOccupy.position + new Vector3(0,0,0.5f);
            // allow the sunshine to be active for the duration, then set inactive
            sunshine.SetActive(true);
            yield return new WaitForSeconds(_random.NextFloat(_minPossibleSunshineDuration, _maxPossibleSunshineDuration));

            _currentBedsShinedOn.Remove(bedToOccupy);
        }
    }

    

}
