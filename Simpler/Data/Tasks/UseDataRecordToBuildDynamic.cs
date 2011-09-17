using System.Collections.Generic;
using System.Data;
using System.Dynamic;

namespace Simpler.Data.Tasks
{
    /// <summary>
    /// Task that builds an instance of a dynamic type using the values found in the given DataRecord.  The dynamic object
    /// will contain properties matching all the columns found in the DataRecord.
    /// </summary>
    public class UseDataRecordToBuildDynamic : Task
    {
        /// <summary>
        /// Used to create the dynamic Object that is returned by UseDataRecordToBuildDynamic.
        /// </summary>
        class DynamicObjectWithBackDoor : DynamicObject
        {
            readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                return _dictionary.TryGetValue(binder.Name, out result);
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                // Add property to the internal dictionary.
                _dictionary[binder.Name.ToLower()] = value;
                return true;
            }

            public void AddMemberThroughTheBackDoor(string memberName, object memberValue)
            {
                // Add property to the internal dictionary.
                _dictionary[memberName] = memberValue;
            }
        }

        // Inputs
        public virtual IDataRecord DataRecord { get; set; }

        // Outputs
        public virtual dynamic Object { get; private set; }

        public override void Execute()
        {
            Object = new DynamicObjectWithBackDoor();

            for (var i = 0; i < DataRecord.FieldCount; i++)
            {
                var columnName = DataRecord.GetName(i);
                var columnValue = DataRecord[columnName];
                if (columnValue.GetType() != typeof(System.DBNull))
                {
                    Object.AddMemberThroughTheBackDoor(columnName, columnValue);
                }
            }
        }
    }
}
