using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plateformeBoss : MonoBehaviour
{

    public float rotationSpeed = 20f;

    public bool isUp = false;

    private void Start(){
        toUp(Random.Range(2f, 8f));
    }

    private void Update()
    {
        if(isUp){
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }


    public void toUp(float height)
    {
        StartCoroutine(Up(height));

        IEnumerator Up(float height)
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
