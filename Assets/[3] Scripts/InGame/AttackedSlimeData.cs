using UnityEngine;

public class AttackedSlimeData
{
    public float remain;
    public SlimeModel slime;
    public GameObject slimeObject;

    public AttackedSlimeData(float remain, SlimeModel slime, GameObject slimeObject)
    {
        this.remain = remain;
        this.slime = slime;
        this.slimeObject = slimeObject;
    }
}
