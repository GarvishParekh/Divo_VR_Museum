using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class PlayerVideo : MonoBehaviour
{
    [SerializeField] private string videoPath;
    [SerializeField] private MediaPlayer mediaPlayer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Play();
        }
    }

    public void Play()
    {
        mediaPlayer.OpenMedia(MediaPathType.AbsolutePathOrURL, videoPath, autoPlay: true);
    }
}
  