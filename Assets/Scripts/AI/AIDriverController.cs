using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIDriverController : MonoBehaviour {

    public float speed;
    public NavMeshAgent agent;

    public ComponentHolder componentHolder;

    public Transform guide;

    public Vector3 direction;
    public List<AIDriverController> boidsInScene;


    public float moveToCenterStrength;//factor by which boid will try toward center Higher it is, higher the turn rate to move to the center
    public float localBoidsDistance;//effective distance to calculate the center

    public float avoidOtherStrength;//factor by which boid will try to avoid each other. Higher it is, higher the turn rate to avoid other.
    public float collisionAvoidCheckDistance;//distance of nearby boids to avoid collision

    public float alignWithOthersStrength;//factor determining turn rate to align with other boids
    public float alignmentCheckDistance;//distance up to which alignment of boids will be checked. Boids with greater distance than this will be ignored

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(guide.position, 1);
    }

    private void Start()
    {
        this.GetComponent<MeshRenderer>().material = componentHolder.material;
        agent.acceleration = componentHolder.acceleration;
        agent.speed = componentHolder.maximumSpeed;
        
        foreach (AIDriverController driver in GameObject.FindObjectsByType(typeof(AIDriverController), FindObjectsSortMode.None)) {
            if(driver != this) {
                boidsInScene.Add(driver);
            }
        }
    }

    void Update() {
        MoveToCenter();
        AlignWithOthers();
        AvoidOtherBoids();
        //transform.LookAt(new Vector3(guide.position.x, this.transform.position.y, guide.position.z));
        //transform.Translate((direction) * (speed * Time.deltaTime));
        //agent.SetDestination(direction + transform.position);
        agent.SetDestination(guide.position);
    }

    void MoveToCenter()
    {

        Vector3 positionSum = transform.position;//calculate sum of position of nearby boids and get count of boid
        int count = 0;

        foreach (AIDriverController boid in boidsInScene)
        {
            float distance = Vector3.Distance(boid.transform.position, transform.position);
            if (distance <= localBoidsDistance)
            {
                positionSum += (Vector3)boid.transform.position;
                count++;
            }
        }

        if (count == 0)
        {
            return;
        }

        //get average position of boids
        Vector3 positionAverage = positionSum / count;
        positionAverage = positionAverage.normalized;
        Vector3 faceDirection = (positionAverage - (Vector3)transform.position).normalized;

        //move boid toward center
        float deltaTimeStrength = moveToCenterStrength * Time.deltaTime;
        direction = direction + deltaTimeStrength * faceDirection / (deltaTimeStrength + 1);
        direction = direction.normalized;
    }

    void AvoidOtherBoids() {

        Vector3 faceAwayDirection = Vector3.zero;//this is a vector that will hold direction away from near boid so we can steer to it to avoid the collision.

        //we need to iterate through all boid
        foreach (AIDriverController boid in boidsInScene)
        {
            float distance = Vector3.Distance(boid.transform.position, transform.position);

            //if the distance is within range calculate away vector from it and subtract from away direction.
            if (distance <= collisionAvoidCheckDistance)
            {
                faceAwayDirection = faceAwayDirection + (Vector3)(transform.position - boid.transform.position);
            }
        }

        faceAwayDirection = faceAwayDirection.normalized;//we need to normalize it so we are only getting direction

        direction = direction + avoidOtherStrength * faceAwayDirection / (avoidOtherStrength + 1);
        direction = direction.normalized;
    }

    void AlignWithOthers() {
        //we will need to find average direction of all nearby boids
        Vector3 directionSum = Vector3.zero;
        int count = 0;

        foreach (var boid in boidsInScene)
        {
            float distance = Vector3.Distance(boid.transform.position, transform.position);
            if (distance <= alignmentCheckDistance)
            {
                directionSum += boid.direction;
                count++;
            }
        }

        Vector3 directionAverage = directionSum / count;
        directionAverage = directionAverage.normalized;

        //now add this direction to direction vector to steer towards it
        float deltaTimeStrength = alignWithOthersStrength * Time.deltaTime;
        direction = direction + deltaTimeStrength * directionAverage / (deltaTimeStrength + 1);
        direction = direction.normalized;

    }

}
