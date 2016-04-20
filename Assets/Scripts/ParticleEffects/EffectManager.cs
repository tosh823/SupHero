using UnityEngine;
using System.Collections;

public class EffectManager : MonoBehaviour {

	public GameObject[] effects;

	public void PlayEffect(int currentEffect)
	{
		effects [currentEffect].GetComponent<Animator> ().SetTrigger ("activated");
	}
}
