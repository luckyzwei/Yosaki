using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HardSlashes
{
	
	public class DemoManager : MonoBehaviour {
		public TextMesh text_fx_name;
		public GameObject[] prefabs;
		public int index_fx = 0;


		private GameObject current_animation;
		private Ray ray;
		private RaycastHit ray_cast_hit;


		void Start () {
			text_fx_name.text = "[" + (index_fx + 1) + "] " + prefabs[ index_fx ].name;
			Destroy(GameObject.Find("Instructions"), 11.5f);
		}


		void Update () 
		{
			if ( Input.GetMouseButtonDown(0) )
			{
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast (ray.origin, ray.direction, out ray_cast_hit, 1000f)) 
				{
					Aim ();
					current_animation = Instantiate (prefabs[index_fx], ray_cast_hit.point, transform.rotation);
				}
			}
			//Change-FX Keyboard	
			if ( Input.GetKeyDown("z") || Input.GetKeyDown("left") ){
				index_fx--;
				if(index_fx <= -1)
					index_fx = prefabs.Length - 1;
				text_fx_name.text = "[" + (index_fx + 1) + "] " + prefabs[ index_fx ].name;
			}
			if ( Input.GetKeyDown("x") || Input.GetKeyDown("right")){
				index_fx++;
				if(index_fx >= prefabs.Length)
					index_fx = 0;
				text_fx_name.text = "[" + (index_fx + 1) + "] " + prefabs[ index_fx ].name;
			}
			if (Input.GetKeyDown ("space"))
				Debug.Break ();
		}


		void Aim()
		{
			transform.LookAt(new Vector3(ray_cast_hit.point.x, 1.0f, ray_cast_hit.point.z));
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
		}

	}
	
}


