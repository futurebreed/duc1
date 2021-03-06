﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public enum SceneMapping
    {
        MainMenu = 0,
        IntroCutscene = 1,
        MainGame = 2,
    }

    class MainMenuController : MonoBehaviour
    {
        public GameObject StartButton;
        public GameObject DuctTape1Button;
        public GameObject BackFromCreditsButton;
        public GameObject CharacterSelectionFlow;
        public GameObject CreditsFlow;
        public GameObject MainMenuFlow;

        public void OnCancel()
        {
            CharacterSelectionFlow.SetActive(false);
            CreditsFlow.SetActive(false);
            MainMenuFlow.SetActive(true);

            EventSystem.current.SetSelectedGameObject(StartButton);
        }

        public void TriggerCharacterSelection(bool show)
        {
            CharacterSelectionFlow.SetActive(show);
            MainMenuFlow.SetActive(!show);
            EventSystem.current.SetSelectedGameObject(show ? DuctTape1Button : StartButton);
        }

        public void TriggerCredits(bool show)
        {
            CreditsFlow.SetActive(show);
            MainMenuFlow.SetActive(!show);
            EventSystem.current.SetSelectedGameObject(show ? BackFromCreditsButton : StartButton);
        }

        public void LoadMainGame(string ColorChoice = "Duct")
        {
            PlayerPrefs.SetString("Color", ColorChoice);
            SceneManager.LoadScene((int)SceneMapping.IntroCutscene);
        }

        public void ExitApplication()
        {
            Application.Quit();
        }

        public void LoadScene(SceneMapping scene)
        {
            SceneManager.LoadScene((int)scene);
        }

        public void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }
    }
}