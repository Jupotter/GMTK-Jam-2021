using TMPro;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    public LevelManager LevelManager;

    public string BaseText = "";
    public bool   ShowFullTime;

    private TMP_Text _text;
    
    void Start()
    {
        _text = GetComponent<TMP_Text>();
    }
    
    void Update()
    {
        var time = ShowFullTime ? LevelManager.TotalTime : LevelManager.LevelTime;
        _text.text = BaseText + time.ToString("m\\:ss\\.fff");
    }
}
