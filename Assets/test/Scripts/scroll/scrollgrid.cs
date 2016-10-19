using UnityEngine;
using System.Collections;

public class scrollgrid : MonoBehaviour {

	public GameObject ScrollView;
	public GameObject Grid;
	public GameObject MapItemPrefab;
	public GameObject NGUICamera;

	RaycastHit hit;
	Ray ray;

	// Use this for initialization
	void Start () {

		//UIPanel scrollViewPanel = ScrollView.GetComponent<UIPanel>();
		//UIScrollView scrollViewUIScroll = ScrollView.GetComponent<UIScrollView>();

//		UIGrid ug = Grid.GetComponent<UIGrid>();
//
//		GameObject mapItemPrefab = Instantiate(MapItemPrefab) as GameObject;
//		mapItemPrefab.transform.parent = Grid.transform;
//		mapItemPrefab.transform.localPosition = new Vector3( 0f, 0f , 0f );
//		mapItemPrefab.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
//		mapItemPrefab.transform.localScale = Vector3.one;
//
//		mapItemPrefab = Instantiate(MapItemPrefab) as GameObject;
//		mapItemPrefab.transform.parent = Grid.transform;
//		mapItemPrefab.transform.localPosition = new Vector3( 0f, 100f , 0f );
//		mapItemPrefab.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
//		mapItemPrefab.transform.localScale = Vector3.one;
//		ug.Reposition();
	}


	public void EvFunc(){

		print ("aaaaa");
	}

}
