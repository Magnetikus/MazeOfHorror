using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public enum State
    {
        Idle = 0,
        Movet = 1,
        Rotation = 2,
        Alert = 3,
        RunTarget = 4,
        Attack = 5,
        Scaner = 6,
    }
    public enum NameMonster
    {
        Nose = 0,
        Ear = 1,
        Eye = 2,
    }

    public State state;
    public NameMonster nameMonster;
    public Transform scanerTransform;
    public Transform scanerForward;


    public float speedNormal = 1f;
    public float speedAttack = 3f;
    public float speedPlayerForChangeInAlert = 0.01f;
    public float distanceToPlayerForChangeInAlert = 7f;
    public Material materialHead;

    private List<Vector3> listPoint;
    private List<float> listScentPlayer;
    private GameObject player;
    private Vector3 positionPlayer;
    private Vector3 positionPlayerOld;
    private Vector3 positionMovet;
    private Vector3 positionPlayerOutSight;
    private GameControler gameContr;
    private Animator animator;
    private static readonly int Rotation = Animator.StringToHash("Rotation");
    private static readonly int Speed = Animator.StringToHash("Speed");



    void Start()
    {
        state = State.Idle;
        listPoint = new List<Vector3>();
        listScentPlayer = new List<float>();
        player = GameObject.Find("Player(Clone)");
        gameContr = GameObject.Find("Controller").GetComponent<GameControler>();
        animator = GetComponentInChildren<Animator>();

    }

    public void Restart()
    {
        Start();
    }

    public void SetState(State newState)
    {
        state = newState;
    }

    bool MovetForward(Vector3 targetPosition, float distanceFromTarget, float speed)
    {

        Vector3 distance = targetPosition - transform.position;
        if (distance.magnitude < 0.001f)
        {
            transform.position = targetPosition;
            return true;
        }

        Vector3 direction = distance.normalized;
        transform.rotation = Quaternion.LookRotation(direction);

        targetPosition -= direction * distanceFromTarget;
        distance = (targetPosition - transform.position);

        Vector3 step = direction * speed * Time.deltaTime;
        if (step.magnitude < distance.magnitude)
        {
            transform.position += step;
            return false;
        }

        transform.position = targetPosition;
        return true;
    }

    bool AnglePlayerAndForwardMonster(Vector3 playerPosition, Vector3 myPosition)
    {
        Vector3 distance = playerPosition - myPosition;

        float angle = Vector3.Angle(transform.forward, distance);
        if (angle < 60f) return true;
        return false;
    }

    List<Vector3> Scaner(Vector3 direction)
    {
        Vector3 ray = scanerTransform.TransformDirection(direction);
        if (Physics.Raycast(scanerTransform.position, ray, out RaycastHit hit, 3f))
        {
            if (hit.collider.CompareTag("Cell"))
            {
                GameObject go = hit.collider.gameObject;
                listPoint.Add(go.GetComponent<Transform>().position);
                listScentPlayer.Add(go.GetComponent<CellMovet>().amountScent);

            }
        }
        return listPoint;
    }

    List<Vector3> ScanerForward()
    {
        bool iMonster = false;
        RaycastHit[] hits;
        hits = Physics.RaycastAll(scanerTransform.position, scanerTransform.TransformDirection(Vector3.forward), 6f);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.collider.CompareTag("Monster"))
            {
                iMonster = true;
            }
        }
        if (!iMonster) Scaner(Vector3.forward);
        else Scaner(Vector3.back);
        return listPoint;
    }

    bool RaycastToPlayer()
    {
        if (Physics.Raycast(transform.position, (positionPlayer - transform.position).normalized, out RaycastHit hitRay))
        {
            if (hitRay.collider.CompareTag("Player"))
            {
                return true;
            }
            else return false;
        }
        else return false;
    }

    void FixedUpdate()
    {
        if (player == null) player = GameObject.Find("Player(Clone)");
        switch (state)
        {
            case State.Idle:
                materialHead.color = Color.white;
                break;
            case State.Alert:
                materialHead.color = Color.yellow;
                break;
            case State.RunTarget:
            case State.Attack:
                materialHead.color = Color.red;
                break;
            default:
                var col = materialHead.color;
                materialHead.color = col;
                break;
        }
        if (!gameContr.pausedGame)
        {
            positionPlayer = player.transform.position;
            float speedPlayerReal = (positionPlayer - positionPlayerOld).magnitude;
            float distanceToPlayer = (positionPlayer - transform.position).magnitude;

            switch (state)
            {
                case State.Idle:
                    transform.position = transform.position;
                    animator.SetFloat(Speed, 0.0f);
                    switch (nameMonster)
                    {
                        case NameMonster.Ear:

                            if (distanceToPlayer < distanceToPlayerForChangeInAlert * 2 && speedPlayerReal > speedPlayerForChangeInAlert)
                            {
                                if (RaycastToPlayer())
                                {
                                    state = State.Alert;
                                }
                            }
                            else if (Random.value < 0.1f)
                            {
                                state = State.Rotation;
                                animator.SetTrigger(Rotation);
                            }
                            else state = State.Scaner;
                            break;

                        case NameMonster.Eye:
                            if (distanceToPlayer < (distanceToPlayerForChangeInAlert * 2) &&
                                AnglePlayerAndForwardMonster(positionPlayer, transform.position) && RaycastToPlayer())
                            {
                                state = State.Alert;

                            }
                            else if (Random.value < 0.1f)
                            {
                                state = State.Rotation;
                                animator.SetTrigger(Rotation);
                            }
                            else state = State.Scaner;
                            break;

                        case NameMonster.Nose:
                            if (Random.value < 0.1f)
                            {
                                state = State.Rotation;
                                animator.SetTrigger(Rotation);
                            }
                            else state = State.Scaner;
                            break;

                        default:
                            break;
                    }


                    break;

                case State.Scaner:

                    listPoint.Clear();
                    listScentPlayer.Clear();
                    ScanerForward();
                    Scaner(Vector3.left);
                    Scaner(Vector3.right);
                    Scaner(Vector3.back);

                    int lenghtListPoint = listPoint.Count;
                    switch (lenghtListPoint)
                    {
                        case (1):
                            positionMovet = listPoint[0];
                            break;
                        case (2):
                            positionMovet = Random.value < 0.95f ? listPoint[0] : listPoint[1];
                            break;
                        case (3):
                            positionMovet = Random.value < 0.8f ? listPoint[0] : Random.value < 0.8f ? listPoint[1] : listPoint[2];
                            break;
                        case (4):
                            positionMovet = Random.value > 0.05f ? listPoint[0] : Random.value < 0.5f ? listPoint[3] :
                                Random.value < 0.5f ? listPoint[1] : listPoint[2];
                            break;
                        default:
                            state = State.Idle;
                            break;
                    }

                    switch (nameMonster)
                    {
                        case NameMonster.Ear:
                        case NameMonster.Eye:
                            state = State.Movet;
                            animator.SetFloat(Speed, speedNormal);
                            break;
                        case NameMonster.Nose:
                            float summa = 0;
                            foreach (var scent in listScentPlayer) summa += scent;
                            if (summa > 0)
                            {
                                state = State.Alert;

                            }
                            else
                            {
                                state = State.Movet;

                            }
                            break;
                    }
                    break;

                case State.Movet:

                    animator.SetFloat(Speed, speedNormal);

                    if (MovetForward(positionMovet, 0.0f, speedNormal + nameMonster.GetHashCode() / 5))
                    {
                        state = State.Idle;
                        break;
                    }
                    break;

                case State.Alert:

                    if (RaycastToPlayer()) positionPlayerOutSight = player.GetComponentInChildren<TriggerCell>().positionCell;
                    animator.SetFloat(Speed, speedNormal * 2);

                    switch (nameMonster)
                    {
                        case NameMonster.Ear:

                            if (RaycastToPlayer() && distanceToPlayer < distanceToPlayerForChangeInAlert && speedPlayerReal > speedPlayerForChangeInAlert * 2)
                            {
                                state = State.RunTarget;

                            }

                            if (MovetForward(positionPlayerOutSight, 0, speedNormal * 2))
                            {
                                state = State.Idle;
                            }
                            break;

                        case NameMonster.Eye:

                            if (RaycastToPlayer() && distanceToPlayer < distanceToPlayerForChangeInAlert / 1.5f)
                            {
                                state = State.RunTarget;

                            }

                            if (MovetForward(positionPlayerOutSight, 0, speedNormal * 2))
                            {
                                state = State.Idle;
                            }
                            break;

                        case NameMonster.Nose:

                            int index = 0;
                            float temp = 0;
                            for (int i = 0; i < listScentPlayer.Count; i++)
                            {
                                if (listScentPlayer[i] > temp)
                                {
                                    index = i;
                                    temp = listScentPlayer[i];
                                }
                            }

                            positionMovet = listPoint[index];
                            if (RaycastToPlayer() && distanceToPlayer < distanceToPlayerForChangeInAlert / 2)
                            {
                                state = State.RunTarget;

                            }

                            if (MovetForward(positionMovet, 0, speedNormal * 2))
                            {
                                state = State.Idle;
                            }
                            break;

                        default: break;
                    }

                    break;

                case State.RunTarget:

                    animator.SetFloat(Speed, speedAttack);

                    if (RaycastToPlayer())
                    {
                        positionPlayerOutSight = player.GetComponentInChildren<TriggerCell>().positionCell;


                        if (MovetForward(positionPlayer, 1, speedAttack))
                        {
                            state = State.Attack;
                        }
                    }
                    else if (MovetForward(positionPlayerOutSight, 1, speedAttack))
                    {
                        state = State.Alert;
                        animator.SetFloat(Speed, speedNormal * 2);
                    }

                    break;

                case State.Attack:
                    gameContr.MinusLifes();
                    state = State.Idle;
                    break;

                case State.Rotation:
                    if (nameMonster == NameMonster.Eye)
                    {
                        Vector3 ray = scanerForward.TransformDirection(Vector3.forward);
                        if (Physics.Raycast(scanerForward.position, ray, out RaycastHit hit, distanceToPlayerForChangeInAlert))
                        {
                            if (hit.collider.CompareTag("Player"))
                            {
                                state = State.Alert;

                            }
                            else state = State.Scaner;
                        }
                    }
                    else state = State.Scaner;
                    break;
            }
        }
    }

    private void LateUpdate()
    {
        if (!gameContr.pausedGame)
        {
            positionPlayerOld = positionPlayer;
        }
    }
    
}
