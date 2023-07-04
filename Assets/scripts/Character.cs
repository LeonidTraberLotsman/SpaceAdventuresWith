using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject Target;

    public enum human_state
    {
        alive,
        dead
    }

    public ShipManager ship;

    public human_state State;

    public List<Task> my_tasks = new List<Task>();
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        my_tasks.Add(new ReachPointTask(transform.position+new Vector3(5,0, 5)));
        my_tasks.Add(new ReachPointTask(transform.position+new Vector3(10,0, 5)));
        my_tasks.Add(new ReachPointTask(transform.position+new Vector3(5,0, 10)));

        //StartCoroutine(X_task.Do(this));

        StartCoroutine(DoTasks());
        
    }

    public IEnumerator DoTasks()
    {
        yield return null;
        while (my_tasks.Count>0)
        {
            yield return  my_tasks[0].Do(this);
            my_tasks.Remove(my_tasks[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            agent.destination = Target.transform.position;
    }

    public class Task
    {
        public virtual IEnumerator Do(Character that_character)
        {
            yield return null;
        }
    }


    public class ReachPointTask:Task
    {
        public Vector3 point;
        public ReachPointTask(Vector3 pointToReach)
        {
            point = pointToReach;
        }

        public override IEnumerator Do(Character that_character)
        {
            that_character.agent.destination = point;
            while (Vector3.Distance( that_character.transform.position,point)>3)
            {
                yield return null;
            }
        }
    }

}
