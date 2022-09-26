using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour{
    
    public int health = 25;
    bool isDead = false;

    EnemyScript enemyScript;
    
    public ParticleSystem hitParticle;

    public SkinnedMeshRenderer enemyMesh;
    Material enemyMat;

    void Start()
    {
        enemyScript = GetComponent<EnemyScript>();

        enemyMat = enemyMesh.material;
        enemyMesh.material = enemyMat;

        enemyMat.SetColor("_EmissionColor", Color.black);
    }

    public void CauseDamage(int amount)
    {   
        if(isDead)
            return;

        health -= amount;

        if(hitParticle != null)
            hitParticle.Play();

        Invoke("StopParticle", 0.5f);

        enemyScript.TargetPlayer();
        StartCoroutine(Damaged());

        if(health <= 0)
        {
            isDead = true;
            enemyScript.Death();
        }
    }

    void StopParticle()
    {
        if(hitParticle != null)
            hitParticle.Stop();
    }

    IEnumerator Damaged()
    {
        enemyMat.SetColor("_EmissionColor", Color.red);
        yield return new WaitForSeconds(0.15f);
        enemyMat.SetColor("_EmissionColor", Color.black);
    } 
}