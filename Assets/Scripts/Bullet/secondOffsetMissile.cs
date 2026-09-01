using UnityEngine;

public class secondOffsetMissile : MonoBehaviour
{
    [Header("References")]
    private Transform target;               // assign at spawn, or auto-find
    //public string enemyTag = "Enemy";

    [Header("Speed")]
    public float speed = 8f;
    public float acceleration = 2f;         // speed ramps up over time
    public float maxSpeed = 10f;

    [Header("Initial Curve Phase")]
    public float initialCurveDuration = 0.5f;   // how long it flies "dumb" before homing
    public float initialCurveAngle = 60f;       // degrees to curve away from straight-forward
    public AnimationCurve curveEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Homing")]
    public float turnSpeed = 200f;          // degrees/sec once homing kicks in
    public float homingRampTime = 0.4f;     // time to smoothly ramp turnSpeed from 0 to full
    public float targetSearchRadius = 15f;  // used if no target assigned

    [Header("Lifetime")]
    public float lifeTime = 5f;

    private float timeAlive = 0f;
    private float currentSpeed;
    private Vector2 launchDirection;
    private float initialCurveSign; // +1 or -1, randomized so missiles fan out

    [SerializeField] float missileDamage;
    private ObjectPooler destroyEffectPool;

    void OnEnable()
    {
        destroyEffectPool = GameObject.Find("BoomPool").GetComponent<ObjectPooler>();
        currentSpeed = speed;
        launchDirection = transform.up; // assumes sprite faces "up"
        initialCurveSign = Random.value > 0.5f ? 1f : -1f;

        if (target == null)
            FindNearestTarget();
    }

    void FixedUpdate()
    {
        timeAlive += Time.fixedDeltaTime;
        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.fixedDeltaTime, maxSpeed);

        if (timeAlive <= initialCurveDuration)
        {
            DoInitialCurve();
        }
        else
        {
            DoHoming();
        }

        // move forward along current facing
        transform.position += transform.up * currentSpeed * Time.fixedDeltaTime;

        if (timeAlive >= lifeTime)
        gameObject.SetActive(false);
    }

    void DoInitialCurve()
    {
        // Ease the rotation offset in over the curve duration, then it settles
        float t = timeAlive / initialCurveDuration;
        float eased = curveEase.Evaluate(t);
        float currentAngleOffset = initialCurveSign * initialCurveAngle * eased;

        Quaternion baseRot = Quaternion.LookRotation(Vector3.forward, launchDirection);
        Quaternion curveRot = baseRot * Quaternion.Euler(0, 0, currentAngleOffset);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, curveRot, 720f * Time.fixedDeltaTime);
    }

    void DoHoming()
    {
        if (target == null)
        {
            FindNearestTarget();
            if (target == null) return; // no target, keep flying straight
        }

        Vector2 direction = (target.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

        // ramp turn speed in smoothly so the transition from curve->homing isn't a snap
        float homingTime = timeAlive - initialCurveDuration;
        float rampFactor = Mathf.Clamp01(homingTime / homingRampTime);
        float currentTurnSpeed = turnSpeed * rampFactor;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, currentTurnSpeed * Time.fixedDeltaTime);
    }

    void FindNearestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDist = Mathf.Infinity;
        Transform closest = null;

        foreach (var enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < closestDist && dist <= targetSearchRadius)
            {
                closestDist = dist;
                closest = enemy.transform;
            }
        }
        target = closest;
    }

//    void OnCollisionEnter2D(Collision2D col)
//     {
//         if(col.gameObject.CompareTag("Obstacles"))
//         {
//             Asteroid asteroid = col.gameObject.GetComponent<Asteroid>();
//             Meteor meteor = col.gameObject.GetComponent<Meteor>();
//             if(asteroid) asteroid.TakeDamage(missileDamage);
//             if(meteor) meteor.TakeDamage(missileDamage);
//             GameObject destroyEffect = destroyEffectPool.GetPooledObject();
//             destroyEffect.transform.position = transform.position;
//             destroyEffect.transform.rotation = transform.rotation;
//             destroyEffect.SetActive(true);            
//             gameObject.SetActive(false); 
//         }
//         else if(col.gameObject.CompareTag("Enemy"))
//         {
//             // GetComponent of the actual gameobject name and not the tag or layer of it
//             EnemyShipWave enemyShipWave = col.gameObject.GetComponent<EnemyShipWave>();
//             Enemy enemy = col.gameObject.GetComponent<Enemy>();
//             GameObject destroyEffect = destroyEffectPool.GetPooledObject();
//             destroyEffect.transform.position = transform.position;
//             destroyEffect.transform.rotation = transform.rotation;
//             destroyEffect.SetActive(true);      
//             if(enemyShipWave) enemyShipWave.TakeDamage(missileDamage);
//             if(enemy)enemy.TakeDamage(missileDamage);
//             gameObject.SetActive(false);
//             Debug.Log("Enemy ship is taking damage");            
//         }
//     }
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            // GetComponent of the actual gameobject name and not the tag or layer of it
            EnemyShipWave enemyShipWave = col.gameObject.GetComponent<EnemyShipWave>();
            EnemyBug enemyBug = col.gameObject.GetComponent<EnemyBug>();
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            OctopusWave octopusWave = col.gameObject.GetComponent<OctopusWave>();
            BugWave bugWave = col.gameObject.GetComponent<BugWave>();
            BeetleWave beetleWave = col.gameObject.GetComponent<BeetleWave>();
            if(enemyShipWave) enemyShipWave.TakeDamage(missileDamage);
            if(enemy)enemy.TakeDamage(missileDamage);
            if(octopusWave)octopusWave.TakeDamage(missileDamage);
            if(enemyBug)enemyBug.TakeDamage(missileDamage);
            if(bugWave)bugWave.TakeDamage(missileDamage);
            if(beetleWave)beetleWave.TakeDamage(missileDamage);   
            gameObject.SetActive(false);               
        }        
    }
}

