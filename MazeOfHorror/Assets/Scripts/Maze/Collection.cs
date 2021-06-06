using UnityEngine;

public class Collection : MonoBehaviour
{

    public enum Name
    {
        Key,
        Gold,

    }

    public Name nameObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameControler controller = GameObject.Find("Controller").GetComponent<GameControler>();
            switch (nameObject)
            {
                case Name.Gold:
                    controller.MinusGold();
                    break;
                case Name.Key:
                    controller.MinusKeys();
                    break;

            }
            Destroy(gameObject);
        }
    }
}
