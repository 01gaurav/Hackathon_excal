using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;

namespace manpower
{
    internal class sub_RepositoryBase : RepositoryBase
    {
        public sub_RepositoryBase(FileLocations fileLocations, string configPath) : base(fileLocations, configPath)
        { }

        public override void SaveAssignments(IEnumerable<Assignment> assignments, string saveToPath)
        {
            System.Text.StringBuilder assignData = new System.Text.StringBuilder();
            assignData.AppendLine(string.Format("{0},{1},{2}", "Task ID", "Person ID", "Day"));
            foreach (var a in assignments)
            {
                var taskId = a.Task.Id;
                var personId = a.Person.Id;
                var day = a.Day;
                var newLine = string.Format("{0},{1},{2}", taskId, personId, day);
                assignData.AppendLine(newLine);
            }
            File.WriteAllText(saveToPath, assignData.ToString());
        }
    }
}