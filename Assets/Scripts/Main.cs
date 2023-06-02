using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public int count = 6 * 7;
    public List<string> nameList = new List<string>() {"Way","Soil","Rock" };
    public int[,] data = new int[6, 7];
    List<ItemBase> items = new List<ItemBase>();
    void Start()
    {
        
        for (int i = 0; i < count; i++)
        {
            var name = RandomGeneration();
            var obj = Resources.Load<GameObject>(name);
            var item = Instantiate(obj, transform);
            var itemBase = item.transform.GetChild(0).GetComponent<ItemBase>();

            // �洢��һ����¼�����ݽṹ��
            var type = name == "Way" ? 0 : 1;
            data[i/7,i%7] = type;
            itemBase.type = type;
            items.Add(itemBase);
        }
        
        // ��ͼ������ɺ󣬸��ݼ�¼����������
        // ������ͬ�ؿ�����
        // ����·����ʾ��·������ 
        FindOnes(data);

        
        //var isConnected = IsConnected(data);
        //Debug.LogError("==========" + isConnected);
    }


    //�ṩһ���ӿ�ˢ�½������ݽṹ
    public void UpdateData()
    {
        for (int i = 0; i < items.Count; i++)
        {
            data[i / 7, i % 7] = items[i].type;
            //ȡ��֮ǰ������
            items[i].transform.parent.GetChild(1).gameObject.SetActive(false);
        }
        //������������
        FindOnes(data);
        var isConnected = IsConnected(data);
        Debug.LogError("==========" + isConnected);
        if (isConnected) //����е�·�ܵ����·�
        {
            DownMove();
        }
    }

    public void DownMove()
    {
        //�����µĵ�ͼ
        for (int i = 6; i >= 0; i--)
        {
            DestroyImmediate(items[i].transform.parent.gameObject);
            //����
            items.RemoveAt(i);
        }
        //��data����ĵ�һ���Ƴ���������ƶ���ǰ�棬���һ�����¸�ֵ
        int[] newData = new int[7];
        for (int i = 0; i < 7; i++)
        {
            var name = RandomGeneration();
            var type = name == "Way" ? 0 : 1;
            newData[i] = type;

            var obj = Resources.Load<GameObject>(name);
            var item = Instantiate(obj, transform);
            var itemBase = item.transform.GetChild(0).GetComponent<ItemBase>();
            itemBase.type = type;
            items.Add(itemBase);
        }
        ShiftRows(data, newData);
        UpdateData();
    }

    /*
    int[,] data = {
    {1,0,0,1,1,1,1},
    {0,1,0,1,1,1,1},
    {1,0,1,1,1,0,1},
    {0,1,1,1,0,1,1},
    {0,1,0,1,1,1,1},
    {1,0,1,1,1,1,0},
};C#���и���ά���飬����һ��{1,0,0,1,1,1,1}�Ƴ������������ƣ��ڶ��б�ɵ�һ�У������б�ɵڶ��У������б�ɵ����У��Դ˵��ƣ�
    ���һ�в����µ�һ�У��ṩһ������
     
     
     */
    public static void ShiftRows(int[,] data, int[] newRow)
    {
        int numRows = data.GetLength(0);

        // �����������ƶ�һ��
        for (int i = 0; i < numRows - 1; i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                data[i, j] = data[i + 1, j];
            }
        }

        // ���һ�б�Ϊ����
        for (int j = 0; j < data.GetLength(1); j++)
        {
            data[numRows - 1, j] = newRow[j];
        }
    }

    public bool IsConnected(int[,] data)
    {
        // ������һ������Ԫ�أ��ҵ���һ��Ϊ0��Ԫ��
        for (int j = 0; j < data.GetLength(1); j++)
        {
            if (data[0, j] == 0)
            {
                // �Ӹ�Ԫ�ؿ�ʼ���������������
                bool[,] visited = new bool[data.GetLength(0), data.GetLength(1)];
                if (DFS(data, visited, 0, j))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool DFS(int[,] data, bool[,] visited, int i, int j)
    {
        // ��ǵ�ǰ�ڵ�Ϊ�ѷ���
        visited[i, j] = true;

        // �����ǰ�ڵ������һ�У�������ܹ����ӳɹ�
        if (i == data.GetLength(0) - 1)
        {
            return true;
        }

        // ������ǰ�ڵ����������Ƿ���0�����������������������
        if (i > 0 && data[i - 1, j] == 0 && visited[i - 1, j] == false && DFS(data, visited, i - 1, j))
        {
            return true;
        }
        if (i < data.GetLength(0) - 1 && data[i + 1, j] == 0 && visited[i + 1, j] == false && DFS(data, visited, i + 1, j))
        {
            return true;
        }
        if (j > 0 && data[i, j - 1] == 0 && visited[i, j - 1] == false && DFS(data, visited, i, j - 1))
        {
            return true;
        }
        if (j < data.GetLength(1) - 1 && data[i, j + 1] == 0 && visited[i, j + 1] == false && DFS(data, visited, i, j + 1))
        {
            return true;
        }

        return false;
    }


    public void FindOnes(int[,] data)
    {
        // ������ά����
        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                // �����������Ӧλ��Ϊ1����������������Ƿ�ҲΪ1
                if (data[i, j] == 1)
                {
                    bool up = (i == 0 || data[i - 1, j] == 1); // ����Ϸ�
                    bool down = (i == data.GetLength(0) - 1 || data[i + 1, j] == 1); // ����·�
                    bool left = (j == 0 || data[i, j - 1] == 1); // ������
                    bool right = (j == data.GetLength(1) - 1 || data[i, j + 1] == 1); // ����Ҳ�

                    // ����������Ҷ���1���������λ��
                    if (up && down && left && right)
                    {
                        Debug.LogError($"({i}, {j})");
                        transform.GetChild(i * 7 + j).transform.Find("Mask").gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    string RandomGeneration()
    {
        int rock = 29;
        int soil = 34;
        int way = 37;
        // ������Ȩ��
        int totalWeight = rock + soil + way;

        int randomValue = Random.Range(0, totalWeight);

        if (randomValue < rock)
        {
            return nameList[2];
        }
        else if (randomValue < soil + way)
        {
            return nameList[1];
        }
        else
        {
            return nameList[0];
        }
    }
}
