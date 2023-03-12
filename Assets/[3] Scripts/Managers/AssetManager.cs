using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssetManager : Singleton<AssetManager>
{
    public Sprite[] selectedTurretThumbnail;
    public Sprite[] selectedTurretIcon;

    public Sprite[] selectedTurretBase;
    public Sprite deselectedTurretBase;

    protected override void Init()
    {
        base.Init();
    }

    public Sprite GetTurretImageThumbnail(int index)
    {
        if (index < 0 || index >= selectedTurretThumbnail.Count())
        {
            Debug.LogErrorFormat("썸네일이 없는 터렛 인덱스: {0}", index);
            return null;
        }

        return selectedTurretThumbnail[index];
    }

    public Sprite GetSelectedTurretBase(TurretType type)
    {
        switch(type)
        {
            case TurretType.D: 
                return selectedTurretBase[0];
            case TurretType.C:
                return selectedTurretBase[1];
            case TurretType.B:
                return selectedTurretBase[2];
            case TurretType.A:
                return selectedTurretBase[3];
            case TurretType.S:
                return selectedTurretBase[4];
            case TurretType.H:
                return selectedTurretBase[5];
            default:
                Debug.LogErrorFormat("알 수 없는 TurretType: {0}", type);
                return deselectedTurretBase;
        }
    }

    public Sprite GetTurretImageIcon(int index)
    {
        if (index < 0 || index >= selectedTurretThumbnail.Count())
        {
            Debug.LogErrorFormat("아이콘이 없는 터렛 인덱스: {0}", index);
            return null;
        }

        return selectedTurretIcon[index];
    }

    public string GetTurretNameByIndex(int index)
    {
        switch(index)
        {
            case 0:
                return "BaseTurret";
            case 1:
                return "SoldierTurret";
            case 2:
                return "TankTurret";
            case 3:
                return "ElectricTurret";
            case 4:
                return "ShotgunTurret";
            case 5:
                return "MachinegunTurret";
            case 6:
                return "CannonTurret";
            case 7:
                return "ImpTurret";
            case 8:
                return "FlaskTurret";
            case 9:
                return "DragonTurret";
            case 10:
                return "TreeTurret";
            case 11:
                return "GravityTurret";
            case 12:
                return "AirplaneTurret";
            default:
                Debug.LogErrorFormat("이름이 등록되지 않은 터렛 인덱스 {0}", index);
                return null;
        }
    }
}
