using System.Collections.Generic;
using System.Linq;

namespace manpower
{
    public class TaskPlanner : ITaskPlanner
    {
        private IRepository repository { get; set; }
        public IEnumerable<Assignment> assignment = Enumerable.Empty<Assignment>();
        // public List<Assignment> assignment = new List<Assignment>();

        //Constructor of TaskPlanner
        public TaskPlanner(IRepository repository)
        {
            this.repository = repository;
        }


        public IEnumerable<Assignment> Execute()
        {
            //Mapping person with the priority tasks
            Dictionary<Person, HashSet<Task>> mp = new Dictionary<Person, HashSet<Task>>();
            foreach (var person in repository.People)
            {
                mp.Add(person, new HashSet<Task>());
            }


            //Mapping person with non priority tasks
            Dictionary<Person, HashSet<Task>> m = new Dictionary<Person, HashSet<Task>>();
            foreach (var person in repository.People)
            {
                m.Add(person, new HashSet<Task>());
            }

            //Adding tasks according to priority
            foreach (var task in repository.Tasks)
            {
                if (task.IsPriority)
                {
                    foreach (var person in repository.People)
                    {
                        if (person.Skills.Contains(task.SkillRequired))
                        {
                            mp[person].Add(task);
                        }
                    }
                }
                else
                {
                    foreach (var person in repository.People)
                    {
                        if (person.Skills.Contains(task.SkillRequired))
                        {
                            m[person].Add(task);
                        }
                    }
                }
            }


            int day = 1;
            //Looping until all tasks are assigned
            while (day >= 1)
            {

                bool flag = false;

                //To check that all persons have assigned task or not
                Dictionary<Person, bool> vis = new Dictionary<Person, bool>();
                foreach (var person in repository.People) vis.Add(person, false);

                //Looping for assigning priority tasks to person
                foreach (var p in repository.People)
                {

                    //taking availble person with least taskid(which can be assigned to him)
                    int mx = 1000000;
                    Person person = new Person();
                    foreach (var per in repository.People)
                    {
                        if (!vis[per] && mx > mp[per].Count && mp[per].Count > 0)
                        {
                            mx = mp[per].Count;
                            person = per;
                        }
                    }

                    //checking if any person is availble or not
                    if (mx == 1000000) break;
                    else
                    {
                        Task tk = new Task();
                        foreach (var tsk in mp[person])
                        {
                            tk = tsk;
                            break;
                        }

                        //assigning task to the selected person
                        var ass = new Assignment();
                        ass.Task = tk;
                        ass.Person = person;
                        ass.Day = day;
                        assignment = Add(assignment, ass);

                        //Making that person assigned for the day
                        vis[person] = true;
                        flag = true;

                        //Removing the assigned task if other's contain
                        foreach (var per in repository.People)
                        {
                            if (mp[per].Contains(tk) && per != person) mp[per].Remove(tk);
                        }
                        mp[person].Remove(tk);
                    }
                }

                //Looping for assigning non-priority tasks to person(functionalities are same as above)
                foreach (var per in repository.People)
                {

                    int mx = 1000000;
                    Person pers = new Person();

                    foreach (var person in repository.People)
                    {
                        if (!vis[person] && mx > m[person].Count && m[person].Count > 0)
                        {
                            mx = m[person].Count;
                            pers = person;
                        }
                    }

                    if (mx == 1000000) break;
                    else
                    {
                        Task tk = new Task();
                        foreach (var tsk in m[pers])
                        {
                            tk = tsk;
                            break;
                        }

                        var ass = new Assignment();
                        ass.Task = tk;
                        ass.Person = pers;
                        ass.Day = day;
                        assignment = Add(assignment, ass);

                        vis[pers] = true;
                        flag = true;

                        foreach (var person in repository.People)
                        {
                            if (m[person].Contains(tk) && person != pers) m[person].Remove(tk);
                        }
                        m[pers].Remove(tk);
                    }
                }

                //incrementing day
                day += 1;
                if (!flag) break;
            }
            return assignment;
        }

        // For adding Assignment in IEnumerable<Assignment> assignment
        public IEnumerable<Assignment> Add<Assignment>(IEnumerable<Assignment> e, Assignment value)
        {
            foreach (var cur in e)
            {
                yield return cur;
            }
            yield return value;
        }

    }
}