using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
    public Turn turn;
    public List<Tube> tubes;
    
    private Tube selectedTube;

    void Update() {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePosition = getMousePosition();
            Debug.Log(mousePosition);
            Tube clickedTube = getClickedTube(mousePosition);
            selectTubeToPlay(clickedTube);
        } else if (Input.GetMouseButtonUp(0)) {
            Vector2 mousePosition = getMousePosition();
            Tube clickedTube = getClickedTube(mousePosition);
            playOrCancelTube(clickedTube);
        }
#else
        if (Input.touchCount <= 0) {
            return;
        }
        
        Touch touch = Input.GetTouch(0);
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
        Tube clickedTube = getClickedTube(touchPosition);
        
        if (touch.phase == TouchPhase.Began) {
            selectTubeToPlay(clickedTube);
        } else if (touch.phase == TouchPhase.Ended) {
            playOrCancelTube(clickedTube);
        }
#endif
    }

    private Vector2 getMousePosition() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2(mousePos.x, mousePos.y);
    }

    private void selectTubeToPlay(Tube clickedTube) {
        foreach (Tube tube in tubes) {
            if (tube == clickedTube) {
                selectedTube = clickedTube;
            }
        }
    }

    private void playOrCancelTube(Tube tube) {
        if (tube == null || tube != selectedTube) {
            cancelPlay();
        } else {
            playSelectedTube();
        }
    }

    private Tube getClickedTube(Vector2 clickPosition) {
        RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);
        return hit.collider == null ? null : hit.transform.parent.GetComponent<Tube>();
    }

    private void playSelectedTube() {
        turn.playTube(selectedTube);
    }

    private void cancelPlay() {
        selectedTube = null;
    }
}