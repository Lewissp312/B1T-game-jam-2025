using System;
using System.Collections.Generic;
using UnityEngine;

// used to spawn in the initial number of plants
public class PlantSpawner : MonoBehaviour{
    [SerializeField] private GameObject _plantPrefab;
    [SerializeField] private int _numPlantsToSpawn;

    // keep track of all the flower beds to know where to spawn in the sunshines
    [SerializeField] private List<Transform> _flowerBedTransforms = new();


    void Start(){
        System.Random helperRandom = new(); // used to randomise the random im using, then can be disposed of
        Unity.Mathematics.Random random = new((uint)helperRandom.Next(1, int.MaxValue-1)); // a uint that is > 0 must be passed in for it to work


        for (int i = 0; i < _numPlantsToSpawn; i++){
            int randomIndex = random.NextInt(0, _flowerBedTransforms.Count);
            Transform bedToSpawnAt = _flowerBedTransforms[randomIndex];
            _flowerBedTransforms.RemoveAt(randomIndex);

            GameObject flower = Instantiate(_plantPrefab);
            _plantPrefab.transform.position = bedToSpawnAt.position;
        }
    }
}