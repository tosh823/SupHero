using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SupHero.Components.UI {

	public enum SelectionState {
		ACTIVE,
		HIGHLIGHTED,
		DEFAULT
	}

	public class ColorChanger : MonoBehaviour {

		public Sprite redDefault;
		public Sprite blueDefault;
		public Sprite yellowDefault;
		public Sprite pinkDefault;
		public Sprite redActive;
		public Sprite blueActive;
		public Sprite yellowActive;
		public Sprite pinkActive;
		public Sprite redHighlighted;
		public Sprite blueHighlighted;
		public Sprite yellowHighlighted;
		public Sprite pinkHighlighted;

		public SelectionState state;

		private Image image;
	
		void Start () {
			image = GetComponent<Image>();
			state = SelectionState.DEFAULT;
		}
	
		void Update () {
	
		}

		public void updateView(CharColor color) {
			switch (state) {
				case SelectionState.ACTIVE:
					setActive (color);
					break;
				case SelectionState.DEFAULT:
					setDefault(color);
					break;
				case SelectionState.HIGHLIGHTED:
					setHighlighted(color);
					break;
			}
		}

		public void setDefault(CharColor color) {
			switch (color) {
				case CharColor.BLUE:
					image.sprite = blueDefault;
					break;
				case CharColor.PINK:
					image.sprite = pinkDefault;
					break;
				case CharColor.RED:
					image.sprite = redDefault;
					break;
				case CharColor.YELLOW:
					image.sprite = yellowDefault;
					break;
			}
			state = SelectionState.DEFAULT;
		}

		public void setActive(CharColor color) {
			switch (color) {
				case CharColor.BLUE:
					image.sprite = blueActive;
					break;
				case CharColor.PINK:
					image.sprite = pinkActive;
					break;
				case CharColor.RED:
					image.sprite = redActive;
					break;
				case CharColor.YELLOW:
					image.sprite = yellowActive;
					break;
			}
			state = SelectionState.ACTIVE;
		}

		public void setHighlighted(CharColor color) {
			switch (color) {
				case CharColor.BLUE:
					image.sprite = blueHighlighted;
					break;
				case CharColor.PINK:
					image.sprite = pinkHighlighted;
					break;
				case CharColor.RED:
					image.sprite = redHighlighted;
					break;
				case CharColor.YELLOW:
					image.sprite = yellowHighlighted;
					break;
			}
			state = SelectionState.HIGHLIGHTED;
		}
	}
}
