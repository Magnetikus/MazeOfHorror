using UnityEngine;

public class WollMovet : MonoBehaviour
{
    private bool onTrigger = false;
    private Transform myTransform;
    private GameControler gameContr;

    public GameObject cubeGreen;
    public GameObject prefabCell;

    void Start()
    {
        gameContr = GameObject.Find("Controller").GetComponent<GameControler>();
        myTransform = transform;
    }

    private void OnMouseOver()
    {
        if (onTrigger && tag == "Movet")
        {
            gameContr.cursorOnOff = true;
            cubeGreen.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                gameContr.cursorOnOff = false;
                gameContr.cubeMovet = true;
                GameObject go = Instantiate(prefabCell);
                go.transform.position = myTransform.position;
                go.GetComponentInChildren<CellMovet>().pressKeyF = true;
                go.GetComponentInChildren<CellMovet>().pressKeyMouse = true;
                Destroy(gameObject);

            }
        }
    }

    private void OnMouseExit()
    {
        if (onTrigger && tag == "Movet")
        {
            cubeGreen.SetActive(false);
            gameContr.cursorOnOff = (false);
        }
    }
    void Update()
    {
        if (!gameContr.pausedGame && gameContr.GetCube() > 0)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!onTrigger) gameContr.MinusCube();
                onTrigger = true;

            }
            if (Input.GetMouseButtonDown(0))
            {
                onTrigger = false;
            }
        }
    }
}
