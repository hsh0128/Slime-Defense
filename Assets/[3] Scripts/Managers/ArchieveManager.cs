using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ArchieveManager : Singleton<ArchieveManager>
{
    public Canvas archiveCanvas;

    public Text[] indexText;
    public Text[] descText;
    public Text[] progressText;
    public Text[] rewardText;

    private ArchiveCategory _archiveCategory;
    private int _nowPage, _maxPage;
    private int _slimeKills;

    private Dictionary<int, TurretMissionInfo> _turretMission;
    private Dictionary<int, SlimeKillMissionInfo> _slimeKillMission;

    private enum ArchiveCategory
    {
        TURRET,
        KILL
    }

    private string _archievePah 
    {
        get
        {
            return Application.dataPath + "/Datas/Archievedata.json";
        }
    }

    public int nowPage
    {
        get
        {
            return _nowPage;
        }
        set
        {
            if (value > _maxPage)
            {
                _nowPage = _maxPage;
                WindowContentUpdate();

                return;
            }
            if (value < 0)
            {
                _nowPage = 0;
                WindowContentUpdate();

                return;
            }

            _nowPage = value;

            WindowContentUpdate();
        }
    }

    public int slimeKills
    {
        get
        {
            return _slimeKills;
        }
        set
        {
            _slimeKills = value;
        }
    }

    protected override void Init()
    {
        base.Init();

        archiveCanvas.gameObject.SetActive(false);

        _turretMission = new Dictionary<int, TurretMissionInfo>();
        _slimeKillMission = new Dictionary<int, SlimeKillMissionInfo>();
        slimeKills = 0;

        if (File.Exists(_archievePah))
        {
            string jsonText = File.ReadAllText(_archievePah);

            ArchieveDatas datas = JsonUtility.FromJson<ArchieveDatas>(jsonText);

            int idx = 0;

            foreach(TurretMission m in datas.turretMissions)
            {
                _turretMission.Add(idx, new TurretMissionInfo(m, false));
                idx += 1;
            }
            idx = 0;
            foreach(SlimeKillMission m in datas.slimeKillMissions)
            {
                _slimeKillMission.Add(idx, new SlimeKillMissionInfo(m, false));
                idx += 1;
            }
        } else
        {
            Debug.LogError("업적 데이터 파일을 찾을 수 없음");
        }
    }

    public void InitOnStartGame()
    {
        int[] keys = _turretMission.Keys.ToArray();

        foreach(int key in keys)
        {
            _turretMission[key].isArchieved = false;
        }

        keys = _slimeKillMission.Keys.ToArray();

        foreach(int key in keys)
        {
            _slimeKillMission[key].isArchieved = false;
        }

        slimeKills = 0;

        _archiveCategory = (ArchiveCategory)1;
        SwapCategory(0);

        archiveCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!archiveCanvas.gameObject.activeSelf) return;

        if (Input.mouseScrollDelta != Vector2.zero)
        {
            int cnt = (int)Input.mouseScrollDelta.y;

            nowPage -= cnt;
        }
    }

    public void OpenArchiveWindow()
    {
        archiveCanvas.gameObject.SetActive(true);
        
    }

    public void CloseArchiveWindow()
    {
        archiveCanvas.gameObject.SetActive(false);
    }

    public void SwapCategory(int categoryIndex)
    {
        ArchiveCategory category = (ArchiveCategory)categoryIndex;

        if (_archiveCategory == category) return;

        switch(category)
        {
            case ArchiveCategory.TURRET:
                _maxPage = (_turretMission.Count - 1) / 4;
                break;
            case ArchiveCategory.KILL:
                _maxPage = (_slimeKillMission.Count - 1) / 4;
                break;
            default:
                Debug.LogErrorFormat("알 수 없는 업적 카테고리: {0}", category);
                break;
        }

        _archiveCategory = category;

        nowPage = 0;
    }

    public void ArchivePageUp()
    {
        nowPage -= 1;
    }

    public void ArchivePageDown()
    {
        nowPage += 1;
    }

    public void WindowContentUpdate()
    {
        if (_archiveCategory == ArchiveCategory.TURRET)
        {
            TurretMissionInfo[] infos = new TurretMissionInfo[4];

            for (int i = 0; i < 4; i++)
            {
                if (!_turretMission.ContainsKey(i + nowPage * 4)) continue;

                infos[i] = _turretMission[i + nowPage * 4];
            }

            for (int i = 0; i < 4; i++)
            {
                if (infos[i] == null)
                {
                    indexText[i].text = "";
                    descText[i].text = "";
                    progressText[i].text = "";
                    rewardText[i].text = "";

                    continue;
                }

                indexText[i].text = (i + nowPage * 4).ToString();
                descText[i].text = infos[i].info.desc;

                if (infos[i].isArchieved) progressText[i].text = "√";
                else progressText[i].text = infos[i].nowProgress + " / " + infos[i].info.maxprogress;

                rewardText[i].text = infos[i].info.reward.ToString();
            }
        } else if (_archiveCategory == ArchiveCategory.KILL)
        {
            SlimeKillMissionInfo[] infos = new SlimeKillMissionInfo[4];

            for (int i = 0; i < 4; i++)
            {
                if (!_slimeKillMission.ContainsKey(i + nowPage * 4)) continue;

                infos[i] = _slimeKillMission[i + nowPage * 4];
            }

            for (int i = 0; i < 4; i++)
            {
                if (infos[i] == null)
                {
                    indexText[i].text = "";
                    descText[i].text = "";
                    progressText[i].text = "";
                    rewardText[i].text = "";

                    continue;
                }

                indexText[i].text = (i + nowPage * 4).ToString();
                descText[i].text = infos[i].info.desc;

                if (infos[i].isArchieved) progressText[i].text = "√";
                else progressText[i].text = slimeKills.ToString() + "/" + infos[i].info.count.ToString();

                rewardText[i].text = infos[i].info.reward.ToString();
            }
        }
    }

    public void SlimeKillObserve()
    {
        slimeKills += 1;

        foreach(KeyValuePair<int, SlimeKillMissionInfo> info in _slimeKillMission)
        {
            if (info.Value.isArchieved) continue;

            if (info.Value.info.count > slimeKills) continue;

            info.Value.isArchieved = true;
            InGameManager.instance.coin += info.Value.info.reward;
            Debug.Log("업적 달성: 슬라임 킬");
        }

        if (_archiveCategory == ArchiveCategory.KILL) WindowContentUpdate();
    }

    public void TurretPlaceObserve(Dictionary<Vector2Int, TurretModel> turretInfo)
    {
        int[] ranks = new int[7];
        Dictionary<int, int> indexes = new Dictionary<int, int>();
        int nowIdx;

        foreach(KeyValuePair<Vector2Int, TurretModel> info in turretInfo)
        {
            ranks[(int)info.Value.type] += 1;

            nowIdx = info.Value.turretIndex;
            if (!indexes.ContainsKey(nowIdx)) indexes.Add(nowIdx, 0);
            indexes[nowIdx] += 1;
        }

        foreach(KeyValuePair<int, TurretMissionInfo> info in _turretMission)
        {
            if (info.Value.isArchieved) continue;

            bool flag = true;
            int nowProgress = info.Value.info.maxprogress;

            foreach(TurretMissionCondition condition in info.Value.info.conditions)
            {
                if (condition.type == "rank")
                {
                    int idx = RankToInt(condition.rank);
                    
                    if (idx == -1)
                    {
                        Debug.LogErrorFormat("알 수 없는 랭크: {0}, 업적 달성을 취소합니다.", condition.rank);
                        flag = false;
                        break;
                    }

                    if (ranks[idx] < condition.count)
                    {
                        flag = false;
                        nowProgress -= (condition.count - ranks[idx]);
                    }
                } else if (condition.type == "index")
                {
                    if (!indexes.ContainsKey(condition.index))
                    {
                        flag = false;
                        nowProgress -= 1;
                        continue;
                    }

                    if (indexes[condition.index] < condition.count)
                    {
                        flag = false;
                        nowProgress -= 1;
                    }
                } else
                {
                    Debug.LogErrorFormat("알 수 없는 조건 타입: {0}, 해당 업적을 스킵합니다.", condition.type);
                    flag = false;
                    break;
                }
            }

            if (flag)
            {
                info.Value.isArchieved = true;
                InGameManager.instance.coin += info.Value.info.reward;
                Debug.LogFormat("업적 달성: {0}", info.Value.info.desc);
            } else
            {
                info.Value.nowProgress = nowProgress;
            }
        }

        if (_archiveCategory == ArchiveCategory.TURRET) WindowContentUpdate();
    }

    private int RankToInt(string rank)
    {
        switch (rank)
        {
            case "D":
                return 0;
            case "C":
                return 1;
            case "B":
                return 2;
            case "A":
                return 3;
            case "S":
                return 4;
            case "H":
                return 5;
            case "J":
                return 6;
            default:
                Debug.LogErrorFormat("알 수 없는 랭크: {0}", rank);
                return -1;
        }
    }
}
