using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    public Sprite mute;
    public Sprite unmute;
    bool isMute;
    // Start is called before the first frame update
    void Start()
    {
        isMute = FindObjectOfType<AudioManager>().IsMute();
        UpdateSprite();
    }
    public void Mute()
    {
        FindObjectOfType<AudioManager>().PlaySound("Click");
        FindObjectOfType<AudioManager>().Mute();
        isMute = !isMute;
        UpdateSprite();
    }
    void UpdateSprite()
    {
        if (isMute)
        {
            GetComponent<Image>().sprite = mute;
        }
        else
        {
            GetComponent<Image>().sprite = unmute;
        }
    }
}
