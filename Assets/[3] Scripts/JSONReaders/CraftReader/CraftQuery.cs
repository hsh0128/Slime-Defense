[System.Serializable]
public class CraftQuery
{
    public string identityType;
    public string rank;
    public int index;
    public int counts;

    public bool IsSameRank(TurretModel turret)
    {
        switch(turret.type)
        {
            case TurretType.D:
                if (rank == "D") return true;
                else return false;
            case TurretType.C:
                if (rank == "C") return true;
                else return false;
            case TurretType.B:
                if (rank == "B") return true;
                else return false;
            case TurretType.A:
                if (rank == "A") return true;
                else return false;
            case TurretType.S:
                if (rank == "S") return true;
                else return false;
            case TurretType.H:
                if (rank == "H") return true;
                else return false;
            case TurretType.J:
                if (rank == "J") return true;
                else return false;
            default:
                return false;
        }
    }

    public bool IsSameIndex(TurretModel turret)
    {
        return turret.turretIndex == index;
    }
}
