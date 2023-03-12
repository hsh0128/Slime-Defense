using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : Singleton<WaveManager>
{
    private readonly float PREPARE_DELAY = 15f;
    private readonly int WAVE_CLEAR_REWARD = 3;

    public Text waveLevelText;
    public Text remainPrepareTimeText;

    private int _nowLevel;
    private int _remainSlimeCount;
    private int _selectedWave;
    private float _remainDelay;
    private WaveInfo _nowWaveInfo;
    private State _state;

    private WaveInfo[] _showWave;
    private Dictionary<int, WaveInfo> _waveTable;
    
    public float remainDelay
    {
        get
        {
            return _remainDelay;
        }
        set
        {
            _remainDelay = value;

            remainPrepareTimeText.text = "���� �ð�: " + Mathf.CeilToInt(_remainDelay).ToString();
        }
    }

    public int nowLevel
    {
        get
        {
            return _nowLevel;
        }
        set
        {
            _nowLevel = value;

            if (waveLevelText != null) waveLevelText.text = "Wave : " + _nowLevel.ToString();
        }
    }

    private enum State
    {
        EXECUTE,
        PREPARE,
        GAMEOVER
    }

    private string _wavePath
    {
        get
        {
            return Application.dataPath + "/Datas/Wavedata.json";
        }
    }

    protected override void Init()
    {
        base.Init();

        _waveTable = new Dictionary<int, WaveInfo>();
        _showWave = new WaveInfo[2];

        if (File.Exists(_wavePath))
        {
            string jsonText = File.ReadAllText(_wavePath);

            WaveDatas data = JsonUtility.FromJson<WaveDatas>(jsonText);

            foreach (WaveData i in data.waves)
            {
                _waveTable.Add(i.key, new WaveInfo(i.counts, i.slimeInfos));
            }
        } else
        {
            Debug.LogError("���̺� ������ ������ ã�� �� ����");
        }

        InitOnStartGame();
    }

    public void InitOnStartGame()
    {
        nowLevel = 0;

        ShowSelectableWave();
    }

    private void Update()
    {
        switch(_state)
        {
            case State.PREPARE:
                remainDelay -= Time.deltaTime;

                if (remainDelay <= 0f)
                {
                    ExecuteWave();
                }

                break;
            default:
                break;
        }
    }

    public void SkipWave()
    {
        if (_state != State.PREPARE) return;

        remainDelay = 0f;
    }

    /// <summary>
    /// �Ƹ� �������� �׾��� �� ȣ��� �޼���
    /// </summary>
    public void OnSlimeDead()
    {
        if (_state == State.PREPARE) return;
        if (_state == State.GAMEOVER) return;

        _remainSlimeCount -= 1;

        remainPrepareTimeText.text = "���� ������: " + _remainSlimeCount.ToString();

        if (_remainSlimeCount <= 0)
        {
            ShowSelectableWave();
        }
    }

    public void ShowSelectableWave()
    {
        nowLevel += 1;

        if (nowLevel != 1) InGameManager.instance.coin += WAVE_CLEAR_REWARD;

        //// 1�� ���̺� ���� �� ǥ��
        //_showWave[0] = _waveTable[0];

        //// 2�� ���̺� ���� �� ǥ��
        //_showWave[1] = _waveTable[1];

        //_selectedWave = -1;
        _remainDelay = PREPARE_DELAY;
        _state = State.PREPARE;
    }

    /// <summary>
    /// �����̰� ������ �� ȣ��Ǵ� �޼�����
    /// </summary>
    public void ExecuteWave()
    {
        if (_state == State.GAMEOVER) return;

        _state = State.EXECUTE;

        _nowWaveInfo = _waveTable[nowLevel];

        _remainSlimeCount = _nowWaveInfo.totalSlimeCount;

        remainPrepareTimeText.text = "���� ������: " + _remainSlimeCount.ToString();

        StartCoroutine(SlimeCreateCoroutine());
    }

    private IEnumerator SlimeCreateCoroutine()
    {
        foreach (WaveSlimeInfo i in _nowWaveInfo.slimeInfos)
        {
            if (_state == State.GAMEOVER) yield break;

            Object slime = Resources.Load("Slimes/" + i.slime);

            if (slime == null)
            {
                Debug.LogErrorFormat("������ {0}�� ã�� �� �����ϴ�. �ش� ������ ������ �ǳʶݴϴ�.", i.slime);
                continue;
            }

            for (int j = 0; j < i.slimeCount; j++)
            {
                InGameManager.instance.CreateSlime(slime);

                yield return new WaitForSeconds(0.2f);
            }
        }

        yield return null;
    }

    public void OnGameOver()
    {
        _state = State.GAMEOVER;
    }
}
