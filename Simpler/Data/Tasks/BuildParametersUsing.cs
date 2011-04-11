﻿using System;
using System.Data;
using System.Reflection;

namespace Simpler.Data.Tasks
{
    /// <summary>
    /// Task that looks in the given command's CommandText for parameters and uses the given object's property
    /// values to build the command parameters.
    /// </summary>
    /// <typeparam name="T">The type of the object that contains the parameter values.</typeparam>
    public class BuildParametersUsing<T> : Task
    {
        // Inputs
        public virtual IDbCommand CommandWithParameters { get; set; }
        public virtual T ObjectWithValues { get; set; }

        // Sub-tasks
        public virtual FindParametersInCommandText FindParametersInCommandText { get; set; }

        public override void Execute()
        {
            // Create the sub-tasks.
            if (FindParametersInCommandText == null) FindParametersInCommandText = new FindParametersInCommandText();

            FindParametersInCommandText.CommandText = CommandWithParameters.CommandText;
            FindParametersInCommandText.Execute();

            foreach (var parameterNameX in FindParametersInCommandText.ParameterNames)
            {
                var objectType = typeof(T);
                object objectContainingPropertyValue = ObjectWithValues;

                // Strip off the first character of the parameter name to find a matching property (e.g. make @Name => Name).
                var nameOfPropertyContainingValue = parameterNameX.Substring(1);

                // If the parameter contains a dot then the property must be a complex object, and therefore we must look inside the object to find the value.
                PropertyInfo property;
                while(nameOfPropertyContainingValue.Contains("."))
                {
                    // Look for a property using the string that comes before the dot.
                    var indexOfDot = nameOfPropertyContainingValue.IndexOf(".");
                    property = objectType.GetProperty(nameOfPropertyContainingValue.Substring(0, indexOfDot));

                    // Apparently there isn't a property that is a complex object that matches the parameter name.
                    if (property == null) break;

                    // Reset variables using the property that was found that matched the string that came before the dot. 
                    objectType = property.PropertyType;
                    objectContainingPropertyValue = property.GetValue(objectContainingPropertyValue, null);
                    nameOfPropertyContainingValue = nameOfPropertyContainingValue.Substring(indexOfDot + 1);
                }

                // Use the property to build the parameter.
                property = objectType.GetProperty(nameOfPropertyContainingValue);
                if (property != null)
                {
                    var dbDataParameter = CommandWithParameters.CreateParameter();

                    // If the property came from a complex object then it contains a dot, and dots aren't allowed in parameter names.
                    dbDataParameter.ParameterName = parameterNameX.Replace(".", "_");
                    CommandWithParameters.CommandText = CommandWithParameters.CommandText.Replace(parameterNameX, parameterNameX.Replace(".", "_"));

                    dbDataParameter.Value = property.GetValue(objectContainingPropertyValue, null) ?? DBNull.Value;
                    CommandWithParameters.Parameters.Add(dbDataParameter);
                }
            }
        }
     }
}
