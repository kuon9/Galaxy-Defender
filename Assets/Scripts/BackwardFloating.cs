using UnityEngine;

public class BackwardFloating : MonoBehaviour
{
    [SerializeField] int speed;

    void Update()
    {
        float moveX = (speed * PlayerMovement.instance.boost) * Time.deltaTime;        
        transform.position += new Vector3(moveX, 0);
        if(transform.position.x >= 18)
        {
            gameObject.SetActive(false);
        }
    }
}
