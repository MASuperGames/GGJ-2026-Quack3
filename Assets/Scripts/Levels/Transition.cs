using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") NextScene();
    }

    private void NextScene()
    {
        LoadSceneAndInvoke();
    }

    private void LoadSceneAndInvoke()
    {
        SceneController.Instance.LoadNextScene();
    }
}
