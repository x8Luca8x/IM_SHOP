using IMAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAPI
{
    public static class ModelManager
    {
        public static void FillDefaults(TBASE Model, bool bCreated = false)
        {
            if(bCreated)
                Model.CREATED = DateTime.Now;

            Model.CHANGED = DateTime.Now;
        }
    }
}
