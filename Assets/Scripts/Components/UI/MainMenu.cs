using UnityEngine;
using System.Collections;
using SupHero.Model;

namespace SupHero.Components.UI {
    public class MainMenu : MonoBehaviour {

        public PlayerCard[] cards;

		void Start() {
            Data.Instance.createSession();
            for (int i = 0; i < Data.Instance.session.players.Count; i++) {
                cards[i].setControl(Data.Instance.session.players[i].inputType, Data.Instance.session.players[i].gamepadNumber);
            }
        }

		public void loadLevel() {
            for (int i = 0; i < Data.Instance.session.players.Count; i++) {
                CharacterData charData = Data.Instance.getCharByGenderColor(cards[i].gender, cards[i].color);
                Data.Instance.session.players[i].character = charData;
                Data.Instance.session.players[i].firstItemId = cards[i].firstItem.getCurrentItemId();
                Data.Instance.session.players[i].secondItemId = cards[i].secondItem.getCurrentItemId();
            }
            Game.Instance.loadLevel();
        }
    }
}
