using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public bool enabled = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled) {
            SceneManager.LoadScene("Game");
            enabled = false;
        }
    }

    void StarGameClick() {
        SceneManager.LoadScene("Game");
    }
}
