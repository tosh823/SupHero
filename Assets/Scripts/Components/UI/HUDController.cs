using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SupHero;
using SupHero.Model;

namespace SupHero.Components.UI {

    public enum UILocation {
        TOP,
        BOTTOM
    }

    // TODO!!!
    // Need to remake TimerController class
    // and interaction with it

    public class HUDController : MonoBehaviour {

        public GameObject timerPrefab;
        public GameObject playerUIPrefabLeft;
        public GameObject playerUIPrefabRight;
        public GameObject popUpPrefab;

        private RectTransform rectTransform;
        private TimerController timerInstance;
        private List<GameObject> playerUIs;

        void Start() {
            rectTransform = gameObject.GetComponent<RectTransform>();
            playerUIs = new List<GameObject>();
        }

        public void updateTimer(float time) {
            timerInstance.updateTimer(getTime(time));
        }

        public void createTimer() {
            GameObject instance = Instantiate(timerPrefab) as GameObject;
            instance.transform.SetParent(transform, false);
            timerInstance = instance.GetComponent<TimerController>();
        }

        public void showMessage(string message, float fadeOut = 2f) {
            GameObject popup = Instantiate(popUpPrefab);
            popup.transform.SetParent(transform, false);
            popup.GetComponent<PopUpMessage>().showWithMessage(message, fadeOut);
        }

        private string getTime(float time) {
            int rounded = Mathf.FloorToInt(time);
            int minutes = rounded / 60;
            int seconds = rounded % 60;
            return string.Format("{0}:{1:D2}", minutes, seconds);
        }

        public GameObject createUIforPlayer(Player player) {
            GameObject hud;
            switch (player.number) {
                case 1:
                    hud = Instantiate(playerUIPrefabLeft);
                    hud.transform.SetParent(transform, false);
                    positionUI(hud, UILocation.BOTTOM);
                    hud.GetComponent<PlayerUIController>().setPlayer(player);
                    break;
                case 2:
                    hud = Instantiate(playerUIPrefabLeft);
                    hud.transform.SetParent(transform, false);
                    positionUI(hud, UILocation.TOP);
                    hud.GetComponent<PlayerUIController>().setPlayer(player);
                    break;
                case 3:
                    hud = Instantiate(playerUIPrefabRight);
                    hud.transform.SetParent(transform, false);
                    positionUI(hud, UILocation.TOP);
                    hud.GetComponent<PlayerUIController>().setPlayer(player);
                    break;
                case 4:
                    hud = Instantiate(playerUIPrefabRight);
                    hud.transform.SetParent(transform, false);
                    positionUI(hud, UILocation.BOTTOM);
                    hud.GetComponent<PlayerUIController>().setPlayer(player);
                    break;
                default:
                    hud = new GameObject();
                    break;
            }
            playerUIs.Add(hud);
            return hud;
        }

        public void clearPlayerUIs() {
            foreach (GameObject ui in playerUIs) {
                Destroy(ui.gameObject);
            }
            playerUIs.Clear();
        }

        public GameObject findUIforPlayer(Player player) {
            foreach (GameObject ui in playerUIs) {
                PlayerUIController pc = ui.GetComponent<PlayerUIController>();
                if (pc.player.number == player.number) {
                    // Found, return existing UI
                    pc.setPlayer(player);
                    return ui;
                }
            }
            // Not found, create one
            return createUIforPlayer(player);
        }

        private void positionUI(GameObject ui, UILocation location) {
            RectTransform rt = ui.GetComponent<RectTransform>();
            float w = rectTransform.sizeDelta.x;
            float h = rectTransform.sizeDelta.y;
            float uiWidth = rt.sizeDelta.x * rt.localScale.x;
            float uiHeight = rt.sizeDelta.y * rt.localScale.y;
            float pos;
            switch (location) {
                case UILocation.BOTTOM:
                    pos = uiHeight / 2 - h / 2;
                    rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, pos);
                    break;
                case UILocation.TOP:
                    pos = h / 2 - uiHeight / 2;
                    rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, pos);
                    break;
                default:
                    break;
            }
        }
    }
}
