using System;
using System.Linq;
using System.Reflection;

namespace AspiringDemo.Saving
{
    public class RuntimeType
    {
        public Type RunType { get; set; }
        public string VariableName { get; set; }

        public RuntimeType(Type type, string name)
        {
            RunType = type;
            VariableName = name;
        }

        //private string GetAdd()
        //{
        //    //RunType.GetProperties()
        //}

        public override string ToString()
        {
            string glorious = String.Format("RuntimeTypeModel.Default.Add(typeof({0}), true)", RunType.Name);

            var allTypes = RunType.GetProperties().Select(info => '\"' + info.Name + '\"').ToList();
            var allFields = RunType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(info => !info.Name.Contains("BackingField")).Select(info => '\"' + info.Name + '\"').ToList();

            allTypes.AddRange(allFields);

            string addString = String.Join(",", allTypes);
            //string fieldAddString = String.Join(",", allFields);

            if (!String.IsNullOrEmpty(addString))
                glorious += String.Format(".Add({0})", addString);


            glorious += ";";

            return glorious;
            //return base.ToString();
        }
    }
}
