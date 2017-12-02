using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public LevelController levelController;

    private GameObject[] tubes;

    void Awake()
    {
        tubes = GameObject.FindGameObjectsWithTag(Tags.Tube);
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                PlayPressedTube(hit.transform.gameObject);
            }
        }
#else
        if (Input.touchCount > 0) {
			RaycastHit2D hit = Physics2D.Raycast (Input.touches [0].position, Vector2.zero);
			if (hit.collider != null) {
				PlayPressedTube (hit.transform.gameObject);
			}
		}
#endif
	}

    private void PlayPressedTube (GameObject clickedObject)
	{
        foreach (GameObject tube in tubes) {
			if (tube == clickedObject) {
                levelController.PlayTube(tube.transform.parent.GetComponent<Tube>());
			}
		}
	}
}
