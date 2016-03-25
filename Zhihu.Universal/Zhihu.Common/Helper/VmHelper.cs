using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using GalaSoft.MvvmLight;


namespace Zhihu.Common.Helper
{
    public sealed class DataAttribute : Attribute
    {

    }

    public static class VmHelper
    {
        private static readonly Dictionary<String, Dictionary<String, object>> ViewModels =
            new Dictionary<String, Dictionary<String, object>>();

        public static Boolean HasSaved(ViewModelBase viewModel, String viewModelId)
        {
            var vmFullName = viewModel.GetType().Name + viewModelId;

            return ViewModels.ContainsKey(vmFullName) ? true : false;
        }

        /// <summary>
        /// 保存现场
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static Boolean SaveStates(ViewModelBase viewModel, String viewModelId)
        {
            var states =
                viewModel.GetType()
                    .GetRuntimeFields()
                    .Where(
                        field => CustomAttributeExtensions.GetCustomAttributes<DataAttribute>((MemberInfo) field).Any())
                    .ToDictionary(field => field.Name, field => field.GetValue(viewModel));

            foreach (
                var property in
                    viewModel.GetType()
                        .GetRuntimeProperties()
                        .Where(property => property.GetCustomAttributes<DataAttribute>().Any()))
            {
                states.Add(property.Name, property.GetValue(viewModel));
            }

            var vmFullName = viewModel.GetType().Name + viewModelId;

            if (ViewModels.ContainsKey(vmFullName))
            {
                ViewModels[vmFullName] = states;

                return true;
            }
            else
            {
                ViewModels.Add(vmFullName, states);
            }

            return true;
        }

        /// <summary>
        /// 还原现场
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static Boolean ResumeStates(ViewModelBase viewModel, String viewModelId)
        {
            Debug.WriteLine("ViewModel Resume Start :{0}", DateTime.Now);

            var vmFullName = viewModel.GetType().Name + viewModelId;

            if (ViewModels.ContainsKey(vmFullName) == false) return false;

            var vmStates = ViewModels[vmFullName];

            if (vmStates.Count <= 0) return false;
            
            foreach (var field in
                viewModel.GetType()
                    .GetRuntimeFields()
                    .Where(field => field.GetCustomAttributes<DataAttribute>().Any()))
            {
                var fileValue = vmStates[field.Name];
                field.SetValue(viewModel, fileValue);
            }

            foreach (var property in
                viewModel.GetType()
                    .GetRuntimeProperties()
                    .Where(property => property.GetCustomAttributes<DataAttribute>().Any()))
            {
                var propertyValue = vmStates[property.Name];
                property.SetValue(viewModel, propertyValue);
            }

            Debug.WriteLine("ViewModel Resume End :{0}", DateTime.Now);
        
            return true;
        }
    }
}