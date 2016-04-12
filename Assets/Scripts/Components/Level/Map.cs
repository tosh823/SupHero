using UnityEngine;
using System.Collections.Generic;

namespace SupHero.Components.Level {
    public class Map : MonoBehaviour {

        private ZoneController zone;
        public GameObject plate;
        public GameObject transfer;
        
        public bool generateRoute = false;
        public int length = 1;
        public GameObject[] prefabs;

        public List<GameObject> plates { get; private set; } // All the plates
        public Plate battleField { get; private set; } // Current plate where hero is

        void Awake() {
            zone = GetComponent<ZoneController>();
        }

        void Start() {
            plates = new List<GameObject>();
            if (generateRoute && prefabs.Length > 0 && length > 0) {
                createRouteExperimental(length);
            }
        }

        void Update() {

        }

        public List<GameObject> getPlatesByType(Side side) {
            List<GameObject> result = new List<GameObject>();
            foreach (GameObject prefab in prefabs) {
                List<Connector> connectors = prefab.GetComponent<Plate>().getAllConnectors();
                if (connectors.Find(x => x.type == side) != null) {
                    result.Add(prefab);
                }
            }
            return result;
        }

        public void createRouteExperimental(int length) {
            // New list of plates
            plates = new List<GameObject>();

            GameObject plateInstance = Instantiate(plate) as GameObject;
            plateInstance.transform.SetParent(transform);
            plateInstance.transform.position = transform.position;
            // Shift base plate to north
            plateInstance.transform.Translate(plateInstance.GetComponent<Plate>().north.transform.localPosition);
            plateInstance.GetComponent<Plate>().southConnector.isFree = false;
            // The first plate will be the first battlefield
            battleField = plateInstance.GetComponent<Plate>();
            plates.Add(plateInstance);
            for (int index = 1; index < length; index++) {

                // Choosing free connector
                Plate last = plates[index - 1].GetComponent<Plate>();
                List<Connector> free = last.getFreeConnectors();
                Connector chosen = Utils.getRandomElement(free);
                List<GameObject> solutions = getPlatesByType(chosen.compatibleType);

                if (solutions.Count > 0) {
                    plateInstance = Instantiate(Utils.getRandomElement(solutions)) as GameObject;
                    plateInstance.transform.SetParent(transform);
                    plateInstance.GetComponent<Plate>().connectTo(chosen);
                    chosen.isFree = false;

                    // Place transfer at last
                    /*if (index == (length - 1)) {
                        GameObject transferInstance = Instantiate(transfer) as GameObject;
                        transferInstance.transform.SetParent(transform);
                        plateInstance.GetComponent<Plate>().placeTransfer(transferInstance);
                    }*/

                    // When hero steps on this plate, made it current battlefield
                    plateInstance.GetComponent<Plate>().OnHeroCome += delegate () {
                        battleField = plateInstance.GetComponent<Plate>();
                    };

                    plates.Add(plateInstance);
                }
            }
        }

        // Constructing a zone route
        public void createRoute(int length) {
            // Building first, base plate
            GameObject plateInstance = Instantiate(plate) as GameObject;
            plateInstance.transform.SetParent(transform);
            plateInstance.transform.position = transform.position;
            // Shift base plate to north
            plateInstance.transform.Translate(plateInstance.GetComponent<Plate>().north.transform.localPosition);
            // The first plate will be the first battlefield
            battleField = plateInstance.GetComponent<Plate>();
            plates.Add(plateInstance);
            for (int index = 1; index < length; index++) {
                // Building a maze
                plateInstance = Instantiate(plate) as GameObject;
                plateInstance.transform.SetParent(transform);
                Plate last = plates[index - 1].GetComponent<Plate>();
                List<Connector> free = last.getFreeConnectors();
                // Protection from looping
                if (plates.Count >= 3) {
                    Plate suspicious = plates[index - 3].GetComponent<Plate>();
                    if (suspicious.northConnector.isFree) {
                        Plate beforeSuspicious = plates[index - 2].GetComponent<Plate>();
                        if (beforeSuspicious.westConnector.isFree) {
                            // Delete east conn from free
                            Connector toRemove = free.Find(x => x.type == Side.EAST);
                            free.Remove(toRemove);
                        }
                        if (beforeSuspicious.eastConnector.isFree) {
                            // Delete west conn from free
                            Connector toRemove = free.Find(x => x.type == Side.WEST);
                            free.Remove(toRemove);
                        }
                    }
                }
                Connector chosen = Utils.getRandomElement(free);
                plateInstance.GetComponent<Plate>().connectTo(chosen);
                chosen.isFree = false;
                // Place transfer at last
                if (index == (length - 1)) {
                    GameObject transferInstance = Instantiate(transfer) as GameObject;
                    transferInstance.transform.SetParent(transform);
                    plateInstance.GetComponent<Plate>().placeTransfer(transferInstance);
                }

                // When hero steps on this plate, made it current battlefield
                plateInstance.GetComponent<Plate>().OnHeroCome += delegate () {
                    battleField = plateInstance.GetComponent<Plate>();
                };

                plates.Add(plateInstance);
            }
        }
    }
}
