using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class Bola : Agent
{
    Rigidbody rBody;
    public Transform portalA;
    public Transform portalB;
    public Transform cube;
    public float speed;
    public float cubeLimitZ1;
    public float cubeLimitZ2;
    public float cubeLimitX1;
    public float cubeLimitX2;
    public float portalLimitX1;
    public float portalLimitX2;
    public float offsetPortal;
    public float fallHeight;
    public float rewardCube;
    public float rewardPortal;
    public float rewardPortalNegative;

    const float ballCubeHeight = 0.5f;
    const float cubeDistance = 1.42f;
    const float portalHeight = 1f;
    const float portalAZPos = 5.25f;
    const float portalBZPos = 7.75f;
    const float counterIncrease = 0.12f;
    const float timeOut = 90f;
    bool passedPortal;
    bool returnedPortal;
    float counter;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        counter += (counterIncrease * Time.deltaTime);
        if (counter > timeOut)
        {
            SetReward(rewardPortalNegative);
            Debug.Log("Acabou o tempo");
            EndEpisode();
            counter = 0f;
        }
    }

    public override void OnEpisodeBegin()
    {
        if (this.transform.localPosition.y < fallHeight)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
        }

        passedPortal = false;
        returnedPortal = false;
        this.transform.localPosition = new Vector3(0, ballCubeHeight, 0);
        cube.localPosition = new Vector3(Random.Range(cubeLimitX1, cubeLimitX2), ballCubeHeight, Random.Range(cubeLimitZ1, cubeLimitZ2));
        portalA.localPosition = new Vector3(Random.Range(portalLimitX1, portalLimitX2), portalHeight, portalAZPos);
        portalB.localPosition = new Vector3(Random.Range(portalLimitX1, portalLimitX2), portalHeight, portalBZPos);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(cube.localPosition);
        sensor.AddObservation(portalA.localPosition);
        sensor.AddObservation(portalB.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rBody.AddForce(controlSignal * speed);

        float distanceToTarget = Vector3.Distance(this.transform.localPosition, cube.localPosition);

        if (distanceToTarget < cubeDistance)
        {
            SetReward(rewardCube);
            Debug.Log("Chegou no cubo");
            counter = 0f;
            EndEpisode();
        }

        if (this.transform.localPosition.y < fallHeight)
        {
            Debug.Log("Caí");
            counter = 0f;
            EndEpisode();
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("A"))
        {
            Debug.Log("Passou do portal");
            this.transform.localPosition = new Vector3(portalB.localPosition.x, ballCubeHeight, portalB.localPosition.z + offsetPortal);
            passedPortal = true;
            SetReward(rewardPortal);
        }

        if (other.gameObject.CompareTag("B"))
        {
            returnedPortal = true;
            SetReward(rewardPortalNegative);
            counter = 0f;
            EndEpisode();
            Debug.Log("Deu ruim");
        }
    }
}