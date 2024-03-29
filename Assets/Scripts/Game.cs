using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Linq;
using static ProjectDataTask.EGETask;

public class Game : MonoBehaviour
{
    public GameObject enemy;

    public Button HitBtn;

    public GameObject TypeHits;

    public List<Texture2D> tasks;

    public RawImage premer;

    public List<TMP_Text> TextField;
    public GameObject TextFieldObj;
    private ProjectDataTask.EGETask _egeTask;

    private List<int> _usetTasks = new List<int>();

    private Head _head;
    private SpriteEnemy enemyState;


    private void Awake()
    {
        _head = this.GetComponent<Head>();
        enemyState = enemy.GetComponent<SpriteEnemy>();
    }


    public void OnTypeHitsSetActive()
    {
        TypeHits.SetActive(true);
        TypeHits.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = 
            !ProjectDataTask.Tasks
            .Where(t => t.Type == TaskType.LogUr)
            .All(t => _usetTasks.Any(u => u == t.Number));
        TypeHits.transform.GetChild(1).gameObject.GetComponent<Button>().interactable = 
            !ProjectDataTask.Tasks
            .Where(t => t.Type == TaskType.MinUr)
            .All(t => _usetTasks.Any(u => u == t.Number));
        TypeHits.transform.GetChild(2).gameObject.GetComponent<Button>().interactable = 
            !ProjectDataTask.Tasks
            .Where(t => t.Type == TaskType.LineUr)
            .All(t => _usetTasks.Any(u => u == t.Number));

        HitBtn.gameObject.SetActive(false);
    }

    public void OnClickLogUr()
    {
        TypeHits.SetActive(false);
        TextFieldObj.SetActive(true);
        UpdateTask(TextField, TaskType.LogUr);
    }

    public void OnClickLineUr()
    {
        TypeHits.SetActive(false);
        TextFieldObj.SetActive(true);
        UpdateTask(TextField, TaskType.LineUr);
    }

    public void OnClickMinUr()
    {
        TypeHits.SetActive(false);
        TextFieldObj.SetActive(true);
        UpdateTask(TextField, TaskType.MinUr);
    }


    private void UpdateTask(List<TMP_Text> TextFields, TaskType type)
    {
        if (!premer.gameObject.activeSelf) premer.gameObject.SetActive(true);
        var filTasks = ProjectDataTask.Tasks.Where(t => t.Type == type && !_usetTasks.Any(u => u == t.Number)).ToList();
        _egeTask = filTasks[ProjectDataTask.Rnd.Next(0, filTasks.Count)];

        _usetTasks.Add(_egeTask.Number);

        var mas = GetRandomArray();
        var requsts = new double[3];
        requsts[mas[0]] = GetRandRequst();
        requsts[mas[1]] = GetRandRequst();
        requsts[3 - mas.Sum()] = _egeTask.Correct;
        for(var i = 0; i < TextFields.Count; i++)
        {
            TextFields[i].text = requsts[i].ToString();
        }
        premer.texture = tasks[_egeTask.Number];
    }

    private int[] GetRandomArray()
    {
        var num1 = ProjectDataTask.Rnd.Next(0, 3);
        int num2;
        do {
            num2 = ProjectDataTask.Rnd.Next(0, 3);
        } while (num1 == num2);
        return new int[] { num1, num2 };
    }

    private double GetRandRequst()
    {
        return ProjectDataTask.Rnd.Next(200) - 100;
    }
    

    private void EnemyStep()
    {
        HitBtn.gameObject.SetActive(true);
        premer.gameObject.SetActive(false);
        TextFieldObj.SetActive(false);
        
    }

    public void OnClick(TMP_Text text)
    {
        if(text.text == _egeTask.Correct.ToString())
        {
            enemyState.FallHP(ProjectDataTask.Rnd.Next(10, 15), _egeTask.Type);
            EnemyStep();
        }
        else
        {
            _head.DeleteHead();
            EnemyStep();

        }
    }
}
