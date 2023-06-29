using UnityEngine;

public class AddSpacing : MonoBehaviour
{
    public bool run = false;


    [Space]
    [SerializeField] Vector3 startPos;
    [SerializeField] Transform[] objects;

    [Space]
    [Range(0, 3f)]
    [SerializeField] float distanceToAdd;
    [SerializeField] float newPos;

    [Space]
    [SerializeField] Axis axis;
    public enum Axis
    {
        x,
        y,
        z
    }


    void Update()
    {
        if (!run)
        {
            startPos = objects[0].localPosition;
            return;
        }

        objects[0].localPosition = startPos;
        newPos = startPos.z;
        for (int i = 1; i < objects.Length; i++)
        {
            newPos -= distanceToAdd;
            objects[i].localPosition = new Vector3(objects[i].localPosition.x, objects[i].localPosition.y, newPos);
        }
    }
}
