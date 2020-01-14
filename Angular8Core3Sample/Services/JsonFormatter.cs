using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;

using Angular8Core3Sample.Services.Models;


namespace Angular8Core3Sample.Services
{
    public class JsonFormatter
    {

        public JsonFormatter()
        {

        }

        private List<TreeTableFormat> GetTreeTables(JArray jObj)
        {

            var treeTables = new List<TreeTableFormat>();

            foreach (JObject child in jObj.Children())
            {
                var treeTable = new TreeTableFormat();

                foreach(var prop in child.Properties())
                {
                    if (prop.Value.Type == JTokenType.Array && prop.Value.Count() > 0)
                    {
                        treeTable.children = new List<TreeTableFormat>();
                        treeTable.children.AddRange(GetTreeTables((JArray)prop.Value));
                    }
                    else
                    {
                        treeTable.data[prop.Name] = prop.Value;
                    }
                }

                treeTables.Add(treeTable);
            }

            return treeTables;
        }

        //public TreeTableFormat GetChildren(JToken jObj)
        //{
        //    var treeTable = new TreeTableFormat();
        //    foreach (var child in jObj.Children())
        //    {
        //        if (child.Type == JTokenType.Object)
        //        {
        //            GetChildren(child);
        //        }
        //        else
        //        {
        //            //treeTable.properties[jObj.]
        //        }
        //    }

        //    return treeTable;
        //}


        public JsonResult FormatForTreeTable(string obj)
        {
            try
            {
                var o = JArray.Parse(obj);

                List<TreeTableFormat> treeTables = GetTreeTables(o);

                var topNode = new TreeTableNode() { data = treeTables  };

                var x = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(topNode, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));

               return new JsonResult(topNode, new JsonSerializerSettings() { Formatting = Formatting.Indented, ReferenceLoopHandling = ReferenceLoopHandling.Serialize, PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
