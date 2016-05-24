using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SupHero.Components.UI {
    public class ItemSelect : MonoBehaviour {

        public Image itemImage;
        public Selectable itemSelect { get; private set; }
        private PlayerCard parent;
        private List<ItemData> items;
        private int index = 0;

        void Awake() {
            parent = GetComponentInParent<PlayerCard>();
        }

        void Start() {
            
            itemSelect = itemImage.GetComponent<Selectable>();
        }

        void Update() {

        }

        public void createFirstItemsList() {
            items = new List<ItemData>();
            items = Data.Instance.getFirstItems();
            showItem(0);
        }

        public void createSecondItemsList() {
            items = new List<ItemData>();
            items = Data.Instance.getSecondItems();
            showItem(0);
        }

        public ItemData getCurrentItem() {
            return items[index];
        }

        public int getCurrentItemId() {
            return items[index].id;
        }

        public void nextItem() {
            if (index < (items.Count - 1)) {
                index++;
                showItem(index);
            }
            else {
                index = 0;
                showItem(index);
            }
        }

        public void prevItem() {
            if (index > 0) {
                index--;
                showItem(index);
            }
            else {
                index = items.Count - 1;
                showItem(index);
            }
        }

        private void showItem(int index = 0) {
            ItemData item = items[index];
            if (item != null) {
                this.index = index;
                updateView(item);
            }
        }

        private void updateView(ItemData item) {
            itemImage.sprite = item.image;
            parent.updateItemInfo(item.name, item.description);
        }
    }
}
