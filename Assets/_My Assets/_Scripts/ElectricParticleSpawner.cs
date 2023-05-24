using UnityEngine;

public class ElectricParticleSpawner : MonoBehaviour
{
    #region Variables
    enum HandSide
    {               
        Left,
        Right
    }
    [SerializeField] private HandSide handSide;

    [Space]
    [Header ("Particle components")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject electricParticles;

    [Space]
    [Header ("Values")]
    [SerializeField] private float timeLimit = 4;

    enum HandsAction
    {
        notInPillars,
        InPillars
    }
    [SerializeField] private HandsAction playerAction;

    float timer = 0;
    #endregion

    #region Subscribe to action
    private void OnEnable()
    {
        HandsState.HandsInPillar += OnInPillars;
        HandsState.HandsOutsidePillar += OnOutsidePillars;
    }

    private void OnDisable()
    {
        HandsState.HandsInPillar -= OnInPillars;
        HandsState.HandsOutsidePillar -= OnOutsidePillars;
    }

    public void OnInPillars() => playerAction = HandsAction.InPillars;
    public void OnOutsidePillars() => playerAction = HandsAction.notInPillars;

    #endregion

    private void Update()
    {
        if (playerAction == HandsAction.notInPillars)
            return;

        ParticleSpawner();
        OVRInput.SetControllerVibration(.5f, .4f, OVRInput.Controller.RTouch);
        OVRInput.SetControllerVibration(.5f, .4f, OVRInput.Controller.LTouch);
    }

    private void ParticleSpawner ()
    {
        if (timer > timeLimit)
        {
            ResetTimer();
            Instantiate(electricParticles, spawnPoint.position, Quaternion.identity);
        }
        else
            timer += Time.deltaTime;
    }

    private void ResetTimer()
    {
        timer = 0;
        timeLimit = Random.Range(0.5f, 1.2f);
    }
}

