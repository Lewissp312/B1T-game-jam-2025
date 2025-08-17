using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantGrower : MonoBehaviour
{
    [Serializable] private struct GrowthState{
        public Sprite spriteW;
        public Sprite spriteB;
        public float minGrowth;

        public GrowthState(Sprite sW, Sprite sB, float g){
            spriteW = sW;
            spriteB = sB;
            minGrowth = g;
        }
    }

    private const float MIN_GROWTH = 0;
    private const float MAX_GROWTH = 100;



    [SerializeField] private SpriteRenderer _sRenderer;
    [SerializeField] private float _growthRate = 0.1f;
    [SerializeField] private bool _isUnderSunshine = false; // ! only serializefield right now for testing

    [SerializeField] private List<GrowthState> _states = new();
    private int _currentGrowthIndex = 0;

    private float _growth = 0;

    public bool IsDoneGrowing{
        get => _growth >= MAX_GROWTH;
        set{} // no setter, only for reading
    }

    private int _plantID;


    // ----------------------------------------------------------------------------------------------- //


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _plantID = GameObject.Find("Game Manager").GetComponent<GameManager>().GetPlantID();
        _growth = MIN_GROWTH;
        _currentGrowthIndex = 0;

        _sRenderer.sprite = _states[0].spriteB;

        HealthComponent hpComp = GetComponent<HealthComponent>();
        GameUI.Instance.UpdatePlantSprite(_plantID, _states[0].spriteW);
        GameUI.Instance.UpdatePlantHealth(_plantID, hpComp.Health, hpComp.MaxHealth);
        GameUI.Instance.UpdatePlantGrowth(_plantID, MIN_GROWTH, MAX_GROWTH);
    }

    // Update is called once per frame
    void Update()
    {
        // update health ui
        HealthComponent hpComp = GetComponent<HealthComponent>();
        GameUI.Instance.UpdatePlantHealth(_plantID, (float)hpComp.Health, (float)hpComp.MaxHealth);

        if (_growth >= MAX_GROWTH) return;

        if (_isUnderSunshine){
            _growth += _growthRate * Time.deltaTime;
            _growth = Mathf.Clamp(_growth, MIN_GROWTH, MAX_GROWTH);

            GameUI.Instance.UpdatePlantGrowth(_plantID, _growth, MAX_GROWTH);

            if (_growth >= _states[_currentGrowthIndex+1].minGrowth){
                _sRenderer.sprite = _states[_currentGrowthIndex+1].spriteB;
                _currentGrowthIndex++;
                GameUI.Instance.UpdatePlantSprite(_plantID, _states[_currentGrowthIndex].spriteW);
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
            GameObject.Find("GameUI").SetActive(false);
            GameObject.Find("EndMenu").SetActive(true);
            Time.timeScale = 0;
            // when all the plants are dead then show the end menu
        }
    }

    // ----------------------------------------------------------------------------------------------- //

    public void IsPickedUp(){
        print("<color=red>flower getting picked up</color>");
        // whenever picked up then switch the sprite to the white one
        _sRenderer.sprite = _states[_currentGrowthIndex].spriteW;
    }

    public void IsPlacedDown(){
        print("<color=red>flower getting placed down</color>");
        _sRenderer.sprite = _states[_currentGrowthIndex].spriteB;
    }


}
