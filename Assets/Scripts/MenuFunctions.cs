using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour{
    public void ChangeToGame(){
        SceneManager.LoadScene("CombinedScene");
        Time.timeScale = 1;
    }

    public void ChangeToStart(){
        SceneManager.LoadScene("Start");
        Time.timeScale = 1;
    }
}