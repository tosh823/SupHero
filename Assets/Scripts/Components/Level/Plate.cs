using UnityEngine;
using System.Collections.Generic;

namespace SupHero.Components {

    public enum Side {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }

    public struct Connector {
        public Side type;
        public GameObject place;
        public bool isFree;

        public Connector(Side type, GameObject place, bool isFree) {
            this.type = type;
            this.place = place;
            this.isFree = isFree;
        }
    }

    public class Plate : MonoBehaviour {

        // Connectors
        public GameObject north;
        public GameObject east;
        public GameObject south;
        public GameObject west;

        public Connector northConnector;
        public Connector eastConnector;
        public Connector southConnector;
        public Connector westConnector;
        public GameObject covers;
        public GameObject interior;

        private Bounds bounds;

        // Events
        public delegate void heroIncoming();
        public event heroIncoming OnHeroCome;

        void Awake() {
            northConnector = new Connector(Side.NORTH, north, true);
            eastConnector = new Connector(Side.EAST, east, true);
            southConnector = new Connector(Side.SOUTH, south, false);
            westConnector = new Connector(Side.WEST, west, true);
        }

        void Start() {
            bounds = GetComponent<Collider>().bounds;
            generateObjects(Theme.FOREST);
        }

        void Update() {

        }

        // Generating objects into plate here

        void generateObjects(Theme theme) {
            EnvironmentData data = Data.Instance.getEnvByTheme(theme);
            // Interior
            for (int i = 0; i < 7; i++) {
                GameObject instance = Instantiate(Utils.getRandomElement(data.interior)) as GameObject;
                //Random.InitState(i);
                Bounds objectBounds = instance.GetComponent<Collider>().bounds;
                float xPos = Random.Range(bounds.min.x + objectBounds.size.x, bounds.max.x - objectBounds.size.x);
                float zPos = Random.Range(bounds.min.z + objectBounds.size.z, bounds.max.z - objectBounds.size.z);
                // Random location
                Vector3 pos = new Vector3(xPos, instance.transform.position.y, zPos);
                instance.transform.SetParent(interior.transform);
                instance.transform.position = pos;
                // Random rotation
                Vector3 euler = instance.transform.eulerAngles;
                euler.y = Random.Range(0f, 360f);
                instance.transform.eulerAngles = euler;
            }
            // Covers
            for (int i = 0; i < 7; i++) {
                GameObject instance = Instantiate(Utils.getRandomElement(data.covers)) as GameObject;
                //Random.InitState(i);
                Bounds objectBounds = instance.GetComponent<Collider>().bounds;
                float xPos = Random.Range(bounds.min.x + objectBounds.size.x, bounds.max.x - objectBounds.size.x);
                float zPos = Random.Range(bounds.min.z + objectBounds.size.z, bounds.max.z - objectBounds.size.z);
                // Random location
                Vector3 pos = new Vector3(xPos, instance.transform.position.y, zPos);
                instance.transform.SetParent(covers.transform);
                instance.transform.position = pos;
                // Random rotation
                Vector3 euler = instance.transform.eulerAngles;
                euler.y = Random.Range(0f, 360f);
                instance.transform.eulerAngles = euler;
            }
        }

        public void dropItem() {
            // Drop weapons, items and supplies here

        }

        void OnTriggerEnter(Collider other) {
            Transform incoming = other.gameObject.transform;
            if (incoming.CompareTag(Tags.Player)) {
                // Player came to this plate
                if (OnHeroCome != null) {
                    OnHeroCome();
                }
            }
        }

        public List<Connector> getFreeConnectors() {
            List<Connector> free = new List<Connector>();
            // Okay, do it manually :(
            if (northConnector.isFree) free.Add(northConnector);
            if (eastConnector.isFree) free.Add(eastConnector);
            if (southConnector.isFree) free.Add(southConnector);
            if (westConnector.isFree) free.Add(westConnector);
            return free;
        }

        public void connectTo(Connector connector) {
            Vector3 move = Vector3.zero;
            switch (connector.type) {
                case Side.NORTH:
                    southConnector.isFree = false;
                    transform.position = connector.place.transform.position;
                    move.z = connector.place.transform.localPosition.z;
                    transform.Translate(move);
                    break;
                case Side.EAST:
                    westConnector.isFree = false;
                    transform.position = connector.place.transform.position;
                    move.x = connector.place.transform.localPosition.x;
                    transform.Translate(move);
                    break;
                case Side.SOUTH:
                    northConnector.isFree = false;
                    transform.position = connector.place.transform.position;
                    move.z = connector.place.transform.localPosition.z;
                    transform.Translate(move);
                    break;
                case Side.WEST:
                    eastConnector.isFree = false;
                    transform.position = connector.place.transform.position;
                    move.x = connector.place.transform.localPosition.x;
                    transform.Translate(move);
                    break;
                default:
                    break;
            }
            //connector.isFree = false;
        }

        public void placeTransfer(GameObject transfer) {
            // Putting transfer to zone accordingly to direction
            if (!westConnector.isFree) {
                transfer.transform.position = east.transform.position;
                transfer.transform.Rotate(new Vector3(0, 90, 0));
            }
            else if (!eastConnector.isFree) {
                transfer.transform.Rotate(new Vector3(0, 90, 0));
                transfer.transform.position = west.transform.position;
            }
            else {
                transfer.transform.position = north.transform.position;
            }
        }
    }
}
