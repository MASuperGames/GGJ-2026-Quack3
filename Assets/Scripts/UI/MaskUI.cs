using UnityEngine;
using UnityEngine.UI;

public class MaskUI : MonoBehaviour
{
    private Image image1;
    private Image image2;
    private Image image3;
    private Image image4;

    private bool init;

    private Color finished;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (init) return;
        image1 = transform.Find("Part1").GetComponent<Image>();
        image2 = transform.Find("Part2").GetComponent<Image>();
        image3 = transform.Find("Part3").GetComponent<Image>();
        image4 = transform.Find("Part4").GetComponent<Image>();
        init = true;

        finished = new Color(160, 255, 0);

    }

    public void SetCount(int count)
    {
        Start();

        image1.enabled = count >= 1;
        image2.enabled = count >= 2;
        image3.enabled = count >= 3;
        image4.enabled = count >= 4;

        if (count == 4)
        {
            image1.color = finished;
            image2.color = finished;
            image3.color = finished;
            image4.color = finished;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
