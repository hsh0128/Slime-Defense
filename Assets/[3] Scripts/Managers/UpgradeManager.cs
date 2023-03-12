using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : Singleton<UpgradeManager>
{
    private readonly int[,] UPGRADE_COST = { { 1,2,3,4,5 },{ 1,2,3,4,5 },{ 1,2,3,4,5 },
                                             { 5, 10, 15, -1, -1 },{ 5, 10, 15, -1, -1 },{ 5, 10, 15, -1, -1 } };

    public Canvas upgradeCanvas;
    public Text[] levelText;
    public Text[] costText;

    // 투사체, 광역, 히트스캔, 공속, 광역크기, 에너지 순
    private int[] _upgradeInfos;

    public float projDamageMultiplier
    {
        get
        {
            return 1f + (0.2f * _upgradeInfos[0]);
        }
    }

    public float areaDamageMultiplier
    {
        get
        {
            return 1f + (0.2f * _upgradeInfos[1]);
        }
    }

    public float hitscanDamageMultiplier
    {
        get
        {
            return 1f + (0.2f * _upgradeInfos[2]);
        }
    }

    public float atkSpeedMultiplier
    {
        get
        {
            return 1f / (1f + 0.05f * _upgradeInfos[3]);
        }
    }

    public float areaSizeMultiplier
    {
        get
        {
            return 1f + (0.1f * _upgradeInfos[4]);
        }
    }

    public float energyMutliplier
    {
        get
        {
            return 1f + (0.25f * _upgradeInfos[5]);
        }
    }

    protected override void Init()
    {
        base.Init();

        upgradeCanvas.gameObject.SetActive(false);
    }

    public void InitOnStartGame()
    {
        _upgradeInfos = new int[6];

        for (int i = 0; i < 6; i++)
        {
            _upgradeInfos[i] = 0;
        }

        upgradeCanvas.gameObject.SetActive(false);
        UpdateUI();
    }

    public void OpenUpgradeWindow()
    {
        upgradeCanvas.gameObject.SetActive(true);
    }

    public void CloseUpgradeWindow()
    {
        upgradeCanvas.gameObject.SetActive(false);
    }

    public void UpdateUI()
    {
        int maxLV = UPGRADE_COST.GetLength(1);

        for (int i = 0; i < 6; i++)
        {
            int lv = _upgradeInfos[i];

            levelText[i].text = "LV. " + lv.ToString();

            if (maxLV <= lv || UPGRADE_COST[i, lv] < 0) costText[i].text = "";
            else costText[i].text = "x " + UPGRADE_COST[i, lv].ToString() + " 필요";
        }
    }

    public void OnUpgradeButtonClicked(int index)
    {
        if (index >= _upgradeInfos.Count())
        {
            Debug.LogErrorFormat("잘못된 업그레이드 버튼 인덱스: {0}", index);
            
            return;
        }

        int lv = _upgradeInfos[index];
        int maxLV = UPGRADE_COST.GetLength(1);

        if (lv >= maxLV)
        {
            Debug.LogWarning("최대 레벨 달성(배열 크기 초과)");
            
            return;
        }

        int cost = UPGRADE_COST[index, lv];

        if (cost < 0)
        {
            Debug.LogWarning("최대 레벨 달성(비용 음수)");

            return;
        }

        if (cost > InGameManager.instance.coin)
        {
            Debug.Log("업그레이드 돈이 부족합니다.");

            return;
        }

        _upgradeInfos[index] += 1;

        InGameManager.instance.coin -= cost;

        UpdateUI();
    }
}