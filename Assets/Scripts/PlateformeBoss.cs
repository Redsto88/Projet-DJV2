using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformeBoss : MonoBehaviour
{

    public float rotationSpeed = 20f;
    public float height = 5;
    public bool isUp = false;



    private void Update()
    {
        if(isUp){
            transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
        }
    }


    public void ToUp()
    {
        StartCoroutine(Up());

        IEnumerator Up()
        {
            yield return new WaitForSeconds(1f);
            float time = 0;
            float speed = height/15;
            float startHeight = transform.position.y;
            while (time < 1)
            {
                time += Time.deltaTime * speed;
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(startHeight, startHeight+height, time), transform.position.z);
                yield return null;
            }
            isUp = true;
        }
    }
}
