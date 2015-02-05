using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Validation.ValidationImpl
{
    public class DeployConfigurationValidator : IDeployConfigurationValidator
    {
        public void ValidateConfiguration(object configObject)
        {
            var configType = configObject.GetType();
            foreach(var propInfo in configType.GetProperties())
            {
                var requiredAttributeList = propInfo.GetCustomAttributes(typeof(RequiredAttribute), true).Cast<RequiredAttribute>();
                if(requiredAttributeList.Any())
                {
                    var value = propInfo.GetValue(configObject);
                    if(value == null || (propInfo.PropertyType == typeof(string) && string.IsNullOrEmpty((string)value)))
                    {
                        string errorMessage = requiredAttributeList.Select(i=>i.ErrorMessage).FirstOrDefault(i=>!string.IsNullOrEmpty(i));
                        if(string.IsNullOrEmpty(errorMessage))
                        {
                            errorMessage = string.Format("Missing required field \"{0}\"", propInfo.Name);
                        }
                        throw new ValidationException(errorMessage, requiredAttributeList.FirstOrDefault(), value);
                    }
                }
            }
        }


        public object ApplyDefaults(object configObject)
        {
            var configType = configObject.GetType();
            var newObject = AutoMapper.Mapper.Map(configObject, Activator.CreateInstance(configType));
            foreach (var propInfo in configType.GetProperties())
            {
                var value = propInfo.GetValue(newObject);
                if (value == null || (propInfo.PropertyType == typeof(string) && string.IsNullOrEmpty((string)value)))
                {
                    var defaultAttribute = propInfo.GetCustomAttributes(typeof(DefaultValueAttribute), true).Cast<DefaultValueAttribute>().FirstOrDefault();
                    if(defaultAttribute != null)
                    {
                        propInfo.SetValue(newObject, defaultAttribute.Value);
                    }
                }
            }
            return newObject;
        }

        public object ValidateAndApplyDefaults(object configObject)
        {
            this.ValidateConfiguration(configObject);
            return this.ApplyDefaults(configObject);
        }
    }
}
