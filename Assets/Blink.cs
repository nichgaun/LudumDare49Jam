using UnityEngine.UI;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public float BlinkTime;
    private Image _image;
    
    void Awake()
    {
        _image = GetComponent<Image>();
    }
    
    void Update()
    {
        BlinkTime = (BlinkTime + Time.deltaTime) % 1f;
        _image.enabled = BlinkTime < 0.5f;
    }
}
