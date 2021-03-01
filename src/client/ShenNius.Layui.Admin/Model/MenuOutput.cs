using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShenNius.Layui.Admin.Model
{
    public class MenuOutput
    {
            public int Id { get; set; }

            /// <summary>
            /// Desc:
            /// Default:0
            /// Nullable:False
            /// </summary>           
            public int ParentId { get; set; }
            public string NameCode { get; set; }
            /// <summary>
            /// Desc:
            /// Default:
            /// Nullable:True
            /// </summary>           
            public string Name { get; set; }

            /// <summary>
            /// Desc:
            /// Default:
            /// Nullable:True
            /// </summary>           
            public string Url { get; set; }

            /// <summary>
            /// Desc:
            /// Default:DateTime.Now
            /// Nullable:False
            /// </summary>           
            public DateTime CreateTime { get; set; }

            /// <summary>
            /// Desc:
            /// Default:
            /// Nullable:True
            /// </summary>           
            public string HttpMethod { get; set; }


            public bool Status { get; set; }
            public int Sort { get; set; }
            public string Icon { get; set; }
            public string[] BtnCodeIds { get; set; }

            public string ParentIdList { get; set; }
            public int Layer { get; set; }
        }
    }
