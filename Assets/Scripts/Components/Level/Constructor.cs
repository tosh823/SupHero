using UnityEngine;
using System.Collections.Generic;

namespace SupHero.Components {
    public class Constructor : MonoBehaviour {

        private ZoneController zone;
        public GameObject plate;
        public GameObject transfer;

        public List<GameObject> plates;

        void Awake() {
            zone = GetComponent<ZoneController>();
        }

        void Start() {
            plates = new List<GameObject>();
            //createRoute(6);
        }

        void Update() {

        }

        // Constructing a zone route
        public void createRoute(int length) {
            // Building first, base plate
            GameObject plateInstance = Instantiate(plate) as GameObject;
            plateInstance.transform.SetParent(transform);
            plateInstance.transform.position = transform.position;
            // Shift base plate to north
            plateInstance.transform.Translate(plateInstance.GetComponent<Plate>().north.transform.localPosition);
            // Setting south to dirty, so we won't connect to it
            plateInstance.GetComponent<Plate>().southConnector.isFree = false;
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
                plates.Add(plateInstance);
            }
        }
    }
}
