using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameMenuManager : MonoBehaviour
{

    public TextMeshProUGUI secondsText;
    public int waitSeconds = 3;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(countSeconds());
    }

   void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }

    public void loadGravityScene()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        //SceneManager.UnLoad(0);
    }
    IEnumerator countSeconds()
    {
        while (waitSeconds > 0)
        {
            secondsText.text = waitSeconds.ToString();
            yield return new WaitForSeconds(1.0f);
            waitSeconds -= 1;


        }


        gameObject.SetActive(false); 
    }
}
