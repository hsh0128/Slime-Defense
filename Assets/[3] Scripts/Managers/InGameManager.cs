using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameManager : Singleton<InGameManager>
{
    #region Variable
    private readonly int START_COIN = 4;
    private readonly float CLICK_CAP_LENGTH = 0.5f;
    private readonly int BASE_SUMMON_COST = 1;
    private readonly int TURRET_MOVE_COST = 1;
    private readonly string[] BASE_D_RANK_TURRETS = {"BaseTurret", "ShotgunTurret", "MachinegunTurret",
                                                     "CannonTurret"};

    [Header("Window")]
    public GameObject gameOverUI;
    public GameObject achieveWindowUI, upgradeWindowUI;

    [Header("Menus")]
    public GameObject turretMenuUI;
    public GameObject placeMenuGameObject, achieveMenuGameObject, upgradeMenuGameObject;

    [Header("ETC")]
    public GameObject gridSelector;
    public SpriteRenderer gridSelectorIcon;
    public Text coinText;

    [Header("Selectionbox Contents")]
    public RectTransform SelectionBoxRect;
    public GameObject SelectedTurretsShower;
    public Button[] selectedTurretButtons;
    public Image[] selectedTurretImages;
    public Text SelectionUIPageText;
    public Text ToggleSelectedUIButtonText;

    [Header("Animators")]
    public Animator selectedTurretsAnimator;
    public Animator turretMenuAnimator;

    [Header("System")]
    public GameObject nexusPrefab;
    public Vector2 slimeStartPosition;
    public List<Vector2> slimePath;

    [HideInInspector]
    public GameObject nexus;

    private Object[] _equippedTurret;
    private Object _selectedSummonTurret;
    private NexusModel _nexusInfo;
    private stateInfo _state;
    private menuStateInfo _menuState;
    private int _coinCount;
    private int _nowPage, _maxPage;
    private int _nowCraftIndex;
    private Vector2 _dragStartMousePosition;
    private Vector2Int _mouseGridPoint;
    private Vector2Int _selectedMoveTurretPos;
    private bool _isSelectedUIFolded;
    private bool _isTurretMenuUIFolded;
    private bool _isRandomTurret;

    private Dictionary<Vector2Int, TurretModel> _turretObjects;
    private List<TurretModel> _selectedTurrets;
    private HashSet<Vector2Int> _railPosition;

    public int coin
    {
        get
        {
            return _coinCount;
        }
        set
        {
            _coinCount = value;
            coinText.text = "x " + _coinCount.ToString();
        }
    }

    public int maxPage
    {
        get
        {
            return _maxPage;
        }
        set
        {
            if (value <= 0) return;

            _maxPage = value;
            SelectionUIPageText.text = _nowPage.ToString() + " / " + _maxPage.ToString();
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
            if (value <= 0) return;
            if (value > _maxPage) return;

            _nowPage = value;
            SelectionUIPageText.text = _nowPage.ToString() + " / " + _maxPage.ToString();

            UpdateSelectedTurretImage();
        }
    }

    public bool isRandomTurret
    {
        get
        {
            return _isRandomTurret;
        }
        set
        {
            _isRandomTurret = value;
        }
    }

    public menuStateInfo menuState
    {
        get
        {
            return _menuState;
        }
        set
        {
            if (_menuState != value) OnMenuStateChanged(value);

            _menuState = value;
        }
    }

    public enum stateInfo
    {
        IDLE,
        PLACE,
        FREEZE,
        REMOVE,
        DRAG,
        CRAFT,
        MOVESELECT,
        MOVETARGET,
        GAMEOVER
    }

    public enum menuStateInfo
    {
        PLACE,
        ACHIEVE,
        UPGRADE
    }
    #endregion

    #region Monobehavior
    protected override void Init()
    {
        base.Init();

        _equippedTurret = new Object[4];

        _equippedTurret[0] = Resources.Load("Turrets/AirplaneTurret");
        _equippedTurret[1] = Resources.Load("Turrets/ElectricTurret");
        _equippedTurret[2] = Resources.Load("Turrets/GravityTurret");
        _equippedTurret[3] = Resources.Load("Turrets/BaseTurretTest");

        if (selectedTurretButtons.Count() != selectedTurretImages.Count())
            Debug.LogError("선택 UI의 버튼 갯수와 이미지 갯수 불일치 발견(InGameManager Inspector 확인)");

        _railPosition = new HashSet<Vector2Int>();

        Vector2Int railPos = new Vector2Int(Mathf.CeilToInt(slimeStartPosition.x), Mathf.CeilToInt(slimeStartPosition.y));

        foreach(Vector2 p in slimePath)
        {
            Vector2Int targetPos = new Vector2Int(Mathf.CeilToInt(p.x), Mathf.CeilToInt(p.y));

            if (railPos.x != targetPos.x)
            {
                int d = targetPos.x > railPos.x ? 1 : -1;

                for (int i = railPos.x; i != targetPos.x; i += d)
                {
                    _railPosition.Add(new Vector2Int(i, targetPos.y));
                }
            } else
            {
                int d = targetPos.y > railPos.y ? 1 : -1;

                for (int i = railPos.y; i != targetPos.y; i += d)
                {
                    _railPosition.Add(new Vector2Int(targetPos.x, i));
                }
            }

            railPos = targetPos;
        }

        _railPosition.Add(railPos);

        InitOnStartGame();
    }

    /// <summary>
    /// Nexus를 Isntantiate해야 하므로, 로딩이 다 끝나고 씬이 완전이 전환된 후 호출하세요
    /// </summary>
    public void InitOnStartGame()
    {
        _state = stateInfo.IDLE;
        menuState = menuStateInfo.PLACE;

        _isTurretMenuUIFolded = true;
        gameOverUI.SetActive(false);

        _turretObjects = new Dictionary<Vector2Int, TurretModel>();
        _selectedTurrets = new List<TurretModel>();

        coin = START_COIN;
        maxPage = 1;
        nowPage = 1;
        _isSelectedUIFolded = false;
        _selectedSummonTurret = null;

        if (nexus != null)
        {
            Debug.LogError("nexus가 Null이 아닙니다. Inspector가 채워져 있다면, 비워주세요");
            Destroy(nexus);
        }

        nexus = Instantiate(nexusPrefab, new Vector3(8, -3.5f, 0), new Quaternion());
        _nexusInfo = nexus.GetComponent<NexusModel>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnRandomTurretPlaceButtonPressed();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            OnTurretCraftButtonPressed();
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _mouseGridPoint = new Vector2Int(Mathf.CeilToInt(mousePos.x), Mathf.CeilToInt(mousePos.y));

        gridSelector.transform.position = new Vector3(_mouseGridPoint.x - 0.5f, _mouseGridPoint.y - 0.5f, gridSelector.transform.position.z);

        if (Input.GetMouseButtonDown(0)) OnLeftClickStart();
        else if (Input.GetMouseButton(0)) OnLeftClickHold();
        else if (Input.GetMouseButtonUp(0)) OnLeftClickEnd();

        if (Input.GetMouseButtonDown(1)) OnRightClickStart();
    }
    #endregion

    #region Selection
    public void SelectTurret(TurretModel turret)
    {
        SelectTurretWithoutUIUpdate(turret);

        UpdateSelectionUI();
    }

    public void SelectTurretWithoutUIUpdate(TurretModel turret)
    {
        if (_selectedTurrets.Contains(turret))
        {
            return;
        }

        _selectedTurrets.Add(turret);
    }

    public void DeselectTurret(TurretModel turret)
    {
        DeselectTurretWithoutUIUpdate(turret);

        UpdateSelectionUI();
    }

    public void DeselectTurretWithoutUIUpdate(TurretModel turret)
    {
        if (!_selectedTurrets.Contains(turret))
        {
            return;
        }

        _selectedTurrets.Remove(turret);
    }

    public void ClearTurretSelection()
    {
        ClearTurretSelectionWithoutUIUpdate();

        UpdateSelectionUI();
    }

    public void ClearTurretSelectionWithoutUIUpdate()
    {
        _selectedTurrets.Clear();
    }

    public bool isSelectedTurret(TurretModel turret)
    {
        return _selectedTurrets.Contains(turret);
    }

    private void UpdateSelectionUI()
    {
        if (_selectedTurrets.Count == 0) maxPage = 1;
        else maxPage = (_selectedTurrets.Count - 1) / selectedTurretButtons.Count() + 1;
        nowPage = 1;

        foreach(KeyValuePair<Vector2Int, TurretModel> t in _turretObjects)
        {
            if (_selectedTurrets.Contains(t.Value)) t.Value.OnSelected();
            else t.Value.OnDeselected();
        }
    }
    #endregion

    #region CreateSomething
    public void CreateSlime(Object slime)
    {
        Instantiate(slime, slimeStartPosition, new Quaternion());
    }

    public bool PlaceTurret(Object turret, Vector2Int position, int cost)
    {
        if (coin < cost)
        {
            Debug.Log("돈이 부족합니다. (설치 취소)");

            _state = stateInfo.IDLE;

            return false;
        }

        if (_turretObjects.ContainsKey(position))
        {
            Debug.Log("이미 설치되어 있는 위치");
            return false;
        }

        if (_railPosition.Contains(position))
        {
            Debug.Log("레일 위에 설치할 수 없습니다.");
            return false;
        }

        GameObject turretCache = (GameObject)Instantiate(turret, new Vector2(position.x - 0.5f, position.y - 0.5f), new Quaternion());
        TurretModel turretData = turretCache.GetComponent<TurretModel>();

        _turretObjects.Add(position, turretData);

        turretData.InitPosition(position.x, position.y);

        ArchieveManager.instance.TurretPlaceObserve(_turretObjects);

        coin -= cost;

        return true;
    }

    public bool RemoveTurret(Vector2Int position)
    {
        if (!_turretObjects.ContainsKey(position))
        {
            Debug.Log("설치되어 있는 터렛 없음");
            return false;
        }

        Destroy(_turretObjects[position].gameObject);
        _turretObjects.Remove(position);

        return true;
    }

    public bool MoveTurret(Vector2Int turretPos, Vector2Int destPos)
    {
        if (!_turretObjects.ContainsKey(turretPos)) return false;

        if (_turretObjects.ContainsKey(destPos)) return false;
        if (_railPosition.Contains(destPos)) return false;

        if (coin < TURRET_MOVE_COST)
        {
            return false;
        }

        Object tur = Resources.Load("Turrets/" + AssetManager.instance.GetTurretNameByIndex(_turretObjects[turretPos].turretIndex));

        RemoveTurret(turretPos);
        PlaceTurret(tur, destPos, 0);

        coin -= TURRET_MOVE_COST;

        return true;
    }
    #endregion

    #region UI_INPUT
    private void OnLeftClickStart()
    {
        if (_state == stateInfo.GAMEOVER) return;

        if (_state == stateInfo.FREEZE) return;

        if (_state == stateInfo.PLACE)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("UI 요소 클릭");
                return;
            }

            if (isRandomTurret)
            {
                System.Random rand = new System.Random();

                int idx = rand.Next(0, BASE_D_RANK_TURRETS.Length);

                Object nowTurret = Resources.Load("Turrets/" + BASE_D_RANK_TURRETS[idx]);

                bool isSucceed = PlaceTurret(nowTurret, _mouseGridPoint, BASE_SUMMON_COST);

                if (!isSucceed) return;

                isRandomTurret = false;
                _state = stateInfo.IDLE;

                return;
            }

            bool isSucceed2 = PlaceTurret(_selectedSummonTurret, _mouseGridPoint, 1);

            if (!isSucceed2) return;

            _state = stateInfo.IDLE;

            return;
        }

        if (_state == stateInfo.CRAFT)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("UI 요소 클릭");
                return;
            }

            Object tur = CraftManager.instance.ReturnCraftResult(_nowCraftIndex);

            if (tur == null)
            {
                Debug.LogErrorFormat("에러: 인덱스 {0}에 대응하는 터렛 결과가 없습니다. CraftManager.ReturnCraftResult 메서드를 확인하세요", _nowCraftIndex);
                return;
            }

            bool isSucceed = PlaceTurret(tur, _mouseGridPoint, 0);

            if (!isSucceed) return;

            foreach (TurretModel i in _selectedTurrets)
            {
                RemoveTurret(new Vector2Int(i.gridX, i.gridY));
            }

            _state = stateInfo.IDLE;
            ClearTurretSelection();

            return;
        }
        
        if (_state == stateInfo.REMOVE)
        {
            Debug.Log("터렛 제거 시도");
            RemoveTurret(_mouseGridPoint);
            _state = stateInfo.IDLE;
            return;
        }

        if (_state == stateInfo.MOVESELECT)
        {
            Debug.Log("이동할 터렛 선택");

            if (!_turretObjects.ContainsKey(_mouseGridPoint))
            {
                return;
            }

            _selectedMoveTurretPos = _mouseGridPoint;
            _state = stateInfo.MOVETARGET;

            gridSelectorIcon.sprite = AssetManager.instance.GetTurretImageIcon(_turretObjects[_selectedMoveTurretPos].turretIndex);

            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            _state = stateInfo.IDLE;

            return;
        }

        _state = stateInfo.DRAG;

        SelectionBoxRect.sizeDelta = Vector2.zero;
        SelectionBoxRect.gameObject.SetActive(true);
        _dragStartMousePosition = Input.mousePosition;
    }

    private void OnLeftClickHold()
    {
        if (_state != stateInfo.DRAG) return;

        ResizeSelectionBox();
    }

    private void OnLeftClickEnd()
    {
        if (_state == stateInfo.MOVETARGET)
        {
            Debug.Log("해당 위치로 이동 시도");

            gridSelectorIcon.sprite = null;

            bool isSucceed = MoveTurret(_selectedMoveTurretPos, _mouseGridPoint);

            if (!isSucceed) return;

            _state = stateInfo.IDLE;

            return;
        }

        if (_state != stateInfo.DRAG) return;

        float ratio = Screen.height / (Camera.main.orthographicSize * 2);

        float rectLen = Mathf.Max(SelectionBoxRect.sizeDelta.x / ratio, SelectionBoxRect.sizeDelta.y / ratio);

        if (rectLen < CLICK_CAP_LENGTH)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (!_turretObjects.ContainsKey(_mouseGridPoint)) return;

                DeselectTurret(_turretObjects[_mouseGridPoint]);

                SelectionBoxRect.sizeDelta = Vector2.zero;
                SelectionBoxRect.gameObject.SetActive(false);

                return;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (!_turretObjects.ContainsKey(_mouseGridPoint)) return;

                SelectTurret(_turretObjects[_mouseGridPoint]);

                SelectionBoxRect.sizeDelta = Vector2.zero;
                SelectionBoxRect.gameObject.SetActive(false);

                return;
            }

            ClearTurretSelectionWithoutUIUpdate();

            if (_turretObjects.ContainsKey(_mouseGridPoint)) SelectTurretWithoutUIUpdate(_turretObjects[_mouseGridPoint]);

            UpdateSelectionUI();

            SelectionBoxRect.sizeDelta = Vector2.zero;
            SelectionBoxRect.gameObject.SetActive(false);

            return;
        }

        Bounds bound = new Bounds(SelectionBoxRect.anchoredPosition, SelectionBoxRect.sizeDelta);

        SelectionBoxRect.sizeDelta = Vector2.zero;
        SelectionBoxRect.gameObject.SetActive(false);

        foreach(KeyValuePair<Vector2Int, TurretModel> turret in _turretObjects)
        {
            Vector2 turretPos = Camera.main.WorldToScreenPoint(turret.Value.transform.position);

            if (turretPos.x > bound.min.x && turretPos.y > bound.min.y 
                && turretPos.x < bound.max.x && turretPos.y < bound.max.y)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    DeselectTurretWithoutUIUpdate(turret.Value);
                } else
                {
                    SelectTurretWithoutUIUpdate(turret.Value);
                }
            } else
            {
                if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
                {
                    DeselectTurretWithoutUIUpdate(turret.Value);
                }
            }
        }

        Debug.Log("UI 업데이트");
        _state = stateInfo.IDLE;
        UpdateSelectionUI();
    }

    private void OnRightClickStart()
    {
        Debug.Log("기본 상태로 변경");
        _state = stateInfo.IDLE;
        _selectedSummonTurret = null;
    }

    public void OnTurretPlaceButtonPressed(int index)
    {
        if (_equippedTurret.Count() <= index)
        {
            Debug.LogWarning("잘못된 인덱스를 가진 터렛(장비중인 것 중)");
            return;
        }

        Debug.Log("터렛 배치 상태로 변경");
        _state = stateInfo.PLACE;
        _selectedSummonTurret = _equippedTurret[index];
        isRandomTurret = false;
    }

    public void OnRandomTurretPlaceButtonPressed()
    {
        Debug.Log("랜덤 터렛 배치 상태로 변경");
        _state = stateInfo.PLACE;
        isRandomTurret = true;
    }

    public void OnTurretRemoveButtonPressed()
    {
        Debug.Log("터렛 제거 상태로 변경");
        _state = stateInfo.REMOVE;
    }

    public void OnTurretCraftButtonPressed()
    {
        _nowCraftIndex = CraftManager.instance.FindCraftableTurret(_selectedTurrets);

        if (_nowCraftIndex == -1) return;

        _state = stateInfo.CRAFT;
    }

    public void OnTurretMoveButtonPressed()
    {
        Debug.Log("터렛 이동 상태로 변경");
        _state = stateInfo.MOVESELECT;
    }
    #endregion

    #region UI_Show
    public void ToggleMenuUI()
    {
        _isTurretMenuUIFolded = !_isTurretMenuUIFolded;

        turretMenuAnimator.SetBool("isFolded", _isTurretMenuUIFolded);
    }

    public void OpenMenuUI()
    {
        _isTurretMenuUIFolded = false;
        turretMenuAnimator.SetBool("isFolded", false);
    }

    public void CloseMenuUI()
    {
        _isTurretMenuUIFolded = true;
        turretMenuAnimator.SetBool("isFolded", true);
    }

    public void ChangeMenuState(menuStateInfo s)
    {
        menuState = s;
    }

    public void OnMenuStateChanged(menuStateInfo s)
    {
        placeMenuGameObject.SetActive(false);
        achieveMenuGameObject.SetActive(false);
        upgradeMenuGameObject.SetActive(false);

        switch(s)
        {
            case menuStateInfo.PLACE:
                placeMenuGameObject.SetActive(true);
                break;
            case menuStateInfo.ACHIEVE:
                achieveMenuGameObject.SetActive(true);
                break;
            case menuStateInfo.UPGRADE:
                upgradeMenuGameObject.SetActive(true);
                break;
            default:
                Debug.LogErrorFormat("지정되지 않은 state: {0}", s);
                break;
        }
    }

    private void ResizeSelectionBox()
    {
        float width = Input.mousePosition.x - _dragStartMousePosition.x;
        float height = Input.mousePosition.y - _dragStartMousePosition.y;

        SelectionBoxRect.anchoredPosition = _dragStartMousePosition + new Vector2(width / 2, height / 2);
        SelectionBoxRect.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
    }

    private void UpdateSelectedTurretImage()
    {
        int pivot = selectedTurretButtons.Count() * (nowPage - 1);

        int counts = _selectedTurrets.Count;

        for (int i = 0; i < selectedTurretButtons.Count(); i++)
        {
            if (pivot + i >= counts)
            {
                selectedTurretButtons[i].gameObject.SetActive(false);
                continue;
            }

            selectedTurretButtons[i].gameObject.SetActive(true);
            selectedTurretImages[i].sprite = AssetManager.instance.GetTurretImageThumbnail(_selectedTurrets[i + pivot].turretIndex);
        }
    }

    public void ToggleUI()
    {
        if (_isSelectedUIFolded)
        {
            ToggleSelectedUIButtonText.text = "▲";
            selectedTurretsAnimator.SetBool("isFolded", false);
            _isSelectedUIFolded = false;
        } else
        {
            ToggleSelectedUIButtonText.text = "▼";
            selectedTurretsAnimator.SetBool("isFolded", true);
            _isSelectedUIFolded = true;
        }
    }
    #endregion

    #region Systems
    public void GetCoin(int value)
    {
        Debug.LogFormat("{0}만큼 코인 획득", value);
        coin += value;
    }

    public void GiveDamageToNexus(float damage)
    {
        if (_nexusInfo == null) return;

        _nexusInfo.GetDamage(damage, DamageType.FIXED);
    }

    public void ExecuteGameOver()
    {
        gameOverUI.SetActive(true);
        _state = stateInfo.GAMEOVER;
        WaveManager.instance.OnGameOver();
    }
    #endregion
}
