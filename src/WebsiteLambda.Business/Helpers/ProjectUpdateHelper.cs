using System;
using WebsiteLambda.Business.Interface;
using WebsiteLambda.Models;

namespace WebsiteLambda.Business.Helpers
{
    public class ProjectUpdateHelper : IProjectUpdateHelper
    {
        public Project CompareAndGetUpdatedProject(Project oldProject, Project newProject)
        {
            return CompareAndReturnNewer(oldProject, newProject);
        }

        private T CompareAndReturnNewer<T> (T older, T newer) where T: new()
        {
            T toReturn = new T();
            var type = toReturn.GetType();
            
            foreach (var prop in type.GetProperties())
            {
                var oldValue = prop.GetValue(older, null);
                var newValue = prop.GetValue(newer, null);

                dynamic toSet;

                if (prop.PropertyType.IsClass) 
                {
                    toSet = CompareAndReturnNewer(oldValue, newValue);
                }
                else
                {
                    toSet = (!Equals(oldValue, newValue) || null == newValue) ? newValue : oldValue;
                }

                prop.SetValue(toReturn, toSet);
            }

            return toReturn;
        }
    }
}
