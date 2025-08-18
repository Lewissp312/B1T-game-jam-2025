using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // defines a global reference to this class

    private int _enemyIDCount;
    private int _plantIDCount = 0;

    public GameObject endMenu;
    public GameObject gameUI;

    [SerializeField] private AudioMixerGroup _masterAudioGroup = null; // needed to add all the dynamic audio clips to the master group to be able to
    // control volume


    void Awake()
    {
        // ensures that only 1 instance of this class ever exists, and destroys any others
        // this is how to define a SINGLETON, these 2 magic lines
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }


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



    // https://discussions.unity.com/t/how-to-route-sound-from-playclipatpoint-to-a-specific-audio-mixer-group/139683/3
    public void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1.0f){
        if (clip == null) return;
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource) gameObject.AddComponent(typeof (AudioSource));
        audioSource.outputAudioMixerGroup = _masterAudioGroup;
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.Play();
        Object.Destroy(gameObject, clip.length * (Time.timeScale < 0.009999999776482582 ? 0.01f : Time.timeScale));
    }    
}
