using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 1f;
    public AgentShooter owner;
    public Transform barrel;
    public float cooldown = 0.5f;
    float lastShoot = -1f;
    public bool CanFire()
    {
        return lastShoot + cooldown <= Time.time;
    }
    public void Fire()
    {
        if (!CanFire())
        {
            return;
        }
        lastShoot = Time.time;
        RaycastHit hit;
        if (Physics.Raycast(barrel.position, barrel.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            var agent = hit.collider.GetComponent<AgentShooter>();
            if (agent != null)
            {
                agent.TakeDamage(damage, owner);
            }
            Debug.DrawRay(barrel.position, barrel.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        }
        else
        {
            Debug.DrawRay(barrel.position, barrel.TransformDirection(Vector3.forward) * 1000, Color.white);
        }
    }

    public void Reset()
    {
        lastShoot = Time.time - 2 * cooldown;
    }

    // Update is called once per frame
    void Update()
    {

    }
}