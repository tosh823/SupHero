using UnityEngine;
using System.Collections.Generic;

namespace SupHero.Components.Level {

    public enum Side {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }

    public enum Direction {
        ANY,
        LEFT,
        RIGHT
    }

    public class Connector {
        public Side type;
        public GameObject place;
        public bool isFree;
        public Side compatibleType {
            get {
                switch (type) {
                    case Side.NORTH:
                        return Side.SOUTH;
                    case Side.EAST:
                        return Side.WEST;
                    case Side.SOUTH:
                        return Side.NORTH;
                    case Side.WEST:
                        return Side.EAST;
                    default:
                        return Side.SOUTH;
                }
            }
            set {
                compatibleType = value;
            }
        }

        public Connector(Side type, GameObject place, bool isFree) {
            this.type = type;
            this.place = place;
            this.isFree = isFree;
        }
    }

    public class Plate : MonoBehaviour {

        // Direction
        public Direction direction;

        // Connectors
        public GameObject north;
        public GameObject east;
        public GameObject south;
        public GameObject west;

        public bool generateView = true;

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
            if (north != null) northConnector = new Connector(Side.NORTH, north, true);
            if (east != null) eastConnector = new Connector(Side.EAST, east, true);
            if (south != null) southConnector = new Connector(Side.SOUTH, south, true);
            if (west != null) westConnector = new Connector(Side.WEST, west, true);
        }

        void Start() {
            bounds = GetComponent<Collider>().bounds;
            if (generateView) generateObjects(Theme.FOREST);
            /*Debug.Log("Renderer bounds are " + GetComponentInChildren<Renderer>().bounds);
            Debug.Log("Renderer bounds min are " + GetComponentInChildren<Renderer>().bounds.min);
            Debug.Log("Renderer bounds max are " + GetComponentInChildren<Renderer>().bounds.max);*/
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

        public List<Connector> getAllConnectors() {
            List<Connector> all = new List<Connector>();
            if (north != null) all.Add(new Connector(Side.NORTH, north, true));
            if (east != null) all.Add(new Connector(Side.EAST, east, true));
            if (south != null) all.Add(new Connector(Side.SOUTH, south, true));
            if (west != null) all.Add(new Connector(Side.WEST, west, true));
            return all;
        }

        public List<Connector> getFreeConnectors() {
            List<Connector> free = new List<Connector>();
            // Okay, do it manually :(
            if (northConnector != null && northConnector.isFree) free.Add(northConnector);
            if (eastConnector != null && eastConnector.isFree) free.Add(eastConnector);
            if (southConnector != null && southConnector.isFree) free.Add(southConnector);
            if (westConnector != null && westConnector.isFree) free.Add(westConnector);
            return free;
        }

        public bool hasFreeConnectorOfType(Side side) {
            List<Connector> all = getFreeConnectors();
            if (all.Find(x => x.type == side) != null) return true;
            else return false;
        }

        public void connectTo(Connector connector) {
            Vector3 move = Vector3.zero;
            switch (connector.type) {
                case Side.NORTH:
                    southConnector.isFree = false;
                    transform.position = connector.place.transform.position;
                    move = -southConnector.place.transform.localPosition;
                    //move.y = 0f;
                    //move.z = connector.place.transform.localPosition.z;
                    transform.Translate(move);
                    break;
                case Side.EAST:
                    westConnector.isFree = false;
                    transform.position = connector.place.transform.position;
                    move = -westConnector.place.transform.localPosition;
                    //move.y = 0f;
                    //move.x = connector.place.transform.localPosition.x;
                    transform.Translate(move);
                    break;
                case Side.SOUTH:
                    northConnector.isFree = false;
                    transform.position = connector.place.transform.position;
                    move = -northConnector.place.transform.localPosition;
                    //move.y = 0f;
                    //move.z = connector.place.transform.localPosition.z;
                    transform.Translate(move);
                    break;
                case Side.WEST:
                    eastConnector.isFree = false; 
                    transform.position = connector.place.transform.position;
                    move = -eastConnector.place.transform.localPosition;
                    //move.y = 0f;
                    //move.x = connector.place.transform.localPosition.x;
                    transform.Translate(move);
                    break;
                default:
                    break;
            }
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
