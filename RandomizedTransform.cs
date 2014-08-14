using UnityEngine;
using System.Collections;

public class RandomizedTransform : MonoBehaviour {
	
	public float xRotationRange = 0;
	public float yRotationRange = 0;
	public float zRotationRange = 0;
	
	public float xPositionRange = 0;
	public float yPositionRange = 0;
	public float zPositionRange = 0;
	
	// Use this for initialization
	void Start () 
	{
	
//		Vector3 V3rotation = gameObject.transform.rotation.eulerAngles;
//		V3rotation += new Vector3(xRotationRange,yRotationRange,zRotationRange);
		
		gameObject.transform.eulerAngles += new Vector3((Random.Range (-xRotationRange,xRotationRange)),(Random.Range (-yRotationRange,yRotationRange)),(Random.Range (-zRotationRange,zRotationRange)));
		gameObject.transform.position += new Vector3((Random.Range (-xPositionRange,xPositionRange)),(Random.Range (-yPositionRange,yPositionRange)),(Random.Range (-zPositionRange,zPositionRange)));
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
