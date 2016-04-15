using UnityEngine;
using System.Collections.Generic;

namespace SupHero.Components.Level {
    public class Map : MonoBehaviour {

        private ZoneController zone;
        public GameObject transfer;
        
        public bool generateRoute = false;
        public int length = 1;

        public List<Plate> plates { get; private set; } // All the plates
        public Plate battleField { get; private set; } // Current plate where hero is

        void Awake() {
            zone = GetComponent<ZoneController>();
        }

        void Start() {
            if (generateRoute && length > 0) {
                createRouteExperimental(length);
                //generateLandscape();
            }
        }

        public void constructZone(int length) {
            createRouteExperimental(length);
            //generateLandscape();
        }

        void Update() {

        }

        public List<PlateData> getPlatesByType(PlateData[] data, Side side) {
            List<PlateData> result = new List<PlateData>();
            foreach (PlateData dict in data) {
                List<Connector> connectors = dict.prefab.GetComponent<Plate>().getAllConnectors();
                if (connectors.Find(x => x.type == side) != null) {
                    result.Add(dict);
                }
            }
            return result;
        }

        public void createRouteExperimental(int length) {
            // New list of plates
            plates = new List<Plate>();
            EnvironmentData data = Data.Instance.getEnvByTheme(Theme.FOREST);
            PlateData[] options = data.plates;

            if (options.Length == 0) {
                Debug.Log("Plates data is empty");
                return;
            }

            // Assume, that first is always exist 
            GameObject plateInstance = Instantiate(options[0].prefab) as GameObject;
            plateInstance.transform.SetParent(transform);
            plateInstance.transform.position = transform.position;
            Plate plate = plateInstance.GetComponent<Plate>();
            // Shift base plate to north
            plateInstance.transform.Translate(plate.north.transform.localPosition);
            plate.southConnector.isFree = false;
            // The first plate will be the first battlefield
            battleField = plate;
            plate.plateData = options[0];
            plates.Add(plate);

            for (int index = 1; index < length; index++) {

                // Choosing free connector
                Plate last = plates[index - 1].GetComponent<Plate>();
                List<Connector> free = last.getFreeConnectors();
                Connector chosen = Utils.getRandomElement(free);
                List<PlateData> solutions = getPlatesByType(options, chosen.compatibleType);

                if (solutions.Count > 0) {
                    PlateData plateData = Utils.getRandomElement(solutions);
                    plateInstance = Instantiate(plateData.prefab) as GameObject;
                    plateInstance.transform.SetParent(transform);
                    plate = plateInstance.GetComponent<Plate>();

                    plate.connectTo(chosen);
                    chosen.isFree = false;

                    // Place transfer at last
                    /*if (index == (length - 1)) {
                        GameObject transferInstance = Instantiate(transfer) as GameObject;
                        transferInstance.transform.SetParent(transform);
                        plateInstance.GetComponent<Plate>().placeTransfer(transferInstance);
                    }*/

                    // When hero steps on this plate, made it current battlefield
                    plate.OnHeroCome += delegate () {
                        battleField = plate;
                    };

                    plate.plateData = plateData;
                    plates.Add(plate);
                }
            }
        }

        private void generateLandscape() {
            foreach (Plate plate in plates) {
                plate.generateObjects(Theme.FOREST);
            }
        }

        // Constructing a zone route out of square plates
        /*public void createRoute(int length) {
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
        }*/
    }
}
