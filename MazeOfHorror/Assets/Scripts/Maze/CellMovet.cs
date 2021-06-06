using UnityEngine;

public class CellMovet : MonoBehaviour
{
    private GameControler gameContr;
    private Transform myTransform;

    public GameObject cell;
    public GameObject prefabWoll;

    [HideInInspector]
    public float amountScent = 0;
    public bool pressKeyF = false;
    public bool pressKeyMouse = false;

    void Start()
    {
        gameContr = GameObject.Find("Controller").GetComponent<GameControler>();
        myTransform = transform;

    }

    private void OnMouseOver()
    {
        if (pressKeyF && pressKeyMouse)
        {
            cell.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                gameContr.cubeMovet = false;
                gameContr.ZeroAllCell();
                GameObject go = Instantiate(prefabWoll);
                go.transform.position = myTransform.position;
                go.tag = "Movet";
                Destroy(gameObject);
            }
        }
    }

    private void OnMouseExit()
    {
        if (pressKeyF && pressKeyMouse)
        {
            cell.SetActive(false);
        }
    }
    void Update()
    {
        if (!gameContr.pausedGame)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                pressKeyF = true;
            }
            if (pressKeyF)
            {
                if (Input.GetMouseButtonDown(0)) pressKeyMouse = true;
            }

            if (amountScent > 0.2)
            {
                amountScent -= 0.5f * Time.deltaTime;
            }
            else amountScent = 0;

        }

    }

}
