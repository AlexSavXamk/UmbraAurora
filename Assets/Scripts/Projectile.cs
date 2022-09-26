using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour{

    public bool isDarkParticle = false;

    public float speed;

    public MeshRenderer meshRend;
    public Collider col;
    public GameObject hitParticles;
    public GameObject particleLight;

    public int damage = 6;

    public ParticleSystem particles;
    bool isDestroyed = false;

    void Start()
    {
        Invoke("DestroyParticle", 10f);
    }

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    void OnTriggerEnter(Collider other)
    {   
        if(other.gameObject.tag == "Destructible")
            other.gameObject.GetComponent<Destructible>().Damage(5);

        if(!isDarkParticle)
        {
            if(other.gameObject.tag == "EnemyDark")
            {
                other.gameObject.GetComponent<EnemyHealth>().CauseDamage(damage);
            }
        }else
        {
            if(other.gameObject.tag == "EnemyLight")
            {
                other.gameObject.GetComponent<EnemyHealth>().CauseDamage(damage);
            }
        }
        
        if(other.gameObject.tag != "Player")
            DestroyParticle();
    }

    void DestroyParticle()
    {
        if(isDestroyed == true)
            return;

        if(hitParticles != null)
            Instantiate(hitParticles, transform.position, transform.rotation);
        
        particles.Stop();
        meshRend.enabled = false;
        col.enabled = false;
        particleLight.SetActive(false);
        //gameObject.SetActive(false);

        isDestroyed = true;
    }
}
