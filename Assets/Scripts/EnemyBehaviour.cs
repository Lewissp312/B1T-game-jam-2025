using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class EnemyBehaviour : MonoBehaviour{
    private Transform _target;

    // ! move speed is set in the NavMeshAgent attached instead
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _attackInterval = 2f;
    private bool _canAttack = true;

    private NavMeshAgent _agent => GetComponentInChildren<NavMeshAgent>();

    // ----------------------------------------------------------------------------------------------- //

    void Start(){
        // set the target
        _target = GetTarget();
    }

    void FixedUpdate()
    {
        if (_target){
            _agent.destination = _target.position;
        }
        else{
            _target = GetTarget();
        }

        if (!_canAttack) return;

        // check if close enough to a flower and flower bed combo
        foreach (GameObject flower in GameObject.FindGameObjectsWithTag("Flower")){
            if (Vector2.Distance(transform.position, flower.transform.position) <= 2f){
                foreach (GameObject flBed in GameObject.FindGameObjectsWithTag("Flower bed")){
                    if (Vector2.Dot((flower.transform.position - transform.position).normalized, (flBed.transform.position - transform.position).normalized) >= 0.7f){
                        // means that the flower pot and flower are in the same direction so they are LIKELY overlapping
                        flower.GetComponent<HealthComponent>().TakeDamage(_damage);
                        StartCoroutine(CanAttackCooldown());

                        break;
                    }
                }
            }
        }
    }

    // ----------------------------------------------------------------------------------------------- //

    private Transform GetTarget(){
        // find the nearest gameobject with the 'Flower' tag
        GameObject[] flowers = GameObject.FindGameObjectsWithTag("Flower");
        Transform nearest = null;
        float distToNearest = float.MaxValue;
        foreach (GameObject fl in flowers){
            if (!fl.activeSelf) continue; // ignore inactive/destroyed flowers
            float distToFlower = Vector2.Distance(transform.position, fl.transform.position);
            if (distToFlower < distToNearest){
                nearest = fl.transform;
                distToNearest = distToFlower;
            }
        }  
        return nearest;
    }

    private IEnumerator CanAttackCooldown(){
        _canAttack = false;
        yield return new WaitForSeconds(_attackInterval);
        _canAttack = true;
    }
}