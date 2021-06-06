using UnityEngine;

public class EventMinusLife : MonoBehaviour
{
    public GameControler gameContr;

    public void EndAnimation()
    {
        gameContr.PausedGame(false);
    }
}
