using UnityEngine;
using UnityEngine.UI;

public class MaskUI : MonoBehaviour
{
    private Image image1;
    private Image image2;
    private Image image3;
    private Image image4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image1 = transform.Find("Part1").GetComponent<Image>();
        image2 = transform.Find("Part2").GetComponent<Image>();
        image3 = transform.Find("Part3").GetComponent<Image>();
        image4 = transform.Find("Part4").GetComponent<Image>();
    }

    public void SetCount(int count)
    {
        image1.enabled = count >= 1;
        image2.enabled = count >= 2;
        image3.enabled = count >= 3;
        image4.enabled = count >= 4;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
