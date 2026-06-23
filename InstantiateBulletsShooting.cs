using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InstantiateBulletsShooting : MonoBehaviour
{

    [SerializeField] public float number;
    
    [SerializeField]         GameObject _prefab;
    public Transform ObjectToShoot;
    [SerializeField] float _timeWaitShooting;

    // Start is called before the first frame update
    void Start() {

           StartCoroutine(_shootingWorker());
    }
    IEnumerator _shootingWorker()
    {
        bool isWork = enabled;
        while (isWork){
            var _vector3direction = (ObjectToShoot.position - transform.position).normalized;
             var NewBullet = Instantiate(_prefab, transform.position + _vector3direction, Quaternion.identity);

                NewBullet.GetComponent<Rigidbody>().transform.up = _vector3direction;
            NewBullet.GetComponent<Rigidbody>().velocity = _vector3direction * number;

              yield return new WaitForSeconds(_timeWaitShooting);
         }


    }
    public void Update(){
        // Update is called once per frame
    }




    
}