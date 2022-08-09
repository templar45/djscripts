using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Luke Sapir
//Editor: Christopher Cruz
//Notes on edit: Added weapon spread when shooting. Will implement ADS spread
public class RayCastWeapon : WeaponStats
{
    public int attack;
    public PoolManager bulletHolePool;
    public PoolManager damageGlobPool;
    public PoolManager DamageParticlePool;
    public PoolManager DustParticlePool;
    public PoolManager BulletLinePool;
    public PoolManager StrongHitPool;
    public PoolManager gunFlashPool;
    public PoolManager gunSmokePool;
    public PoolManager gunLightPool;
    public Transform gunTransform;
    public Camera shootingCamera;
    public float weaponRange;
    private float gunSpread;
    public float adsSpread;
    public float defaultSpread;
    public LayerMask ignoreMask;
    public GameObject playerStatsObject;
    public void Start()
    {
        playerStatsObject = GameObject.Find("Player");
        gunSpread = defaultSpread;
        ignoreMask = ~(ignoreMask);
    }

    public override void Shoot()
    {
        if(isReloading)
        {
            return;
        }
        if(isADS)
        {
            gunSpread = adsSpread;
            AdsFire.Invoke();
            //Debug.Log("aim");
        }
        else
        {
            //StartCoroutine(weaponSpread());
            gunSpread = defaultSpread;
            fireEvent.Invoke();
            //[[Debug.Log("hip");
        }
        HitScanLogic();
        magazineAmmo--;

    }
    public void HitScanLogic()
    {
        Ray ray = shootingCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        //taking targetPoint logic from Chris's prior coding work here, will shoot regardless of a viable target in range.
        Vector3 targetPoint;
       
        if (Physics.Raycast(shootingCamera.transform.position, getSpread(), out hit, weaponRange, ignoreMask))
        {
           
            LineSpawn(BulletLinePool, hit);
            EffectSpawn(gunFlashPool, gunTransform);
            EffectSpawn(gunSmokePool, gunTransform);
            EffectSpawn(gunLightPool, gunTransform);
            //fireEvent.Invoke();
            //magazineAmmo -= 1;


            if (hit.transform.gameObject.GetComponent<TargetPointStats>())
            {
                TargetPointStats myStats = hit.transform.gameObject.GetComponent<TargetPointStats>();
                myStats.TakeDamage(attack, playerStatsObject);
                if (myStats.uniqueDamagePool != null)
                {
                    EffectSpawn(damageGlobPool, hit, .1f);
                    EffectSpawn(myStats.uniqueDamagePool, hit);
                }
                else
                {
                    EffectSpawn(damageGlobPool, hit, .3f);
                    EffectSpawn(DamageParticlePool, hit);
                }
                if (myStats.multiplier > 1f)
                {
                    EffectSpawn(StrongHitPool, hit, .3f);
                }
                else if (myStats.multiplier < 1f)
                {

                }
            }
            else
            {
                EffectSpawn(bulletHolePool, hit, .001f, hit.transform);
                EffectSpawn(DustParticlePool, hit);
            }
            
        }
        else 
        {
            //Ray ray = shootingCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            targetPoint = ray.GetPoint(weaponRange);
            
            //Chris: Added spread when firing at any objected note tagged/layered
    
            targetPoint = shootingCamera.transform.position + shootingCamera.transform.forward * weaponRange;
            targetPoint = new Vector3(targetPoint.x + Random.Range(-gunSpread, gunSpread), targetPoint.y + Random.Range(-gunSpread, gunSpread), targetPoint.z + Random.Range(-gunSpread, gunSpread));
            //Vector3 direction = targetPoint - shootingCamera.transform.position;
            //end of edit

            LineSpawn(BulletLinePool, targetPoint);
            EffectSpawn(gunFlashPool, gunTransform);
            EffectSpawn(gunSmokePool, gunTransform);
            EffectSpawn(gunLightPool, gunTransform);
            //fireEvent.Invoke();
        }
      
    }
    public void HitScanLogic(Vector3 directionVector)
    {
        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(shootingCamera.transform.position, directionVector, out hit, weaponRange, ignoreMask))
        {

            LineSpawn(BulletLinePool, hit);
            EffectSpawn(gunFlashPool, gunTransform);
            EffectSpawn(gunSmokePool, gunTransform);
            EffectSpawn(gunLightPool, gunTransform);
            //fireEvent.Invoke();
           
            /*
            if (hit.transform.tag == "Wall")
            {
              
                //fireEvent.Invoke();

            }
            */

            if (hit.transform.gameObject.GetComponent<TargetPointStats>())
            {
                TargetPointStats myStats = hit.transform.gameObject.GetComponent<TargetPointStats>();
                myStats.TakeDamage(attack, playerStatsObject);
                if (myStats.uniqueDamagePool != null)
                {
                    EffectSpawn(damageGlobPool, hit, .1f);
                    EffectSpawn(myStats.uniqueDamagePool, hit);
                }
                else
                {
                    EffectSpawn(damageGlobPool, hit, .3f);
                    EffectSpawn(DamageParticlePool, hit);
                }
                if (myStats.multiplier > 1f)
                {
                    EffectSpawn(StrongHitPool, hit, .3f);
                }
                else if (myStats.multiplier < 1f)
                {

                }
               
            }
            else
            {
                EffectSpawn(bulletHolePool, hit, .001f, hit.transform);
                EffectSpawn(DustParticlePool, hit);
            }
        }
        else
        {
            Vector3 spreadForward = shootingCamera.transform.forward + directionVector;
            Ray ray = new Ray(shootingCamera.transform.position, spreadForward.normalized);
            targetPoint = ray.GetPoint(weaponRange);
            LineSpawn(BulletLinePool, targetPoint);
            EffectSpawn(gunFlashPool, gunTransform);
            EffectSpawn(gunSmokePool, gunTransform);
            EffectSpawn(gunLightPool, gunTransform);
            //fireEvent.Invoke();

        }
    }
    //canFire = false;
    public void EffectSpawn(PoolManager spawnPool, Transform spawnPoint)
    {
        for (int i = 0; i < spawnPool.holePool.Count; i++)
        {
            if (spawnPool.holePool[i].activeInHierarchy == false)
            {
                spawnPool.holePool[i].SetActive(true);
                spawnPool.holePool[i].transform.position = spawnPoint.position;
                spawnPool.holePool[i].transform.rotation = spawnPoint.rotation;
                break;
            }
        }
    }
    
    public void LineSpawn(PoolManager spawnPool, RaycastHit hit)
    {
        for (int i = 0; i < spawnPool.holePool.Count; i++)
        {
            if (spawnPool.holePool[i].activeInHierarchy == false)
            {
                spawnPool.holePool[i].SetActive(true);
                LineRenderer line = spawnPool.holePool[i].GetComponent<LineRenderer>();
                line.SetPosition(0, gunTransform.position);
                line.SetPosition(1, hit.point);
                break;
            }
        }
    }
    public void LineSpawn(PoolManager spawnPool, Vector3 hitPoint)
    {
        for (int i = 0; i < spawnPool.holePool.Count; i++)
        {
            if (spawnPool.holePool[i].activeInHierarchy == false)
            {
                spawnPool.holePool[i].SetActive(true);
                LineRenderer line = spawnPool.holePool[i].GetComponent<LineRenderer>();
                line.SetPosition(0, gunTransform.position);
                line.SetPosition(1, hitPoint);
                break;
            }
        }

    }
    public void EffectSpawn(PoolManager spawnPool, RaycastHit hit)
    {
        for (int i = 0; i < spawnPool.holePool.Count; i++)
        {
            if (spawnPool.holePool[i].activeInHierarchy == false)
            {
                spawnPool.holePool[i].SetActive(true);
                spawnPool.holePool[i].transform.position = Vector3.Lerp(hit.point, shootingCamera.transform.position, .001f);
                spawnPool.holePool[i].transform.rotation = Quaternion.LookRotation(hit.normal);
                break;
            }
        }
    }
    public void EffectSpawn(PoolManager spawnPool, RaycastHit hit, float offset)
    {
        for (int i = 0; i < spawnPool.holePool.Count; i++)
        {
            if (spawnPool.holePool[i].activeInHierarchy == false)
            {
                spawnPool.holePool[i].SetActive(true);
                spawnPool.holePool[i].transform.position = Vector3.Lerp(hit.point, shootingCamera.transform.position, offset);
                spawnPool.holePool[i].transform.rotation = Quaternion.LookRotation(hit.normal);
                break;
            }
        }
    }
    public void EffectSpawn(PoolManager spawnPool, RaycastHit hit, float offset, Transform parentTransform)
    {
        for (int i = 0; i < spawnPool.holePool.Count; i++)
        {
            if (spawnPool.holePool[i].activeInHierarchy == false)
            {
                spawnPool.holePool[i].SetActive(true);
                spawnPool.holePool[i].transform.position = Vector3.Lerp(hit.point, shootingCamera.transform.position, offset);
                if (parentTransform.parent != null)
                {
                    spawnPool.holePool[i].transform.parent = parentTransform.parent;
                }
                spawnPool.holePool[i].transform.rotation = Quaternion.LookRotation(hit.normal);
                break;
            }
        }
    }
    Vector3 getSpread()
    {
        Vector3 targetPos = gunTransform.transform.position + gunTransform.transform.forward * weaponRange;

        targetPos = new Vector3(targetPos.x + Random.Range(-gunSpread, gunSpread), targetPos.y + Random.Range(-gunSpread, gunSpread), targetPos.z + Random.Range(-gunSpread, gunSpread));

        Vector3 direction = targetPos - gunTransform.transform.position;
        return direction.normalized;
    }
    /*
    public IEnumerator weaponSpread()
    {
        if(isShooting & !isADS)
        {

        }
    }
    */
}

