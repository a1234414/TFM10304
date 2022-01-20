using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_TFM10304.Attributes
{
    public class ViewLayoutAttribute : ResultFilterAttribute
    {
        private string _layout;
        public ViewLayoutAttribute(string layout)
        {
            _layout = layout;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var viewResult = context.Result as ViewResult;
            if (viewResult != null)
            {
                viewResult.ViewData["Layout"] = this._layout;
            }
            base.OnResultExecuting(context);
        }
    }
}
