using UnityEngine;
using RenderHeads.Media.AVProVideo.Demos;

public class AutoHideHandler : MonoBehaviour
{
    [SerializeField] MediaPlayerUI mediaPlayerUI;
    public void CustomPointerEnter()
    {
        mediaPlayerUI._autoHide = false;
    }
    public void CustomPointerExit()
    {
        mediaPlayerUI._autoHide = true;
    }
}
