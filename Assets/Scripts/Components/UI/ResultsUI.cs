using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using SupHero.Components.Level;

namespace SupHero.Components.UI {
    public class ResultsUI : MonoBehaviour {

        public Text[] places;

        void Start() {
            setUpTexts();
        }
        
        void Update() {

        }

        private void setUpTexts() {
            Dictionary<string, int> stats = LevelController.Instance.getStatistics();
            var sorted = from pair in stats orderby pair.Value descending select pair;
            Dictionary<string, int> dict = sorted.ToDictionary(x => x.Key, x => x.Value); 

            for (int i = 0; i < dict.Count; i++) {
                places[i].text = dict.ElementAt(i).Key + ": " + dict.ElementAt(i).Value;
            }
        }
    }
}
