using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKMusic : MonoBehaviour
{
    private static BKMusic instance;
    public static BKMusic Instance => instance;
    private AudioSource bkSource;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        bkSource = GetComponent<AudioSource>();
        MusicData data = GameDataMgr.Instance.musicData;
        SetIsOpen(data.musicOpen);
        ChangeValue(data.musicValue);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetIsOpen(bool isOpen)
    {
        bkSource.mute = !isOpen;
    }
    public void ChangeValue(float value)
    {
        bkSource.volume = value;
    }
}
