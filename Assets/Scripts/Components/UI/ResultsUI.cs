using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using SupHero.Components.Level;
using SupHero.Model;

namespace SupHero.Components.UI {
    public class ResultsUI : MonoBehaviour {

        public PlayerStat[] playerStats;

        void Start() {
            createStats();
        }
        
        void Update() {

        }

        private void createStats() {
            Dictionary<int, int> stats = Data.Instance.getStatistics();
            for (int i = 0; i < Data.Instance.session.players.Count; i++) {
                Player player = Data.Instance.session.players[i];
                int place = stats[player.number];
                playerStats[i].createWithPlayer(player, place);
            }
        }
    }
}
