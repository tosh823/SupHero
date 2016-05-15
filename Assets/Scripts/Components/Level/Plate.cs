using UnityEngine;
using System.Collections.Generic;
using SupHero.Components.Character;
using SupHero.Model;

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

        public Transform surface;

        public bool generateView = true;

        public Connector northConnector;
        public Connector eastConnector;
        public Connector southConnector;
        public Connector westConnector;
        public GameObject covers;
        public GameObject interior;
        public PlateData plateData;

        //private Bounds bounds;
        private int numberOfAttempts = 3;
        private bool visited = false;

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
            //bounds = GetComponent<Collider>().bounds;
            if (generateView) generateObjects(Theme.FOREST);
        }

        void Update() {

        }

        private Vector3 getRandomPointOnMesh(float offsetX = 0f, float offsetZ = 0f) {
            Collider collider = surface.GetComponent<Collider>();
            // Assuming that all plates are far smaller
            float length = 100f;
            // Getting random point far away
            // And raycasting from it to the center of the plate
            // Thus getting point on the edge of the plate
            Vector3 direction = Random.onUnitSphere;
            direction.y = 0;
            Vector3 from = transform.position;
            from.y -= 2f;
            Vector3 faraway = from + direction * length;
            Ray ray = new Ray(faraway, -direction);
            RaycastHit hit;
            if (collider.Raycast(ray, out hit, length * 2)) {
                // Now, getting random point between edge and center
                Vector3 edgePoint = hit.point;
                // Randomize X
                Vector3 result = Vector3.zero;
                if (edgePoint.x > from.x) {
                    result.x = Random.Range(transform.position.x + offsetX, edgePoint.x - offsetX);
                }
                else result.x = Random.Range(edgePoint.x + offsetX, transform.position.x - offsetX);

                // Randomize Z
                if (edgePoint.z > from.z) {
                    result.z = Random.Range(transform.position.z + offsetZ, edgePoint.z - offsetZ);
                }
                else result.z = Random.Range(edgePoint.z + offsetZ, transform.position.z - offsetZ);

                return result;
            }
            else {
                return surface.transform.position;
            }
        }

        private bool placeAvailable(Bounds objectBounds, Vector3 placement) {
            /*int mask = 1 << LayerMask.GetMask(Layers.Surface);
            mask = ~mask;*/
            Collider[] overlap = Physics.OverlapBox(placement, objectBounds.extents, Quaternion.identity, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            if (overlap.Length > 2) {
                return false;
            }
            else return true;
        }

        private bool placeObject(GameObject obj) {
            Bounds objectBounds = obj.GetComponent<Collider>().bounds;
            // Trying to find location
            Vector3 pos;
            int attempt = 0;
            bool placed = false;
            do {
                pos = getRandomPointOnMesh(Mathf.Abs(objectBounds.extents.x), Mathf.Abs(objectBounds.extents.z));
                pos.y = obj.transform.position.y;
                obj.transform.position = pos;
                placed = placeAvailable(objectBounds, pos);
                attempt++;
            } while (!placed && attempt <= numberOfAttempts);
            return placed;
        }

        // Generating objects into plate here
        public void generateObjects(Theme theme) {
            EnvironmentData data = Data.Instance.getEnvByTheme(theme);
            // Interior
            for (int i = 0; i < plateData.interiorAmount; i++) {
                InteriorData interiorData = Utils.getRandomElement(data.interiors);
                GameObject instance = Instantiate(interiorData.prefab) as GameObject;
                instance.transform.SetParent(interior.transform);
                if (placeObject(instance)) {
                    // Random rotation
                    Vector3 euler = instance.transform.eulerAngles;
                    euler.y = Random.Range(0f, 360f);
                    instance.transform.eulerAngles = euler;
                }
                else Destroy(instance.gameObject);
            }
            // Covers
            for (int i = 0; i < plateData.coverAmount; i++) {
                CoverData coverData = Utils.getRandomElement(data.covers);
                GameObject instance = Instantiate(coverData.prefab) as GameObject;
                instance.transform.SetParent(covers.transform);
                if (placeObject(instance)) {
                    // Random rotation
                    Vector3 euler = instance.transform.eulerAngles;
                    euler.y = Random.Range(0f, 360f);
                    instance.transform.eulerAngles = euler;
                }
                else Destroy(instance.gameObject);
            }

            // Placing weapon drops
            for (int i = 0; i < 2; i++) {
                int weaponId = Data.Instance.getRandomWeaponId();
                GameObject dropInstance = Instantiate(data.dropPrefab) as GameObject;
                dropInstance.transform.SetParent(transform);
                dropInstance.GetComponent<Drop>().createDropSimple(Entity.WEAPON, weaponId);
                if (!placeObject(dropInstance)) Destroy(dropInstance.gameObject);
            }

            // Placing item drops
            for (int i = 0; i < 2; i++) {
                int itemId = Data.Instance.getRandomItemId();
                GameObject dropInstance = Instantiate(data.dropPrefab) as GameObject;
                dropInstance.transform.SetParent(transform);
                dropInstance.GetComponent<Drop>().createDropSimple(Entity.ITEM, itemId);
                if (!placeObject(dropInstance)) Destroy(dropInstance.gameObject);
            }
        }

        public void Drop(Drop drop) {
            if (drop != null && drop.entity != Entity.NONE) {
                drop.transform.SetParent(transform);
                if (!placeObject(drop.gameObject)) Destroy(drop.gameObject);
            }
        }

        void OnTriggerEnter(Collider other) {
            Transform incoming = other.gameObject.transform;
            if (incoming.CompareTag(Tags.Player)) {
                PlayerController pc = incoming.GetComponent<PlayerController>();
                if (!visited && (pc.player is Hero)) {
                    pc.player.applyPoints(Data.Instance.mainSettings.points.plateFinished);
                    visited = true;
                }
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
