using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AiMageBoss : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject shield;
    [SerializeField] GameObject shield2;
    [SerializeField] GameObject transitionShield;
    [SerializeField] GameObject sporeProj;
    [SerializeField] GameObject gasProj;
    [SerializeField] EnemyStats enemyStats;
    [SerializeField] GameObject summon0;
    [SerializeField] GameObject summon1;
    [SerializeField] GameObject summon2;
    [SerializeField] GameObject summon3;
    [SerializeField] GameObject summon4;
    [SerializeField] GameObject summon5;
    [SerializeField] GameObject summon6;
    [SerializeField] GameObject summon7;
    [SerializeField] GameObject summon8;
    [SerializeField] GameObject summon9;
    [SerializeField] GameObject summon10;
    [SerializeField] GameObject bossSummon;
    [SerializeField] GameObject hRoots;
    [SerializeField] GameObject castCircle;
    [SerializeField] GameObject charm;
    [SerializeField] Animator animator;
    private bool lookingRight;
    private bool casting;
    private bool magicShield;
    private List<GameObject> summonCircles;
    private List<GameObject> summonedCircles;
    private int poisonProjCount;
    private float castDelay;
    private bool stage2;
    private bool stopCasts;

    private void Start()
    {

        castDelay = 3.5f;
        summonCircles = new List<GameObject> {
            summon1, summon2, summon3, summon4, summon5, summon6, summon7, summon8, summon9, summon10,
        };
        summonedCircles = new List<GameObject>();
        StartCoroutine(Ability());
    }

    private void Update()
    {
        // Enter Stage 2 if at 25% hp
        if (enemyStats.currentHealth / enemyStats.maxHealth <= 0.25f && !stage2) {
            castDelay = 3f;
            stage2 = true;
            bossSummon.SetActive(true);
            stopCasts = true;
            transitionShield.SetActive(true);
            casting = true;
            animator.SetBool("Stage2", true);
            hRoots.GetComponent<RootFollow>().speed = 5;
            foreach(GameObject obj in summonCircles) {
                obj.GetComponent<SummonCircle>().stage2 = true;
            }
            StartCoroutine(EnterStage2());
        }

        if (!casting) {
            // Chase player
            float speed = enemyStats.speed;
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, enemyStats.speed * Time.deltaTime);

            // Fix rotation
            if (transform.position.x - player.transform.position.x < 0 && lookingRight) {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
                lookingRight = !lookingRight;
            }
            if (transform.position.x - player.transform.position.x > 0 && !lookingRight) {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
                lookingRight = !lookingRight;
            }
        }
    }

    private IEnumerator EnterStage2() {
        yield return new WaitForSeconds(6);
        casting = false;
        stopCasts = false;
        transitionShield.SetActive(false);
        StartCoroutine(Ability());
    }

    private IEnumerator Ability() {
        if (!stopCasts) {
            yield return new WaitForSeconds(castDelay);
            animator.SetBool("Casting", true);
            casting = true;
        }
    }

    private void EndCastAnimation() {
        if (enemyStats.speed == 7) {
            enemyStats.speed = 0.5f;
        }
        animator.SetBool("Casting", false);
        castCircle.SetActive(true);
        int ability = Random.Range(0,7);
        if (ability == 0) {
            if (!magicShield) {
                MagicShield();
            } else {
                SporeProjectile();
            }
        } else if (ability == 1) {
            SporeProjectile();
        } else if (ability == 2) {
            SummonEnemies();
        } else if (ability == 3) {
            StartCoroutine(SpawnGasProj());
        } else if (ability == 4) {
            HomingRoots();
        } else if (ability == 5) {
            Dash();
        } else if (ability == 6) {
            Hypnosis();
        }
        StartCoroutine(CastingTime());
        StartCoroutine(Ability());
    }

    private IEnumerator CastingTime() {
        yield return new WaitForSeconds(1f);
        castCircle.SetActive(false);
        casting = false;
    }

    private void MagicShield() {
        if (!stage2) {
            shield.SetActive(true);
        } else {
            shield2.SetActive(true);
        }
        enemyStats.shielded = true;
        magicShield = true;
        StartCoroutine(MagicShieldOff());
    }
    
    private IEnumerator MagicShieldOff() {
        yield return new WaitForSeconds(4f);
        enemyStats.shielded = false;
        magicShield = false;
        shield2.SetActive(false);
        shield.SetActive(false);
    }

    private void SummonEnemies() {
        int rand;
        if (!stage2) {
            rand = Random.Range(1,4);
        } else {
            rand = Random.Range(1,3);
        }
        for (int i = 0; i < rand; i++) 
        {
            GetClosestEnemy().SetActive(true);
        }
        summonedCircles.Clear();
    }

    private IEnumerator SpawnGasProj() {
        if (!stage2) {
            GameObject proj = Instantiate(gasProj, new Vector3(player.position.x,player.position.y + 6f, 0), Quaternion.identity);
            proj.SetActive(true);
        } else {
            if (poisonProjCount == 0) {
                GameObject proj = Instantiate(gasProj, new Vector3(player.position.x,player.position.y + 6f, 0), Quaternion.identity);
                proj.SetActive(true);
                poisonProjCount += 1;
                StartCoroutine(SpawnGasProj());
            } else if (poisonProjCount == 1) {
                yield return new WaitForSeconds(1.3f);
                GameObject proj = Instantiate(gasProj, new Vector3(player.position.x,player.position.y + 6f, 0), Quaternion.identity);
                proj.SetActive(true);
                poisonProjCount = 0;
            }
        }
    }

    private void Dash() {
        SporeProjectile();
        enemyStats.speed = 7;
    }
    

    private void Hypnosis() {
        if (!stage2) {
            Vector3 pos = new Vector3(Random.Range(transform.position.x-2f,transform.position.x+2f),Random.Range(transform.position.y-2f,transform.position.y+2f), 0);
            GameObject proj = Instantiate(charm, pos, Quaternion.identity);
            proj.SetActive(true);
        } else {
            Vector3 pos = new Vector3(Random.Range(transform.position.x-4f,transform.position.x+4f),Random.Range(transform.position.y-4f,transform.position.y+4f), 0);
            GameObject proj = Instantiate(charm, pos, Quaternion.identity);
            proj.SetActive(true);
            pos = new Vector3(Random.Range(transform.position.x-4f,transform.position.x+4f),Random.Range(transform.position.y-4f,transform.position.y+4f), 0);
            proj = Instantiate(charm, pos, Quaternion.identity);
            proj.SetActive(true);
        }
    }

    private void HomingRoots() {
        GameObject newRoots = Instantiate(hRoots, transform.position,Quaternion.identity);
        newRoots.SetActive(true);
        StartCoroutine(DespawnRoots(newRoots));
    }

    private IEnumerator DespawnRoots(GameObject root) {
        if (!stage2) {
            yield return new WaitForSeconds(3f);
        } else {
            yield return new WaitForSeconds(4f);
        }
        Destroy(root);
    }

    private void SporeProjectile() {
        if (!stage2) {
            Vector3 pos;
            for(int i = 0; i < 20; i++) {
                pos = new Vector3(Random.Range(transform.position.x-3f,transform.position.x+3f),Random.Range(transform.position.y-3f,transform.position.y+3f), 0);
                GameObject proj = Instantiate(sporeProj, pos, Quaternion.identity);
                proj.SetActive(true);
            }
        } else {
            Vector3 pos;
            for(int i = 0; i < 40; i++) {
                pos = new Vector3(Random.Range(transform.position.x-6f,transform.position.x+6f),Random.Range(transform.position.y-6f,transform.position.y+6f), 0);
                GameObject proj = Instantiate(sporeProj, pos, Quaternion.identity);
                proj.SetActive(true);
            }
        }
    }

    GameObject GetClosestEnemy ()
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = player.transform.position;
        foreach(GameObject potentialTarget in summonCircles)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr && !summonedCircles.Contains(potentialTarget))
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        summonedCircles.Add(bestTarget);
        return bestTarget;
    }

}
