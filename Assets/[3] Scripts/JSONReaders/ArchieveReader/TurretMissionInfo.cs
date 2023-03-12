public class TurretMissionInfo
{
    public TurretMission info;
    public bool isArchieved;
    public int nowProgress;

    public TurretMissionInfo(TurretMission info, bool isArchieved)
    {
        this.info = info;
        this.isArchieved = isArchieved;
        nowProgress = 0;
    }
}
