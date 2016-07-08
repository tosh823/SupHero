using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

namespace SupHero.Components.UI {
	public class PlayerCard : MonoBehaviour {

        private enum SelectionDirection {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }

        // UI components
		public Selectable maleButton;
		public Selectable femaleButton;
        public Text itemName;
        public Text itemDescription;
        public ItemSelect firstItem;
        public ItemSelect secondItem;
		public Button colorButton;
		public Button readyButton;

        public Selectable currentSelection;
        private float threshold = 0.5f;
        private float timer = 0f;

        // Control type
        private InputType inputType;
        private int gamepadNumber;

        public CharColor color;
        public Gender gender;

		void Start () {
            firstItem.createFirstItemsList();
            secondItem.createSecondItemsList();
		}
	
		void Update () {
            float h = 0f;
            float v = 0f;
            bool action = false;
	        switch (inputType) {
                case InputType.GAMEPAD:
                    h = Input.GetAxis(Utils.getControlForPlayer(Control.LeftStickX, gamepadNumber));
                    v = Input.GetAxis(Utils.getControlForPlayer(Control.LeftStickY, gamepadNumber));
                    action = Input.GetButtonDown(Utils.getControlForPlayer(Control.A, gamepadNumber));
                    break;
                case InputType.KEYBOARD:
                    h = Input.GetAxis(Control.Horizontal);
                    v = Input.GetAxis(Control.Vertical);
                    action = Input.GetButtonDown(Control.Space);
                    break;
                default:
                    break;
            }
            if (h < 0f && timer >= threshold) {
                // Go to left selection
                timer = 0f;
                moveSelection(SelectionDirection.LEFT);
            }
            else if (h > 0f && timer >= threshold) {
                // Go to right selection
                timer = 0f;
                moveSelection(SelectionDirection.RIGHT);
            }
            if (v > 0f && timer >= threshold) {
                // Go to up selection
                timer = 0f;
                moveSelection(SelectionDirection.UP);
            }
            else if (v < 0f && timer >= threshold) {
                // Go to down selection
                timer = 0f;
                moveSelection(SelectionDirection.DOWN);
            }

            if (action) {
				if (currentSelection == colorButton) {
					Debug.Log ("Change color for " + this);
					colorButton.onClick.Invoke ();
				} else if (currentSelection == readyButton) {
					Debug.Log (this + " is ready");
					activateSelection ();
					readyButton.onClick.Invoke ();
				} else if (currentSelection == maleButton) {
					Debug.Log (this + " is male");
					ColorChanger pallete = currentSelection.GetComponent<ColorChanger>();
					if (pallete.state != SelectionState.ACTIVE) {
						activateSelection ();
						femaleButton.GetComponent<ColorChanger> ().setDefault (color);
					} 
					else {
						pallete.setDefault(color);
						femaleButton.GetComponent<ColorChanger> ().setActive (color);
					}
				} else if (currentSelection == femaleButton) {
					Debug.Log (this + " is female");
					ColorChanger pallete = currentSelection.GetComponent<ColorChanger>();
					if (pallete.state != SelectionState.ACTIVE) {
						activateSelection();
						maleButton.GetComponent<ColorChanger> ().setDefault(color);
					}
					else {
						pallete.setDefault(color);
						maleButton.GetComponent<ColorChanger> ().setActive (color);
					}
				}
            }
            // Increase wait time
            if (timer <= threshold) timer += Time.deltaTime;
		}

        public void setControl(InputType type, int number = 0) {
            inputType = type;
            gamepadNumber = number;
            Debug.Log(this + " has " + inputType + " input type with " + gamepadNumber + " number");
            maleButton.Select();
            currentSelection = maleButton;
        }

        public void updateItemInfo(string itemName, string itemDescription) {
            this.itemName.text = itemName;
            this.itemDescription.text = itemDescription;
        }

        public void changeColor() {

        }

        public void getReady() {

        }

		public void highlightSelection() {
			if (currentSelection == firstItem.itemSelect || currentSelection == secondItem.itemSelect)
				return;
			ColorChanger pallete = currentSelection.GetComponent<ColorChanger>();
			pallete.setHighlighted(color);
		}

		public void activateSelection() {
			ColorChanger pallete = currentSelection.GetComponent<ColorChanger>();
			if (pallete.state != SelectionState.ACTIVE) {
				pallete.setActive(color);
			}
			else pallete.setDefault(color);
		}

		public void clearSelection() {
			if (currentSelection == firstItem.itemSelect || currentSelection == secondItem.itemSelect)
				return;
			ColorChanger pallete = currentSelection.GetComponent<ColorChanger>();
			if (pallete.state != SelectionState.ACTIVE) {
				pallete.setDefault(color);
			}
		}

        // Jeez, this gonna be stupid
        // Seriously, Unity, why I can't have multiple inputs?
        private void moveSelection(SelectionDirection direction) {
			clearSelection();
            if (currentSelection == maleButton) {
                switch (direction) {
                    case SelectionDirection.UP:
                        readyButton.Select();
                        currentSelection = readyButton;
                        break;
                    case SelectionDirection.DOWN:
                        firstItem.itemSelect.Select();
                        currentSelection = firstItem.itemSelect;
                        break;
                    case SelectionDirection.LEFT:
                        break;
                    case SelectionDirection.RIGHT:
                        femaleButton.Select();
                        currentSelection = femaleButton;
                        break;
                }
            }
            else if (currentSelection == femaleButton) {
                switch (direction) {
                    case SelectionDirection.UP:
                        readyButton.Select();
                        currentSelection = readyButton;
                        break;
                    case SelectionDirection.DOWN:
                        secondItem.itemSelect.Select();
                        currentSelection = secondItem.itemSelect;
                        break;
                    case SelectionDirection.LEFT:
                        maleButton.Select();
                        currentSelection = maleButton;
                        break;
                    case SelectionDirection.RIGHT:
                        firstItem.itemSelect.Select();
                        currentSelection = firstItem.itemSelect;
                        break;
                }
            }
            else if (currentSelection == firstItem.itemSelect) {
                switch (direction) {
                    case SelectionDirection.UP:
                        firstItem.nextItem();
                        break;
                    case SelectionDirection.DOWN:
                        firstItem.prevItem();
                        break;
                    case SelectionDirection.LEFT:
                        femaleButton.Select();
                        currentSelection = femaleButton;
                        break;
                    case SelectionDirection.RIGHT:
                        secondItem.itemSelect.Select();
                        currentSelection = secondItem.itemSelect;
                        break;
                }
            }
            else if (currentSelection == secondItem.itemSelect) {
                switch (direction) {
                    case SelectionDirection.UP:
                        secondItem.nextItem();
                        break;
                    case SelectionDirection.DOWN:
                        secondItem.prevItem();
                        break;
                    case SelectionDirection.LEFT:
                        firstItem.itemSelect.Select();
                        currentSelection = firstItem.itemSelect;
                        break;
                    case SelectionDirection.RIGHT:
                        colorButton.Select();
                        currentSelection = colorButton;
                        break;
                }
            }
            else if (currentSelection == colorButton) {
                switch (direction) {
                    case SelectionDirection.UP:
                        secondItem.itemSelect.Select();
                        currentSelection = secondItem.itemSelect;
                        break;
                    case SelectionDirection.DOWN:
                        readyButton.Select();
                        currentSelection = readyButton;
                        break;
                    case SelectionDirection.LEFT:
                        secondItem.itemSelect.Select();
                        currentSelection = secondItem.itemSelect;
                        break;
                    case SelectionDirection.RIGHT:
                        readyButton.Select();
                        currentSelection = readyButton;
                        break;
                }
            }
            else if (currentSelection == readyButton) {
                switch (direction) {
                    case SelectionDirection.UP:
                        colorButton.Select();
                        currentSelection = colorButton;
                        break;
                    case SelectionDirection.DOWN:
                        maleButton.Select();
                        currentSelection = maleButton;
                        break;
                    case SelectionDirection.LEFT:
                        colorButton.Select();
                        currentSelection = colorButton;
                        break;
                    case SelectionDirection.RIGHT:
                        maleButton.Select();
                        currentSelection = maleButton;
                        break;
                }
            }
            Debug.Log("Current selection is " + currentSelection);
			highlightSelection();
        }
    }
}
