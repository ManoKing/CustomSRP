using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Soil : ItemBase
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(() => {
            // ��ɵ�·
            type = 0;
            transform.GetComponent<CanvasGroup>().alpha = 0;
            // ֪ͨȫ�֣�ˢ�½���
            transform.parent.parent.GetComponent<Main>().UpdateData();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
