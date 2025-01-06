using UnityEngine;

public class MuteButton : MonoBehaviour
{
    public void ToggleMute()
    {
        SoundManager.GetInstance().ToggleMute();
    }
}