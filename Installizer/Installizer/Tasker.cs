using Microsoft.Win32.TaskScheduler;
using Newtonsoft.Json.Linq;
namespace Installizer
{
    public class Tasker
    {
        string TaskName;
        string TaskPath;
        TaskService serv;
        TaskDefinition taskDefinition;
        public Tasker(string TaskName, string TaskPath)
        {
            this.TaskName = TaskName;
            this.TaskPath = TaskPath;
            this.serv = new();
            this.taskDefinition = this.serv.NewTask();
        }
        public Tasker(string TaskName, string TaskPath, string description)
        {
            this.TaskName = TaskName;
            this.TaskPath = TaskPath;
            this.serv = new();
            this.taskDefinition = this.serv.NewTask();
            TaskDescribe(description);
        }
        public void TaskDescribe(string description)
        {
            this.taskDefinition.RegistrationInfo.Description = description;
            this.taskDefinition.RegistrationInfo.Author = $"{System.Environment.MachineName}\\{System.Environment.UserName}";
        }
        public void TaskSettingsDefine(bool batteries = true, bool enable = true, bool startwhenavailibale = true, bool hidden = false)
        {
            this.taskDefinition.Settings.DisallowStartIfOnBatteries = !batteries;
            this.taskDefinition.Settings.StopIfGoingOnBatteries = !batteries;
            this.taskDefinition.Settings.Enabled = enable;
            this.taskDefinition.Settings.StartWhenAvailable = startwhenavailibale;
            this.taskDefinition.Settings.Hidden = hidden;
        }

        public void TaskPrincipal(bool highest = false, bool service = false, string user = "")
        {
            if (highest)
            {
                this.taskDefinition.Principal.RunLevel = TaskRunLevel.Highest;
            }
            else
            {
                this.taskDefinition.Principal.RunLevel = TaskRunLevel.LUA;
            }
            if (service)
            {
                this.taskDefinition.Principal.LogonType = TaskLogonType.S4U;
            }
            else
            {
                this.taskDefinition.Principal.LogonType = TaskLogonType.Password;
            }
            if (user != "")
            {
                this.taskDefinition.Principal.Id = user;
            }
            else
            {
                this.taskDefinition.Principal.Id = $"{System.Environment.MachineName}\\{System.Environment.UserName}";
            }
        }
        private System.DateTime Day(int day)
        {
            if (day > 0 && day < 8)
            {
                return new System.DateTime(1977, 12, 31).AddDays(day);
            }
            return new System.DateTime(1978, 1, 1);
        }
        private System.DateTime HourTime(string time)
        {
            return System.DateTime.Parse(time);
        }
        public void TaskEventTriggerDefine(string logName, string logSource, int eventID)
        {
            EventTrigger trigger = new EventTrigger();
            trigger.Enabled = true;
            trigger.SetBasic(logName, logSource, eventID);
            this.taskDefinition.Triggers.Add(trigger);
        }

        public void TaskDailyTriggerrDefine(string hour)
        {
            DailyTrigger trigger = new();
            trigger.Enabled = true;
            int h = System.Int32.Parse(hour.Split(":")[0]);
            int m = System.Int32.Parse(hour.Split(":")[1]);
            trigger.StartBoundary = System.DateTime.Today + System.TimeSpan.FromHours(h) + System.TimeSpan.FromMinutes(m);
            trigger.Repetition.Duration = System.TimeSpan.FromDays(1);
            trigger.Repetition.Interval = System.TimeSpan.FromMinutes(1);
            this.taskDefinition.Triggers.Add(trigger);
        }
        public void TaskDailyTriggersDefine(string[] hours)
        {
            foreach (string hour in hours)
            {
                TaskDailyTriggerrDefine(hour);
            }
        }
        //public void TaskMonthlyTriggerDefine(int month, int day, string hour)
        //{
        //    MonthlyTrigger trigger = new();
        //    int[] d = new int[] { day };
        //    trigger.DaysOfMonth = d;
        //    trigger.
        //}
        public void TaskWeeklyTriggerDefine(string hour, int day)
        {
            WeeklyTrigger trigger = new();
            DaysOfTheWeek dayS;
            switch (day)
            {
                case 1:
                    {
                        dayS = DaysOfTheWeek.Sunday;
                        break;
                    }
                case 2:
                    {
                        dayS = DaysOfTheWeek.Monday;
                        break;
                    }
                case 3:
                    {
                        dayS = DaysOfTheWeek.Tuesday;
                        break;
                    }
                case 4:
                    {
                        dayS = DaysOfTheWeek.Wednesday;
                        break;

                    }
                case 5:
                    {
                        dayS = DaysOfTheWeek.Thursday;
                        break;

                    }
                case 6:
                    {
                        dayS = DaysOfTheWeek.Friday;
                        break;
                    }
                default:
                    {
                        dayS = DaysOfTheWeek.Saturday;
                        break;
                    }
            }
            trigger.DaysOfWeek = dayS;
            trigger.StartBoundary = System.DateTime.Parse(hour, System.Globalization.CultureInfo.InvariantCulture);
            trigger.Repetition.Interval = System.TimeSpan.FromMinutes(1);
            this.taskDefinition.Triggers.Add(trigger);
        }
        public void TaskActionsDefine(string app, string? argus, string? location)
        {
            ExecAction action = new();
            action.Path = app;
            if (argus != null)
            {
                action.Arguments = argus;
            }
            if (location != null)
            {
                action.WorkingDirectory = location;
            }
            this.taskDefinition.Actions.Add(action);
        }
        public void TaskActionsDefine(string[] apps, string?[] argus, string?[] locations)
        {
            for (int i = 0; i < apps.Length; i++)
            {
                TaskActionsDefine(apps[i], argus[i], locations[i]);
            }
        }
        public void TaskWeeklyTriggerDefine(string hour, int[] days)
        {
            foreach (int day in days)
            {
                TaskWeeklyTriggerDefine(hour, day);
            }
        }
        public void TaskWeeklyTriggerDefine(string[] hours, int[] days)
        {
            foreach (string hour in hours)
            {
                TaskWeeklyTriggerDefine(hour, days);
            }
        }

        //public void TaskTriggersDefine(int[] days, string[] hours)
        //{
        //    Trigger trigger;
        //    DateTime[] hrs = new DateTime[hours.Length];
        //    DateTime[] dates = new System.DateTime[days.Length];
        //    for (int i = 0; i < days.Length; i++)
        //    {
        //        foreach(string hor in hours)
        //        {
        //        if (days[i] > 0 && days[i] < 8)
        //        {
        //            dates[i] = System.DateTime.Parse($"0{days}/01/1978",null);

        //                int horr=Int32.Parse( hor.Split(":")[0]);
        //                int minn = Int32.Parse(hor.Split(":")[1]);
        //                dates[i] = dates[i].AddHours(horr);
        //                dates[i].AddMinutes(minn);
        //        }

        //        }
        //    }

        //    trigger.StartBoundary = new DateTime[dates.Length];
        //}
        public void RegisterTask()
        {
            try
            {
                this.serv.RootFolder.RegisterTaskDefinition($"{this.TaskPath}\\{this.TaskName}", this.taskDefinition);
            }
            catch
            {
                this.serv.RootFolder.RegisterTaskDefinition(this.TaskPath, this.taskDefinition);
            }

            //TaskService.Instance.RootFolder.RegisterTaskDefinition($"{this.TaskPath}\\{this.TaskName}", this.taskDefinition, TaskCreation.CreateOrUpdate, "סארט", null, TaskLogonType.Password);

            //serv.RootFolder.RegisterTaskDefinition(TaskPath);
        }
        public static void ExportJson(string path, Tasker task)
        {
            var tas = Newtonsoft.Json.JsonConvert.SerializeObject(task, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(path, tas);
        }
        public void ExportJson()
        {
            JObject Task = new();
            Task.Add(new JProperty("TaskName", this.TaskName));
            Task.Add(new JProperty("TaskPath", this.TaskPath));
            JArray actions = new();
            foreach (var act in this.taskDefinition.Actions)
            {

                JArray array = new();
                array.Add(((ExecAction)act).Path);
                array.Add(((ExecAction)act).Arguments);
                array.Add(((ExecAction)act).WorkingDirectory);
                actions.Add(array);
            }
            foreach (var trig in this.taskDefinition.Triggers)
            {
                JArray triggers = new JArray();
                // TODO: continue
            }
            //Task.Add(new JProperty())
        }
        public static void ImportTasks(string jsonFile)
        {

            JObject SchedTask = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(System.IO.File.ReadAllText(jsonFile));
            Tasker tasker = new(SchedTask["TaskName"].ToString(), SchedTask["TaskPath"].ToString(), SchedTask["Description"].ToString());

        }
    }
}
