using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlantGrower : MonoBehaviour
{
    [Serializable] private struct GrowthState{
        public Sprite sprite;
        public float minGrowth;

        public GrowthState(Sprite s, float g){
            sprite = s;
            minGrowth = g;
        }
    }

    private const float MIN_GROWTH = 0;
    private const float MAX_GROWTH = 100;



    [SerializeField] private SpriteRenderer _sRenderer;
    [SerializeField] private Image _growthBarImg; // ! use material rather than shared material
    [SerializeField] private float _growthRate = 0.1f;
    [SerializeField] private bool _isUnderSunshine = false; // ! only serializefield right now for testing

    [SerializeField] private List<GrowthState> _states = new();
    private int _currentGrowthIndex = 0;

    private float _growth = 0;

    public bool IsDoneGrowing{
        get => _growth >= MAX_GROWTH;
        set{} // no setter, only for reading
    }


    // ----------------------------------------------------------------------------------------------- //


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _growth = MIN_GROWTH;
        _currentGrowthIndex = 0;

        _growthBarImg.material = new Material(_growthBarImg.material);


        _growthBarImg.material.SetFloat("_MaxValue", MAX_GROWTH);
        _growthBarImg.material.SetFloat("_CurrentValue", MIN_GROWTH);
    }

    // Update is called once per frame
    void Update()
    {
        if (_growth >= MAX_GROWTH) return;

        if (_isUnderSunshine){
            _growth += _growthRate * Time.deltaTime;
            _growth = Mathf.Clamp(_growth, MIN_GROWTH, MAX_GROWTH);

            _growthBarImg.material.SetFloat("_CurrentValue", _growth);

            if (_growth >= _states[_currentGrowthIndex+1].minGrowth){
                _sRenderer.sprite = _states[_currentGrowthIndex+1].sprite;
                _currentGrowthIndex++;
            }
        }
    }

    // ----------------------------------------------------------------------------------------------- //


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sunshine")){
            _isUnderSunshine = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Sunshine")){
            _isUnderSunshine = false;
        }
    }

    public void CheckIfAllPlantsDead(){
        if (GameObject.FindGameObjectsWithTag("Flower").Count() == 0){
            GameObject.Find("EndMenu").SetActive(true);
            Time.timeScale = 0;
            // when all the plants are dead then show the end menu
        }
    }



}
