﻿using System;
using Simpler.Core.Tasks;

namespace Simpler
{
    public abstract class Task
    {
        public static T New<T>()
        {
            var createTask = new CreateTask {In = {TaskType = typeof (T)}};
            createTask.Execute();
            return (T)createTask.Out.TaskInstance;
        }

        public virtual string Name
        {
            get
            {
                var baseType = GetType().BaseType;
                return baseType == null
                           ? "Unknown"
                           : String.Format("{0}.{1}", baseType.Namespace, baseType.Name);
            }
        }

        public abstract void Execute();
    }
}