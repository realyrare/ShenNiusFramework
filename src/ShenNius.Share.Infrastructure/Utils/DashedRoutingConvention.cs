using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ShenNius.Share.Infrastructure.Utils
{
    public class DashedRoutingConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var hasRouteAttributes = controller.Selectors.Any(selector =>
                selector.AttributeRouteModel != null);
            if (hasRouteAttributes)
            {
                // This controller manually defined some routes, so treat this 
                // as an override and not apply the convention here.
                return;
            }

            foreach (var controllerAction in controller.Actions)
            {
                foreach (var selector in controllerAction.Selectors.Where(x => x.AttributeRouteModel == null))
                {
                    var parts = new List<string>();
                    foreach (var attr in controller.Attributes)
                    {
                        if (attr is AreaAttribute area)
                        {
                            parts.Add(area.RouteValue);
                        }
                    }

                    if (
                        parts.Count == 0
                        && controller.ControllerName == "Home"
                        && controllerAction.ActionName == "Index"
                    )
                    {
                        continue;
                    }

                    parts.Add(PascalToKebabCase(controller.ControllerName));

                    if (controllerAction.ActionName != "Index")
                    {
                        parts.Add(PascalToKebabCase(controllerAction.ActionName));
                    }

                    selector.AttributeRouteModel = new AttributeRouteModel
                    {
                        Template = string.Join("/", parts)
                    };
                }
            }
        }

        private static string PascalToKebabCase(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return Regex.Replace(
                    value,
                    "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
                    "-$1",
                    RegexOptions.Compiled)
                .Trim()
                .ToLower();
        }
    }
}
