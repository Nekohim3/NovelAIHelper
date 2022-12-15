using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelAIHelper
{
    public interface ISelected
    {
        public bool IsSelected { get; set; }
    }

    public interface IExpanded
    {
        public bool IsExpanded { get; set; }
    }

    public interface IDraggable
    {
        public bool IsDrag { get; set; }
    }
}
