using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : Singleton<SettingManager>
{
    public Canvas settingCanvas;
    public GameObject graphicSettings, audioSettings, gameplaySettings;

    public Dropdown resolutionDropdown, screenModeDropdown;

    private Dictionary<int, Resolution> _validResolution;

    protected override void Init()
    {
        base.Init();

        graphicSettings.SetActive(true);
        audioSettings.SetActive(false);
        gameplaySettings.SetActive(false);

        CloseSettings();

        _validResolution = new Dictionary<int, Resolution>();

        InitResolutionDropdown();
        InitScreenModeDropdown();
    }

    private void InitResolutionDropdown()
    {
        List<Resolution> availables = new List<Resolution>();

        foreach(Resolution r in Screen.resolutions)
        {
            bool flag = true;

            if (r.width < 1280 || r.height < 720) flag = false;

            foreach(Resolution i in availables)
            {
                if (i.width == r.width && i.height == r.height)
                {
                    flag = false;
                    break;
                }
            }

            if (flag) availables.Add(r);
        }

        resolutionDropdown.ClearOptions();

        Resolution cr = Screen.currentResolution;
        int startVal = -1;

        for (int i = 0; i < availables.Count; i++)
        {
            _validResolution.Add(i, availables[i]);
            resolutionDropdown.options.Add(new Dropdown.OptionData(string.Format("{0} x {1}", availables[i].width, availables[i].height)));

            if (availables[i].width == cr.width && availables[i].height == cr.height)
            {
                startVal = i;
            }
        }

        if (startVal != -1) resolutionDropdown.value = startVal;
    }

    private void InitScreenModeDropdown()
    {
        screenModeDropdown.ClearOptions();

        screenModeDropdown.options.Add(new Dropdown.OptionData("전체 화면"));
        screenModeDropdown.options.Add(new Dropdown.OptionData("창 모드"));

        if (Screen.fullScreen) screenModeDropdown.value = 0;
        else screenModeDropdown.value = 1;
    }

    public void OnResolutionDropdownValueChanged(int index)
    {
        Resolution r = _validResolution[index];

        Screen.SetResolution(r.width, r.height, Screen.fullScreen);
    }

    public void OnScreenModeDropdownValueChanged(int index)
    {
        bool mode = index == 0 ? true : false;
        Resolution r = Screen.currentResolution;

        Screen.SetResolution(r.width, r.height, mode);
    }

    public void OpenSettings()
    {
        settingCanvas.gameObject.SetActive(true);
    }

    public void CloseSettings()
    {
        settingCanvas.gameObject.SetActive(false);
    }

    public void OnSettingModeButtonClicked(int index)
    {
        graphicSettings.SetActive(false);
        audioSettings.SetActive(false);
        gameplaySettings.SetActive(false);

        switch(index)
        {
            case 0:
                graphicSettings.SetActive(true);
                break;
            case 1:
                audioSettings.SetActive(true);
                break;
            case 2:
                gameplaySettings.SetActive(true);
                break;
            default:
                Debug.LogErrorFormat("알 수 없는 설정 버튼 인덱스: {0}", index);
                break;
        }
    }
}
