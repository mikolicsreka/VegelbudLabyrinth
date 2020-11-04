using UnityEngine;
/*
 * A kamerára ráhúzott szkript, a mozgathatóságáért felelős.
 * 
 */
public class CameraController : MonoBehaviour
{

	public float panSpeed = 30f;
	public float panBorderThickness = 10f;

	public float scrollSpeed = 5f;
	public float minY = 10f;
	public float maxY = 80f;

	private bool doMovement = true;

	float rotationSpeed = 5.0f;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.H))
		{
			GameObject home = null;
			
			try
			{
				home = FindObjectOfType<EnemySpawner>().gameObject;
			}
			catch (System.NullReferenceException)
			{
				Debug.Log("We dont have EnemySpawner");
			}
			

			if (home == null)
			{
				home = FindObjectOfType<MazeGenerator>().gameObject;
			}


			this.transform.position = new Vector3(home.transform.position.x, this.transform.position.y, home.transform.position.z) ;
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			doMovement = !doMovement;
		}

		if (!doMovement)
		{
			return;
		}


		if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
		{
			transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
		{
			transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
		{
			transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
		{
			transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
		}

		float scroll = Input.GetAxis("Mouse ScrollWheel");

		Vector3 pos = transform.position;

		pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
		pos.y = Mathf.Clamp(pos.y, minY, maxY);

		transform.position = pos;

		//ROTATION
		//if (Input.GetKey("q"))

		//{

		//	transform.Rotate(Vector3.up * Time.deltaTime * - scrollSpeed * 20, Space.World);

		//}

		//else if (Input.GetKey("e"))

		//{

		//	transform.Rotate(Vector3.down * Time.deltaTime * -scrollSpeed * 20, Space.World);

		//}

	}
}
