using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour{
    public void ChangeToGame(){
        SceneManager.LoadScene("CombinedScene");
    }

    public void ChangeToStart(){
        SceneManager.LoadScene("Start");
    }
}