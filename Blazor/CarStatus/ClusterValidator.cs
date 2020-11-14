using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using KernelCars.Data;

namespace KernelCars.Blazor.CarStatus
{
    public class ClusterValidator : OwningComponentBase<DataContext>
    {
        public DataContext Context => Service;


        //[Parameter]
        //public int ClustertId { get; set; }

        [CascadingParameter]
        public EditContext CurrentEditContext { get; set; }


        protected override void OnInitialized()
        {
            ValidationMessageStore store =
            new ValidationMessageStore(CurrentEditContext);
            CurrentEditContext.OnFieldChanged += (sender, args) =>
            {
                string name = args.FieldIdentifier.FieldName;
                if (name == "ClusterId")
                {
                    Validate(store);
                }
            };
        }






        private void Validate(ValidationMessageStore store)
        {
            store.Add(CurrentEditContext.Field("ClusterId"),"Hello!");
            //if (model.DepartmentId == DepartmentId &&
            //(!LocationStates.ContainsKey(model.LocationId) ||
            //LocationStates[model.LocationId] != State))
            //{
            //    store.Add(CurrentEditContext.Field("LocationId"),
            //    $"{DeptName} staff must be in: {State}");
            //}
            //else
            //{
            //    store.Clear();
            //}
            CurrentEditContext.NotifyValidationStateChanged();
        }




    }
}