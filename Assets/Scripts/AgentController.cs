using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentController : Agent
{
    [SerializeField]
    private Transform target;
    private float moveSpeed = 8f;

    private Rigidbody rb;

    private Animator animator;

    [SerializeField]
    private GameObject hunter;
    private HunterController hunterController;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        hunterController = hunter.GetComponent<HunterController>();

    }

    public override void OnEpisodeBegin()
    {
        animator.SetBool("isRunning", false);
        Vector3 randomTargetPosition = new Vector3(Random.Range(-10f, 8.5f), 0.01f, Random.Range(-7f, 10f));
        Vector3 randomPlayerPosition = new Vector3(Random.Range(-10f, 8.5f), 0.01f, Random.Range(-7f, 10f));
        float randomRotation = Random.Range(0f, 360f);

        if (randomTargetPosition == randomPlayerPosition)
        {
            randomTargetPosition = new Vector3(Random.Range(-10f, 10f), 0.1f, Random.Range(-10f, 10f));
        }

        target.localPosition = randomTargetPosition;
        target.Rotate(0, randomRotation, 0);
        this.transform.localPosition = randomPlayerPosition;
        hunterController.ResetHunter();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveRotate = actions.ContinuousActions[0];
        float moveForward = (actions.ContinuousActions[1]+1)/2;

        animator.SetBool("isRunning", true);
        rb.MovePosition(transform.position + transform.forward * moveForward * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(0f, moveRotate * moveSpeed, 0f, Space.Self);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");

    }

    private void OnTriggerEnter(Collider other)
    {
        animator.SetBool("isRunning", false);
        if (other.gameObject.tag == "Chest")
        {
            AddReward(5f);
            EndEpisode();
        }

        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Mummy")
        {
            AddReward(-1f);
            EndEpisode();
        }
    }
}
