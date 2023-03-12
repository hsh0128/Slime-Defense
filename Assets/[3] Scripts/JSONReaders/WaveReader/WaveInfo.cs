public class WaveInfo
{
    public int totalSlimeCount;
    public WaveSlimeInfo[] slimeInfos;

    public WaveInfo(int totalSlimeCount, WaveSlimeInfo[] slimeInfos)
    {
        this.totalSlimeCount = totalSlimeCount;
        this.slimeInfos = slimeInfos;
    }
}
