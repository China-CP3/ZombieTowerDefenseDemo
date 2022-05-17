 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingPanel : BasePanel
{
    public Button btnClose;
    public Toggle togMusic; 
    public Toggle togSound;
    public Slider sliderMusic;
    public Slider sliderSound;
    public override void Init()
    {
        MusicData musicData = GameDataMgr.Instance.musicData;
        togMusic.isOn = musicData.musicOpen;
        togSound.isOn = musicData.soundOpen;
        sliderMusic.value = musicData.musicValue;
        sliderSound.value = musicData.soundValue;

        btnClose.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.SaveMusicData();
            UiMgr.Instance.HidePanel<SettingPanel>();
        });
        togMusic.onValueChanged.AddListener((b) =>
        {
           BKMusic.Instance.SetIsOpen(b);
           GameDataMgr.Instance.musicData.musicOpen = b;
        });
        togSound.onValueChanged.AddListener((b) =>
        {
            GameDataMgr.Instance.musicData.soundOpen = b;
        });
        sliderMusic.onValueChanged.AddListener((v) =>
        {
            BKMusic.Instance.ChangeValue(v);
            GameDataMgr.Instance.musicData.musicValue = v;
        });
        sliderSound.onValueChanged.AddListener((v) =>
        {
            GameDataMgr.Instance.musicData.soundValue = v;
        });
    }
}
