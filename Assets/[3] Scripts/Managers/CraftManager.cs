using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CraftManager : Singleton<CraftManager>
{
    private Dictionary<int, CraftQueries> _craftTable;
    private Dictionary<int, string[]> _craftResults;

    private string _craftPath
    {
        get
        {
            return Application.dataPath + "/Datas/Craftdata.json";
        }
    }

    protected override void Init()
    {
        base.Init();

        _craftTable = new Dictionary<int, CraftQueries>();
        _craftResults = new Dictionary<int, string[]>();

        if (File.Exists(_craftPath))
        {
            string jsonText = File.ReadAllText(_craftPath);

            CraftDatas data = JsonUtility.FromJson<CraftDatas>(jsonText);

            foreach(CraftData i in data.datas)
            {
                _craftTable.Add(i.key, new CraftQueries(i.counts, i.queries));
                _craftResults.Add(i.key, i.results);
            }
        } else
        {
            Debug.LogError("조합 데이터 파일을 찾을 수 없음");
        }
    }

    public int FindCraftableTurret(List<TurretModel> selected)
    {
        foreach (KeyValuePair<int, CraftQueries> table in _craftTable)
        {
            if (selected.Count != table.Value.craftCounts) continue;

            bool valid = true;
            bool[] crafted = new bool[selected.Count];

            foreach(CraftQuery query in table.Value.queries)
            {
                int cnt = query.counts;
                int pivot_index = -1;

                for (int i = 0; i < selected.Count; i++)
                {
                    if (crafted[i]) continue;
                    if (cnt == 0) break;

                    if (query.identityType == "rank")
                    {
                        if (query.IsSameRank(selected[i]))
                        {
                            cnt -= 1;
                            crafted[i] = true;
                        }
                    } else if (query.identityType == "index")
                    {
                        if (query.IsSameIndex(selected[i]))
                        {
                            cnt -= 1;
                            crafted[i] = true;
                        }
                    } else if (query.identityType == "rank_same")
                    {
                        if (query.IsSameRank(selected[i]))
                        {
                            if (pivot_index == -1)
                            {
                                pivot_index = selected[i].turretIndex;

                                cnt -= 1;
                                crafted[i] = true;
                            }

                            else if (pivot_index == selected[i].turretIndex)
                            {
                                cnt -= 1;
                                crafted[i] = true;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogErrorFormat("알 수 없는 쿼리: {0}", query.identityType);
                        valid = false;
                        break;
                    }
                }

                if (cnt > 0)
                {
                    valid = false;
                    break;
                }
            }

            if (!valid) continue;

            foreach(bool i in crafted)
            {
                if (!i)
                {
                    valid = false;
                    break;
                }
            }

            if (!valid) continue;

            return table.Key;
        }

        return -1;
    }

    public Object ReturnCraftResult(int index)
    {
        if (index == -1)
        {
            Debug.LogError("조합 에러: -1 인덱스(조합식 없음) 상태에서 조합을 시도했습니다.");
            return null;
        }

        if (!_craftResults.ContainsKey(index))
        {
            Debug.LogErrorFormat("조합 에러: 알 수 없는 인덱스({0})의 조합 시도", index);
        }

        System.Random rand = new System.Random();

        int num = rand.Next(0, _craftResults[index].Count());

        string retString = _craftResults[index][num];

        return Resources.Load("Turrets/" + retString);
    }
}
