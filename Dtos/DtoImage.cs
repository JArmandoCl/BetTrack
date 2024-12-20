using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetTrack.Dtos
{
    public class DtoImage : BindableBase
    {
        private string image = "";
        public string Image
        {
            get { return image; }
            set { SetProperty(ref image, value); }
        }
        private string description="";
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }
    }
}
