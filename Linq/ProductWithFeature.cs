using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq
{
    public class ProductWithFeature
    {
        public string Name { get; set; }

        public ICollection<Feature> Features { get; set; } = new List<Feature>();

        public ProductWithFeature(string name, params int[] featId)
        {
            Name = name;
            foreach (int id in featId)
            {
                Features.Add(new Feature(id));
            }
        }
    }
}
