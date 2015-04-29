using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Damascus.Message;

namespace Damascus.Core.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static void CreateMaps()
        {
            Mapper.CreateMap<InviteInput, ReducedInviteInput>();
        }
    }
}
