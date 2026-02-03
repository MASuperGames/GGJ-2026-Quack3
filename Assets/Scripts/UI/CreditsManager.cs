using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private float timeout = 10.0f;
    [SerializeField] TextMeshProUGUI countdown;
    [SerializeField] TextMeshProUGUI deathCounter;

    private float startTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
        deathCounter.text = "You died " + DeathCounter.Instance.GetDeathCount() + " times";
    }

    // Update is called once per frame
    void Update()
    {
        if(countdown != null)
            countdown.text = "" + (int)(timeout - (Time.time - startTime) + 1);
        if (startTime + timeout < Time.time)
        {
            //SceneController.Instance.LoadScene(0);
        }
    }
}
