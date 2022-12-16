using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public Image[] images;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setActiveImages(int count)
    {
        for (int i = 0; i < images.Length - count; i++)
        {
            images[images.Length - i-1].gameObject.SetActive(false);
        }
        for (int i = 0; i < count; i++)
        {
            images[i].gameObject.SetActive(true);
        }
    }
}
