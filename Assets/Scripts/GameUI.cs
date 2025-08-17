using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    [Header("Row 1")]
    [SerializeField] private Image _plantSprite1;
    [SerializeField] private Image _healthBarImg1;
    [SerializeField] private Image _growthBarImg1;

    [Space(10), Header("Row 2")]
    [SerializeField] private Image _plantSprite2;
    [SerializeField] private Image _healthBarImg2;
    [SerializeField] private Image _growthBarImg2;

    [Space(10), Header("Row 3")]
    [SerializeField] private Image _plantSprite3;
    [SerializeField] private Image _healthBarImg3;
    [SerializeField] private Image _growthBarImg3;

    [Space(10), Header("Row 4")]
    [SerializeField] private Image _plantSprite4;
    [SerializeField] private Image _healthBarImg4;
    [SerializeField] private Image _growthBarImg4;    


    // ----------------------------------------------------------------------------------------------- //

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        _plantSprite1.material = new(_plantSprite1.material);
        _plantSprite2.material = new(_plantSprite2.material);
        _plantSprite3.material = new(_plantSprite3.material);
        _plantSprite4.material = new(_plantSprite4.material);

        _healthBarImg1.material = new(_healthBarImg1.material);
        _healthBarImg2.material = new(_healthBarImg2.material);
        _healthBarImg3.material = new(_healthBarImg3.material);
        _healthBarImg4.material = new(_healthBarImg4.material);

        _growthBarImg1.material = new(_growthBarImg1.material);
        _growthBarImg2.material = new(_growthBarImg2.material);
        _growthBarImg3.material = new(_growthBarImg3.material);
        _growthBarImg4.material = new(_growthBarImg4.material);
    }


    // ----------------------------------------------------------------------------------------------- //

    public void UpdatePlantSprite(int id, Sprite sprite){
        if (id == 1){
            _plantSprite1.sprite = sprite;
        }
        else if (id == 2){
            _plantSprite2.sprite = sprite;
        }
        else if (id == 3){
            _plantSprite3.sprite = sprite;
        }
        else if (id == 4){
            _plantSprite4.sprite = sprite;
        }
        else{
            throw new System.Exception("Invalid plant id passed");
        }
    }

    public void UpdatePlantHealth(int id, float current, float max){
        if (id == 1){
            _healthBarImg1.material.SetFloat("_CurrentValue", current);
            _healthBarImg1.material.SetFloat("_MaxValue", max);
        }
        else if (id == 2){
            _healthBarImg2.material.SetFloat("_CurrentValue", current);
            _healthBarImg2.material.SetFloat("_MaxValue", max);
        }
        else if (id == 3){
            _healthBarImg3.material.SetFloat("_CurrentValue", current);
            _healthBarImg3.material.SetFloat("_MaxValue", max);
        }
        else if (id == 4){
            _healthBarImg4.material.SetFloat("_CurrentValue", current);
            _healthBarImg4.material.SetFloat("_MaxValue", max);
        }
        else{
            throw new System.Exception("Invalid plant id passed");
        }        
    }

    public void UpdatePlantGrowth(int id, float current, float max){
        if (id == 1){
            _growthBarImg1.material.SetFloat("_CurrentValue", current);
            _growthBarImg1.material.SetFloat("_MaxValue", max);
        }
        else if (id == 2){
            _growthBarImg2.material.SetFloat("_CurrentValue", current);
            _growthBarImg2.material.SetFloat("_MaxValue", max);
        }
        else if (id == 3){
            _growthBarImg3.material.SetFloat("_CurrentValue", current);
            _growthBarImg3.material.SetFloat("_MaxValue", max);
        }
        else if (id == 4){
            _growthBarImg4.material.SetFloat("_CurrentValue", current);
            _growthBarImg4.material.SetFloat("_MaxValue", max);
        }
        else{
            throw new System.Exception("Invalid plant id passed");
        }           
    }

}
